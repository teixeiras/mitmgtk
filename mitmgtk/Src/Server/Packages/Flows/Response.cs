using System;
using System.Collections.Generic;

namespace Mitmgtk
{
	public class Response
	{

		public String http_version { get; set; }

		public String status_code { get; set; }

		public String reason { get; set; }

		public List<List<String>> headers { get; set; }

		public String contentLength { get; set; }

		public String contentHash { get; set; }

		public Double timestamp_start { get; set; }

		public Double timestamp_end { get; set; }

		public Boolean is_replay { get; set; }

		public Response()
		{
			

		}
	}
}
