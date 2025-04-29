using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class StageCreator : MonoBehaviour
{

    // ���������� ������ ������Ʈ���� �θ�
    public Transform stageParent;
    // ������ ���������� ������ CSV�� ����
    public TextAsset csvFile;
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
        string[] lines = csvFile.text.Split("\n");

        for (int z = 0 ; z < lines.Length ; z++)
        {
            if (string.IsNullOrWhiteSpace(lines[z]))
                continue;

            string[] fields = lines[z].Split(',');

            for (int x = 0 ; x < fields.Length ; x++)
            {
                int value = int.Parse(fields[x]);

                // -1 �� ���� ��ŵ
                if (value == -1) continue;

                CreateInstance(x, z, value);
            }
        }
    }

    private void CreateInstance(int x, int z, int value)
    {
        // ������ ������ ���� (üũ���̸� ���� �� üũ)
        GameObject prefab;
        if ((x + z) % 2 == 0 && value < prefabList_Check.Count)
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
        newObject.transform.position += new Vector3(x * cellSize.x, 0, z * cellSize.y) + new Vector3(x * cellGap.x, 0, z * cellGap.y) + cellOffset;
    }

}
