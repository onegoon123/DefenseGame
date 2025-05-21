using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// 타일 타입
[Serializable]
public enum TileType
{
    None,
    Walkable,
    Sea,
    Wall,
}

public class StageManager : MonoBehaviour
{
    public static StageManager instance { get; private set; }

    public int landNum;
    public int stageNum;

    public PlayerUnit king { get; private set; }
    public GameObject kingObject;
    public int2 kingPos;

    // 하나의 칸마다의 사이즈를 지정
    [SerializeField]
    private Vector2 cellSize = new Vector2(10.0f, 10.0f);
    // 칸마다의 간격을 지정
    [SerializeField]
    private Vector2 cellGap = Vector3.zero;
    // 칸의 위치를 조정
    [SerializeField]
    private Vector3 cellOffset = Vector3.zero;

    public int2 stageSize;

    [SerializeField]
    private PieceUnit[] units = new PieceUnit[10];

    [SerializeField]
    private TileType[] tiles = new TileType[10];

    [SerializeField]
    private GameObject[] unitPrefabs = new GameObject[10];

    // 가상의 평면 충돌체
    private Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

    // 이미 유닛이 설치된 위치 저장용
    private HashSet<int2> occupiedTiles = new HashSet<int2>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;
    }

    private void Start()
    {
        GameObject obj = Instantiate(kingObject);
        king = obj.GetComponent<PlayerUnit>();
        king.Setting(kingPos);
    }

    public int2 WorldToGridPosition(Vector3 worldPos)
    {
        int GridX = Mathf.FloorToInt((worldPos.x + cellOffset.x) / cellSize.x);
        int GridY = Mathf.FloorToInt((worldPos.z + cellOffset.z) / cellSize.y);
        return new int2(GridX, GridY);
    }

    public Vector3 GridToWorldPosition(int2 gridPos)
    {
        Vector3 worldPos = new Vector3(gridPos.x * cellSize.x, 0, gridPos.y * cellSize.y)
                         + new Vector3(gridPos.x * cellGap.x, 0, gridPos.y * cellGap.y);
        return worldPos;
    }

    public void ClearStage()
    {
        if (DataManager.instance.GetClearStage(landNum) < stageNum)
        {
            DataManager.instance.SetClearStage(landNum, stageNum);
        }

        SceneLoader.instance.LoadScene("LobbyScene");
    }
    
    public void SpawnUnit(Piece piece, int2 pos)
    {
        int id = piece.GetId();

        if (unitPrefabs.Length < id) return;

        GameObject obj = Instantiate(unitPrefabs[id]);
        PlayerUnit unit = obj.GetComponent<PlayerUnit>();
        unit.SetPiece(piece);
        unit.Setting(pos);
    }

    private int GetStageIndex(int2 pos) { return pos.y * stageSize.x + pos.x; }
    public PieceUnit GetUnit(int2 pos)
    {
        return units[GetStageIndex(pos)];
    }

    public void SetUnit(PieceUnit unit)
    {
        SetUnit(unit.gridPos, unit);
    }
    public void SetUnit(int2 pos, PieceUnit unit)
    {
        PieceUnit findUnit = units[GetStageIndex(pos)];
        if (findUnit == null)
        {
            units[GetStageIndex(pos)] = unit;
            return;
        }
        Debug.Log(findUnit.gameObject.name + " 이미 있어요");
    }
    public void ClearUnit(int2 pos)
    {
        units[GetStageIndex(pos)] = null;
    }

    public bool IsValidTile(int2 pos)
    {
        return pos.x >= 0 && pos.y >= 0
            && pos.x < stageSize.x
            && pos.y < stageSize.y;
    }

    public TileType GetTileType(int2 pos)
    {
        return tiles[GetStageIndex(pos)];
    }
    public void SetTileType(int2 pos, TileType type)
    {
        tiles[GetStageIndex(pos)] = type;
    }
    public List<int2> GetBlockedTiles()
    {
        // 막혀있는 타일 좌표 삽입해야함
        return new List<int2>
        {
            new int2(3, 2),
            new int2(4, 2),
            new int2(5, 2)
        };
    }

    public int2 GetMouseGridPos()
    {
        int2 result = new int2();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            result = WorldToGridPosition(hitPoint);
        }

        return result;
    }
}