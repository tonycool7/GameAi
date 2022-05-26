using UnityEngine;

// The green players get chased
public class GreenPlayer : Player
{
    private StateMachine stateMachine;

    private bool _isGoaled = false;
    
    public bool isGoaled { get { return _isGoaled; } set { _isGoaled = value; } }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        stateMachine = new StateMachine(new GreenIdleState(this));
        _position = new Vector2((transform.position.x + 350.0f) * Time.deltaTime, (transform.position.z + 350.0f) * Time.deltaTime);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!_isGoaled)
        {
            RandomMove();
            base.Update();
        }
        stateMachine.Execute();
    }

    // Take the prisoner to gaol and leave them there.  This method is incomplete.
    public bool MovedToGaol()
    {
        Vector2 direction = new Vector2(gaol.transform.position.x, gaol.transform.position.z) - this._position;
        float futureRotation = Mathf.Atan2(direction.y, direction.x);
        this.currentRotation += Mathf.Clamp(futureRotation - currentRotation, -maxRotationSpeed, maxRotationSpeed);
        //this.currentSpeed = Mathf.Clamp(distance, 0.0f, maxSpeed);

        if (this._position.x <= gaol.transform.position.x + this.gaolOffset)
        {
            this._isGoaled = true;
            this.gameManager.RemoveGreenPlayerFromList(this);
            return true;
        }

        return false;
    }

    public bool PursuerApproching()
    {

        return false;
    }

    public bool HasBeenCaught()
    {

        return false;
    }
}
