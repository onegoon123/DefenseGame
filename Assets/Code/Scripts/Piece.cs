
// ü���� Ŭ�����Դϴ�
using System;
using UnityEditor.SceneTemplate;
using UnityEngine;

[Serializable]
public struct PieceStats
{
    /// <summary> ĳ������ �ִ� ü���Դϴ� </summary>
    public int hp;
    /// <summary> ĳ������ ���ݷ��Դϴ� </summary>
    public int atk;
    /// <summary> ĳ������ ���ݼӵ��Դϴ� </summary>
    public float atkSpeed;
    /// <summary> ĳ������ ���ݻ�Ÿ��Դϴ� </summary>
    public float atkRange;
}

[Serializable]
public class Piece
{
    [SerializeField]
    private string pieceName = "";    // �̸�
    [SerializeField]
    private int pieceId = -1;            // id
    [SerializeField]
    private int level = 1;              // ����
    //private int rank  = 0;            // ��ũ(����)

    /// <summary> ĳ������ �����Դϴ� </summary>
    public PieceStats stats { get; private set; }

    public Piece(int id)
    {
        pieceId = id;
        level = 1;
        PieceData data = DataManager.instance.GetPieceData(pieceId);
        pieceName = data.pieceName;
        stats = data.pieceStats[level - 1];
    }

    public Piece(int id, int lv)
    {
        pieceId = id;
        level = lv;
        PieceData data = DataManager.instance.GetPieceData(pieceId);
        pieceName = data.pieceName;
        stats = data.pieceStats[level - 1];
    }

    public void StatsSetting()
    {
        PieceData data = DataManager.instance.GetPieceData(pieceId);
        stats = data.pieceStats[level - 1];
    }
    public PieceStats GetStats()
    {
        StatsSetting();
        return stats;
    }

    public string GetName() { return pieceName; }
    public int GetId() { return pieceId; }
    public void SetId(int id) { pieceId = id; }

    public void SetLevel(int lv) { level = lv; }
    public int GetLevel() { return level; }
}
