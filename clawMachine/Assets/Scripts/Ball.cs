using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour 
{
    public enum eBallType { plain, prize }
    public eBallType ballType;
    public bool grabbed = false;

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
