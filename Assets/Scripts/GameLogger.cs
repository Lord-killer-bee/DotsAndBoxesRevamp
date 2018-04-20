using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Logger
{
	public static class GameLogger
	{
		private static ELoggingStatus m_logStatus = ELoggingStatus.ENABLE_LOGGING;

		public static void LogMessage(string message)
		{
			switch (m_logStatus)
			{
				case ELoggingStatus.ENABLE_LOGGING:

					Debug.Log(message);

					break;
				case ELoggingStatus.DISABLE_LOGGING:
					break;
				default:
					break;
			}
		}

		public static void SetLogStatus(ELoggingStatus logStatus)
		{
			m_logStatus = logStatus;
		}

		public enum ELoggingStatus
		{
			ENABLE_LOGGING,
			DISABLE_LOGGING
		}
	}

}