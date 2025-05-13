using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    public Transform contents;
    public List<Image> images;

    public int gachaMax;

    private Animator animator;

    private void Start()
    {
        gachaMax = DataManager.instance.GetPieceDataList().Count;
        animator = GetComponent<Animator>();
    }

    public void Gacha10()
    {
        for (int i = 0; i < 10; i++)
        {
            // 아직 별도 확률 없이 균등합니다
            int r = Random.Range(0, gachaMax);
            DataManager.instance.AddPiece(r);
            images[i].sprite = DataManager.instance.GetPieceData(r).icon;
        }
        animator.SetTrigger("Gacha10");
    }
}
