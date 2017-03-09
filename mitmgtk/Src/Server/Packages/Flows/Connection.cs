using System;
namespace Mitmgtk.UpdatesPackage
{
	public class Connection
	{
		"client_conn": {
      "address": {
        "address": [
          "192.168.2.3",
          53727
        ],
        "use_ipv6": false
      },
      "ssl_established": true,
      "clientcert": null,
      "timestamp_start": 1489067785.0445,
      "timestamp_ssl_setup": 1489067785.4294,
      "timestamp_end": null,
      "sni": "mesu.apple.com",
      "cipher_name": "ECDHE-RSA-AES128-GCM-SHA256",
      "alpn_proto_negotiated": "http\/1.1",
      "tls_version": "TLSv1.2"
    },
    "server_conn": {
      "address": {
        "address": [
          "mesu.apple.com",
          443
        ],
        "use_ipv6": false
      },
      "ip_address": {
        "address": [
          "17.253.37.205",
          443
        ],
        "use_ipv6": false
      },
      "source_address": {
        "address": [
          "172.18.2.250",
          51242
        ],
        "use_ipv6": false
      },
      "ssl_established": true,
      "sni": "mesu.apple.com",
      "alpn_proto_negotiated": "http\/1.1",
      "timestamp_start": 1489067785.0985,
      "timestamp_tcp_setup": 1489067785.2734,
      "timestamp_ssl_setup": 1489067785.3764,
      "timestamp_end": null,
      "via": null
    },

		public Connection()
		{
		}
	}
}
