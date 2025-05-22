using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public abstract class PieceUnit : MonoBehaviour
{
    /// <summary> �� ������ �÷��̾�� true�Դϴ� </summary>
    public bool isPlayer { get; private set; }

    public Animator animator;               // ���� �ִϸ��̼� (�̵�, �ǰ�, ����)
    public SpriteRenderer spriteRenderer;   // ��������Ʈ ������
    public Animator spriteAnimator;         // ��������Ʈ �ִϸ��̼�
    public Slider hpSlider;                 // HP��
    public SkillBase attackData;            // ���ݽ�ų ������
    public SkillBase skillData;             // ��ų ������
    public Transform projectileSpawnPoint;  // ����ü�� ������ ��ġ
    private SkillBase attack;               // ���� �ν��Ͻ�
    private SkillBase skill;                // ��ų �ν��Ͻ�

    private List<StatusEffectInstance> activeEffects = new List<StatusEffectInstance>();    // �����̻�

    protected int maxHP;        // �ִ� ü��
    protected int maxMP;        // �ִ� ����

    [SerializeField]
    protected int currentHP;    // ���� ü��
    [SerializeField]
    protected int currentMP;    // ���� ����

    protected int atk;                      // ���ݷ�
    protected int atkRange;                 // �⺻ ���� ��Ÿ�
    protected bool diagonalAttack = false;  // �⺻ ���� �밢�� ����
    public int2 gridPos { get; private set; }   // �׸���� ��ġ
    public float moveSpeed = 2.0f;              // �̵� �ӵ�

    protected bool isMove = false;      // ���� �̵�������
    protected Vector3 moveStartPos;     // �̵� ���� ��ġ
    protected Vector3 moveTargetPos;    // �̵� ��ǥ ��ġ
    private float moveTimer = 0;        // �̵� Ÿ�̸�

    // �ʱ� ��ġ ����
    public void Setting(int2 pos)
    {
        gridPos = pos;
        transform.position = StageManager.instance.GridToWorldPosition(gridPos);
        StageManager.instance.SetUnit(this);
    }

    // ������� ����
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

    // �����̻� �߰�
    public void AddStatusEffect(StatusEffectBase effectData)
    {
        StatusEffectInstance instance = new StatusEffectInstance(effectData);
        instance.data.OnStart(this);
        activeEffects.Add(instance);
    }

    // Awake
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

    // Update
    protected virtual void Update()
    {
        // �����̻� ������Ʈ
        for (int i = activeEffects.Count - 1 ; i >= 0 ; i--)
        {
            StatusEffectInstance effect = activeEffects[i];
            effect.Update(this);

            if (effect.IsEnd())
            {
                effect.data.OnEnd(this);
                activeEffects.RemoveAt(i);
            }
        }

        // �̵�
        if (isMove)
        {
            MoveUpdate();
            return;
        }

        // ����
        if (attack != null && attack.CanActivate(this))
        {
            attack.Activate(this);
        }

        // ��ų
        if (skill != null)
        {
            skill.CanActivate(this);
        }

        
    }

    // _______________ �� Ž��(ã��) ���� �޼��� ________________

    // �ٸ� PieceUnit�� �ٶ󺸴� ���
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

    // �ڽ� �����ȿ� �ִ� ������ ��� ã�´�
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

    // _________________ �̵� �� ��ġ ���� �޼��� __________________

    // �̵����� ���
    private float EaseOutQuad(float progress)
    {
        return 1 - (1 - progress) * (1 - progress);
    }
    // �̵� �� �������� ����
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


    // _______________ ���� ���� �޼��� ___________________

    // ���ݷ��� �����մϴ�, �����̻����� ���ݷ��� �������� ������ ���ݷ��� �����մϴ�.
    public int GetAtk() { return atk; }

    public int GetMP() => currentMP;
}
