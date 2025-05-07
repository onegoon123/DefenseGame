using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

// 스킬 데이터
[Serializable]
public class SkillData
{
    public int skillId = -1;
    public string skillName = "";
    public int range = 0;
    public string information = "";
    public string icon = "";
}

public class SkillInformation : MonoBehaviour
{
    public SkillIcon[] skillIcons = new SkillIcon[4];

    public TMP_Text nameText;
    public TMP_Text infoText;

    private Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    public void SetPieceId(int id)
    {
        PieceData pieceData = DataManager.instance.GetPieceData(id);

        SetSkillId(pieceData.skill_0, 0);
        SetSkillId(pieceData.skill_1, 1);
        SetSkillId(pieceData.skill_2, 2);
        SetSkillId(pieceData.skill_3, 3);
    }

    private void SetSkillId(int id, int index)
    {
        SkillData skill = DataManager.instance.GetSkillData(id);
        skillIcons[index].SetSkillData(skill);
    }

    public void ShowInformation(SkillData skill)
    {
        nameText.text = skill.skillName;
        infoText.text = skill.information;
        anim.Play("Alpha0to1");
    }

    public void OffInformation()
    {
        anim.Play("Alpha1to0");
    }
}
