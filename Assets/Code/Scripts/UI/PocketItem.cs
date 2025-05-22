using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PocketItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameObject previewPrefab;

    private Piece piece;
    private Image image;
    private GameObject previewObject;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public Piece GetPiece()
    {
        return piece;
    }
    public void SetPiece(Piece piece)
    {
        this.piece = piece;
        PieceData data =  piece.GetPieceData();
        image.sprite = data.sprite;
        image.color = Color.white;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("�巡�� ����");
        previewObject = Instantiate(previewPrefab);
        image.color = new Color(1, 1, 1, .5f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        int2 gridPos = StageManager.instance.GetMouseGridPos();
        previewObject.transform.position = StageManager.instance.GridToWorldPosition(gridPos);

        if (StageManager.instance.IsValidTile(gridPos))
        {
            // �ȱ� ���� Ÿ�ϸ� ��ġ ���� (�ӽ�)
            if (StageManager.instance.GetPlayer(gridPos) == null && StageManager.instance.GetTileType(gridPos)==TileType.Ground)
            {
                previewObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 8f);
                return;
            }
        }
        
        previewObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, .2f, .2f, .8f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.color = Color.white;
        Destroy(previewObject);

        int2 gridPos = StageManager.instance.GetMouseGridPos();

        if (StageManager.instance.IsValidTile(gridPos) == false) return;
        if (StageManager.instance.GetPlayer(gridPos) != null) return;
        // �ϴ� �ӽ÷� ���డ�� Ÿ�Ͽ��� ����
        if (StageManager.instance.GetTileType(gridPos) != TileType.Ground) return;

        StageManager.instance.SpawnUnit(piece, gridPos);
        piece = null;
        image.color = Color.clear;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Ŭ�� ����");
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Ŭ�� ����");
    }

}
