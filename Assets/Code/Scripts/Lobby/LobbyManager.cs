using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 로비화면에서 UI끼리 화면 전환을 하기위한 클래스입니다.
/// </summary>
public class LobbyManager : MonoBehaviour
{

    private GameObject currentPanel;
    //private GameObject nextPanel;

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

    /// 디버그용 기능들 입니다
    
    [ContextMenu("Reset Panel")]
    private void ResetPanel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(true);
        transform.Find("Land Panel")?.gameObject.SetActive(false);
        transform.Find("Stage1 Panel")?.gameObject.SetActive(false);
        transform.Find("Character Panel")?.gameObject.SetActive(false);
        transform.Find("Shop Panel")?.gameObject.SetActive(false);
    }

    [ContextMenu("Land Panel")]
    private void LandPanel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(false);
        transform.Find("Land Panel")?.gameObject.SetActive(true);
        transform.Find("Stage1 Panel")?.gameObject.SetActive(false);
        transform.Find("Character Panel")?.gameObject.SetActive(false);
        transform.Find("Shop Panel")?.gameObject.SetActive(false);
    }

    [ContextMenu("Stage1 Panel")]
    private void Stage1Panel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(false);
        transform.Find("Land Panel")?.gameObject.SetActive(false);
        transform.Find("Stage1 Panel")?.gameObject.SetActive(true);
        transform.Find("Character Panel")?.gameObject.SetActive(false);
        transform.Find("Shop Panel")?.gameObject.SetActive(false);
    }

    [ContextMenu("Character Panel")]
    private void CharacterPanel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(false);
        transform.Find("Land Panel")?.gameObject.SetActive(false);
        transform.Find("Stage1 Panel")?.gameObject.SetActive(false);
        transform.Find("Character Panel")?.gameObject.SetActive(true);
        transform.Find("Shop Panel")?.gameObject.SetActive(false);
    }

    [ContextMenu("Shop Panel")]
    private void ShopPanel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(false);
        transform.Find("Land Panel")?.gameObject.SetActive(false);
        transform.Find("Stage1 Panel")?.gameObject.SetActive(false);
        transform.Find("Character Panel")?.gameObject.SetActive(false);
        transform.Find("Shop Panel")?.gameObject.SetActive(true);
    }
}
