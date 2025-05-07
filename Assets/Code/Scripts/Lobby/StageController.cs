using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    public int worldNum = 0;
    public Transform stages;

    public void SetStage()
    {
        int stage = DataManager.instance.GetClearStage(worldNum);
        if (stages.childCount <= stage)
        {
            Debug.Log("클리어 스테이지 기록에 오류가 있습니다.");
            return;
        }

        int i;
        for (i = 0; i < stage; i++)
        {
            stages.GetChild(i).GetComponent<Button>().interactable = true;
        }

        for (; i < stages.childCount; i++ )
        {
            stages.GetChild(i).GetComponent<Button>().interactable = false;
        }

    }
}
