using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 체스말 캐릭터들의 레벨별 스텟 등 데이터를 받아오는 클래스입니다
[Serializable]
public class PieceData
{
    public int pieceId = -1;
    public string pieceName = "";
    public int pieceClass  = 0;
    public int skill_0 = 0;
    public int skill_1 = 0;
    public int skill_2 = 0;
    public int skill_3 = 0;

    public List<PieceStats> pieceStats = new List<PieceStats>();
}
