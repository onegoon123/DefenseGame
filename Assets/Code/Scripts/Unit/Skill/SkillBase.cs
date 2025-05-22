using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SkillBase : ScriptableObject
{
    public string skillName;
    public float manaCost;
    public float cooldown;
    protected float cooldownTimer;

    public abstract void Activate(PieceUnit unit);
    public virtual bool CanActivate(PieceUnit unit)
    {
        cooldownTimer -= Time.deltaTime;
        return cooldownTimer < 0 && manaCost <= unit.GetMP();
    }

    // 스킬들이 범용적으로 사용하는 메서드들은 밑에 추가해주세요
    public static int CalculateDamage(int atk, float percent)
    {
        return (int)(atk * percent); 
    }
    public static int CalculateDamage(PieceUnit unit, float percent)
    {
        return (int)(unit.GetAtk() * percent);
    }
}
