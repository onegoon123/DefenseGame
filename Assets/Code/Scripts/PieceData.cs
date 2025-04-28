using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 체스말 캐릭터들의 레벨별 스텟 등 데이터를 받아오는 클래스입니다
[System.Serializable]
public class PieceData
{
    public int pieceId = -1;
    public string pieceName = "";
    public int rank  = 0;
    public int Type = 0;

    //List<PieceStats> pieceStats = new List<PieceStats>();
}
