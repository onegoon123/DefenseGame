using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffect/Stun")]
public class Stun : StatusEffectBase
{
    public override void OnStart(PieceUnit unit)
    {
        unit.canAct = false;
    }

    public override void OnEnd(PieceUnit unit)
    {
        unit.canAct = true;
    }

    public override void OnUpdate(PieceUnit unit)
    {
    }

}
