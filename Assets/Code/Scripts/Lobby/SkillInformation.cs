using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SkillInformation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private int SkillId = -1;

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void SetId(int id)
    {
        SkillId = id;
    }

}
