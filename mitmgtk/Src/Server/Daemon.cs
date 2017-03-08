using System;
using NLog;
using System.Diagnostics; // Process
using System.IO; // StreamWriter
using System.Threading;

namespace Mitmgtk
{
	public class Daemon
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private void launchServer()
		{
			Process p = new Process();
			p.StartInfo.FileName = "/usr/local/bin/mitmdump";
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.UseShellExecute = false;
			p.OutputDataReceived += (sender, args) => logger.Info(args.Data);
			p.Start();
			p.BeginOutputReadLine();
			p.WaitForExit();
		}

		public Daemon()
		{
			Thread oThread = new Thread(new ThreadStart(this.launchServer));
			oThread.Start();

		}
	}
}
