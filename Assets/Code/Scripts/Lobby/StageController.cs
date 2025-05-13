using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    public Transform stages;
    public Image background;
    public List<Sprite> backgroundSprites;

    public void SetLand(int land)
    {
        FindFirstObjectByType<LobbyManager>().SetLand(land);

        background.sprite = backgroundSprites[land];

        int stage = DataManager.instance.GetClearStage(land);
        if (stages.childCount <= stage)
        {
            Debug.Log("Ŭ���� �������� ��Ͽ� ������ �ֽ��ϴ�.");
            return;
        }

        int i;
        for (i = 0; i <= stage; i++)
        {
            stages.GetChild(i).GetComponent<Button>().interactable = true;
        }

        for (i = stage+1; i < stages.childCount; i++ )
        {
            stages.GetChild(i).GetComponent<Button>().interactable = false;
        }

    }
}
