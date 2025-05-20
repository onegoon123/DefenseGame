using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePocket : MonoBehaviour
{
    public PocketItem[] pocketItems = new PocketItem[5];

    // ���� �ǽ��� �������� ����
    public void CreatePiece()
    {
        foreach (var item in pocketItems)
        {
            if (item.GetPiece() == null)
            {
                int rand = Random.Range(0, GameManager.instance.pieces.Count);
                item.SetPiece(GameManager.instance.pieces[rand]);
                return;
            }
        }
    }
}
