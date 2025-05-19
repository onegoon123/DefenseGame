using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// 타일 타입
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

    public GameObject TestPrefab1;
    public GameObject TestPrefab2;
    // 하나의 칸마다의 사이즈를 지정
    [SerializeField]
    private Vector2 cellSize = new Vector2(10.0f, 10.0f);
    // 칸마다의 간격을 지정
    [SerializeField]
    private Vector2 cellGap = Vector3.zero;
    // 칸의 위치를 조정
    [SerializeField]
    private Vector3 cellOffset = Vector3.zero;

    private PieceUnit[,] units = new PieceUnit[11,7];

    public TileType[,] tiles = new TileType[11, 7];

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                int2 gridPos = WorldToGridPosition(hitPoint);
                Debug.Log(gridPos);
                // 중복 체크
                if (occupiedTiles.Contains(gridPos))
                {
                    Debug.Log("이미 유닛이 있는 타일입니다.");
                    return;
                }

                // 유닛 생성
                GameObject newObject = Instantiate(TestPrefab1);
                newObject.transform.position = GridToWorldPosition(gridPos) + Vector3.up * 0.5f;

                // 좌표 기록
                occupiedTiles.Add(gridPos);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                int2 gridPos = WorldToGridPosition(hitPoint);
                Debug.Log(gridPos);
                // 중복 체크
                if (occupiedTiles.Contains(gridPos))
                {
                    Debug.Log("이미 유닛이 있는 타일입니다.");
                    return;
                }

                // 유닛 생성
                GameObject newObject = Instantiate(TestPrefab2);
                newObject.transform.position = GridToWorldPosition(gridPos) + Vector3.up * 0.5f;

                // 좌표 기록
                occupiedTiles.Add(gridPos);
            }
        }
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

    public PieceUnit GetUnit(int2 pos)
    {
        return units[pos.x, pos.y];
    }

    public void SetUnit(PieceUnit unit)
    {
        SetUnit(unit.gridPos, unit);
    }
    public void SetUnit(int2 pos, PieceUnit unit)
    {
        PieceUnit findUnit = units[pos.x, pos.y];
        if (findUnit == null)
        {
            units[pos.x, pos.y] = unit;
            return;
        }
        Debug.Log(findUnit.gameObject.name + " 이미 있어요");
    }
    public void ClearUnit(int2 pos)
    {
        units[pos.x, pos.y] = null;
    }

    public bool IsValidTile(int2 pos)
    {
        return pos.x >= 0 && pos.y >= 0
            && pos.x < 11
            && pos.y < 7;

        /*
        return pos.x >= 0 && pos.y >= 0
            && pos.x < units.GetLength(0)
            && pos.y < units.GetLength(1);*/
    }

    public TileType GetTileType(int2 pos)
    {
        if (11 <= pos.x || 7 <= pos.y || pos.x < 0 || pos.y < 0)
        {
            Debug.Log(pos);
            Debug.Log("얘 뭐임");
            return TileType.None;
        }
        return tiles[pos.x, pos.y];
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
}