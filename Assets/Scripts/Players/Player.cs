using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The player class contains the common features for green and purple players
public abstract class Player : MonoBehaviour
{
    // Where is the player?
    protected Vector2 _position;
    // What is the maximum speed of the player?
    protected float maxSpeed = 0.4f;
    // What is the maximum rotation speed of the player?
    protected float maxRotationSpeed = 20f;
    // What is the current speed of the player?
    protected float currentSpeed = 0.05f;
    // What is the current direction (rotation around the y axis) of the player?
    protected float currentRotation = 45f;
    // The gaol we are taking the target to
    protected GameObject gaol = null;
    protected GameManager gameManager;
    protected float gaolOffset = 3f;

    // The purpose of these Dictionaries (see https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2?view=net-6.0)
    // is to keep track of where this player last saw the other players.
    // The Dictionary is indexed by a player, and returns a timestamp.
    protected PlayerHistory greenPlayersHistory = new PlayerHistory();
    protected PlayerHistory purplePlayersHistory = new PlayerHistory();

    public PlayerHistory myHistory = new PlayerHistory();

    public Vector2 position { get { return _position; } set { _position = value; } }

    // We assume all obstacles have a fixed diameter to simplify processing
    const float obstacleDiameter = 2.0f;
    // We set the field of view to 90°
    const float viewAngleHalf = 45f;
    // We ignore = cannot see players further away than 30 units
    const float viewDistance = 30f;

    Vector3 planeCenterPos;

    protected bool _isIdle = true;
    public bool isIdle { get { return _isIdle; } set { _isIdle = value; } }

    // We expect the subclasses to have their own implementations of Start
    protected virtual void Start()
    {
        gameManager = GameManager.Instance();
        gaol = gameManager.GetGaol();
        planeCenterPos = gameManager.planeCenter.position;

        // Locate all players and store them (which includes their position) along with a time stamp for when they were last seen
        foreach (Player green in gameManager.GreenPlayers())
            greenPlayersHistory.Add(green);
        foreach (Player purple in gameManager.PurplePlayers())
            purplePlayersHistory.Add(purple);

        myHistory.Add(this);
    }

    // Update can be overridden too.
    protected virtual void Update()
    {
        // views is a list of triangles that determine what we currently can see
        List<VisionTriangle> views = new List<VisionTriangle>();
        // This is our base view, looking forward.
        VisionTriangle baseview = new VisionTriangle(currentRotation - viewAngleHalf, currentRotation + viewAngleHalf, viewDistance);

        // For each obstacle, we compute where it currently is in relation to us, and therefore the angles in which it obscures vision
        foreach (GameObject obstacle in gameManager.obstacles)
        {
            Vector2 position = new Vector2(obstacle.transform.position.x, obstacle.transform.position.z);
            float radius = obstacleDiameter / 2;
            float distance = (position - this._position).magnitude;
            float viewAngle = Mathf.Asin(radius / distance);
            float objectAngle = Mathf.Atan2(position.y - this._position.y, position.x - this._position.x);
            views.Add(new VisionTriangle(objectAngle - viewAngle, objectAngle + viewAngle, distance));
        }

        // We go through all the players (only green players here, extend as needed) to determine which of them we can see right now
        foreach (Player green in gameManager.GreenPlayers())
        {
            Vector2 position = green._position;
            float angle = Mathf.Atan2(position.y - this._position.y, position.x - this._position.x);
            float distance = (position - this._position).magnitude;
            // First we check if the player is within the basic view 
            if (angle >= baseview.minimumAngle && angle <= baseview.maximumAngle && distance <= baseview.maximumDistance)
            {
                bool keep = true;
                // If so, we check if any of the obstacles are obscuring the player
                foreach (VisionTriangle vt in views)
                {  // This code is buggy, does not manage angles around 0°
                    if (vt.Inside(angle) && distance > vt.maximumDistance)
                    {
                        keep = false;
                    }
                }
                // If the player is visible, we update its timestamp
                if (keep)
                    greenPlayersHistory.Update(green);
            }
        }

        Move();
        myHistory.Update(this);
    }

    protected virtual void RandomMove()
    {
        float offset = (Random.value - Random.value) / 2f;
        currentRotation += (Mathf.Clamp(offset, 0.0f, maxRotationSpeed));
    }

    public float Rotation()
    {
        return currentRotation;
    }

    // checks if player is on the plane / playing field
    bool isPlayerWithinBoundary()
    {
        Vector3 pos = transform.position;
        float leftBoundary = planeCenterPos.x - gameManager.planeRadius;
        float rightBoundary = planeCenterPos.x + gameManager.planeRadius;
        float topBoundary = planeCenterPos.z + gameManager.planeRadius;
        float bottomBoundary = planeCenterPos.z - gameManager.planeRadius;
        return (pos.x > leftBoundary && pos.x < rightBoundary && pos.z > bottomBoundary && pos.z < topBoundary);
    }

    bool IsPlayerRunningIntoCylinder()
    {
        float distanceBtwCenterOfCylinderAndPlayer = 0f;
        foreach (GameObject cylinder in gameManager.obstacles)
        {
            distanceBtwCenterOfCylinderAndPlayer = Vector3.Distance(transform.position, cylinder.transform.position);
            if (distanceBtwCenterOfCylinderAndPlayer > obstacleDiameter / 2) return false;
        }
        return true;
    }

    // Move the player according to its current speed and direction
    protected virtual void Move()
    {
        // we ensure player does not run into boundaries or obstacles
        if (!IsPlayerRunningIntoCylinder() && isPlayerWithinBoundary()) return;
        _position += new Vector2(currentSpeed * Mathf.Cos(currentRotation), currentSpeed * Mathf.Sin(currentRotation));
        transform.rotation = Quaternion.Euler(0.0f, currentRotation, 0.0f);
        transform.position = new Vector3(_position.x, 0.0f, _position.y);
    }

    protected void Move(List<Player> attractivePlayers, List<Player> repulsivePlayer)
    {
        Vector2 movementSum = Vector2.zero;

        float attraction = 3.0f;
        float repulsion = 0.1f;

        foreach (Player p in attractivePlayers)
        {
            Vector2 direction = p.position - this.position;
            float distance = direction.magnitude;
            movementSum += (direction * attraction) / distance;
        }

        foreach (Player p in repulsivePlayer)
        {
            Vector2 direction = this.position - p.position;
            float distance = direction.magnitude;
            movementSum -= (direction * repulsion) / distance;
        }

        currentSpeed = Mathf.Clamp(movementSum.magnitude, 0.0f, maxSpeed);
        currentRotation = Mathf.Atan2(movementSum.y, movementSum.x);
        Move();
    }

}
