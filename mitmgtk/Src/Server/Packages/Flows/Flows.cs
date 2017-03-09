using System;
using System.Web.Script.Serialization;

namespace Mitmgtk.UpdatesPackage
{
	public class Flows: DataPackage
	{
		static JavaScriptSerializer serializer = new JavaScriptSerializer();


		public Boolean intercepted { get; set; }

		public String type { get; set; }

		public Boolean modified { get; set; }

		public Boolean marked { get; set; }

		public Connection client_conn { get; set; }

		public Connection server_conn { get; set; }

		public Request request { get; set; }

		public Response response { get; set; }
	
		public Flows(String id, Boolean intercepted, String type, Boolean modified, Boolean marked,
		             String client_conn, String server_conn, String request, String response): base (id)
		{
			this.intercepted = intercepted;
			this.type = type;
			this.modified = modified;
			this.marked = marked;
			this.client_conn = (Connection)serializer.Deserialize(client_conn, typeof(Connection));
			this.server_conn = (Connection)serializer.Deserialize(client_conn, typeof(Connection));

			this.request = (Request)serializer.Deserialize(client_conn, typeof(Request));
			this.response = (Response)serializer.Deserialize(client_conn, typeof(Response));

		}
	}
}
