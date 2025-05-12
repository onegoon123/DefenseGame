using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

/// <summary>
/// ��Ƽ ��â���� ����ϴ�
/// </summary>
public class PartyManager : MonoBehaviour
{

    // ������ �ִ� ��
    private Dictionary<PieceMainClass, int> classLimits = new Dictionary<PieceMainClass, int>
    {
        {PieceMainClass.Queen, 1 },
        {PieceMainClass.Rook, 2 },
        {PieceMainClass.Bishop, 2 },
        {PieceMainClass.Knight, 2 },
        {PieceMainClass.Pawn, 2 },
    };

    // ���� ���� ����� id
    private List<int> membersId = new List<int>(9);

    [SerializeField]
    private List<PartyMember> partyMembers;

    public void AddMember(int memberId)
    {
        PieceMainClass memberClass = DataManager.instance.GetPieceClass(memberId);
        int count = 0;
        foreach(int id in membersId)
        {
            if (memberClass == DataManager.instance.GetPieceClass(id))
            {
                count++;
            }
        }

        // Ŭ���� ������
        if (count >= classLimits[memberClass])
            return;

        // �ߺ�
        if (membersId.Contains(memberId))
            return;

        membersId.Add(memberId);
        UIUpdate();
    }

    public void RemoveMember(int memberId)
    {
        membersId.Remove(memberId);
        UIUpdate();
    }

    public void UIUpdate()
    {
        foreach (PartyMember member in partyMembers)
        {
            member.ClearId();
        }

        foreach (int id in membersId)
        {
            PieceMainClass pieceClass = DataManager.instance.GetPieceClass(id);
            
            foreach (PartyMember member in partyMembers)
            {
                if (member.GetId() == -1 && member.pieceClass == pieceClass)
                {
                    member.SetId(id);
                    break;
                }
            }
        }
    }
}
