using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(menuName = "Skill/Fireball")]
public class Fireball : SkillBase
{
    PieceUnit target;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10.0f;
    public float damagePercent = 1.0f;

    public override void Activate(PieceUnit unit)
    {
        // Ÿ�� ã��
        if (target == null)
        {
            target = unit.FindTargetInRange();
            if (target == null) { return; }
        }

        // ����ü ����
        GameObject obj = Instantiate(projectilePrefab, unit.transform.position, Quaternion.identity);
        var projectile = obj.GetComponent<FireballProjectile>();
        // ����ü ���� (Ÿ��, �����, �ӵ�)
        projectile.Init(target, (int)(unit.GetAtk() * damagePercent), projectileSpeed);

        // ��Ÿ�� ����
        cooldownTimer = cooldown;
    }

    public override bool CanActivate(PieceUnit unit)
    {
        cooldownTimer -= Time.deltaTime;
        return cooldownTimer < 0;
    }
}
