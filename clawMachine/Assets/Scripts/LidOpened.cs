using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidOpened : MonoBehaviour 
{
    private ClawMachine machine;

    private void Awake()
    {
        machine = GetComponentInParent<ClawMachine>();
    }

    public void OnLidOpenComplete()
    {
        machine.SetState(ClawMachine.GameState.Spawning);
    }
}
