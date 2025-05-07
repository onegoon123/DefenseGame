using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 씬 전환 시 로딩화면을 보여주기 위한 클래스입니다. 테스트 시 Prefab에서 SceneLoader 오브젝트를 씬에 배치하고 사용해주세요.
///   예시) SceneLoader.instance.LoadScene("씬이름") 
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
        fadeAnim.PlayQueued("Alpha0to1");

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
                    AnimationState state = fadeAnim["Alpha0to1"];
                    float remainingTime = state.length - state.time;
                    yield return new WaitForSeconds(remainingTime);
                }
                operation.allowSceneActivation = true;
            }
            yield return null;
        }

        fadeAnim.PlayQueued("Alpha1to0");
        yield return null;
    }
}
