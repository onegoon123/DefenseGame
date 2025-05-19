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
        // Ÿ�� ã��
        if (target == null)
        {
            target = unit.FindTargetInRange();
            if (target == null) { return; }
        }

        // �ִϸ��̼�
        //unit.spriteAnimator.SetTrigger("Attack");

        // ����Ʈ ����
        GameObject effect = Instantiate(effectPrefab, target.transform.position, Quaternion.identity);
        Destroy(effect, 1.0f);

        // �����
        target.TakeDamage(CalculateDamage(unit, damagePercent));

        // ��Ÿ�� ����
        cooldownTimer = cooldown;
    }

}
