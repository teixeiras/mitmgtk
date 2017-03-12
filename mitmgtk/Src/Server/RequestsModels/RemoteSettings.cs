using System;
using System.Collections.Generic;
namespace Mitmgtk
{
	public class RemoteSettings
	{
		public String version { get; set;}
	   	public String mode { get; set;}
	   	public String intercept { get; set;}
	   	public Boolean showhost { get; set;}
	   	public Boolean no_upstream_cert { get; set;}
	   	public Boolean rawtcp { get; set;}
	   	public Boolean http2 { get; set;}
	   	public Boolean websocket { get; set;}
	   	public Boolean anticache { get; set;}
	   	public Boolean anticomp { get; set;}
	   	public Boolean stickyauth { get; set;}
	   	public Boolean stickycookie { get; set;}
	   	public String stream { get; set;}
	   	public List<String> contentViews  { get; set;}
	   	public String listen_host { get; set;}
	   	public String listen_port { get; set;}

		public RemoteSettings()
		{
		}
	}
}
