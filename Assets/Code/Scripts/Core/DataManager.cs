using System.Collections;
using System.Collections.Generic;
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
    private List<PieceData> pieceDatas = new List<PieceData>();

    [SerializeField]
    private List<Piece> pieces = new List<Piece>();

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

            pieceDatas.Add(piece);
        }
    }
}
