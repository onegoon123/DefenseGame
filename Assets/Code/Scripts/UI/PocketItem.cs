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
        Debug.Log("드래그 시작");
        previewObject = Instantiate(previewPrefab);
        image.color = new Color(1, 1, 1, .5f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        int2 gridPos = StageManager.instance.GetMouseGridPos();
        previewObject.transform.position = StageManager.instance.GridToWorldPosition(gridPos);

        if (StageManager.instance.IsValidTile(gridPos))
        {
            // 걷기 가능 타일만 배치 가능 (임시)
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
        // 일단 임시로 보행가능 타일에만 한정
        if (StageManager.instance.GetTileType(gridPos) != TileType.Ground) return;

        StageManager.instance.SpawnUnit(piece, gridPos);
        piece = null;
        image.color = Color.clear;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("클릭 시작");
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("클릭 해제");
    }

}
