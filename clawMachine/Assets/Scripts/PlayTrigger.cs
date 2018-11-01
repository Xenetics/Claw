using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTrigger : MonoBehaviour 
{
    [SerializeField] private ClawMachine machine;
    [SerializeField] private MachineAnimation anim;

    private void OnTriggerEnter(Collider col)
    {
        anim.ClosePrizeGate();
        machine.SetState(ClawMachine.GameState.Playing);
    }
}
