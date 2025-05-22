using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectIcon : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Image durationImage;

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void SetValue(float value)
    {
        durationImage.fillAmount = value;
    }
}
