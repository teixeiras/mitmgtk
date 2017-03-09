using System;
namespace Mitmgtk.UpdatesPackage
{
	public class Events : DataPackage
	{
		public String message { get; set; }
		public String level { get; set; }

		public Events(String id, String message, String level) :base(id)
		{
			this.message = message;
			this.level = level;
		}
	}
}
