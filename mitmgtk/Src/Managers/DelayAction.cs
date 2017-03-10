using System;
using System.Timers;

namespace Mitmgtk
{
	public class DelayAction
	{
		Action action;
		Timer timer = new Timer();

		public DelayAction(int millisecond, Action action)
		{
			timer.Interval = millisecond;
			this.action = action;
			timer.Elapsed += timer_Tick;
			timer.Enabled = true;
		}
			
		void timer_Tick(object sender, ElapsedEventArgs e)
		{
			action.Invoke();
			timer.Stop();

		}
	}

}
