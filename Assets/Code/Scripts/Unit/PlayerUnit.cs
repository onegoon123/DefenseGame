using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : PieceUnit
{
    // �ʱ� ���� ����
    public void SetPiece(Piece piece)
    {
        PieceStats stats =  piece.GetStats();
        maxHP = stats.hp;
        atk = stats.atk;
    }
}
