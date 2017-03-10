using System;
using System.Collections.Generic;

namespace Mitmgtk.UpdatesPackage
{
	public class Request
	{
		public String method { get; set; }
		public String scheme { get; set; }
		public String host { get; set; }
		public String port { get; set; }
		public String path { get; set; }
		public String http_version { get; set; }
		public List<List<String>> headers { get; set; }


		public String contentLength { get; set; }
		public String contentHash { get; set; }
		public String timestamp_start { get; set; }
		public String timestamp_end { get; set; }
		public Boolean is_replay { get; set; }

		public Request()
		{
		}
	}
}
