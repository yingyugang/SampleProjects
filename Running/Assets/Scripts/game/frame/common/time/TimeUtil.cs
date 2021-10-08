using System;
using UnityEngine;
using System.Text;
using System.Collections.Generic;


public class TimeUtil
{
	private static int timeDifference;
	private static readonly List<string> dayList = new List<string> () {
		LanguageJP.DAY0,
		LanguageJP.DAY1,
		LanguageJP.DAY2,
		LanguageJP.DAY3,
		LanguageJP.DAY4,
		LanguageJP.DAY5,
		LanguageJP.DAY6
	};

	public static int GetCurrentLocalTimeStamp ()
	{
		DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime (new DateTime (1970, 1, 1));
		return (int)(DateTime.Now - startTime).TotalSeconds;
	}

	public static DateTime TimestampToDateTime (int timestamp)
	{
		DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime (new DateTime (1970, 1, 1));
		return startTime.AddSeconds (timestamp);
	}

	public static string TimestampToString (int timestamp, string mask)
	{
		DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime (new DateTime (1970, 1, 1));
		DateTime dateTime = startTime.AddSeconds (timestamp);
		return dateTime.ToString (mask);
	}


	public static string TimeToString (int time)
	{
		int leftSeconds = time % (60 * 60);
		int minutes = leftSeconds / 60;
		int seconds = leftSeconds % 60;

		StringBuilder stringBuilder = new StringBuilder ();
		stringBuilder.Append (AddZeroPrefix (minutes));
		stringBuilder.Append (":");
		stringBuilder.Append (AddZeroPrefix (seconds));
		return stringBuilder.ToString ();
	}

	public static string TimeStampToDateString (DateTime dateTime)
	{
		return string.Format ("{0}{1}{2}{3}{4}", dateTime.Year, LanguageJP.DEVIDE, AddZeroPrefix (dateTime.Month), LanguageJP.DEVIDE, AddZeroPrefix (dateTime.Day));
	}

	public static string TimeStampToHourAndMinuteString (DateTime dateTime)
	{
		return string.Format ("{0}{1}{2}", AddZeroPrefix (dateTime.Hour), LanguageJP.COLON, AddZeroPrefix (dateTime.Minute));
	}

	public static string DateToDay (DateTime dateTime)
	{
		return dayList [Convert.ToInt32 (dateTime.DayOfWeek.ToString ("d"))].ToString ();
	}

	public static int GetCurrentServerTimeStamp ()
	{
		return GetCurrentLocalTimeStamp () - GetTimeDifference ();
	}

	public static int GetTimeDifference ()
	{
		if (timeDifference == 0) {
			timeDifference = TimeUtil.GetCurrentLocalTimeStamp () - SystemInformation.GetInstance.current_time;
		}
		return timeDifference;
	}

	public static string AddZeroPrefix (int number)
	{
		if (number < 10) {
			return "0" + number;
		} else {
			return "" + number;
		}
	}

	public static string Seconds2DateTimeString (int seconds)
	{
		DateTime dateTime = TimestampToDateTime (seconds);
		StringBuilder stringBuilder = new StringBuilder ();
		stringBuilder.Append (dateTime.Year);
		stringBuilder.Append ("年");
		if (dateTime.Month < 10) {
			stringBuilder.Append ("0");
		}
		stringBuilder.Append (dateTime.Month);
		stringBuilder.Append ("月");
		if (dateTime.Day < 10) {
			stringBuilder.Append ("0");
		}
		stringBuilder.Append (dateTime.Day);
		stringBuilder.Append ("日 ");
		int hour = dateTime.Hour;
		stringBuilder.Append (hour >= 12 ? "PM " : "AM ");
		if (hour > 12) {
			hour = hour - 12;
			if (hour < 10) {
				stringBuilder.Append ("0");
			}
		}
		stringBuilder.Append (hour);
		stringBuilder.Append (":");
		if (dateTime.Minute < 10) {
			stringBuilder.Append ("0");
		}
		stringBuilder.Append (dateTime.Minute);
		return stringBuilder.ToString ();
	}

}
