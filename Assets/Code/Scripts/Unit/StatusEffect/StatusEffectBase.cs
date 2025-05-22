using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class StatusEffectBase : ScriptableObject
{
    public string effectName;   // 상태이상 이름
    public float duration;      // 지속시간

    // 상태이상이 시작될 때 호출
    public abstract void OnStart(PieceUnit unit);

    // 매 프레임 호출
    public abstract void OnUpdate(PieceUnit unit);

    // 상태이상이 끝났을 때 호출
    public abstract void OnEnd(PieceUnit unit);
}
