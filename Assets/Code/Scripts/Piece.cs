
// ü���� Ŭ�����Դϴ�

using UnityEditor.SceneTemplate;
using UnityEngine;
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

public class Piece
{
    //private int pieceId = -1;        // id
    //private string pieceName = "";   // �̸�
    //private int level = 1;           // ����
    //private int rank  = 0;           // ��ũ(����)

    /// <summary> ĳ������ �����Դϴ� </summary>
    public PieceStats stats
    {
        get
        {
            return stats;
        }
    }

}
