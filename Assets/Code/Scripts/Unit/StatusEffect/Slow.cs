using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffect/Slow")]
public class Slow : StatusEffectBase
{
    [Range(0f, 1f)]
    public float speedMultiplier = 0.5f;    // 속도 배율

    public override void OnStart(PieceUnit unit)
    {
        unit.SetSpeedModifier(speedMultiplier);
    }

    public override void OnEnd(PieceUnit unit)
    {
        unit.SetSpeedModifier(1.0f);
    }

    public override void OnUpdate(PieceUnit unit)
    {
    }

}
