using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Purple players chase green players
public class PurplePlayer : Player
{
    // Our current target
    GreenPlayer target = null;

    float captureRange = 0.05f;

    // Start overrides the baseclass Start, but uses it.
    protected override void Start()
    {
        base.Start();
        currentSpeed = 0.05f;
    }

    // Update decides what to do, chase greens or bring them to gaol
    protected override void Update()
    {
        if (gameManager.CheckPurpleTeamWinningCondition()) return;

        if (!captured)
        {
            RandomMove();
            Chase();
        }
        else
        {
            TransportToGaol();
        }
   
        // Use the Move method of the parent class
        base.Update();
    }

    // Change direction by a bit and move forward
    private void RandomMove()
    {
        float offset = (Random.value - Random.value) / 3;

        currentRotation += (Mathf.Clamp(offset, 0.0f, maxRotationSpeed));
    }

    // Locate a target, if needed, and follow it.
    private void Chase()
    {
        if (target == null)
        {
            target = gameManager.FindClosestTarget(this) as GreenPlayer;
            if (target == null) return;
        }

        Vector2 targetPosition = target.Position();
        Vector2 direction = targetPosition - this.position;
        float distance = direction.magnitude;

        float futureRotation = Mathf.Atan2(direction.y, direction.x);

        // Limit the rotation and linear speeds
        currentRotation += Mathf.Clamp(futureRotation - currentRotation, -maxRotationSpeed, maxRotationSpeed);
        currentSpeed = Mathf.Clamp(distance, 0.0f, maxSpeed);

        if (distance < captureRange) this.captured = true;
    }

    // Take the prisoner to gaol and leave them there.  This method is incomplete.
    private void TransportToGaol()
    {
        target.OpponnentCaptured?.Invoke();
        Vector2 direction = new Vector2(gaol.transform.position.x, gaol.transform.position.z) - this.position;
        float distance = direction.magnitude;
        float futureRotation = Mathf.Atan2(direction.y, direction.x);
        currentRotation += Mathf.Clamp(futureRotation - currentRotation, -maxRotationSpeed, maxRotationSpeed);
        if (this.position.x <= gaol.transform.position.x)
        {
            this.captured = false;
            target = null;
        }
        currentSpeed = Mathf.Clamp(distance, 0.0f, maxSpeed);
    }
}
