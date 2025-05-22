using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class StatusEffectBase : ScriptableObject
{
    public string effectName;   // �����̻� �̸�
    public float duration;      // ���ӽð�

    // �����̻��� ���۵� �� ȣ��
    public abstract void OnStart(PieceUnit unit);

    // �� ������ ȣ��
    public abstract void OnUpdate(PieceUnit unit);

    // �����̻��� ������ �� ȣ��
    public abstract void OnEnd(PieceUnit unit);
}
