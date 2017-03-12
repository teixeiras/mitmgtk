using System;
using System.Net;
using System.IO;
using NLog;
using System.Web.Script.Serialization;
namespace Mitmgtk
{
	public class FechData: GenericRequest 
	{
		private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

		static JavaScriptSerializer serializer = new JavaScriptSerializer();

		public const String SERVICE_SETTINGS = "settings";
		public const String SERVICE_FLOWS = "flows";
		public const String SERVICE_EVENTS = "events";
		public static T Fetch<T>(String service)
		{
			try
			{
				String url = "http://" + Settings.instance().defaultHost + "/" + service;
				WebResponse resp = GenericRequest.GetResponse(url);
				using (var reader = new StreamReader(resp.GetResponseStream()))
				{
					String result = reader.ReadToEnd();
					logger.Info(result);
					T package = (T)serializer.Deserialize(result, typeof(T));
					return package;
				}
			}
			catch (Exception e)
			{
				logger.Error(e.Message);
				return default(T);
			}

		}

	}
}
