using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightManager : MonoBehaviour
{
    public Color EnablePink;

    [Space]
    [SerializeField] private Animator animator;
    [SerializeField] Light mainSpotLight;
    [SerializeField] Spotlight interiorLight;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MainSpotLight();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EnableInteriorLight();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EnableStrobeInteriorLight(true, 1f);
        }
    }

    public void MainSpotLight()
    {
        animator.SetTrigger("mainSpotlight");
    }

    public void EnableInteriorLight()
    {
        interiorLight.gameObject.SetActive(true);
    }

    public void EnableStrobeInteriorLight(bool isOn, float f)
    {
        interiorLight.changeColors = isOn;
        interiorLight.colorChangeDuration = f;
    }
}
