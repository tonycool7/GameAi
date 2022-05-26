using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Purple players chase green players
public class PurplePlayer : Player
{
    StateMachine stateMachine;
    float captureRange = 0.5f;
    float dispersedTime = 0;
    float disperseDuration = 6f;

    // Start overrides the baseclass Start, but uses it.
    protected override void Start()
    {
        base.Start();
        stateMachine = new StateMachine(new PurpleIdleState(this));
        currentSpeed = 0.05f;
        _position = new Vector2((transform.position.x - 350.0f) * Time.deltaTime, (transform.position.z - 350.0f) * Time.deltaTime);
    }

    // Update decides what to do, chase greens or bring them to gaol
    protected override void Update()
    {
        stateMachine.Execute();
        if (gameManager.CheckPurpleTeamWinningCondition()) return;
        // Use the Move method of the parent class
        base.Update();
    }

    protected override void RandomMove()
    {
        float offset = (Random.value - Random.value) / 7f;
        currentRotation += (Mathf.Clamp(offset, 0.0f, maxRotationSpeed));
    }

    bool DispersePlayer()
    {
        RandomMove();
        if (dispersedTime < disperseDuration) dispersedTime += Time.deltaTime;
        return dispersedTime > disperseDuration;
    }

    // Locate a target, if needed, and follow it.
    public bool Chase(GreenPlayer target)
    {
        if (!DispersePlayer()) return false;
        Vector2 targetPosition = target.position;
        Vector2 direction = targetPosition - this._position;
        float distance = direction.magnitude;
        float futureRotation = Mathf.Atan2(direction.y, direction.x);
        // Limit the rotation and linear speeds
        currentRotation += Mathf.Clamp(futureRotation - currentRotation, -maxRotationSpeed, maxRotationSpeed);
        //currentSpeed = Mathf.Clamp(distance, 0.0f, maxSpeed);
        return (distance < captureRange);
    }

    // Take the prisoner to gaol and leave them there.  This method is incomplete.
    public bool TransportToGaol()
    {
        Vector2 direction = new Vector2(gaol.transform.position.x, gaol.transform.position.z) - this._position;
        float futureRotation = Mathf.Atan2(direction.y, direction.x);
        currentRotation += Mathf.Clamp(futureRotation - currentRotation, -maxRotationSpeed, maxRotationSpeed);
        return (this._position.x <= gaol.transform.position.x + this.gaolOffset);
    }
}
