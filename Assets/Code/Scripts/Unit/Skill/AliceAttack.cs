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
        // Ÿ�� ã��
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

        // ����ü ����
        GameObject obj = Instantiate(projectilePrefab, unit.projectileSpawnPoint.position, Quaternion.identity);
        var projectile = obj.GetComponent<AttackProjectile>();
        // ����ü ���� (Ÿ��, �����, �ӵ�)
        projectile.Init(target, (int)(unit.GetAtk() * damagePercent), projectileSpeed);

        // ��Ÿ�� ����
        cooldownTimer = cooldown;
    }

}
