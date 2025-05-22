using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUIController : MonoBehaviour
{
    [SerializeField]
    private Slider hpBar;
    [SerializeField]
    private RectTransform statusLayout;
    [SerializeField]
    private GameObject statusEffectPrefab;

    public void SetHPPercent(float value)
    {
        hpBar.gameObject.SetActive(true);
        hpBar.value = value;
    }
    public void AddStatusEffect(StatusEffectInstance effect)
    {
        var obj = Instantiate(statusEffectPrefab, statusLayout);
        StatusEffectIcon newIcon = obj.GetComponent<StatusEffectIcon>();
        effect.iconUI = newIcon;
        newIcon.SetIcon(effect.data.icon);
    }
}
