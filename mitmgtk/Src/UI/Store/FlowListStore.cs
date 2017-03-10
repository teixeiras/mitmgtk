using System;
using Gtk;
using Gdk;
using Mitmgtk;
using Mitmgtk.UpdatesPackage;

namespace Mitmgtk
{
	public class FlowListStore : ListStore
	{
		public FlowListStore() : base(typeof(string), typeof(string), typeof(Boolean), typeof(string), typeof(string))
		{
			Update();
		}
		public void Update()
		{
			this.Clear();
			foreach (Package<Flows> flow in PackagesManager.GetFlows())
			{
				if (flow.data.response != null)
				{
					this.AppendValues(flow.data.request.path,
											flow.data.request.method,
											flow.data.intercepted,
											flow.data.response.contentLength,
												"" + (flow.data.response.timestamp_end - flow.data.response.timestamp_start));
				}
				else
				{
					this.AppendValues(flow.data.request.path,
										flow.data.request.method,
										flow.data.intercepted,
										"", "");
				}

			}

		}

	}
}
