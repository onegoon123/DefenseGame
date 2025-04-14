
// ü���� Ŭ�����Դϴ�

using UnityEditor.SceneTemplate;
using UnityEngine;

public class Piece
{

    private int level;  // ����
    private int rank;   // ��ũ(����)


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

    /// <summary> ĳ������ �����Դϴ� </summary>
    public PieceStats stats
    {
        get
        {
            return stats;
        }
    }

}
