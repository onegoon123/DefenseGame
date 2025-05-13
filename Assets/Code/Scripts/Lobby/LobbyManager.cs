using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 로비씬을 전반적으로 관리하는 클래스입니다
/// </summary>
public class LobbyManager : MonoBehaviour
{
    
    public PiecePanel piecePanel;
    private GameObject currentPanel;

    private int selectLand;
    private int selectStage;

    private void Start()
    {
#if UNITY_EDITOR
        ResetPanel();
#endif
        currentPanel = GameObject.Find("Lobby Panel");
    }

    public void LoadScene(string sceneName)
    {
        SceneLoader.instance.LoadScene(sceneName);
    }

    public void SwitchPanel(GameObject panel)
    {
        if (currentPanel != null)
        {
            var anim = currentPanel.GetComponent<Animator>();
            StartCoroutine(FadeOutPanel(currentPanel));
        }

        currentPanel = panel;
        StartCoroutine(FadeInPanel(panel));
        return;
    }

    public void SelectCharacter(int id)
    {
        SwitchPanel(piecePanel.gameObject);
        piecePanel.SetPieceId(id);
    }

    private IEnumerator FadeOutPanel(GameObject panel)
    {
        var anim = panel.GetComponent<Animator>();
        anim.Play("FadeOut");
        yield return new WaitForSeconds(0.25f);
        panel.SetActive(false);
    }

    private IEnumerator FadeInPanel(GameObject panel)
    {
        yield return new WaitForSeconds(0.25f);
        panel.SetActive(true);
        var anim = panel.GetComponent<Animator>();
        anim.Play("FadeIn");
        yield return null;
    }

    /// ����׿� ��ɵ� �Դϴ�
    
    [ContextMenu("Reset Panel")]
    private void ResetPanel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(true);
        transform.Find("Land Panel")?.gameObject.SetActive(false);
        transform.Find("Stage Panel")?.gameObject.SetActive(false);
        transform.Find("Characters Panel")?.gameObject.SetActive(false);
        transform.Find("Piece Panel")?.gameObject.SetActive(false);
        transform.Find("Gacha Panel")?.gameObject.SetActive(false);
        transform.Find("Party Panel")?.gameObject.SetActive(false);
    }

    [ContextMenu("Land Panel")]
    private void LandPanel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(false);
        transform.Find("Land Panel")?.gameObject.SetActive(true);
        transform.Find("Stage Panel")?.gameObject.SetActive(false);
        transform.Find("Characters Panel")?.gameObject.SetActive(false);
        transform.Find("Piece Panel")?.gameObject.SetActive(false);
        transform.Find("Gacha Panel")?.gameObject.SetActive(false);
        transform.Find("Party Panel")?.gameObject.SetActive(false);
    }

    [ContextMenu("Stage Panel")]
    private void Stage1Panel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(false);
        transform.Find("Land Panel")?.gameObject.SetActive(false);
        transform.Find("Stage Panel")?.gameObject.SetActive(true);
        transform.Find("Characters Panel")?.gameObject.SetActive(false);
        transform.Find("Piece Panel")?.gameObject.SetActive(false);
        transform.Find("Gacha Panel")?.gameObject.SetActive(false);
        transform.Find("Party Panel")?.gameObject.SetActive(false);
    }

    [ContextMenu("Character Panel")]
    private void CharacterPanel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(false);
        transform.Find("Land Panel")?.gameObject.SetActive(false);
        transform.Find("Stage Panel")?.gameObject.SetActive(false);
        transform.Find("Characters Panel")?.gameObject.SetActive(true);
        transform.Find("Piece Panel")?.gameObject.SetActive(false);
        transform.Find("Gacha Panel")?.gameObject.SetActive(false);
        transform.Find("Party Panel")?.gameObject.SetActive(false);
    }

    [ContextMenu("Piece Panel")]
    private void PiecePanel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(false);
        transform.Find("Land Panel")?.gameObject.SetActive(false);
        transform.Find("Stage Panel")?.gameObject.SetActive(false);
        transform.Find("Characters Panel")?.gameObject.SetActive(false);
        transform.Find("Piece Panel")?.gameObject.SetActive(true);
        transform.Find("Gacha Panel")?.gameObject.SetActive(false);
        transform.Find("Party Panel")?.gameObject.SetActive(false);
    }

    [ContextMenu("Gacha Panel")]
    private void GachaPanel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(false);
        transform.Find("Land Panel")?.gameObject.SetActive(false);
        transform.Find("Stage Panel")?.gameObject.SetActive(false);
        transform.Find("Characters Panel")?.gameObject.SetActive(false);
        transform.Find("Piece Panel")?.gameObject.SetActive(false);
        transform.Find("Gacha Panel")?.gameObject.SetActive(true);
        transform.Find("Party Panel")?.gameObject.SetActive(false);
    }

    [ContextMenu("Party Panel")]
    private void UnitPanel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(false);
        transform.Find("Land Panel")?.gameObject.SetActive(false);
        transform.Find("Stage Panel")?.gameObject.SetActive(false);
        transform.Find("Characters Panel")?.gameObject.SetActive(false);
        transform.Find("Piece Panel")?.gameObject.SetActive(false);
        transform.Find("Gacha Panel")?.gameObject.SetActive(false);
        transform.Find("Party Panel")?.gameObject.SetActive(true);
    }

    public void SetLand(int land)
    {
        selectLand = land;
    }

    public void SetStage(int stage) { selectStage = stage; }

    public void StartStage()
    {
        SceneLoader.instance.LoadScene("Stage_"+selectLand+ "_" + selectStage);
    }
}
