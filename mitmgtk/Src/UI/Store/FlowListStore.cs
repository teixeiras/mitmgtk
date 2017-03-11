using System;
using Gtk;
using Gdk;
using Mitmgtk;
using Mitmgtk.UpdatesPackage;
using NLog;

namespace Mitmgtk
{
	public class FlowListStore : ListStore
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public FlowListStore() : base(typeof(string), typeof(string), typeof(Boolean), typeof(string), typeof(string))
		{
		}

		public void Add(Package<Flows> flow)
		{

			String id = "";
			String path = "";
			String method = "";
			String intercepted = "";
			String contentLength = "";
			String time = "";

			if (flow.data.response != null)
			{
				id = flow.data.id;
				path = flow.data.request.path;
				method = flow.data.request.method;
				intercepted = flow.data.intercepted?"Intercepted":"";
				contentLength = flow.data.response.contentLength;
				time = "" + (Convert.ToInt64(flow.data.response.timestamp_end) - Convert.ToInt64(flow.data.response.timestamp_start));
			}
			else
			{
				id = flow.data.id;
				path = flow.data.request.path;
				method = flow.data.request.method;
				intercepted = flow.data.intercepted ? "Intercepted" : "";
			}
			logger.Info("id:" + id);
			logger.Info("path:" + path);
			logger.Info("method:" + method);
			logger.Info("intercepted:" + intercepted);
			logger.Info("time:" + time);

			this.AppendValues(id, path, method, intercepted,
							  contentLength, time);


		}

		
	}
}
