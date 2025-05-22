using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public abstract class PieceUnit : MonoBehaviour
{
    protected Animator animator;
    public SpriteRenderer spriteRenderer;
    public Animator spriteAnimator;
    public Slider hpSlider;
    public SkillBase attackData;
    public SkillBase skillData;
    private SkillBase attack;
    private SkillBase skill;
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
        hpSlider.gameObject.SetActive(true);
        hpSlider.value = (float)currentHP / maxHP;
        if (currentHP <= 0)
        {
            StageManager.instance.RemoveUnit(this);
            animator.SetTrigger("Die");
            Destroy(gameObject, 1.0f);
            hpSlider.gameObject.SetActive(false);
            Destroy(this);
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        isPlayer = this is PlayerUnit;      // �� Ŭ������ PlayerUnit�̰ų� PlayerUnit�� ��ӹ����� isPlayer�� true

        currentHP = maxHP;

        if (attackData)
        {
            attack = Instantiate(attackData);
        }

        if (skillData)
        {
            skill =  Instantiate(skillData);
        }
    }

    protected virtual void Update()
    {
        if (isMove)
        {
            MoveUpdate();
            return;
        }

        if (!attack) return;

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
            spriteRenderer.flipX = false;
            if (projectileSpawnPoint != null)
            {
                Vector3 pos = projectileSpawnPoint.localPosition;
                pos.x = -math.abs(pos.x);
                projectileSpawnPoint.localPosition = pos;
            }
        }
        else
        {
            spriteRenderer.flipX = true;
            if (projectileSpawnPoint != null)
            {
                Vector3 pos = projectileSpawnPoint.localPosition;
                pos.x = math.abs(pos.x);
                projectileSpawnPoint.localPosition = pos;
            }
        }
    }
    // �� ��Ÿ�(����) �� �� ĳ���͸� ��� ã�´�
    public List<PieceUnit> FindTargetsInRange()
    {
        return FindTargetsInRange(atkRange, diagonalAttack);
    }
    // �� ��Ÿ�(�Ű�����) �� �� ĳ���͸� ��� ã�´�
    public List<PieceUnit> FindTargetsInRange(int atkRange, bool diagonalAttack = false)
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


                if (this.isPlayer)
                {
                    List<EnemyUnit> units = StageManager.instance.GetEnemies(targetPos);
                    if (0 <units.Count)
                        targets.AddRange(units);
                }
                else
                {
                    PlayerUnit unit = StageManager.instance.GetPlayer(targetPos);
                    if (unit != null)
                        targets.Add(unit);
                }
            }
        }
        return targets;
    }

    // �ڽ� �����ȿ� �ִ� ������ �����մϴ�
    public List<PieceUnit> FindTargetsInBox(int2 boxScale, int2 boxPos)
    {
        List<PieceUnit> targets = new List<PieceUnit>(boxScale.x * boxScale.y);

        for (int x = 0; x <= boxScale.x; x++)
        {
            for (int y = 0; y <= boxScale.y; y++)
            {
                // ���� �� ��ġ + �ڽ� ��ġ ���� ���� �ڽ�
                int2 targetPos = gridPos + boxPos + new int2(x, y);

                // ��ȿ�� Ÿ������ Ȯ��
                if (!StageManager.instance.IsValidTile(targetPos)) continue;

                if (this.isPlayer)
                {
                    List<EnemyUnit> units = StageManager.instance.GetEnemies(targetPos);
                    if (0 < units.Count)
                        targets.AddRange(units);
                }
                else
                {
                    PlayerUnit unit = StageManager.instance.GetPlayer(targetPos);
                    if (unit != null)
                        targets.Add(unit);
                }
            }
        }
        return targets;
    }

    // �� ��Ÿ�(����) �� ���� ����� ���� ã�´�
    public PieceUnit FindTargetInRange()
    {
        return FindTargetInRange(atkRange, diagonalAttack);
    }

    // �� ��Ÿ�(�μ�) �� ���� ����� ���� ã�´�
    public PieceUnit FindTargetInRange(int atkRange, bool diagonalAttack = false)
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

                PieceUnit target = null;

                if (this.isPlayer)
                {
                    if (0 < StageManager.instance.GetEnemies(targetPos).Count)
                        target = StageManager.instance.GetEnemies(targetPos)[0];
                }
                else
                {
                    target = StageManager.instance.GetPlayer(targetPos);
                }

                // ��ȿ�� ������ Ȯ��
                if (target != null)
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

    // �ڽ� �����ȿ� �ִ� ���� �� ���� ����� ���� �����մϴ�
    public PieceUnit FindTargetInBox(int2 boxScale, int2 boxPos)
    {
        int maxDistance = 9999;
        PieceUnit resultTarget = null;

        for (int x = 0; x <= boxScale.x; x++)
        {
            for (int y = 0; y <= boxScale.y; y++)
            {
                // ���� �� ��ġ + �ڽ� ��ġ ���� ���� �ڽ�
                int2 targetPos = gridPos + boxPos + new int2(x, y);

                // ��ȿ�� Ÿ������ Ȯ��
                if (!StageManager.instance.IsValidTile(targetPos)) continue;

                PieceUnit target = null;

                if (this.isPlayer)
                {
                    if (0 < StageManager.instance.GetEnemies(targetPos).Count)
                        target = StageManager.instance.GetEnemies(targetPos)[0];
                }
                else
                {
                    target = StageManager.instance.GetPlayer(targetPos);
                }

                // ��ȿ�� ������ Ȯ��
                if (target != null)
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

    // Ÿ���� �ڽ� �����ȿ� �ִ��� ���θ� �����մϴ�
    public bool IsTargetInBox(PieceUnit target, int2 boxScale, int2 boxPos)
    {
        for (int x = 0; x <= boxScale.x; x++)
        {
            for (int y = 0; y <= boxScale.y; y++)
            {
                // ���� �� ��ġ + �ڽ� ��ġ ���� ���� �ڽ�
                int2 targetPos = gridPos + boxPos + new int2(x, y);
                if (target.gridPos.Equals(targetPos))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Ÿ���� ���� �����ȿ� �ִ��� ���θ� �����մϴ�
    public bool IsTargetInRange(PieceUnit target)
    {
        return IsTargetInRange(target, atkRange, diagonalAttack);
    }
    // Ÿ���� ���� �����ȿ� �ִ��� ���θ� �����մϴ�
    public bool IsTargetInRange(PieceUnit target, int atkRange, bool diagonalAttack = false)
    {
        for (int x = -atkRange; x <= atkRange; x++)
        {
            for (int y = -atkRange; y <= atkRange; y++)
            {
                int2 targetPos = gridPos + new int2(x, y);
                if (target.gridPos.Equals(targetPos))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // �ٸ� ���ְ��� �Ÿ��� �����մϴ�
    public int GetDistance(PieceUnit other)
    {
        return math.abs(gridPos.x - other.gridPos.x) + math.abs(gridPos.y - other.gridPos.y);
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
        StageManager.instance.RemoveUnit(this);
        gridPos = pos;
        StageManager.instance.SetUnit(this);
    }

    public void AddGridPos(int2 pos)
    {
        StageManager.instance.RemoveUnit(this);
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
