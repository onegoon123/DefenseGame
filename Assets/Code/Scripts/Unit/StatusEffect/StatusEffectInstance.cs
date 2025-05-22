using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectInstance
{
    public StatusEffectBase data;
    public float timer;

    public StatusEffectInstance(StatusEffectBase data)
    {
        this.data = data;
        this.timer = data.duration;
    }

    public void Update(PieceUnit unit)
    {
        data.OnUpdate(unit);
        timer -= Time.deltaTime;
    }

    public bool IsEnd() { return timer <= 0f; }
}
