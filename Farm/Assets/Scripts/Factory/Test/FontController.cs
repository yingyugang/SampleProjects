using UnityEngine;
using System.Collections;
using BaPK;
using System.Xml;

public class FontController : MonoBehaviour
{
    public static Animator animator;

    public Label labelNameMachinePrefabs;
    public Label labelTimeProductPrefabs;
    public Texture2D textureFont;
    private BitmapFont bitmapFont;
    public XmlDocument xmlFont;
  //  public static GameObject machineSelected;
    private string textNameMachine;
    private string textTimeProductHour;
    private string textTimeProductDay;

    private int timesLeft;
    private int dayLeft;
    private int hourLeft;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        #region lines for font
        string[] lines = new string[]{
	"36 416 35 33 27 31 A",
	"429 125 28 33 27 24 E",
	"479 436 18 33 27 14 I",
	"72 480 33 34 26 29 O",
	"141 298 33 33 27 29 Y",
	"274 71 31 33 27 27 U",
	"175 398 32 33 27 28 B",
	"398 300 29 34 26 25 C",
	"208 304 32 33 27 28 D",
	"399 0 28 33 27 24 F",
	"367 176 30 34 26 26 G",
	"273 331 31 33 27 27 H",
	"457 294 24 33 27 20 J",
	"106 480 33 33 27 29 K",
	"457 159 27 33 27 23 L",
	"0 135 37 33 27 33 M",
	"274 37 31 33 27 27 N",
	"208 376 32 33 27 28 P",
	"109 115 33 37 26 29 Q",
	"111 38 33 33 27 29 R",
	"398 335 29 34 26 25 S",
	"305 481 30 33 27 26 T",
	"141 153 33 33 27 29 V",
	"0 31 41 33 27 37 W",
	"72 169 34 33 27 30 X",
	"399 34 28 33 27 24 Z",
	"277 0 31 30 30 27 a",
	"273 365 31 30 30 27 e",
	"485 148 17 35 25 13 i",
	"209 116 31 30 30 27 o",
	"367 364 30 34 30 26 y",
	"305 371 30 30 30 26 u",
	"305 105 31 35 25 27 b",
	"429 193 27 30 30 23 c",
	"336 292 30 35 25 26 d",
	"457 328 24 35 25 20 f",
	"336 364 30 35 30 26 g",
	"336 400 30 35 25 26 h",
	"479 39 19 40 25 15 j",
	"337 0 30 35 25 26 k",
	"482 338 17 35 25 13 l",
	"0 0 42 30 30 38 m",
	"337 71 30 30 30 26 n",
	"273 183 31 35 30 27 p",
	"273 436 31 35 30 27 q",
	"457 364 23 30 30 19 r",
	"398 483 28 30 30 24 s",
	"457 224 25 34 26 21 t",
	"242 81 31 30 30 27 v",
	"0 65 41 30 30 37 w",
	"43 0 32 30 30 28 x",
	"457 193 26 30 30 22 z",
	"367 470 30 34 26 26 0",
	"457 0 21 34 26 17 1",
	"428 385 28 34 26 24 2",
	"429 159 27 33 27 23 3",
	"368 0 30 33 27 26 4",
	"428 460 28 33 27 24 5",
	"398 405 29 34 26 25 6",
	"429 224 27 33 27 23 7",
	"398 370 29 34 26 25 8",
	"368 34 30 34 26 26 9",
	"457 75 20 34 26 16 !",
	"457 259 24 34 26 20 ?",
	"28 492 18 19 45 14 ,",
	"47 492 17 15 45 13 .",
	"177 79 17 29 31 13 :",
	"479 114 18 33 31 14 ;",
	"61 75 12 18 28 8 '",
	"398 440 29 42 23 25 $",
	"36 169 35 34 26 31 %",
	"0 455 35 34 26 31 &",
	"479 470 18 33 27 14 (",
	"479 80 19 33 27 15 )",
	"74 153 19 15 38 15 -",
	"309 0 27 29 31 23 +",
	"0 492 27 21 35 23 =",
	"368 69 30 37 26 26 /",
	"498 374 13 42 26 9 |",
	"368 146 29 16 38 25 ~",
	"0 96 38 38 27 34 @",
	"140 497 27 17 28 23 ^",
	"427 494 20 20 27 16 *"
};

        #endregion
        bitmapFont = new BitmapFont(textureFont, lines);

        //if (machineSelected != null)
        //    textNameMachine = machineSelected.GetComponent<MachineController>().nameMachine;
        //else
        //    textNameMachine = "";
        labelNameMachinePrefabs.setSortingLayer("ButtonBG1");
        labelNameMachinePrefabs.setSortingOrderInLayer(2);
        labelNameMachinePrefabs.setAlignment(TextAlignment.Left);
        labelNameMachinePrefabs.createLabel(bitmapFont, textNameMachine, 0, 8);

        labelTimeProductPrefabs.setSortingLayer("ButtonBG1");
        labelTimeProductPrefabs.setSortingOrderInLayer(2);
        labelTimeProductPrefabs.setAlignment(TextAlignment.Center);
        labelTimeProductPrefabs.createLabel(bitmapFont, "2 days 23 hours", 0, 8);

       
    }

  
    void Update()
    {
        //if (machineSelected != null)
        //{
        //    timesLeft = (int)machineSelected.GetComponent<MachineController>().timeLeft;
            labelTimeProductPrefabs.setText(ChangeTimeToText(timesLeft));
            labelTimeProductPrefabs.refresh();
      //  }
    }

    string ChangeTimeToText(int timeLeftClone)
    {
        dayLeft = timeLeftClone / 24;
        hourLeft = timeLeftClone % 24;
        if (hourLeft <= 0)
        {
            textTimeProductHour = "";
        }
        else if (hourLeft == 1)
        {
            textTimeProductHour = "1 " + FactoryScenesController.languageHungBV["HOUR"];
        }
        else if (hourLeft > 1)
        {
            textTimeProductHour = hourLeft.ToString() + " " + FactoryScenesController.languageHungBV["HOURS"];
        }
        if (dayLeft <= 0)
        {
            textTimeProductDay = "";
        }
        else if (dayLeft == 1)
        {
            textTimeProductDay = "1 " + FactoryScenesController.languageHungBV["DAY"] + " ";
        }
        else if (dayLeft > 1)
        {
            textTimeProductDay = dayLeft.ToString() + " " + FactoryScenesController.languageHungBV["DAYS"] + " ";
        }
        return (textTimeProductDay + textTimeProductHour);
    }
}
