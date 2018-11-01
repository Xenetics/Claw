using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSmoke : MonoBehaviour 
{
    private ParticleSystem particle;

	private void Awake() 
	{
        particle = GetComponentInChildren<ParticleSystem>();
    }

    public void SmokeOn()
    {
        if (particle)
        {
            particle.Play();
        }
    }

    public void SmokeReset()
    {
        if (particle)
        {
            particle.Clear();
        }
    }
}
