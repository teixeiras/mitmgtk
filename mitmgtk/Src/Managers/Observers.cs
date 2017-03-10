using System;
using System.Collections.Generic;

namespace Mitmgtk
{
	public abstract class Observers<Observer, Argument>
	{
		private List<Observer> _observers = new List<Observer>();

		public void Attach(Observer observer)
		{
			_observers.Add(observer);
		}

		public void Detach(Observer observer)
		{
			_observers.Remove(observer);
		}

		public void Notify(Argument argument)
		{
			foreach (Observer o in _observers)
			{
				callBack(o, argument);
			}
		}

		abstract public void callBack(Observer observer, Argument argument);
	}
}
