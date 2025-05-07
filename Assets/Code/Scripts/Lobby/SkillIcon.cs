using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillIcon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public SkillInformation information;
    private SkillData skillData;

    public void SetSkillData(SkillData data)
    {
        skillData = data;
        //skillData.icon;
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
