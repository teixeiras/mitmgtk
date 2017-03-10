using System;
using Gtk;
using Gdk;
using Mitmgtk;
using Mitmgtk.UpdatesPackage;

public partial class MainWindow : Gtk.Window, EventsObserverImpl, FlowsObserverImpl, ConnectionObserverImp
{
	private static StatusIcon trayIcon;
	private FlowListStore store;
	private const String WINDOW_NAME = "main";
	private Mitmgtk.Connection connection;
	private Menu popupMenu = new Menu();


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

		stateIcon.Stock = Stock.No;
		stateLabel.Text = "Disconnected";

		/*Setup dialog = new Setup
				();
		dialog.Modal = true;
		dialog.AddButton("Close", ResponseType.Close);
		dialog.Response += new ResponseHandler(on_dialog_response);
		dialog.Run();
		dialog.Destroy();*/

		InitializeTray();

		connection = new Mitmgtk.Connection();
		connection.Attach(this);
		connection.Connect();

		initializeFlowListView();

		PackagesManager.eventsObserver.Attach(this);
		PackagesManager.flowsObserver.Attach(this);

		store = new FlowListStore();
		flowsTree.Model = store;

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

	private void InitializeTray()
	{
		trayIcon = new StatusIcon();
		trayIcon.Visible = true;

		// Show/Hide the window (even from the Panel/Taskbar) when the TrayIcon has been clicked.
		trayIcon.Activate += delegate
		{
			this.Visible = !this.Visible;
		};
		// Show a pop up menu when the icon has been right clicked.
		trayIcon.PopupMenu += OnTrayIconPopup;

		// A Tooltip for the Icon

		ImageMenuItem menuItemQuit = new ImageMenuItem("Quit");
		Gtk.Image appimg = new Gtk.Image(Stock.Quit, IconSize.Menu);
		menuItemQuit.Image = appimg;
		popupMenu.Add(menuItemQuit);
		menuItemQuit.Activated += delegate
		{
			Application.Quit();
		};

	}

	// Create the popup menu, on right click.
	void OnTrayIconPopup(object o, EventArgs args)
	{
		popupMenu.ShowAll();
		popupMenu.Popup();
	}

	protected override bool OnConfigureEvent(EventConfigure args)
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

	public void connectionStatusChanged(bool isConnected)
	{
		if (isConnected)
		{
			stateIcon.Stock = Stock.Yes;
			trayIcon.Stock = Stock.Yes;
			stateLabel.Text = "Connected";
			trayIcon.Tooltip = "Connected";

		}
		else
		{
			stateIcon.Stock = Stock.No;
			trayIcon.Stock = Stock.No;
			stateLabel.Text = "Disconnected";
			trayIcon.Tooltip = "Disconnected";
		}


	}

	public void NewEventHasArrived(Package<Events> packageEvent)
	{
		String str = "";
		foreach (Package<Events> eventPackage in PackagesManager.GetEvents())
		{
			str += "\n[" + eventPackage.data.level + "]" + eventPackage.data.message;
		}
	
		//eventsTextview.Buffer.Text = str;
	}

	public void NewFlowHasArrived(Package<Flows> packageFlows)
	{
		updateData();
	}


	void updateData()
	{
		store.Update();

	}

	void initializeFlowListView()
	{
		TreeViewColumn pathColumn = new TreeViewColumn();
		pathColumn.Title = "Path";
		CellRendererText pathNameCell = new CellRendererText();
		pathColumn.PackStart(pathNameCell, true);

		TreeViewColumn methodColumn = new Gtk.TreeViewColumn();
		methodColumn.Title = "Method";
		CellRendererText methodNameCell = new CellRendererText();
		methodColumn.PackStart(methodNameCell, true);

		TreeViewColumn statusColumn = new TreeViewColumn();
		statusColumn.Title = "Status";
		CellRendererText statusNameCell = new CellRendererText();
		statusColumn.PackStart(statusNameCell, true);

		TreeViewColumn sizeColumn = new TreeViewColumn();
		sizeColumn.Title = "Size";
		CellRendererText sizeNameCell = new CellRendererText();
		sizeColumn.PackStart(sizeNameCell, true);

		TreeViewColumn timeColumn = new TreeViewColumn();
		timeColumn.Title = "Time";
		CellRendererText timeNameCell = new CellRendererText();
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
	}

	protected void ExitAction(object sender, EventArgs e)
	{
		Application.Quit();
	}

}
