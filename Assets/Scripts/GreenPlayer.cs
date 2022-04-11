using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The green players get chased
public class GreenPlayer : Player
{
    // The player should know if it has been captured, in which case it should follow its captor to gaol

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        position = new Vector2((transform.position.x + 350.0f) * Time.deltaTime, (transform.position.z + 350.0f) * Time.deltaTime);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!captured)
        {
            RandomMove();
            base.Update();
        }
    }

    // Change direction by a bit and move forward
    private void RandomMove()
    {
        float offset = (Random.value - Random.value) / 3;

        currentRotation += (Mathf.Clamp(offset, 0.0f, maxRotationSpeed));
    }

    // Take the prisoner to gaol and leave them there.  This method is incomplete.
    public void TransportToGaol()
    {
        Vector2 direction = new Vector2(gaol.transform.position.x, gaol.transform.position.z) - this.position;
        float distance = direction.magnitude;
        float futureRotation = Mathf.Atan2(direction.y, direction.x);
        this.currentRotation += Mathf.Clamp(futureRotation - currentRotation, -maxRotationSpeed, maxRotationSpeed);
        this.currentSpeed = Mathf.Clamp(distance, 0.0f, maxSpeed);

        if (this.position.x <= gaol.transform.position.x + this.gaolOffset)
        {
            this.captured = true;
            this.gameManager.RemoveGreenPlayerFromList(this);
        }

    }
}
