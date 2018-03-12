using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTerminalPhantasyLabLogo : SingleTonManager<TestTerminalPhantasyLabLogo>
{

    public Image testPhantasyLabLogoImage;

    public bool isPhantasyLabLogoImage;

    private void Start()
    {
        isPhantasyLabLogoImage = true;
        testPhantasyLabLogoImage.fillAmount = 0;
    }

    private void Update()
    {
        if (TestTerminal.instance.isSplashImageClose == false)
        {
            testPhantasyLabLogoImage.fillAmount = 1;

            if (isPhantasyLabLogoImage == false)
            {
                testPhantasyLabLogoImage.fillAmount = 0;
            }
        }
    }
}
