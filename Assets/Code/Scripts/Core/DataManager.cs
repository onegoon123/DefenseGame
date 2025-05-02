using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;


public class DataManager : MonoBehaviour
{
    public static DataManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    [SerializeField]
    private List<PieceData> pieceDatas = new List<PieceData>();     // 고정되어 있는 Piece 데이터

    [SerializeField]
    private List<Piece> pieces = new List<Piece>(20);               // 플레이어의 Piece 데이터

    [ContextMenu("Load Piece Data")]
    public void LoadPieceData()
    {
        pieceDatas.Clear();

        List<PieceData> list = new List<PieceData>();
        TextAsset csvFile = Resources.Load<TextAsset>("PieceData");
        string[] lines = csvFile.text.Split("\n");

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] fields = lines[i].Split(',');

            PieceData piece = new PieceData
            {
                pieceId = int.Parse(fields[0]),
                pieceName = fields[1],
                rank = int.Parse(fields[2]),
                Type = int.Parse(fields[3])
            };
            piece.pieceStats = GetPieceStatsData(fields[4]);

            pieceDatas.Add(piece);
        }
    }

    private List<PieceStats> GetPieceStatsData(string file)
    {
        List<PieceStats> stats = new List<PieceStats>(50);
        TextAsset csvFile = Resources.Load<TextAsset>(file.Trim());

        string[] lines = csvFile.text.Split("\n");

        for(int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] fields = lines[i].Split(',');

            stats.Add(new PieceStats
            {
                hp = int.Parse(fields[0]),
                atk = int.Parse(fields[1]),
                atkSpeed = float.Parse(fields[2]),
                atkRange = float.Parse(fields[3]),
            });
        }

        return stats;
    }

    public PieceData GetPieceData(int id)
    {
        if (pieceDatas.Count <= id)
        {
            Debug.LogError("pieceDatas 사이즈가 작습니다.");
            return null;
        }
        return pieceDatas[id];
    }

    public void SetPiece(Piece piece)
    {
        if (pieces.Count <= piece.GetId())
        {
            Debug.LogError("Pieces 사이즈가 작습니다.");
            return;
        }

        pieces[piece.GetId()] = piece;
    }

    public Piece GetPiece(int id)
    {
        if (pieces.Count <= id)
        {
            Debug.LogError("Pieces 사이즈가 작습니다.");
            return null;
        }
        if (pieces[id] == null)
        {
            Debug.LogError("존재하지 않는 Piece ID 입니다");
            return null;
        }

        return pieces[id];
    }


}
