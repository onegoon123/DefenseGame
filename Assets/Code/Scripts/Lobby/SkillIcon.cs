using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public SkillInformation information;
    public Image iconImage;

    private SkillData skillData;

    public void SetSkillData(SkillData data)
    {
        skillData = data;
        Sprite sprite = Resources.Load<Sprite>(data.icon.Trim());
        iconImage.sprite = sprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        information.ShowInformation(skillData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        information.OffInformation();
    }
}
