using System;
using System.Collections.Generic;

namespace Mitmgtk
{
	public class Response
	{

		public String http_version { get; set; }

		public String status_code { get; set; }

		public String reason { get; set; }

		public Dictionary<String, String> headers { get; set; }

		public String contentLength { get; set; }

		public String contentHash { get; set; }

		public String timestamp_start { get; set; }

		public String timestamp_end { get; set; }

		public Boolean is_replay { get; set; }

		public Response(String http_version, String status_code, String reason, Dictionary<String, String> headers, String contentLength, String contentHash, String timestamp_start, String timestamp_end, Boolean is_replay)
		{
			this.http_version = http_version;
			this.status_code = status_code;
			this.reason = reason;
			this.headers = headers;
			this.contentLength = contentLength;
			this.contentHash = contentLength;
			this.timestamp_start = timestamp_start;
			this.timestamp_end = timestamp_end;
			this.is_replay = is_replay;

		}
	}
}
