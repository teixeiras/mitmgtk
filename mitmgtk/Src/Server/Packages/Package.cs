using System;
using System.Web.Script.Serialization;
using NLog;
namespace Mitmgtk.UpdatesPackage
{
	public class Package
	{
		private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

		static JavaScriptSerializer serializer = new JavaScriptSerializer();

		const String EVENTS = "events";
		const String FLOWS = "flows";
		String resource { get; set;}
		String cmd { get; set; }
		String data { get; set; }

		DataPackage package
		{
			get
			{
				if (resource.Equals(EVENTS))
				{
					return (DataPackage)serializer.Deserialize(data, typeof(Events));

				}
				else if (resource.Equals(FLOWS))
				{
					return (DataPackage)serializer.Deserialize(data, typeof(Flows));
				}
				else
				{
					logger.Error("Invalid Resource " + resource);
					return null;
				}
			}
		}
		public Package()
		{
		}
	}
}
