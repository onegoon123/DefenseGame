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
    public PieceMainClass pieceClass  = 0;
    public PieceSubClass pieceSubClass  = 0;
    public int autoAttack = 0;
    public int skill = 0;
    public Sprite icon;

    public List<PieceStats> pieceStats = new List<PieceStats>();
}
