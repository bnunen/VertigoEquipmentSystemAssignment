using System;
using UnityEngine;

public class Rock :HoldibleItem {

    [SerializeField]
    private int force = 20;

    public override void OnInteract() {
        Player.Instance.DropItem(location);
        Rigidbody.AddForce(transform.forward * force, UnityEngine.ForceMode.Impulse);
    }
}