using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEditor.Compilation;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


/// <summary>
/// 게임에서 저장될 모든 데이터가 들어있는 클래스입니다
/// </summary>
[System.Serializable]
public class SaveData
{
    public string playerName = "";          // 플레이어 닉네임
    public int level = 1;                   // 플레이어 레벨
    public int money = 0;                   // 보유 돈

    [SerializeField]
    public List<Piece> pieces = new List<Piece>(20);    // 보유 캐릭터들 데이터

    [SerializeField]
    public List<int> clearStageList = new List<int>(4); // 스테이지 클리어 진도

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
/// 게임에서 사용될 데이터들을 관리하는 클래스입니다
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

        // 게임 시작시 데이터 로드
        LoadGame();
    }

    [SerializeField]
    private SaveData data = new SaveData();                         // 세이브 데이터

    [SerializeField]
    private List<PieceData> pieceDatas = new List<PieceData>();     // 고정되어 있는 Piece 데이터

    [SerializeField]
    private List<SkillData> skillDatas = new List<SkillData>();     // Skill 데이터


    /// <summary>
    /// id로 캐릭터를 검색해 PieceData를 받아옵니다
    /// </summary>
    public PieceData GetPieceData(int id)
    {
        if (pieceDatas.Count <= id)
        {
            Debug.LogError("pieceDatas 사이즈가 작습니다.");
            return null;
        }
        return pieceDatas[id];
    }

    /// <summary>
    /// 해당 id의 메인 클래스를 받아옵니다
    /// </summary>
    public PieceMainClass GetPieceClass(int id)
    {
        if (pieceDatas.Count <= id)
        {
            Debug.LogError("pieceDatas 사이즈가 작습니다.");
            return PieceMainClass.None;
        }
        return pieceDatas[id].pieceClass;
    }

    /// <summary>
    /// 해당 id의 서브 클래스(진화 후)를 받아옵니다
    /// </summary>
    public PieceSubClass GetPieceSubClass(int id)
    {
        if (pieceDatas.Count <= id)
        {
            Debug.LogError("pieceDatas 사이즈가 작습니다.");
            return PieceSubClass.None;
        }
        return pieceDatas[id].pieceSubClass;
    }

    /// <summary>
    /// 해당 id의 스킬 데이터를 받아옵니다
    /// </summary>
    public SkillData GetSkillData(int id)
    {
        if (skillDatas.Count <= id)
        {
            Debug.LogError("skillDatas 사이즈가 작습니다.");
            return null;
        }
        return skillDatas[id];
    }

    /// <summary>
    /// 새로운 Piece 캐릭터를 획득합니다.
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
    /// Piece 캐릭터에 대한 정보를 저장합니다
    /// 레벨업을 하면 실행해주세요
    /// </summary>
    public void SetPiece(Piece piece)
    {
        if (data.pieces.Count <= piece.GetId())
        {
            Debug.LogError("Pieces 사이즈가 작습니다.");
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
    /// 해당 id를 가진 Piece를 받아옵니다.
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
    /// PieceData CSV파일을 읽어 데이터를 세팅합니다
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
    /// Piece의 레벨별 스텟 CSV파일을 읽어 세팅합니다
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
    /// 게임 데이터를 저장합니다.
    /// </summary>
    [ContextMenu("Save")]
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }

    /// <summary>
    /// 게임 데이터를 불러옵니다.
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
            // 초기 세이브데이터 세팅
            data = new SaveData();
            data.ClearData();
        }
    }

    // 디버그용
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
