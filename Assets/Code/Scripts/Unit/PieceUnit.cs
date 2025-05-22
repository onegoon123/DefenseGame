using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public abstract class PieceUnit : MonoBehaviour
{
    /// <summary> 이 유닛이 플레이어면 true입니다 </summary>
    public bool isPlayer { get; private set; }

    public Animator animator;               // 공용 애니메이션 (이동, 피격, 죽음)
    public SpriteRenderer spriteRenderer;   // 스프라이트 렌더러
    public Animator spriteAnimator;         // 스프라이트 애니메이션
    public Slider hpSlider;                 // HP바
    public SkillBase attackData;            // 공격스킬 데이터
    public SkillBase skillData;             // 스킬 데이터
    public Transform projectileSpawnPoint;  // 투사체가 생성될 위치
    private SkillBase attack;               // 공격 인스턴스
    private SkillBase skill;                // 스킬 인스턴스

    private List<StatusEffectInstance> activeEffects = new List<StatusEffectInstance>();    // 상태이상

    protected int maxHP;        // 최대 체력
    protected int maxMP;        // 최대 마나

    [SerializeField]
    protected int currentHP;    // 현재 체력
    [SerializeField]
    protected int currentMP;    // 현재 마나

    protected int atk;                      // 공격력
    protected int atkRange;                 // 기본 공격 사거리
    protected bool diagonalAttack = false;  // 기본 공격 대각선 여부
    public int2 gridPos { get; private set; }   // 그리드상 위치
    public float moveSpeed = 2.0f;              // 이동 속도

    protected bool isMove = false;      // 현재 이동중인지
    protected Vector3 moveStartPos;     // 이동 시작 위치
    protected Vector3 moveTargetPos;    // 이동 목표 위치
    private float moveTimer = 0;        // 이동 타이머

    // 초기 위치 세팅
    public void Setting(int2 pos)
    {
        gridPos = pos;
        transform.position = StageManager.instance.GridToWorldPosition(gridPos);
        StageManager.instance.SetUnit(this);
    }

    // 대미지를 받음
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

    // 상태이상 추가
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
        isPlayer = this is PlayerUnit;      // 이 클래스가 PlayerUnit이거나 PlayerUnit을 상속받으면 isPlayer가 true

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
        // 상태이상 업데이트
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

        // 이동
        if (isMove)
        {
            MoveUpdate();
            return;
        }

        // 공격
        if (attack != null && attack.CanActivate(this))
        {
            attack.Activate(this);
        }

        // 스킬
        if (skill != null)
        {
            skill.CanActivate(this);
        }

        
    }

    // _______________ 적 탐색(찾기) 관련 메서드 ________________

    // 다른 PieceUnit을 바라보는 기능
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
    // ▶ 사거리(스텟) 내 적 캐릭터를 모두 찾는다
    public List<PieceUnit> FindTargetsInRange()
    {
        return FindTargetsInRange(atkRange, diagonalAttack);
    }
    // ▶ 사거리(매개변수) 내 적 캐릭터를 모두 찾는다
    public List<PieceUnit> FindTargetsInRange(int atkRange, bool diagonalAttack = false)
    {
        int maxTargets = (atkRange * 2 + 1) * (atkRange * 2 + 1) - 1;
        List<PieceUnit> targets = new List<PieceUnit>(maxTargets);

        for (int x = -atkRange; x <= atkRange; x++)
        {
            for (int y = -atkRange; y <= atkRange; y++)
            {
                // 대각선 공격 가능 여부에 따라 처리
                if (!diagonalAttack && math.abs(x) + math.abs(y) > atkRange) continue;

                int2 targetPos = gridPos + new int2(x, y);

                // 유효한 타일인지 확인
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

    // 박스 범위안에 있는 적들을 모두 찾는다
    public List<PieceUnit> FindTargetsInBox(int2 boxScale, int2 boxPos)
    {
        List<PieceUnit> targets = new List<PieceUnit>(boxScale.x * boxScale.y);

        for (int x = 0; x <= boxScale.x; x++)
        {
            for (int y = 0; y <= boxScale.y; y++)
            {
                // 현재 내 위치 + 박스 위치 기준 우상단 박스
                int2 targetPos = gridPos + boxPos + new int2(x, y);

                // 유효한 타일인지 확인
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

    // ▶ 사거리(스텟) 내 가장 가까운 적을 찾는다
    public PieceUnit FindTargetInRange()
    {
        return FindTargetInRange(atkRange, diagonalAttack);
    }

    // ▶ 사거리(인수) 내 가장 가까운 적을 찾는다
    public PieceUnit FindTargetInRange(int atkRange, bool diagonalAttack = false)
    {
        int maxDistance = 9999;
        PieceUnit resultTarget = null;

        for (int x = -atkRange; x <= atkRange; x++)
        {
            for (int y = -atkRange; y <= atkRange; y++)
            {
                // 대각선 공격 가능 여부에 따라 처리
                if (!diagonalAttack && math.abs(x) + math.abs(y) > atkRange) continue;

                int2 targetPos = gridPos + new int2(x, y);

                // 유효한 타일인지 확인
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

                // 유효한 적인지 확인
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

    // 박스 범위안에 있는 적들 중 가장 가까운 적을 리턴합니다
    public PieceUnit FindTargetInBox(int2 boxScale, int2 boxPos)
    {
        int maxDistance = 9999;
        PieceUnit resultTarget = null;

        for (int x = 0; x <= boxScale.x; x++)
        {
            for (int y = 0; y <= boxScale.y; y++)
            {
                // 현재 내 위치 + 박스 위치 기준 우상단 박스
                int2 targetPos = gridPos + boxPos + new int2(x, y);

                // 유효한 타일인지 확인
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

                // 유효한 적인지 확인
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

    // 타깃이 박스 범위안에 있는지 여부를 리턴합니다
    public bool IsTargetInBox(PieceUnit target, int2 boxScale, int2 boxPos)
    {
        for (int x = 0; x <= boxScale.x; x++)
        {
            for (int y = 0; y <= boxScale.y; y++)
            {
                // 현재 내 위치 + 박스 위치 기준 우상단 박스
                int2 targetPos = gridPos + boxPos + new int2(x, y);
                if (target.gridPos.Equals(targetPos))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // 타겟이 공격 범위안에 있는지 여부를 리턴합니다
    public bool IsTargetInRange(PieceUnit target)
    {
        return IsTargetInRange(target, atkRange, diagonalAttack);
    }
    // 타겟이 공격 범위안에 있는지 여부를 리턴합니다
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

    // 다른 유닛과의 거리를 리턴합니다
    public int GetDistance(PieceUnit other)
    {
        return math.abs(gridPos.x - other.gridPos.x) + math.abs(gridPos.y - other.gridPos.y);
    }

    // _________________ 이동 및 위치 관련 메서드 __________________

    // 이동에서 사용
    private float EaseOutQuad(float progress)
    {
        return 1 - (1 - progress) * (1 - progress);
    }
    // 이동 중 매프레임 실행
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


    // _______________ 스텟 관련 메서드 ___________________

    // 공격력을 리턴합니다, 상태이상으로 공격력이 낮아지면 낮아진 공격력을 리턴합니다.
    public int GetAtk() { return atk; }

    public int GetMP() => currentMP;
}
