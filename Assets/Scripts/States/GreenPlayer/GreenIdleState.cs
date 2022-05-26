using UnityEngine;

public class GreenIdleState : State
{
    GreenPlayer player;

    public GreenIdleState(GreenPlayer player)
    {
        this.player = player;
    }

    public override State Execute()
    {
        if (player.PursuerApproching())
        {
            return new GreenRunningState(player);
        }
        return this;
    }
}
