using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �κ�ȭ�鿡�� UI���� ȭ�� ��ȯ�� �ϱ����� Ŭ�����Դϴ�.
/// </summary>
public class LobbyManager : MonoBehaviour
{

    private GameObject currentPanel;
    public PiecePanel piecePanel;

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
        transform.Find("Stage1 Panel")?.gameObject.SetActive(false);
        transform.Find("Characters Panel")?.gameObject.SetActive(false);
        transform.Find("Piece Panel")?.gameObject.SetActive(false);
        transform.Find("Shop Panel")?.gameObject.SetActive(false);
        transform.Find("Party Panel")?.gameObject.SetActive(false);
    }

    [ContextMenu("Land Panel")]
    private void LandPanel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(false);
        transform.Find("Land Panel")?.gameObject.SetActive(true);
        transform.Find("Stage1 Panel")?.gameObject.SetActive(false);
        transform.Find("Characters Panel")?.gameObject.SetActive(false);
        transform.Find("Piece Panel")?.gameObject.SetActive(false);
        transform.Find("Shop Panel")?.gameObject.SetActive(false);
        transform.Find("Party Panel")?.gameObject.SetActive(false);
    }

    [ContextMenu("Stage1 Panel")]
    private void Stage1Panel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(false);
        transform.Find("Land Panel")?.gameObject.SetActive(false);
        transform.Find("Stage1 Panel")?.gameObject.SetActive(true);
        transform.Find("Characters Panel")?.gameObject.SetActive(false);
        transform.Find("Piece Panel")?.gameObject.SetActive(false);
        transform.Find("Shop Panel")?.gameObject.SetActive(false);
        transform.Find("Party Panel")?.gameObject.SetActive(false);
    }

    [ContextMenu("Character Panel")]
    private void CharacterPanel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(false);
        transform.Find("Land Panel")?.gameObject.SetActive(false);
        transform.Find("Stage1 Panel")?.gameObject.SetActive(false);
        transform.Find("Characters Panel")?.gameObject.SetActive(true);
        transform.Find("Piece Panel")?.gameObject.SetActive(false);
        transform.Find("Shop Panel")?.gameObject.SetActive(false);
        transform.Find("Party Panel")?.gameObject.SetActive(false);
    }

    [ContextMenu("Piece Panel")]
    private void PiecePanel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(false);
        transform.Find("Land Panel")?.gameObject.SetActive(false);
        transform.Find("Stage1 Panel")?.gameObject.SetActive(false);
        transform.Find("Characters Panel")?.gameObject.SetActive(false);
        transform.Find("Piece Panel")?.gameObject.SetActive(true);
        transform.Find("Shop Panel")?.gameObject.SetActive(false);
        transform.Find("Party Panel")?.gameObject.SetActive(false);
    }

    [ContextMenu("Party Panel")]
    private void UnitPanel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(false);
        transform.Find("Land Panel")?.gameObject.SetActive(false);
        transform.Find("Stage1 Panel")?.gameObject.SetActive(false);
        transform.Find("Characters Panel")?.gameObject.SetActive(false);
        transform.Find("Piece Panel")?.gameObject.SetActive(false);
        transform.Find("Shop Panel")?.gameObject.SetActive(false);
        transform.Find("Party Panel")?.gameObject.SetActive(true);
    }
}
