using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PieceUnit : MonoBehaviour
{
    protected Piece piece;

    protected SpriteRenderer sprite;

    protected List<Skill> skills;

    /// <summary> �� ������ �÷��̾�� true�Դϴ� </summary>
    public bool isPlayer { get; private set; }

    protected virtual void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

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


}
