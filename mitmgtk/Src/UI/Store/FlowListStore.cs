using System;
using Gtk;
using Gdk;
using Mitmgtk;
using Mitmgtk.UpdatesPackage;
using NLog;
using System.Collections.Generic;
namespace Mitmgtk
{
	public class FlowListStore : ListStore
	{
		public const int ID_COLUMN = 5;
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private List<Package<Flows>> flows;
		//Default constructor
		public FlowListStore(List<Package<Flows>> flows) : base(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string))
		{
			foreach (Package<Flows> flow in flows)
			{
				this.Add(flow);
			}
		}


		/// <summary>
		/// Update the specified flow.
		/// </summary>
		/// <returns></returns>
		/// <param name="flow">Flow to be updated in the list</param>
		public void Update(Package<Flows> flow)
		{
			try
			{
				bool bUpdated = false;
				TreeIter tmpTreeIter;

				object o;
				this.GetIterFirst(out tmpTreeIter);
				do
				{
					o = this.GetValue(tmpTreeIter, ID_COLUMN);
					//First instance is null
					if (o == null)
					{
						break;
					}
					if (o.ToString() == flow.data.id)
					{

						string[] values = ValuesForFlow(flow);

						this.SetValues(tmpTreeIter,values[0], values[1], values[2],
										  values[3], values[4], values[5]);
						bUpdated = true;
						break;
					}

					if (this.IterNext(ref tmpTreeIter))
					{
						o = this.GetValue(tmpTreeIter, ID_COLUMN);
					}
					else
					{
						o = null;
					}

				} while (o != null);
					
				//Fall back if there was not any previous update
				if (!bUpdated)
					this.Add(flow); // Add some data to the store
			}
			catch (Exception e)
			{
				Console.WriteLine("WARNING: adding to treeview caused exception");
			}
		}

		/// <summary>
		/// Add the specified flow to the tree datasoure.
		/// </summary>
		/// <returns>The add.</returns>
		/// <param name="flow">Flow.</param>
		public void Add(Package<Flows> flow)
		{
				string[] values = ValuesForFlow(flow);

				this.AppendValues(values[0], values[1], values[2],
								  values[3], values[4], values[5]);
			
		}

		public string[] ValuesForFlow(Package<Flows> flow)
		{
			
			string id = "";
			string path = "";
			string method = "";
			string intercepted = "";
			string contentLength = "";
			string time = "";
			if (flow == null || flow.data == null)
			{
				logger.Error("This should not happen");
				return new string[6] { "", "", "", "", "", "" };

			}
			if (flow.data.response != null)
			{
				id = flow.data.id;
				path = flow.data.request.path;
				method = flow.data.request.method;
				intercepted = (flow.data.intercepted ? "Intercepted" : "");
				contentLength = flow.data.response.contentLength;
				time = "" + (Convert.ToInt64(flow.data.response.timestamp_end) - Convert.ToInt64(flow.data.response.timestamp_start));
			}
			else
			{
				id = flow.data.id;
				path = flow.data.request.path;
				method = flow.data.request.method;
				intercepted = (flow.data.intercepted ? "Intercepted" : "");
			}
			logger.Info("id:" + id);
			logger.Info("path:" + path);
			logger.Info("method:" + method);
			logger.Info("intercepted:" + intercepted);
			logger.Info("time:" + time);

			return new string[6] { path, method, intercepted, contentLength, time, id }; 
		}

		
	}
}
