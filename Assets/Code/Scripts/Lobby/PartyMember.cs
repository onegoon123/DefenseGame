using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMember : MonoBehaviour
{
    [SerializeField]
    private PartyManager partyManager;

    public PieceMainClass pieceClass;
    public Image image;
    private int pieceId = -1;

    public void ClearId()
    {
        pieceId = -1;
        image.sprite = null;
    }
    public int GetId()
    {
        return pieceId;
    }
    public void SetId(int id)
    {
        // ����� ���õ� id�� ���ɴϴ�
        pieceId = id;
        image.sprite = DataManager.instance.GetPieceData(id).sprite;
    }

    public void OnClick()
    {
        // ��ư�� ������ �� ����.
        // ������� ���ϴ�.

        // ���� ��� ������ �ȵ� ���
        if (pieceId == -1)
            return;

        partyManager.RemoveMember(pieceId);
    }
}
