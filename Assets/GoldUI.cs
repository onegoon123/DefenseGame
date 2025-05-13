using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// DataManager������ ��� ��ȭ�� ���� UI�� �ݿ��ϴ� Ŭ�����Դϴ�
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
