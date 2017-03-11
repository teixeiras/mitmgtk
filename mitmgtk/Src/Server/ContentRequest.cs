using System;
using System.Net;
using System.IO;
using NLog;
namespace Mitmgtk
{
	public class ContentRequest
	{
		private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

		public enum RequestType
		{
			response, request
		};
		public ContentRequest()
		{
		}

		static public String getContent(String id, RequestType requestEnum)
		{
			String request = (requestEnum == RequestType.request ? "request" : "response");
			try
			{
				
				WebRequest req = WebRequest.Create(
					"http://" + Settings.instance().defaultHost + "/flows/" + System.Uri.EscapeDataString(id) + "/" + request + "/content");
				logger.Error("http://" + Settings.instance().defaultHost + "/flows/" + System.Uri.EscapeDataString(id) + "/" + request + "/content");

				req.Method = "GET";
				WebResponse resp = req.GetResponse();

				using (var reader = new StreamReader(resp.GetResponseStream()))
				{
					return reader.ReadToEnd(); // do something fun...
				}
			}
			catch (Exception e)
			{
				logger.Error(e.Message);
				return "";
			}

		}

	}
}
