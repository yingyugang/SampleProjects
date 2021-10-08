using UnityEngine;
using UnityEngine.UI;

public class ExceptionCatcher : MonoBehaviour
{
	public Text textField;
	public Button clearBtn;
	public Button closeBtn;
	public ScrollRect scrollRect;

	private void Start ()
	{
		Application.logMessageReceived += LogMessageReceivedHandler;
		clearBtn.onClick.AddListener (ClearBtnOnClickHandler);
		closeBtn.onClick.AddListener (CloseBtnOnClickHandler);
	}

	private void LogMessageReceivedHandler (string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Error || type == LogType.Exception) {
			Show (condition, stackTrace);
		}
	}
		
	private void ClearBtnOnClickHandler ()
	{
		textField.text = "";
	}

	private void CloseBtnOnClickHandler ()
	{
		HideView ();
	}

	private void Show (string message, string stackTrace = "")
	{
		ShowView ();
		textField.text += "Exception:\n\t\t\t\t[message:" + message + "]\n\t\t\t\t[stackTrace:" + stackTrace + "]\n\n";
	}

	private void ShowView ()
	{
		ShowOrHideView (scrollRect, true);
	}

	private void HideView ()
	{
		ShowOrHideView (scrollRect, false);
	}

	private void ShowOrHideView (MonoBehaviour monoBehaviour, bool isShow)
	{
		monoBehaviour.gameObject.SetActive (isShow);
	}

	private void OnDestroy ()
	{
		Application.logMessageReceived -= LogMessageReceivedHandler;
		clearBtn.onClick.RemoveListener (ClearBtnOnClickHandler);
		closeBtn.onClick.RemoveListener (CloseBtnOnClickHandler);
		clearBtn.onClick = null;
		closeBtn.onClick = null;
	}
}
