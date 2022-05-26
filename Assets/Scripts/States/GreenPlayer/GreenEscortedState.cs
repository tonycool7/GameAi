using UnityEngine;

public class GreenEscortedState : State
{
    GreenPlayer player;
    public GreenEscortedState(GreenPlayer player)
    {
        this.player = player;
    }

    public override State Execute()
    {
        if (player.MovedToGaol()) return new GreenGoaledState(player);
        return this;
    }
}
