using System.Collections;
using System.Collections.Generic;

public class BreakState : State
{

  public override State Execute()
  {
    // ...
    return states.Pop();
  }
}
