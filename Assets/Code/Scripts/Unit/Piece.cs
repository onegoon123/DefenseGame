
// 체스말 클래스입니다
using System;
using UnityEditor.SceneTemplate;
using UnityEngine;

public enum PieceMainClass
{
    None,
    King,
    Queen,
    Rook,
    Bishop,
    Knight,
    Pawn,
}

public enum PieceSubClass
{
    None,
    Amarzon,
    B,
}

[Serializable]
public struct PieceStats
{
    /// <summary> 캐릭터의 최대 체력입니다 </summary>
    public int hp;
    /// <summary> 캐릭터의 공격력입니다 </summary>
    public int atk;
    /// <summary> 캐릭터의 공격속도입니다 </summary>
    public float atkSpeed;
    /// <summary> 캐릭터의 공격사거리입니다 </summary>
    public float atkRange;
}

[Serializable]
public class Piece
{
    [SerializeField]
    private string pieceName = "";    // 이름
    [SerializeField]
    private int pieceId = -1;            // id
    [SerializeField]
    private int level = 1;              // 레벨

    /// <summary> 캐릭터의 스텟입니다 </summary>
    public PieceStats stats { get; private set; }
    public Piece(int id)
    {
        pieceId = id;
        level = 1;
        PieceData data = GetPieceData();
        pieceName = data.pieceName;
        stats = data.pieceStats[level - 1];
    }

    public Piece(int id, int lv)
    {
        pieceId = id;
        level = lv;
        PieceData data = GetPieceData();
        pieceName = data.pieceName;
        stats = data.pieceStats[level - 1];
    }

    public void StatsSetting()
    {
        stats = GetPieceData().pieceStats[level - 1];
    }
    public PieceStats GetStats()
    {
        StatsSetting();
        return stats;
    }

    public void LevelUp(int lv = 1)
    {
        level += lv;
        stats = GetPieceData().pieceStats[level - 1];
    }

    public string GetName() { return pieceName; }
    public int GetId() { return pieceId; }
    public void SetId(int id) { pieceId = id; }

    public void SetLevel(int lv) { level = lv; }
    public int GetLevel() { return level; }

    public PieceData GetPieceData()
    {
        return DataManager.instance.GetPieceData(pieceId);
    }
}
