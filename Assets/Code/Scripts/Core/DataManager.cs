using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEditor.Compilation;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


/// <summary>
/// ���ӿ��� ����� ��� �����Ͱ� ����ִ� Ŭ�����Դϴ�
/// </summary>
[System.Serializable]
public class SaveData
{
    public string playerName = "";          // �÷��̾� �г���
    public int level = 1;                   // �÷��̾� ����
    public int money = 0;                   // ���� ��

    [SerializeField]
    public List<Piece> pieces = new List<Piece>(20);    // ���� ĳ���͵� ������

    [SerializeField]
    public List<int> clearStageList = new List<int>(4); // �������� Ŭ���� ����

    public void ClearData()
    {
        level = 1;
        money = 0;
        pieces.Clear();
        clearStageList.Clear();
        clearStageList.Capacity = 4;
        for(int i = 0; i < clearStageList.Count; i++)
        {
            clearStageList[i] = 0;
        }
    }
}

/// <summary>
/// ���ӿ��� ���� �����͵��� �����ϴ� Ŭ�����Դϴ�
/// </summary>
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
            return;
        }

        // ���� ���۽� ������ �ε�
        LoadGame();
    }

    [SerializeField]
    private SaveData data = new SaveData();                         // ���̺� ������

    [SerializeField]
    private List<PieceData> pieceDatas = new List<PieceData>();     // �����Ǿ� �ִ� Piece ������

    [SerializeField]
    private List<SkillData> skillDatas = new List<SkillData>();     // Skill ������


    /// <summary>
    /// id�� ĳ���͸� �˻��� PieceData�� �޾ƿɴϴ�
    /// </summary>
    public PieceData GetPieceData(int id)
    {
        if (pieceDatas.Count <= id)
        {
            Debug.LogError("pieceDatas ����� �۽��ϴ�.");
            return null;
        }
        return pieceDatas[id];
    }

    /// <summary>
    /// �ش� id�� ���� Ŭ������ �޾ƿɴϴ�
    /// </summary>
    public PieceMainClass GetPieceClass(int id)
    {
        if (pieceDatas.Count <= id)
        {
            Debug.LogError("pieceDatas ����� �۽��ϴ�.");
            return PieceMainClass.None;
        }
        return pieceDatas[id].pieceClass;
    }

    /// <summary>
    /// �ش� id�� ���� Ŭ����(��ȭ ��)�� �޾ƿɴϴ�
    /// </summary>
    public PieceSubClass GetPieceSubClass(int id)
    {
        if (pieceDatas.Count <= id)
        {
            Debug.LogError("pieceDatas ����� �۽��ϴ�.");
            return PieceSubClass.None;
        }
        return pieceDatas[id].pieceSubClass;
    }

    /// <summary>
    /// �ش� id�� ��ų �����͸� �޾ƿɴϴ�
    /// </summary>
    public SkillData GetSkillData(int id)
    {
        if (skillDatas.Count <= id)
        {
            Debug.LogError("skillDatas ����� �۽��ϴ�.");
            return null;
        }
        return skillDatas[id];
    }

    /// <summary>
    /// ���ο� Piece ĳ���͸� ȹ���մϴ�.
    /// </summary>
    public void AddPiece(int id)
    {
        if (ContainsPiece(id))
        {
            return;
        }
        data.pieces.Add(new Piece(id));
    }

    /// <summary>
    /// Piece ĳ���Ϳ� ���� ������ �����մϴ�
    /// �������� �ϸ� �������ּ���
    /// </summary>
    public void SetPiece(Piece piece)
    {
        if (data.pieces.Count <= piece.GetId())
        {
            Debug.LogError("Pieces ����� �۽��ϴ�.");
            return;
        }

        data.pieces[piece.GetId()] = piece;
    }

    public bool ContainsPiece(int id)
    {
        foreach (Piece piece in data.pieces)
        {
            if (piece.GetId() == id)
                return true;
        }

        return false;
    }

    /// <summary>
    /// �ش� id�� ���� Piece�� �޾ƿɴϴ�.
    /// </summary>
    public Piece GetPiece(int id)
    {
        foreach (Piece piece in data.pieces)
        {
            if (piece.GetId() == id)
                return piece;
        }

        return null;
    }

    public List<Piece> GetPieceList()
    {
        return data.pieces;
    }

    public int GetClearStage(int world)
    {
        return data.clearStageList[world];
    }
    public void SetClearStage(int world, int stage)
    {
        data.clearStageList[world] = stage;
    }

    public int GetMoney()
    {
        return data.money;
    }

    public void SetMoney(int money)
    {
        data.money = money;
    }

    /// <summary>
    /// PieceData CSV������ �о� �����͸� �����մϴ�
    /// </summary>
    [ContextMenu("Load Piece Data")]
    public void LoadPieceData()
    {
        pieceDatas.Clear();

        List<PieceData> list = new List<PieceData>();
        TextAsset csvFile = Resources.Load<TextAsset>("Piece/PieceData");
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
                pieceClass = (PieceMainClass)int.Parse(fields[2]),
                pieceSubClass = (PieceSubClass)int.Parse(fields[3]),
                skill_autoAttack = int.Parse(fields[4]),
                skill_passive = int.Parse(fields[5]),
                skill_active = int.Parse(fields[6]),
                icon = Resources.Load<Sprite>(fields[7].Trim())
            };
            piece.pieceStats = GetPieceStatsData(fields[8]);

            pieceDatas.Add(piece);
        }
    }

    /// <summary>
    /// Piece�� ������ ���� CSV������ �о� �����մϴ�
    /// </summary>
    private List<PieceStats> GetPieceStatsData(string file)
    {
        List<PieceStats> stats = new List<PieceStats>(50);
        TextAsset csvFile = Resources.Load<TextAsset>(file.Trim());

        string[] lines = csvFile.text.Split("\n");

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] fields = lines[i].Split(',');

            stats.Add(new PieceStats
            {
                hp = int.Parse(fields[1]),
                atk = int.Parse(fields[2]),
                atkSpeed = float.Parse(fields[3]),
            });
        }

        return stats;
    }

    [ContextMenu("Load Skill Data")]
    public void LoadSkillData()
    {
        skillDatas.Clear();

        List<SkillData> list = new List<SkillData>();
        TextAsset csvFile = Resources.Load<TextAsset>("Skill/SkillData");
        string[] lines = csvFile.text.Split("\n");

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] fields = lines[i].Split(',');

            SkillData skill = new SkillData
            {
                skillId = int.Parse(fields[0]),
                skillName = fields[1],
                range = int.Parse(fields[2]),
                information = fields[3],
                icon = fields[4]
            };

            skillDatas.Add(skill);
        }
    }


    /// <summary>
    /// ���� �����͸� �����մϴ�.
    /// </summary>
    [ContextMenu("Save")]
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }

    /// <summary>
    /// ���� �����͸� �ҷ��ɴϴ�.
    /// </summary>
    [ContextMenu("Load")]
    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/save.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            // �ʱ� ���̺굥���� ����
            data = new SaveData();
            data.ClearData();
        }
    }

    // ����׿�
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddPiece(0);
            AddPiece(1);
            AddPiece(2);
            AddPiece(3);
            AddPiece(4);
            AddPiece(5);
            AddPiece(6);
            AddPiece(7);
            AddPiece(8);
            AddPiece(9);
            AddPiece(10);
            AddPiece(11);
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha2))
        {

        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha3))
        {

        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha4))
        {

        }
    }
#endif
}
