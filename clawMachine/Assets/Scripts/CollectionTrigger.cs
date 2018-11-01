using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionTrigger : MonoBehaviour 
{
    [SerializeField] private ClawMachine machine;
    [SerializeField] private MachineAnimation anim;

    private void OnTriggerEnter(Collider col)
    {
        anim.OpenPrizeGate();
        machine.SetState(ClawMachine.GameState.Collecting);
    }
}
