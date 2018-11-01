using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAnimation : MonoBehaviour 
{
    [SerializeField] private GameObject vents;
    private Animator[] ventAnims;

    [SerializeField] private Animator lidAnim;

    [SerializeField] private Animator prizeGate;

    [SerializeField] private Animator leftSpawn;
    [SerializeField] private Animator rightSpawn;


	private void Awake() 
	{
        if (vents)
        {
            ventAnims = vents.GetComponentsInChildren<Animator>();
        }
    }
	
	private void Update () 
	{
		
	}

    public void OpenVents()
    {
        foreach(Animator a in ventAnims)
        {
            a.SetBool("open", true);
        }
    }

    public void CloseVents()
    {
        foreach (Animator a in ventAnims)
        {
            a.SetBool("open", false);
        }
    }

    public void OpenLid()
    {
        lidAnim.SetBool("open", true);
    }

    public void CloseLid()
    {
        lidAnim.SetBool("open", false);
    }

    public void OpenPrizeGate()
    {
        prizeGate.SetBool("open", true);
    }

    public void ClosePrizeGate()
    {
        prizeGate.SetBool("open", false);
    }

    public void OpenLeftSpawn()
    {
        leftSpawn.SetBool("open", true);
    }

    public void CloseLeftSpawn()
    {
        leftSpawn.SetBool("open", false);
    }

    public void OpenRightSpawn()
    {
        rightSpawn.SetBool("open", true);
    }

    public void CloseRightSpawn()
    {
        rightSpawn.SetBool("open", false);
    }
}
