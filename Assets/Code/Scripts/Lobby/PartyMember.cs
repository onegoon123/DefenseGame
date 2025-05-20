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
        // 멤버로 세팅된 id가 들어옵니다
        pieceId = id;
        image.sprite = DataManager.instance.GetPieceData(id).sprite;
    }

    public void OnClick()
    {
        // 버튼을 눌렀을 때 실행.
        // 멤버에서 뺍니다.

        // 아직 멤버 세팅이 안된 경우
        if (pieceId == -1)
            return;

        partyManager.RemoveMember(pieceId);
    }
}
