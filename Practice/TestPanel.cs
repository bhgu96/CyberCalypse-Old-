using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : SingleTonManager<TestPanel>
{
    public Color color;
    public float fadeAlpha;
    public float fadeSecond;
    public bool isCheckWait;

    public Image fadePanel;
    public GameObject LoadLevel;

    public void Start()
    {
        color = fadePanel.color;
        fadeAlpha = 1.0f;
        isCheckWait = false;
        fadePanel = GetComponent<Image>();
    }

    public void Update()
    {
        //첫번째(스플래쉬 이미지)
        if (TestTerminal.instance.isSplashImageClose == true)
        {
            StartCoroutine(Fade());
            if(fadeAlpha < 0.0f)
            {
                StartCoroutine(StopSecond());

                if(fadeSecond > 2.0f)
                {
                    StartCoroutine(Fade());
                }
            }

            if (fadeAlpha >= 1.0f && TestTerminal.instance.isSplashImageClose == true && isCheckWait == true)
            {
                TestTerminal.instance.isSplashImageClose = false;
                isCheckWait = false;
                fadeAlpha = 1.0f;
                fadeSecond = 0.0f;
            }
        }

        //2번째(로고)
        else if (TestTerminal.instance.isSplashImageClose == false && TestTerminal2.instance.isLogoImageClose == true && TestTerminalPhantasyLabLogo.instance.isPhantasyLabLogoImage == true)
        {
            /*StartCoroutine(Fade());
            if (fadeAlpha < 0.0f)
            {
                StartCoroutine(StopSecond());

                if (fadeSecond > 2.0f)
                {
                    StartCoroutine(Fade());
                }
            }

            if (fadeAlpha >= 1.0f && TestTerminal.instance.isSplashImageClose == false && TestTerminal2.instance.isLogoImageClose == true && isCheckWait == true)
            {
                TestTerminal2.instance.isLogoImageClose = false;
                isCheckWait = false;
                fadeAlpha = 1.0f;
                fadeSecond = 0.0f;
            }*/

            color.a = 0;
            fadePanel.color = color;
            StartCoroutine(StopSecond());

            if(fadeSecond > 2.0f)
            {
                if (fadeAlpha >= 1.0f && TestTerminal.instance.isSplashImageClose == false && TestTerminal2.instance.isLogoImageClose == true && TestTerminalPhantasyLabLogo.instance.isPhantasyLabLogoImage == true && isCheckWait == true)
                {
                    TestTerminal2.instance.isLogoImageClose = false;
                    TestTerminalPhantasyLabLogo.instance.isPhantasyLabLogoImage = false;
                    isCheckWait = false;
                    fadeAlpha = 1.0f;
                    fadeSecond = 0.0f;
                }
            }

        }

        // 3번째(데이터 로딩)
        else if (TestTerminal.instance.isSplashImageClose == false && TestTerminal2.instance.isLogoImageClose == false && TestTerminal3.instance.isLodingImageClose == true)
        {
            color.a = 0;
            fadePanel.color = color;

            //데이터 로딩 시작
            LoadLevel.SetActive(true);
        }
    }

    IEnumerator Fade()
    {
        if(fadeSecond <= 0.0f)
        {
            while (color.a > 0f)
            {
                fadeAlpha -= Time.deltaTime * 0.01f;
                color.a = Mathf.Lerp(0, 1, fadeAlpha);
                fadePanel.color = color;
                yield return null;
            }
        }
        
        else if(fadeSecond >= 2.0f)
        {
            while (color.a < 1.0f)
            {
                fadeAlpha += Time.deltaTime * 0.01f;

                color.a = Mathf.Lerp(0, 1, fadeAlpha);
                fadePanel.color = color;
                yield return null;
            }  
        }
    }

    IEnumerator StopSecond()
    {
        while (fadeSecond < 2.0f)
        {
            yield return new WaitForEndOfFrame();
            fadeSecond += Time.deltaTime * 0.01f;
        }
        isCheckWait = true;
    }
}
