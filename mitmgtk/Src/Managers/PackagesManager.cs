using System;
using Mitmgtk.UpdatesPackage;
using System.Collections.Generic;
namespace Mitmgtk
{
	public interface EventsObserverImpl 
	{
		void NewEventHasArrived(Package<Events> packageEvent);
	}

	public interface FlowsObserverImpl
	{
		void NewFlowHasArrived(Package<Flows> packageFlows);
	}


	public class PackagesManager
	{
		public class EventsObserver : Observers<EventsObserverImpl, Package<Events>>
		{
			override public void callBack(EventsObserverImpl observer, Package<Events> package)
			{
				observer.NewEventHasArrived(package);
			}
		}

		public class FlowsObserver : Observers<FlowsObserverImpl, Package<Flows>>
		{
			override public void callBack(FlowsObserverImpl observer, Package<Flows> package)
			{
				observer.NewFlowHasArrived(package);
			}
		}

		public static EventsObserver eventsObserver = new EventsObserver();

		public static FlowsObserver flowsObserver = new FlowsObserver();

		private static List<Package<Flows>> flows = new List<Package<Flows>>();
		private static List<Package<Events>> events = new List<Package<Events>>();

		public static List<Package<Flows>>GetFlows()
		{
			return flows;
		}

		public static List<Package<Events>> GetEvents()
		{
			return events;
		}
		public static void AddFlow(Package<Flows> package) 
		{
			flows.Add(package);
			flowsObserver.Notify(package);
		}

		public static void AddEvents(Package<Events> package)
		{
			events.Add(package);
			eventsObserver.Notify(package);

		}
	}
}
