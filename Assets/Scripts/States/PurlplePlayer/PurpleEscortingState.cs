using UnityEngine;

public class PurpleEscortingState : State
{
  GreenPlayer target;
  PurplePlayer player;

  public PurpleEscortingState(PurplePlayer player, GreenPlayer target)
  {
    this.player = player;
    this.target = target;
  }

  public override State Execute()
  {
    if (player.TransportToGaol())
    {
        return new PurpleIdleState(player);
    }
    return this;
  }
}
