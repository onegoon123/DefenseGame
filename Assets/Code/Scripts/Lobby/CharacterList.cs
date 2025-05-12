using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterList : MonoBehaviour
{
    private void OnEnable()
    {
        for (int i = 0 ; i < transform.childCount ; i++)
        {
            transform.GetChild(i).gameObject.SetActive(DataManager.instance.ContainsPiece(i));
            Image image = transform.GetChild(i).GetChild(0).GetComponent<Image>();
            image.sprite = DataManager.instance.GetPieceData(i).icon;
        }
    }
}
