using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// Ÿ�� Ÿ��
public enum TileType
{
    None,
    Walkable,
    Sea,
    Wall,
}

public class StageManager : MonoBehaviour
{

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

    public static StageManager instance { get; private set; }

    public GameObject TestPrefab;
    // �ϳ��� ĭ������ ����� ����
    [SerializeField]
    private Vector2 cellSize = new Vector2(10.0f, 10.0f);
    // ĭ������ ������ ����
    [SerializeField]
    private Vector2 cellGap = Vector3.zero;
    // ĭ�� ��ġ�� ����
    [SerializeField]
    private Vector3 cellOffset = Vector3.zero;

    public PieceUnit[,] units;

    public TileType[,] tiles;

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                int2 gridPos = WorldToGridPosition(hitPoint);
                Debug.Log(gridPos);
                // �ߺ� üũ
                if (occupiedTiles.Contains(gridPos))
                {
                    Debug.Log("�̹� ������ �ִ� Ÿ���Դϴ�.");
                    return;
                }

                // ���� ����
                GameObject newObject = Instantiate(TestPrefab);
                newObject.transform.position = GridToWorldPosition(gridPos) + Vector3.up * 0.5f;

                // ��ǥ ���
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
}