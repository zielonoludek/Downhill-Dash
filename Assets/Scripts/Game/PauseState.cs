using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : BaseGameState
{
    public override void EnterState()
    {
        //Time.timeScale = 0;
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        //Time.timeScale = 1;
    }
}
