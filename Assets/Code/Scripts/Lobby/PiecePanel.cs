using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PiecePanel : MonoBehaviour
{

    private Piece piece;
    private int LevelUpMoney;

    public TMP_Text nameText;
    public TMP_Text LVText;
    public TMP_Text HPText;
    public TMP_Text ATKText;
    public TMP_Text ATKSpeedText;

    public Button LevelUpButton;
    public TMP_Text CurrentMoneyText;
    public TMP_Text LevelUpMoneyText;

    public SkillInformation[] skills = new SkillInformation[4];

    public void SetPieceId(int id)
    {
        piece = DataManager.instance.GetPiece(id);
        SetPieceInformation();
    }

    public void LevelUp()
    {
        DataManager.instance.SetMoney(DataManager.instance.GetMoney() - LevelUpMoney);
        piece.LevelUp();
        DataManager.instance.SetPiece(piece);
        SetPieceInformation();
    }

    public void SetPieceInformation()
    {
        nameText.text = piece.GetName();
        LVText.text = piece.GetLevel().ToString();

        var stats = piece.GetStats();
        HPText.text = stats.hp.ToString();
        ATKText.text = stats.atk.ToString();
        ATKSpeedText.text = stats.atkSpeed.ToString();

        CurrentMoneyText.text = DataManager.instance.GetMoney().ToString();
        LevelUpMoney = piece.GetLevel() * 100;
        LevelUpMoneyText.text = LevelUpMoney.ToString();

        LevelUpButton.interactable = LevelUpMoney <= DataManager.instance.GetMoney();
    }
}
