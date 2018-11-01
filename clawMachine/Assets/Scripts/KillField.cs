using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillField : MonoBehaviour 
{
    [SerializeField] private bool killAll = false;

    private void OnTriggerEnter(Collider otherCol)
    {
        if (otherCol.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            if (otherCol.GetComponent<Ball>().grabbed || killAll)
            {
                Destroy(otherCol.gameObject);
            }
        }
    }
}
