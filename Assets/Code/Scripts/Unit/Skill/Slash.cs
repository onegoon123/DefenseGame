using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

[CreateAssetMenu(menuName = "Skill/Slash")]
public class Slash : SkillBase
{
    PieceUnit target;
    public GameObject effectPrefab;
    public float damagePercent = 1.0f;

    public override void Activate(PieceUnit unit)
    {
        // 타겟 찾기
        if (target == null)
        {
            target = unit.FindTargetInRange();
            if (target == null) { return; }
        }

        // 애니메이션
        //unit.spriteAnimator.SetTrigger("Attack");

        // 이펙트 생성
        GameObject effect = Instantiate(effectPrefab, target.transform.position, Quaternion.identity);
        Destroy(effect, 1.0f);

        // 대미지
        target.TakeDamage(CalculateDamage(unit, damagePercent));

        // 쿨타임 설정
        cooldownTimer = cooldown;
    }

}
