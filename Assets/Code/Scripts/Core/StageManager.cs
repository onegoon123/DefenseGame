using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StageManager : MonoBehaviour
{
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

    // ������ ��� �浹ü
    private Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

    void Awake()
    {
        if (instance != null && instance != this)
        {
            // ���� �ν��Ͻ� ����
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
                GameObject newObject = Instantiate(TestPrefab);
                newObject.transform.position += GridToWorldPosition(gridPos);
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
        Vector3 worldPos = new Vector3(gridPos.x * cellSize.x, 0, gridPos.y * cellSize.y) + new Vector3(gridPos.x * cellGap.x, 0, gridPos.y * cellGap.y);
        return worldPos;
    }
}
