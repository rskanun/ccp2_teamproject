using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LoadingUI))]
public class SceneLoadManager : MonoBehaviour
{
    public static string nextScene;

    // 참조 UI
    private LoadingUI ui;

    private void Awake()
    {
        ui = GetComponent<LoadingUI>();
    }

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.6f);

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        float percentage = 0f;
        while (!op.isDone)
        {
            timer += Time.deltaTime;
            percentage += Time.deltaTime * 0.7f;

            if (op.progress < 0.9f || percentage < 1.0f)
            {
                if (op.progress > percentage) ui.UpdateBar(percentage);
                else ui.UpdateBar(op.progress, timer);

                if (ui.FillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                ui.UpdateBar(1f, timer);

                if (ui.FillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;

                    yield break;
                }
            }

            yield return null;
        }
    }
}