using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �κ�ȭ�鿡�� UI���� ȭ�� ��ȯ�� �ϱ����� Ŭ�����Դϴ�.
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

    /// ����׿� ��ɵ� �Դϴ�
    
    [ContextMenu("Reset Panel")]
    private void ResetPanel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(true);
        transform.Find("Land Panel")?.gameObject.SetActive(false);
    }

    [ContextMenu("Land Panel")]
    private void LandPanel()
    {
        transform.Find("Lobby Panel")?.gameObject.SetActive(false);
        transform.Find("Land Panel")?.gameObject.SetActive(true);
    }

}
