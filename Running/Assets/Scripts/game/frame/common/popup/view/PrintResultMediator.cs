using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PrintResultMediator : PopupContentActivityMediator
{
	private PrintResult printResult;

	private void OnEnable ()
	{
		printResult = popupContent as PrintResult;
		string code = (string)objectList [0];
		string text = (string)objectList [1];
		printResult.code.text = code;
		if ((int)objectList [2] == 1) {
			printResult.text.text = LanguageJP.PRINT_RESULT_END_1;
		} else {
			printResult.text.text = LanguageJP.PRINT_RESULT_END_2;
		}
	}
}
