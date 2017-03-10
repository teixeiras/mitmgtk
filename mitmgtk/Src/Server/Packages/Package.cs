using System;
using System.Web.Script.Serialization;
using NLog;
namespace Mitmgtk.UpdatesPackage
{
	public class SimplePackage
	{
		public String resource { get; set;}
		public String cmd { get; set; }
		public SimplePackage()
		{
		}
	}

	public class Package<T> : SimplePackage
	{
		public T data { get; set; }
		public Package()
		{
		}
	}
}
