using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ü���� ĳ���͵��� ������ ���� �� �����͸� �޾ƿ��� Ŭ�����Դϴ�
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
