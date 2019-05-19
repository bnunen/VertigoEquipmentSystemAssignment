using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]
    private HoldibleItem itemLeft, itemRight;
    [SerializeField]
    private int maxPickUpDistance = 5;
    [SerializeField]
    private float animateToLocationTime = 0.5f;
    private Hat itemHat;
    private bool gunFireModeAuto = true;
    private int ammo = 30;
    [SerializeField]
    private Transform itemLeftLocation, itemRightLocation, hatLocation, ammoLocation, camera;
    [SerializeField]
    private UIController uiController;

    private static Player instance;

    public static Player Instance {
        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<Player>();
            }
            if (instance == null) {
                Debug.LogError("Failing completely to get a singleton reference from the Player component");
            }
            return instance;
        }
    }

    public bool GunFireModeAuto { get => gunFireModeAuto; }
    public int MaxPickUpDistance { get => maxPickUpDistance; }
    public int Ammo { get => ammo; set { ammo = value; uiController.Ammo = ammo; } }

    public void Awake() {
        uiController.Ammo = ammo;
    }

    public void PickUpItem(HoldibleItem item, HoldLocation location) {
        item.Rigidbody.detectCollisions = false;
        if (item.GetType() == typeof(Ammo)) {
            Ammo ammoClip = (Ammo)item;
            Ammo += ammoClip.size;
            StartCoroutine(AnimateToLocation(item, ammoLocation));
            Destroy(item.gameObject, animateToLocationTime + 0.1f);
        }
        else {
            if (item.GetType() == typeof(Hat)) {
                DropItem(HoldLocation.Head);
                itemHat = (Hat)item;
                StartCoroutine(AnimateToLocation(item, hatLocation));
            }
            else {
                DropItem(location);
                if (location == HoldLocation.Left) {
                    itemLeft = item;
                    StartCoroutine(AnimateToLocation(item, itemLeftLocation));
                }
                else if (location == HoldLocation.Right) {
                    itemRight = item;
                    StartCoroutine(AnimateToLocation(item, itemRightLocation));
                }
            }
        }
    }

    public IEnumerator AnimateToLocation(HoldibleItem item, Transform location) {
        item.Rigidbody.isKinematic = true;
        item.Collider.enabled = false;
        item.transform.parent = location;

        float startTime = Time.time;
        Vector3 startPosition = item.transform.position;
        Quaternion startRotation = item.transform.rotation;

        while (Time.time < startTime + animateToLocationTime) {
            item.transform.position = Vector3.Lerp(startPosition, location.position, (Time.time - startTime) / animateToLocationTime);
            item.transform.rotation = Quaternion.Lerp(startRotation, location.rotation, (Time.time - startTime) / animateToLocationTime);
            yield return null;
        }
        item.transform.position = location.position;
        item.transform.rotation = location.rotation;
    }

    public void DropItem(HoldLocation location) {
        HoldibleItem item = null;
        if (location == HoldLocation.Left) {
            item = itemLeft;
            itemLeft = null;
        }
        else if (location == HoldLocation.Right) {
            item = itemRight;
            itemRight = null;
        }
        else if (location == HoldLocation.Head) {
            item = itemHat;
            itemHat = null;
        }
        if (item != null) {
            item.Rigidbody.isKinematic = false;
            item.Rigidbody.detectCollisions = true;
            item.transform.SetParent(null);
            item.Collider.enabled = true;
            //item.Rigidbody.AddForce((transform.forward+transform.up)*10);
        }

    }

    public void Update() {
        Ray ray = new Ray(camera.position, camera.transform.TransformDirection(Vector3.forward));
        Debug.DrawRay(camera.position, camera.transform.TransformDirection(Vector3.forward) * maxPickUpDistance, Color.green);
        if (Physics.Raycast(ray, out RaycastHit hit, maxPickUpDistance)) {//Layer 8 is Item
            AbstractItem item = hit.transform.GetComponent<AbstractItem>();
            if (item != null) {
                HoverItem(item);
            }
            else {
                nonHover();
            }
        }
        else {
            nonHover();
        }

        if (Input.GetButtonDown("ToggleFireMode")) {
            gunFireModeAuto = !gunFireModeAuto;
        }

        if (Input.GetKey("escape")) {
            Application.Quit();
        }
    }

    public void HoverItem(AbstractItem item) {
        uiController.HoverItem();
        if (Input.GetButtonDown("LeftHand")) {
            item.OnClick(HoldLocation.Left);
            Debug.Log(item.name);
        }

        if (Input.GetButtonDown("RightHand")) {
            item.OnClick(HoldLocation.Right);
            Debug.Log(item.name);
        };
    }

    public void nonHover() {
        uiController.StopHoverItem();
        if (itemLeft != null) {
            if (Input.GetButtonDown("LeftHand")) {
                itemLeft.OnInteract();
            }
            if (Input.GetButtonUp("LeftHand")) {
                itemLeft.OnStopInteract();
            }
        }
        if (itemRight != null) {
            if (Input.GetButtonDown("RightHand")) {
                itemRight.OnInteract();
            }
            if (Input.GetButtonUp("RightHand")) {
                itemRight.OnStopInteract();
            }
        }
    }
}