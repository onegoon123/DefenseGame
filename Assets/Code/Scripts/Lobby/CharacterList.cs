using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterList : MonoBehaviour
{
    void Start()
    {
        var data = DataManager.instance.GetPieceList();
        for (int i = 0 ; i < transform.childCount ; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i < data.Count && data[i] != null);
        }
    }

}
