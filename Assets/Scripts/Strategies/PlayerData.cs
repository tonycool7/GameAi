using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Information about where a player was at a given point in time
public class PlayerData
{
    public Vector2 position;
    public float direction;
    public int timestamp;

    public PlayerData(Vector2 position, float direction, int timestamp)
    {
        this.position = position;
        this.direction = direction;
        this.timestamp = timestamp;
    }
}
