using System;
using WebSocketSharp;
using NLog;
namespace Mitmgtk
{
	public class Connection
	{

		int retries = 0;

		static WebSocket socket;
		private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

		public Connection()
		{

		}

		public void Connect() 
		{
			try
			{
				logger.Info("Connecting to: " + Settings.instance().defaultHost + "/updates");
				socket = new WebSocket(Settings.instance().defaultHost);
				socket.OnError += (sender, e) =>
				{
					logger.Error(e.Message);
				};
				socket.OnMessage += (sender, e) =>
					Console.WriteLine("Laputa says: " + e.Data);

				socket.Connect();

			}catch (Exception e)
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
