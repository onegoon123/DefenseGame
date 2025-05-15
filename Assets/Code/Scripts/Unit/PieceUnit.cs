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

    protected int hp = 3;
    protected int atkRange = 1;                     // ���� ��Ÿ�
    protected bool canAttackDiagonally = false;     // �밢�� ������ ��������
    protected bool isMove = false;
    protected Vector3 moveStartPos;
    protected Vector3 moveTargetPos;
    private float moveTimer = 0;
    [SerializeField]
    protected float moveSpeed = 2f;
    [SerializeField]
    private float moveDelay = .5f;

    public virtual void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            StageManager.instance.units[gridPos.x, gridPos.y] = null;
            Destroy(gameObject);
        }
    }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();

        isPlayer = this is PlayerUnit;      // �� Ŭ������ PlayerUnit�̰ų� PlayerUnit�� ��ӹ����� isPlayer�� true
        gridPos = StageManager.instance.WorldToGridPosition(transform.position);
        StageManager.instance.units[gridPos.x, gridPos.y] = this;
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

        if (isMove)
        {
            MoveUpdate();
        }

        // �׽�Ʈ��
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveGridPos(gridPos + new int2(1, 0));
        }
        // �׽�Ʈ��
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveGridPos(gridPos + new int2(-1, 0));
        }
    }

    // �� ��Ÿ� �� �� ĳ���͸� ��� ã�´�
    protected List<PieceUnit> FindTargetsInRange()
    {
        int maxTargets = (atkRange * 2 + 1) * (atkRange * 2 + 1) - 1;
        List<PieceUnit> targets = new List<PieceUnit>(maxTargets);

        for (int x = -atkRange ; x <= atkRange ; x++)
        {
            for (int y = -atkRange ; y <= atkRange ; y++)
            {
                // �밢�� ���� ���� ���ο� ���� ó��
                if (!canAttackDiagonally && math.abs(x) + math.abs(y) > atkRange) continue;

                int2 targetPos = gridPos + new int2(x, y);

                // ��ȿ�� Ÿ������ Ȯ��
                if (!StageManager.instance.IsValidTile(targetPos)) continue;

                PieceUnit target = StageManager.instance.units[targetPos.x, targetPos.y];
                // ��ȿ�� ������ Ȯ��
                if (target != null && target.isPlayer != this.isPlayer)
                {
                    targets.Add(target);
                }
            }
        }
        return targets;
    }

    // �� ��Ÿ� �� ���� ����� ���� ã�´�
    protected PieceUnit FindTargetInRange()
    {
        int maxDistance = 9999;
        PieceUnit resultTarget = null;

        for (int x = -atkRange ; x <= atkRange ; x++)
        {
            for (int y = -atkRange ; y <= atkRange ; y++)
            {
                // �밢�� ���� ���� ���ο� ���� ó��
                if (!canAttackDiagonally && math.abs(x) + math.abs(y) > atkRange) continue;

                int2 targetPos = gridPos + new int2(x, y);

                // ��ȿ�� Ÿ������ Ȯ��
                if (!StageManager.instance.IsValidTile(targetPos)) continue;

                PieceUnit target = StageManager.instance.units[targetPos.x, targetPos.y];
                // ��ȿ�� ������ Ȯ��
                if (target != null && target.isPlayer != this.isPlayer)
                {
                    int distance = math.abs(target.gridPos.x - gridPos.x) + math.abs(target.gridPos.y - gridPos.y);
                    if (distance < maxDistance)
                    {
                        resultTarget = target;
                        maxDistance = distance;
                    }
                }
            }
        }
        return resultTarget;
    }

    private void MoveUpdate()
    {
        if (moveTimer < 1)
        {
            transform.position = Vector3.Lerp(moveStartPos, moveTargetPos, moveTimer);
            moveTimer += Time.deltaTime * moveSpeed;
            return;
        }

        transform.position = moveTargetPos;
        isMove = false;
    }

    protected void MoveGridPos(int2 pos)
    {
        SetGridPos(pos);
        moveStartPos = transform.position;
        moveTargetPos = StageManager.instance.GridToWorldPosition(pos);
        //animator.SetBool()
        isMove = true;
        moveTimer = 0;
    }

    protected void SetGridPos(int2 pos)
    {
        StageManager.instance.units[gridPos.x, gridPos.y] = null;
        gridPos = pos;
        StageManager.instance.units[gridPos.x, gridPos.y] = this;
    }

    public void AddGridPos(int2 pos)
    {
        StageManager.instance.units[gridPos.x, gridPos.y] = null;
        gridPos += pos;
        StageManager.instance.units[gridPos.x, gridPos.y] = this;
    }
}
