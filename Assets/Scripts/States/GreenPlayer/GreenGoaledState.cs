using UnityEngine;

public class GreenGoaledState : State
{
    GreenPlayer player;
    public GreenGoaledState(GreenPlayer player)
    {
        this.player = player;
    }

    public override State Execute()
    {
        return this;
    }
}
