using System;
using UnityEngine;

public class Flashlight : HoldibleItem{

    [SerializeField]
    private Light light;

    public override void OnInteract() {
        light.enabled = !light.enabled;
    }
}