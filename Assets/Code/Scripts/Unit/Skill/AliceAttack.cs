using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(menuName = "Skill/AliceAttack")]
public class AliceAttack : SkillBase
{
    PieceUnit target;
    public GameObject projectilePrefab;
    public int2 rangeScale;
    public int2 rangePos;
    public float projectileSpeed = 10.0f;
    public float damagePercent = 1.0f;

    public override void Activate(PieceUnit unit)
    {
        // 타겟 찾기
        if (target == null)
        {
            target = unit.FindTargetInBox(rangeScale, rangePos);
            if (target == null) { return; }
        }
        else if (unit.IsTargetInBox(target, rangeScale, rangePos) == false)
        {
            target = null;
            Activate(unit);
            return;
        }

        unit.LookAtTarget(target);
        unit.spriteAnimator.SetTrigger("Attack");

        // 투사체 생성
        GameObject obj = Instantiate(projectilePrefab, unit.projectileSpawnPoint.position, Quaternion.identity);
        var projectile = obj.GetComponent<AttackProjectile>();
        // 투사체 세팅 (타겟, 대미지, 속도)
        projectile.Init(target, (int)(unit.GetAtk() * damagePercent), projectileSpeed);

        // 쿨타임 설정
        cooldownTimer = cooldown;
    }

}
