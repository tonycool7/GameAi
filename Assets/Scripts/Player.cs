using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The player class contains the common features for green and purple players
public abstract class Player : MonoBehaviour
{
    // Where is the player?
    protected Vector2 position;
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
    // Have we managed to capture the target?
    protected bool captured = false;
    protected float gaolOffset = 3f;

    public System.Action OpponnentCaptured;


    // We expect the subclasses to have their own implementations of Start
    protected virtual void Start()
    {
        gameManager = GameManager.Instance();
        gaol = gameManager.GetGaol();
    }

    // Update can be overridden too.
    protected virtual void Update()
    {
        Move();
    }

    // We could alternatively implement Position as a property (see https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/properties)
    public Vector2 Position()
    {
        return position;
    }

    // Move the player according to its current speed and direction
    protected void Move()
    {
        position += new Vector2(currentSpeed * Mathf.Cos(currentRotation), currentSpeed * Mathf.Sin(currentRotation));

        transform.rotation = Quaternion.Euler(0.0f, currentRotation, 0.0f);
        transform.position = new Vector3(position.x, 0.0f, position.y);
    }

}
