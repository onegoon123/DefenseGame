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
        return cooldownTimer < 0 && manaCost <= unit.currentMP;
    }
}
