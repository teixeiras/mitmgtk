using System;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
namespace Mitmgtk
{
	public class FechData
	{
		static JavaScriptSerializer serializer = new JavaScriptSerializer();

		public static bool SendAnSMSMessage(Object serializable)
		{
			var httpWebRequest = WebRequest.Create("https://apiURL");
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "PUT";

			using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				string json = serializer.Serialize(serializable);
				streamWriter.Write(json);
			}

			var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
			{
				var responseText = streamReader.ReadToEnd();
				//Now you have your response.
				//or false depending on information in the response
				return true;
			}
		}

		public FechData()
		{
		}
	}
}
