using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTerminal : SingleTonManager<TestTerminal>
{
    public Image testSplashImage;

    public bool isSplashImageClose;

    private void Start()
    {
        isSplashImageClose = true;
        testSplashImage.fillAmount = 1;
    }

    private void Update()
    {
        if(isSplashImageClose == false)
        {
            testSplashImage.fillAmount = 0;
        }
    }
}
