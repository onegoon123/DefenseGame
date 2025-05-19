using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// DataManager에서의 골드 변화에 따라 UI에 반영하는 클래스입니다
/// </summary>
public class GoldUI : MonoBehaviour
{
    public TMP_Text text;

    private void Start()
    {
        DataManager.instance.OnGoldChanged += UpdateGoldUI;

        if (DataManager.instance)
            UpdateGoldUI(DataManager.instance.GetGold());
    }

    private void OnEnable()
    {
        if (DataManager.instance)
            UpdateGoldUI(DataManager.instance.GetGold());
    }

    private void UpdateGoldUI(int gold)
    {
        text.text = gold.ToString();
    }
}
