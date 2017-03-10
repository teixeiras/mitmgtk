using System;
using Gtk;
using NLog;
namespace Mitmgtk
{
	class MainClass
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public static void Main(string[] args)
		{
			Application.Init();
			GLib.ExceptionManager.UnhandledException += (GLib.UnhandledExceptionArgs errorArg) =>
			{
				logger.Error(errorArg.ExceptionObject.ToString);
				logger.Error(new System.Diagnostics.StackTrace(true));
			};

			//Check if log is enabled or not
			if (!Settings.instance().logEnabled)
			{
				LogManager.DisableLogging();
			}


			MainWindow win = new MainWindow();
			win.Show();
			//Daemon daemon = new Daemon();
			Application.Run();
		}
	}

}
