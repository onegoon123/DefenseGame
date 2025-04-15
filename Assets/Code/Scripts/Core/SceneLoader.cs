using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �� ��ȯ �� �ε�ȭ���� �����ֱ� ���� Ŭ�����Դϴ�. �׽�Ʈ �� Prefab���� SceneLoader ������Ʈ�� ���� ��ġ�ϰ� ������ּ���.
///   ����) SceneLoader.instance.LoadScene("���̸�") 
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup loadingUI;
    [SerializeField]
    private Slider progressBar;
    [SerializeField]
    private Animation fadeAnim;

    public static SceneLoader instance { get;  private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        fadeAnim.PlayQueued("FadeOut");

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;

            if (operation.progress >= 0.9f)
            {
                if (fadeAnim.isPlaying)
                {
                    AnimationState state = fadeAnim["FadeOut"];
                    float remainingTime = state.length - state.time;
                    yield return new WaitForSeconds(remainingTime);
                }
                operation.allowSceneActivation = true;
            }
            yield return null;
        }

        fadeAnim.PlayQueued("FadeIn");
        yield return null;
    }
}
