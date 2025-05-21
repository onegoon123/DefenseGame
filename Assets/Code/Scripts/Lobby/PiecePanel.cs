using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PiecePanel : MonoBehaviour
{

    private Piece piece;
    private int LevelUpMoney;

    public Image characterImage;
    public TMP_Text nameText;
    public TMP_Text lvText;
    public TMP_Text hpText;
    public TMP_Text atkText;
    public TMP_Text atkSpeedText;

    public Button levelUpButton;
    public TMP_Text currentMoneyText;
    public TMP_Text levelUpMoneyText;

    public SkillInformation skillInfo;
    public void SetPieceId(int id)
    {
        piece = DataManager.instance.GetPiece(id);
        SetPieceInformation();
        skillInfo.SetPieceId(id);
        characterImage.sprite = piece.GetPieceData().sprite;
    }

    public void LevelUp()
    {
        DataManager.instance.SetGold(DataManager.instance.GetGold() - LevelUpMoney);
        piece.LevelUp();
        DataManager.instance.SetPiece(piece);
        SetPieceInformation();
    }

    public void SetPieceInformation()
    {
        nameText.text = piece.GetName();
        lvText.text = piece.GetLevel().ToString();

        var stats = piece.GetStats();
        hpText.text = stats.hp.ToString();
        atkText.text = stats.atk.ToString();
        atkSpeedText.text = stats.atkSpeed.ToString();

        currentMoneyText.text = DataManager.instance.GetGold().ToString();
        LevelUpMoney = piece.GetLevel() * 100;
        levelUpMoneyText.text = LevelUpMoney.ToString();

        levelUpButton.interactable = LevelUpMoney <= DataManager.instance.GetGold();
    }
}
