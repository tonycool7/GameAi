using UnityEngine;

public class PurpleIdleState : State
{
  PurplePlayer player;

  public PurpleIdleState(PurplePlayer player) {
    this.player = player;
  }

  public override State Execute()
  {
        GreenPlayer target = GameManager.Instance().FindClosestTarget(this.player);
        if (target != null)
        {
            return new PurpleChasingState(this.player, target);
        }
        return this;
  }
}
