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
    public int pieceClass  = 0;
    public int skill_0 = 0;
    public int skill_1 = 0;
    public int skill_2 = 0;
    public int skill_3 = 0;

    public List<PieceStats> pieceStats = new List<PieceStats>();
}
