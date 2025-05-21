using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterList : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> characters = new List<GameObject>(10);

    private void Awake()
    {
        characters.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            characters.Add(transform.GetChild(i).gameObject);
        }
    }

    private void OnEnable()
    {
        for (int i = 0 ; i < transform.childCount ; i++)
        {
            transform.GetChild(i).gameObject.SetActive(DataManager.instance.ContainsPiece(i));
            Image image = transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>();
            image.sprite = DataManager.instance.GetPieceData(i).sprite;
        }
    }

    public void SetCharacterActive(int index, bool active)
    {
        characters[index].SetActive(active);
    }
}
