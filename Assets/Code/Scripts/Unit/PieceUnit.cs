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

    /// <summary> �� ������ �÷��̾�� true�Դϴ� </summary>
    public bool isPlayer { get; private set; }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();

        isPlayer = this is PlayerUnit;      // �� Ŭ������ PlayerUnit�̰ų� PlayerUnit�� ��ӹ����� isPlayer�� true
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
