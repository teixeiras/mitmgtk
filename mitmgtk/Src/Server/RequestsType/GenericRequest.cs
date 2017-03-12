using System;
using System.Net;
using System.IO;
using NLog;
using System.Net.Mime;
namespace Mitmgtk
{
	public class GenericRequest
	{
		
		private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

		public enum RequestType
		{
			response, request
		};



		public static HttpWebResponse GetResponse(String url)
		{
			try
			{
				HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
				logger.Info(req.RequestUri.AbsolutePath);
				req.CookieContainer = new CookieContainer();
				foreach (Cookie cookie in CookiesManager.cookies)
				{
					cookie.Domain = req.RequestUri.Host;
					req.CookieContainer.Add(cookie);
				}

				req.Method = "GET";
				WebResponse resp = req.GetResponse();

				return (HttpWebResponse)resp;
			}
			catch (Exception e)
			{
				logger.Error(e.Message);
				return null;
			}

		}

	}
}
