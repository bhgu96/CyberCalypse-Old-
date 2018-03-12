using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestLoadLevel : SingleTonManager<TestLoadLevel>
{
    public Image LoadingBar;
    public Text LoadingProgressText;

    public int sceneIndex;
    public bool isActive = true;

    private void OnEnable()
    {
        LoadLevel(sceneIndex);
    }

    private void LoadLevel (int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public void OnDisable()
    {
        isActive = false;
    }

    IEnumerator LoadAsynchronously(int sceneIndexs)
    {
        while(sceneIndexs <= 2)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndexs);
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                if(sceneIndexs > 3)
                {
                    Debug.Log(sceneIndexs);
                    LoadingBar.fillAmount = 1.0f;
                    operation.allowSceneActivation = true;

                    TestTerminal3.instance.gameObject.SetActive(false);
                    TestTerminal.instance.gameObject.SetActive(false);
                    TestTerminal2.instance.gameObject.SetActive(false);
                    TestTerminalPhantasyLabLogo.instance.gameObject.SetActive(false);
                    TestPanel.instance.gameObject.SetActive(false);
                    yield break;
                }
        
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                Debug.Log(progress +"   " +  sceneIndexs);
                LoadingBar.fillAmount = progress;
                LoadingProgressText.text = progress * 100 + "%";

                if (operation.progress >= 0.9f + Mathf.Epsilon && isActive == true)
                {
                    sceneIndexs++;
                    StartCoroutine(LoadAsynchronously(sceneIndexs));
                }
                yield return null;
            }
            }

        }
    }

