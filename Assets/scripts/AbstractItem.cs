using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;


public abstract class AbstractItem : MonoBehaviour {
    public abstract void OnClick(HoldLocation location);
}
