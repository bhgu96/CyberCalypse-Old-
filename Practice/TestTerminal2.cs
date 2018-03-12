using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTerminal2 : SingleTonManager<TestTerminal2>
{
    public Image testLogoImage;

    public bool isLogoImageClose;

    private void Start()
    {
        isLogoImageClose = true;
        testLogoImage.fillAmount = 0;
    }

    private void Update()
    {
        if(TestTerminal.instance.isSplashImageClose == false)
        {
            testLogoImage.fillAmount = 1;

            if (isLogoImageClose == false)
            {
                testLogoImage.fillAmount = 0;
            }
        }
    }
}
