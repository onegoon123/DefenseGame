using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class PieceUnit : MonoBehaviour
{
    protected Piece piece;
    protected Animator animator;
    protected List<Skill> skills = new List<Skill>();

    protected int2 gridPos;

    /// <summary> 이 유닛이 플레이어면 true입니다 </summary>
    public bool isPlayer { get; private set; }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();

        isPlayer = this is PlayerUnit;      // 이 클래스가 PlayerUnit이거나 PlayerUnit을 상속받으면 isPlayer가 true
    }

    protected virtual void Update()
    {
        foreach (Skill skill in skills)
        {
            if (skill.CheckCondition(this))
            {
                skill.Activate(this);
            }
        }
    }

    protected void Move(int2 pos)
    {
        StageManager.instance.units[gridPos.x, gridPos.y] = null;
        gridPos = pos;
        StageManager.instance.units[gridPos.x, gridPos.y] = this;
    }

}
