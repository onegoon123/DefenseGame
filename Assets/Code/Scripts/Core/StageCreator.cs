using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageCreator : MonoBehaviour
{
    public Transform stageParent;
    public TextAsset csvFile;
    public List<GameObject> prefabList;
    public Vector2 cellSize = new Vector2(10.0f, 10.0f);
    public Vector2 cellGap = new Vector2(0.0f, 0.0f);

    /// <summary>
    /// csvFile
    /// </summary>
    public void CreateStage()
    {
        // stageParent가 가지고 있는 자식 오브젝트 삭제하는 과정
        Transform[] childrens = new Transform[stageParent.childCount];
        for (int i = 0 ; i < stageParent.childCount ; i++)
        {
            childrens[i] = stageParent.GetChild(i);
        }

        foreach (var child in childrens)
        {
            DestroyImmediate(child.gameObject);
        }

        // CSV 파일을 읽어 프리팹생성
        string[] lines = csvFile.text.Split("\n");

        for (int z = 0 ; z < lines.Length ; z++)
        {
            if (string.IsNullOrWhiteSpace(lines[z]))
                continue;

            string[] fields = lines[z].Split(',');

            for (int x = 0 ; x < fields.Length ; x++)
            {
                int value = int.Parse(fields[x]);

                if (value == -1) continue;

                GameObject newObject = Instantiate(prefabList[value], stageParent);
                newObject.transform.position += new Vector3(x * cellSize.x, 0, z * cellSize.y) + new Vector3(x * cellGap.x, 0, z * cellGap.y);
            }
        }
    }


}
