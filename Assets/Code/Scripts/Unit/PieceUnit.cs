using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.UIElements;
using UnityEngine;

public abstract class PieceUnit : MonoBehaviour
{
    protected Animator animator;
    public Animator spriteAnimator;

    public SkillBase attack;
    public SkillBase skill;
    public int maxHP;
    public int currentHP;
    public int maxMP;
    public int currentMP;
    public int atkRange;                    // ���� ��Ÿ�
    public bool diagonalAttack = false;     // �밢�� ����
    public int atk;
    public int2 gridPos;
    public Transform projectileSpawnPoint;
    public float moveSpeed = 2.0f;

    /// <summary> �� ������ �÷��̾�� true�Դϴ� </summary>
    public bool isPlayer { get; private set; }

    protected bool isMove = false;
    protected Vector3 moveStartPos;
    protected Vector3 moveTargetPos;
    private float moveTimer = 0;

    public void Setting(int2 pos)
    {
        gridPos = pos;
        transform.position = StageManager.instance.GridToWorldPosition(gridPos);
        StageManager.instance.SetUnit(this);
    }

    public virtual void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
        {
            StageManager.instance.ClearUnit(gridPos);
            Debug.Log("���");
            Destroy(gameObject);
        }
    }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        isPlayer = this is PlayerUnit;      // �� Ŭ������ PlayerUnit�̰ų� PlayerUnit�� ��ӹ����� isPlayer�� true

        currentHP = maxHP;
    }

    protected virtual void Update()
    {
        if (isMove)
        {
            MoveUpdate();
            return;
        }

        if (attack.CanActivate(this))
        {
            attack.Activate(this);
        }

        //skill.CanActivate(this);
    }

    public void LookAtTarget(PieceUnit unit)
    {
        if (unit.gridPos.x <= gridPos.x)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    // �� ��Ÿ� �� �� ĳ���͸� ��� ã�´�
    public List<PieceUnit> FindTargetsInRange()
    {
        int maxTargets = (atkRange * 2 + 1) * (atkRange * 2 + 1) - 1;
        List<PieceUnit> targets = new List<PieceUnit>(maxTargets);

        for (int x = -atkRange; x <= atkRange; x++)
        {
            for (int y = -atkRange; y <= atkRange; y++)
            {
                // �밢�� ���� ���� ���ο� ���� ó��
                if (!diagonalAttack && math.abs(x) + math.abs(y) > atkRange) continue;

                int2 targetPos = gridPos + new int2(x, y);

                // ��ȿ�� Ÿ������ Ȯ��
                if (!StageManager.instance.IsValidTile(targetPos)) continue;

                PieceUnit target = StageManager.instance.GetUnit(targetPos);
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
    public PieceUnit FindTargetInRange()
    {
        int maxDistance = 9999;
        PieceUnit resultTarget = null;

        for (int x = -atkRange; x <= atkRange; x++)
        {
            for (int y = -atkRange; y <= atkRange; y++)
            {
                // �밢�� ���� ���� ���ο� ���� ó��
                if (!diagonalAttack && math.abs(x) + math.abs(y) > atkRange) continue;

                int2 targetPos = gridPos + new int2(x, y);

                // ��ȿ�� Ÿ������ Ȯ��
                if (!StageManager.instance.IsValidTile(targetPos)) continue;

                PieceUnit target = StageManager.instance.GetUnit(targetPos);
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

    private float EaseOutQuad(float progress)
    {
        return 1 - (1 - progress) * (1 - progress);
    }

    private void MoveUpdate()
    {
        if (moveTimer < 1)
        {
            transform.position = Vector3.Lerp(moveStartPos, moveTargetPos, EaseOutQuad(moveTimer));
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
        animator.SetTrigger("Move");
        isMove = true;
        moveTimer = 0;
    }

    protected void SetGridPos(int2 pos)
    {
        StageManager.instance.ClearUnit(gridPos);
        gridPos = pos;
        StageManager.instance.SetUnit(this);
    }

    public void AddGridPos(int2 pos)
    {
        StageManager.instance.ClearUnit(gridPos);
        gridPos += pos;
        StageManager.instance.SetUnit(this);
    }

    [ContextMenu("Set World From Grid")]
    public void SetWorldPosFromGridPos()
    {
        transform.position = FindFirstObjectByType<StageManager>().GridToWorldPosition(gridPos);
    }


    // ���� ����
    public int GetAtk() { return atk; }
}
