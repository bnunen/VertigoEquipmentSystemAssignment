using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ammoCountText;
    [SerializeField]
    private Animation pointerAnimation;
    [SerializeField]
    private Image pointerSprite;

    public int Ammo { set { ammoCountText.text = value.ToString(); } }

    public void HoverItem() {
        pointerAnimation.Play();
        pointerSprite.color = Color.white;
    }

    public void StopHoverItem() {
        pointerAnimation.Stop();
        pointerSprite.color = new Color(1, 1, 1, 0.5f);
    }
}
