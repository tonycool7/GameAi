using UnityEngine;

public class GreenRunningState : State
{
    GreenPlayer player;
    public GreenRunningState(GreenPlayer player)
    {
        this.player = player;
    }

    public override State Execute()
    {
        if (player.HasBeenCaught()) return new GreenEscortedState(player);

        return this;
    }
}
