using System;
using Gtk;
using Gdk;
using Mitmgtk;
using Mitmgtk.UpdatesPackage;
public partial class MainWindow : Gtk.Window, EventsObserverImpl, FlowsObserverImpl
{
	private static StatusIcon trayIcon;

	private const String WINDOW_NAME = "main";
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
		this.DeleteEvent += delegate { 
			Application.Quit(); 
		};

		WindowsPersistence windowDimension = WindowsPersistence.findByName(WINDOW_NAME);
		if (windowDimension != null)
		{
			this.Resize(windowDimension.width, windowDimension.height);
		}

		trayIcon = new StatusIcon(new Pixbuf("images/tray_icon_ok.png"));
		trayIcon.Visible = true;

		// Show/Hide the window (even from the Panel/Taskbar) when the TrayIcon has been clicked.
		trayIcon.Activate += delegate { 
			this.Visible = !this.Visible; 
		};
		// Show a pop up menu when the icon has been right clicked.
		trayIcon.PopupMenu += OnTrayIconPopup;

		// A Tooltip for the Icon
		trayIcon.Tooltip = "Hello World Icon";


		/*Setup dialog = new Setup
				();
		dialog.Modal = true;
		dialog.AddButton("Close", ResponseType.Close);
		dialog.Response += new ResponseHandler(on_dialog_response);
		dialog.Run();
		dialog.Destroy();*/
		PackagesManager.eventsObserver.Attach(this);
		PackagesManager.flowsObserver.Attach(this);
		updateData();
	}

	void on_dialog_response(object obj, ResponseArgs args)
	{
		Console.WriteLine(args.ResponseId);
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	// Create the popup menu, on right click.
	static void OnTrayIconPopup(object o, EventArgs args)
	{
		Menu popupMenu = new Menu();
		ImageMenuItem menuItemQuit = new ImageMenuItem("Quit");
		Gtk.Image appimg = new Gtk.Image(Stock.Quit, IconSize.Menu);
		menuItemQuit.Image = appimg;
		popupMenu.Add(menuItemQuit);
		// Quit the application when quit has been clicked.
		menuItemQuit.Activated += delegate { 
			Application.Quit(); 
		};
		popupMenu.ShowAll();
		popupMenu.Popup();
	}

	protected override bool OnConfigureEvent(Gdk.EventConfigure args)
	{
		base.OnConfigureEvent(args);
		storeActualWindowDimension();
		return true;
	}

	void storeActualWindowDimension()
	{
		int width, height;
		this.GetSize(out width,out height);
		WindowsPersistence windowDimension = WindowsPersistence.factory(WINDOW_NAME);
		windowDimension.height = height;
		windowDimension.width = width;
		windowDimension.save();

	}


	public void NewEventHasArrived(Package<Events> packageEvent)
	{
		//eventsTextview.Buffer.Text = eventsTextview.Buffer.Text+"\n[" + packageEvent.data.level + "]" + packageEvent.data.message;
	}

	public void NewFlowHasArrived(Package<Flows> packageFlows)
	{
		updateData();
	}


	void updateData()
	{

		Gtk.TreeViewColumn pathColumn = new Gtk.TreeViewColumn();
		pathColumn.Title = "Path";
		Gtk.CellRendererText pathNameCell = new Gtk.CellRendererText();
		pathColumn.PackStart(pathNameCell, true);

		Gtk.TreeViewColumn methodColumn = new Gtk.TreeViewColumn();
		methodColumn.Title = "Method";
		Gtk.CellRendererText methodNameCell = new Gtk.CellRendererText();
		methodColumn.PackStart(methodNameCell, true);

		Gtk.TreeViewColumn statusColumn = new Gtk.TreeViewColumn();
		statusColumn.Title = "Status";
		Gtk.CellRendererText statusNameCell = new Gtk.CellRendererText();
		statusColumn.PackStart(statusNameCell, true);

		Gtk.TreeViewColumn sizeColumn = new Gtk.TreeViewColumn();
		sizeColumn.Title = "Size";
		Gtk.CellRendererText sizeNameCell = new Gtk.CellRendererText();
		sizeColumn.PackStart(sizeNameCell, true);

		Gtk.TreeViewColumn timeColumn = new Gtk.TreeViewColumn();
		timeColumn.Title = "Time";
		Gtk.CellRendererText timeNameCell = new Gtk.CellRendererText();
		timeColumn.PackStart(timeNameCell, true);


		// Add the columns to the TreeView
		flowsTree.AppendColumn(pathColumn);
		flowsTree.AppendColumn(methodColumn);
		flowsTree.AppendColumn(statusColumn);
		flowsTree.AppendColumn(sizeColumn);
		flowsTree.AppendColumn(timeColumn);


		// Tell the Cell Renderers which items in the model to display
		pathColumn.AddAttribute(pathNameCell, "text", 0);
		methodColumn.AddAttribute(methodNameCell, "text", 1);
		statusColumn.AddAttribute(statusNameCell, "text", 2);
		sizeColumn.AddAttribute(sizeNameCell, "text", 3);
		timeColumn.AddAttribute(timeNameCell, "text", 4);


		// Create a model that will hold two strings - Artist Name and Song Title
		Gtk.ListStore flowsListStore = new Gtk.ListStore(typeof(string), typeof(string), typeof(Boolean), typeof(string), typeof(string));

		// Add some data to the store
		foreach (Package<Flows> flow in PackagesManager.GetFlows())
		{
			if (flow.data.response != null)
			{
				flowsListStore.AppendValues(flow.data.request.path,
										flow.data.request.method,
										flow.data.intercepted,
										flow.data.response.contentLength,
											"" + (flow.data.response.timestamp_end - flow.data.response.timestamp_start));
			}
			else
			{
				flowsListStore.AppendValues(flow.data.request.path,
									flow.data.request.method,
									flow.data.intercepted,
									"","");
			
			}

		}



		// Assign the model to the TreeView
		flowsTree.Model = flowsListStore;


	}
}
