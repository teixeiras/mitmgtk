﻿using System;
using WebSocketSharp;
using System.Net;
using NLog;
using System.Web.Script.Serialization;
using Mitmgtk.UpdatesPackage;
//https://www.dropbox.com/s/c5ckquxwiuff2l6/Screenshot%202017-03-09%2014.40.33.png?dl=0
//https://www.dropbox.com/s/9sfh0v0d6gyyzwh/Screenshot%202017-03-09%2014.41.02.png?dl=0
//https://www.dropbox.com/s/libh8oy4cykt47j/Screenshot%202017-03-09%2014.41.19.png?dl=0
//https://www.dropbox.com/s/5zr5x5njq9rvt3p/Screenshot%202017-03-09%2014.41.36.png?dl=0
namespace Mitmgtk
{
	public class Connection
	{
		const String EVENTS = "events";
		const String FLOWS = "flows";


		private static NLog.Logger logger = LogManager.GetCurrentClassLogger();
		static JavaScriptSerializer serializer = new JavaScriptSerializer();
		int retries = 0;
		static WebSocket socket;
		public Connection()
		{
		}

		public void updateCookiesCookies(WebSocket socket)
		{
			string cookieHeader;
			WebRequest req = WebRequest.Create("http://" + Settings.instance().defaultHost);
			req.ContentType = "application/x-www-form-urlencoded";
			req.Method = "GET";
			WebResponse resp = req.GetResponse();
			cookieHeader = resp.Headers["Set-cookie"];

			foreach (Cookie localCookie in CookiesManager.GetHttpCookiesFromHeader(cookieHeader))
			{
				WebSocketSharp.Net.Cookie cookie = new WebSocketSharp.Net.Cookie(localCookie.Name, localCookie.Value);
				socket.SetCookie(cookie);
			}
		}

		public void Connect()
		{
			try
			{
				logger.Info("Connecting to: " + Settings.instance().defaultHost + "/updates");
				socket = new WebSocket("ws://" + Settings.instance().defaultHost + "/updates");
				updateCookiesCookies(socket);
				socket.OnError += (sender, e) =>
				{
					logger.Error(e.Message);
				};
				socket.OnMessage += (sender, e) =>
				{
					try
					{
						logger.Info("Request arrived in updates tunnel:" + e.Data);
						SimplePackage package = (SimplePackage)serializer.Deserialize(e.Data, typeof(SimplePackage));
						SimplePackage completePackage = null;

						if (package.resource.Equals(EVENTS))
						{
							completePackage = (Package<Events>)serializer.Deserialize(e.Data, typeof(Package<Events>));
						}
						else if (package.resource.Equals(FLOWS))
						{
							completePackage = (Package<Flows>)serializer.Deserialize(e.Data, typeof(Package<Flows>));
						}
						else
						{
							logger.Error("Invalid Resource " + package.resource);
						}

						logger.Info(completePackage);
					}
					catch (Exception ex)
					{
						logger.Error(ex.Message);
					}	

				};
				socket.Connect();

			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public void send(String message)
		{
			try
			{
				socket.Send(message);
				retries = 0;
			}
			catch (Exception e)
			{
				logger.Error(e.Message);
				this.Connect();
				this.send(message);
				retries++;
			}
		}

	}
}
