using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    public string skillName;
    public float cooldown;

    public abstract bool CheckCondition(PieceUnit unit);

    public abstract void Activate(PieceUnit unit);

}
