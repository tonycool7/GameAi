using UnityEngine;

public class PurpleChasingState : State
{
  private GreenPlayer target = null;
  private PurplePlayer player;

  public PurpleChasingState(PurplePlayer player, GreenPlayer target)
  {
        this.player = player;
        this.target = target;
  }

  public override State Execute()
  {
        if (player.Chase(target)) return new PurpleEscortingState(player, target);
        else return this;
  }

}
