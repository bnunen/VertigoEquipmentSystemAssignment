using System;
using UnityEngine;

public class InteractebleItem : AbstractItem {

    [SerializeField]
    private Animator animator;

    public override void OnClick(HoldLocation location) {
        animator.SetTrigger("Interact");
    }
}