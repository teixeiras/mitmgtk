using System;
using System.Net;
using System.IO;
using NLog;
using System.Net.Mime;
namespace Mitmgtk
{
	public class ContentRequest : GenericRequest
	{

		public class Content
		{
			public ContentType contentType { get; set;}
			public byte[] contentByte { get; set; }
		}

		private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

		public static byte[] ReadAllBytes(BinaryReader reader)
		{
			const int bufferSize = 4096;
			using (var ms = new MemoryStream())
			{
				byte[] buffer = new byte[bufferSize];
				int count;
				while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
					ms.Write(buffer, 0, count);
				return ms.ToArray();
			}

		}

		public static Content GetContent(String id, GenericRequest.RequestType requestEnum)
		{
			String request = (requestEnum == RequestType.request ? "request" : "response");
			Content content = new Content();
			try{
				String url = "http://" + Settings.instance().defaultHost + "/flows/" + System.Uri.EscapeDataString(id) + "/" + request + "/content";
				WebResponse resp = GenericRequest.GetResponse(url);

				content.contentType = new ContentType(resp.ContentType);

				using (var reader = new BinaryReader(resp.GetResponseStream()))
				{
					content.contentByte = ReadAllBytes(reader);
				}
				logger.Info(System.Text.Encoding.UTF8.GetString(content.contentByte));
			}
			catch (Exception e)
			{
				logger.Error(e.Message);
				return null;
			}
			return content;

		}

	}
}
