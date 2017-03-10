using System;
using System.Web.Script.Serialization;

namespace Mitmgtk.UpdatesPackage
{
	public class Flows: DataPackage
	{

		public Boolean intercepted { get; set; }

		public String type { get; set; }

		public Boolean modified { get; set; }

		public Boolean marked { get; set; }

		public Connection client_conn { get; set; }

		public Connection server_conn { get; set; }

		public Request request { get; set; }

		public Response response { get; set; }
	
		public Flows()
		{
			
		
		}
	}
}
