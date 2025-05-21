using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// Ÿ�� Ÿ��
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

    // �ϳ��� ĭ������ ����� ����
    [SerializeField]
    private Vector2 cellSize = new Vector2(10.0f, 10.0f);
    // ĭ������ ������ ����
    [SerializeField]
    private Vector2 cellGap = Vector3.zero;
    // ĭ�� ��ġ�� ����
    [SerializeField]
    private Vector3 cellOffset = Vector3.zero;

    public int2 stageSize;

    [SerializeField]
    private PieceUnit[] units = new PieceUnit[10];

    [SerializeField]
    private TileType[] tiles = new TileType[10];

    [SerializeField]
    private GameObject[] unitPrefabs = new GameObject[10];

    // ������ ��� �浹ü
    private Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

    // �̹� ������ ��ġ�� ��ġ �����
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
        Debug.Log(findUnit.gameObject.name + " �̹� �־��");
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
        // �����ִ� Ÿ�� ��ǥ �����ؾ���
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