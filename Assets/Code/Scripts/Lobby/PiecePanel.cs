using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PiecePanel : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text LVText;
    public TMP_Text HPText;
    public TMP_Text ATKText;
    public TMP_Text ATKSpeedText;

    public SkillInformation[] skills = new SkillInformation[4];

    public void SetPieceId(int id)
    {
        Piece piece = DataManager.instance.GetPiece(id);
        nameText.text = piece.GetName();
        LVText.text = piece.GetLevel().ToString();

        var stats = piece.GetStats();
        HPText.text = stats.hp.ToString();
        ATKText.text = stats.atk.ToString();
        ATKSpeedText.text = stats.atkSpeed.ToString();
    }
}
