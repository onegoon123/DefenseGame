
// 체스말 클래스입니다

using UnityEditor.SceneTemplate;
using UnityEngine;
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

public class Piece
{
    //private int pieceId = -1;        // id
    //private string pieceName = "";   // 이름
    //private int level = 1;           // 레벨
    //private int rank  = 0;           // 랭크(돌파)

    /// <summary> 캐릭터의 스텟입니다 </summary>
    public PieceStats stats
    {
        get
        {
            return stats;
        }
    }

}
