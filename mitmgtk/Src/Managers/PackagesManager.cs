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
		void FlowHasBeenUpdated(Package<Flows> packageFlows);


	}


	public class PackagesManager
	{

		public static EventsObserver eventsObserver = new EventsObserver();
		public static FlowsObserver flowsObserver = new FlowsObserver();

		public static List<Package<Flows>> flows = new List<Package<Flows>>();
		public static List<Package<Events>> events = new List<Package<Events>>();

		private const String CMD_UPDATE = "update";
		private const String CMD_ADD = "add";


		public class EventsObserver : Observers<EventsObserverImpl, Package<Events>>
		{
			override public void callBack(EventsObserverImpl observer, Package<Events> package)
			{
				observer.NewEventHasArrived(package);
			}
		}

		/// <summary>
		/// Flows observer.
		/// Will notify all the listener from news events 
		/// </summary>
		public class FlowsObserver : Observers<FlowsObserverImpl, Package<Flows>>
		{
			override public void callBack(FlowsObserverImpl observer, Package<Flows> package)
			{
				if (package.cmd.Equals(CMD_ADD))
				{
					flows.Add(package);

					observer.NewFlowHasArrived(package);
				}
				else
				{
					
					Package<Flows> result = flows.Find(x => x.data.id == package.data.id);
					if (result != null)
					{
						int index = flows.IndexOf(result);
						if (index != -1)
							flows[index] = package;
						
						observer.FlowHasBeenUpdated(package);
					}
					else
					{
						flows.Add(package);

						observer.NewFlowHasArrived(package);	
					}



				}
			
			}
		}

	
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
			
			flowsObserver.Notify(package);
		}

		public static void AddEvents(Package<Events> package)
		{
			events.Add(package);
			eventsObserver.Notify(package);

		}
	}
}
