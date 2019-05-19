using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class HoldibleItem : AbstractItem {

    private Rigidbody rigidbody;
    private Collider collider;
    protected HoldLocation location;

    public Rigidbody Rigidbody { get => rigidbody; }
    public Collider Collider { get => collider; }

    public void Awake() {
        rigidbody = this.GetComponent<Rigidbody>();
        collider = this.GetComponent<Collider>();
    }

    public virtual void OnInteract() {

    }

    public virtual void OnStopInteract() {

    }

    public override void OnClick(HoldLocation location) {
        this.location = location;
        Player.Instance.PickUpItem(this, location);
    }
}