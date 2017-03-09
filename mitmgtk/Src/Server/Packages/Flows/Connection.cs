using System;
namespace Mitmgtk.UpdatesPackage
{
	public class Connection
	{
		public class Address
		{
			public String[] address { get; set; }
			public Boolean use_ipv6 { get; set; }
			public Address(String[] address, Boolean use_ipv6)
			{
				this.address = address;
				this.use_ipv6 = use_ipv6;
			}
		}
		public Address address { get; set; }
		public Address ip_address { get; set; }
		public Address source_address { get; set; }
		public Boolean ssl_established { get; set; }
		public String sni { get; set; }
		public String alpn_proto_negotiated { get; set; }
		public String timestamp_start { get; set; }
		public String timestamp_tcp_setup { get; set; }
		public String timestamp_ssl_setup { get; set; }
		public String timestamp_end { get; set; }
		public String via { get; set; }
		public String clientcert { get; set; }
		public String cipher_name { get; set; }

		public String tls_version { get; set; }


		public Connection()
		{
		}
	}
}
