using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PieceUnit : MonoBehaviour
{
    protected Piece piece;

    protected SpriteRenderer sprite;

    /// <summary> 이 유닛이 플레이어면 true입니다 </summary>
    public bool isPlayer { get; private set; }

    protected virtual void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        isPlayer = this is PlayerUnit;      // 이 클래스가 PlayerUnit이거나 PlayerUnit을 상속받으면 isPlayer가 true
    }

    protected virtual void Update()
    {
        
    }
}
