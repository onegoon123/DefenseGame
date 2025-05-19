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

    // 스테이지를 생성할 오브젝트들의 부모
    public Transform stageParent;
    // 생성할 스테이지의 데이터 CSV를 지정
    public TextAsset stageCSVFile;
    // 생성할 스테이지의 타일 데이터 CSV를 지정
    public TextAsset tileCSVFile;
    // 숫자에 해당하는 오브젝트를 지정
    public List<GameObject> prefabList;
    // 숫자에 해당하는 오브젝트를 지정(체크무늬)
    public List<GameObject> prefabList_Check;
    // 하나의 칸마다의 사이즈를 지정
    public Vector2 cellSize = new Vector2(10.0f, 10.0f);
    // 칸마다의 간격을 지정
    public Vector2 cellGap = Vector3.zero;
    // 칸의 위치를 조정
    public Vector3 cellOffset = Vector3.zero;
    

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

        // CSV 파일을 읽어 스테이지 생성
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

                // -1 인 경우는 스킵
                if (value == -1) continue;

                CreateInstance(x, y, value);
            }
        }

        CreateTile();
    }

    private void CreateInstance(int x, int y, int value)
    {
        // 생성할 프리팹 지정 (체크무늬를 만들 지 체크)
        GameObject prefab;
        if ((x + y) % 2 == 0 && value < prefabList_Check.Count)
        {
            // 체크무늬
            prefab = prefabList_Check[value];
        }
        else
        {
            prefab = prefabList[value];
        }

        // 인스턴스 생성
        GameObject newObject = Instantiate(prefab, stageParent);

        // 위치 조정
        newObject.transform.position += new Vector3(x * cellSize.x, 0, y * cellSize.y) + new Vector3(x * cellGap.x, 0, y * cellGap.y) + cellOffset;
    }

    public void CreateTile()
    {
        // CSV 파일을 읽어 타일 생성
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
