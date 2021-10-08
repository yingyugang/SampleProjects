using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class MailChecker : MonoBehaviour
{
	public static bool IsEmail (string str_email)
	{
		return Regex.IsMatch (str_email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
	}
}
