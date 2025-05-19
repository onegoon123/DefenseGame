using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class StageCreator : MonoBehaviour
{

    public StageManager stageManager;

    // ���������� ������ ������Ʈ���� �θ�
    public Transform stageParent;
    // ������ ���������� ������ CSV�� ����
    public TextAsset stageCSVFile;
    // ������ ���������� Ÿ�� ������ CSV�� ����
    public TextAsset tileCSVFile;
    // ���ڿ� �ش��ϴ� ������Ʈ�� ����
    public List<GameObject> prefabList;
    // ���ڿ� �ش��ϴ� ������Ʈ�� ����(üũ����)
    public List<GameObject> prefabList_Check;
    // �ϳ��� ĭ������ ����� ����
    public Vector2 cellSize = new Vector2(10.0f, 10.0f);
    // ĭ������ ������ ����
    public Vector2 cellGap = Vector3.zero;
    // ĭ�� ��ġ�� ����
    public Vector3 cellOffset = Vector3.zero;
    

    public void CreateStage()
    {
        // stageParent�� ������ �ִ� �ڽ� ������Ʈ �����ϴ� ����
        Transform[] childrens = new Transform[stageParent.childCount];
        for (int i = 0 ; i < stageParent.childCount ; i++)
        {
            childrens[i] = stageParent.GetChild(i);
        }

        foreach (var child in childrens)
        {
            DestroyImmediate(child.gameObject);
        }

        // CSV ������ �о� �������� ����
        string[] lines = stageCSVFile.text.Split("\n");
        {
            string[] fields = lines[0].Split(',');
            stageManager.stageSize = new (fields.Length, lines.Length);
        }

        for (int y = 0 ; y < lines.Length ; y++)
        {
            if (string.IsNullOrWhiteSpace(lines[y]))
                continue;

            string[] fields = lines[y].Split(',');

            for (int x = 0 ; x < fields.Length ; x++)
            {
                int value = int.Parse(fields[x]);

                // -1 �� ���� ��ŵ
                if (value == -1) continue;

                CreateInstance(x, y, value);
            }
        }

        CreateTile();
    }

    private void CreateInstance(int x, int y, int value)
    {
        // ������ ������ ���� (üũ���̸� ���� �� üũ)
        GameObject prefab;
        if ((x + y) % 2 == 0 && value < prefabList_Check.Count)
        {
            // üũ����
            prefab = prefabList_Check[value];
        }
        else
        {
            prefab = prefabList[value];
        }

        // �ν��Ͻ� ����
        GameObject newObject = Instantiate(prefab, stageParent);

        // ��ġ ����
        newObject.transform.position += new Vector3(x * cellSize.x, 0, y * cellSize.y) + new Vector3(x * cellGap.x, 0, y * cellGap.y) + cellOffset;
    }

    public void CreateTile()
    {
        // CSV ������ �о� Ÿ�� ����
        string[] lines = tileCSVFile.text.Split("\n");
        //stageManager.tiles = new TileType[lines.Length, lines[0].Split(',').Length];
        for (int y = 0 ; y < lines.Length ; y++)
        {
            if (string.IsNullOrWhiteSpace(lines[y]))
                continue;

            string[] fields = lines[y].Split(',');

            for (int x = 0 ; x < fields.Length ; x++)
            {
                int value = int.Parse(fields[x]);
                stageManager.SetTileType(new(x, y), (TileType)value);
            }
        }
    }
}
