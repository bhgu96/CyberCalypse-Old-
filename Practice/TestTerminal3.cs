using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestTerminal3 : SingleTonManager<TestTerminal3>
{
    public Image testLodingImage;//
    public Image testLodingBar;//
    public Image testLodingCircle;//
    public Image testLodingProgressBar;//
    public Text testLodingProgressNumber;//
    public Text testLodingProgressString;//

    public bool isLodingImageClose;
    public GameObject LoadLevel;

    Color testColor;
    Color testVisualColor;

    private void Start()
    {
        testColor = new Color(0, 0, 0, 0);
        testVisualColor = new Color(0, 0, 0, 1);

        isLodingImageClose = true;
        testLodingImage.fillAmount = 0;//
        testLodingBar.fillAmount = 0;//
        testLodingProgressBar.fillAmount = 0;//
        testLodingCircle.fillAmount = 0;//

        testLodingProgressNumber.color = testColor;
        testLodingProgressString.color = testColor;
    }

    private void Update()
    {
        if (TestTerminal2.instance.isLogoImageClose == false)
        {
            testLodingImage.fillAmount = 1;
            testLodingBar.fillAmount = 1;
            testLodingCircle.fillAmount = 1;

            testLodingProgressNumber.color = testVisualColor;
            testLodingProgressString.color = testVisualColor;
        }
    }
}
