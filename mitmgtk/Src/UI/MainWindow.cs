using System;
using Gtk;
using Gdk;
using Mitmgtk;
using Mitmgtk.UpdatesPackage;
using NLog;
using System.Text;
using System.IO;
using System.Collections.Generic;

public partial class MainWindow : Gtk.Window, EventsObserverImpl, FlowsObserverImpl, ConnectionObserverImp
{
	private static Logger logger = LogManager.GetCurrentClassLogger();

	private const int REQUEST_TAB = 0;
	private const int RESPONSE_TAB = 1;

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

		notebook.GetNthPage(REQUEST_TAB).Hide();
	
		notebook.GetNthPage(RESPONSE_TAB).Hide();

		connection = new Mitmgtk.Connection();
		connection.Attach(this);
		connection.Connect();


		PackagesManager.eventsObserver.Attach(this);
		PackagesManager.flowsObserver.Attach(this);


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

		RemoteSettings settings = FechData.Fetch<RemoteSettings>(FechData.SERVICE_SETTINGS);
		List<Package<Flows>> flows = FechData.Fetch<List<Package<Flows>>>(FechData.SERVICE_FLOWS);
		if (flows != null)
		{
			PackagesManager.flows = flows;
		}
		initializeFlowListView(PackagesManager.flows);
		/*
		List<Package<Events>> events = FechData.Fetch<List<Package<Events>>>(FechData.SERVICE_EVENTS);
		if (events != null)
		{
			PackagesManager.events = events;
			String str = "";
			foreach (Package<Events> package in events)
			{
				str += "\n[" + package.data.level + "]" + package.data.message; ;
			}
			eventsTextview.Buffer.Text = str;
		}*/

	}

	public void NewEventHasArrived(Package<Events> packageEvent)
	{
		Application.Invoke(delegate
		{
			TextIter mIter = eventsTextview.Buffer.EndIter;
			String str = "\n[" + packageEvent.data.level + "]" + packageEvent.data.message; ;
			eventsTextview.Buffer.Insert(ref mIter, str);
		});


	}
	public void FlowHasBeenUpdated(Package<Flows> packageFlows)
	{
		Application.Invoke(delegate
		{
			store.Update(packageFlows);
		});
	}

	public void NewFlowHasArrived(Package<Flows> packageFlows)
	{
		Application.Invoke(delegate
		{
			store.Add(packageFlows);
		});

	}


	void initializeFlowListView(List<Package<Flows>> flows)
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

		store = new FlowListStore(flows);
		flowsTree.Model = store;

		flowsTree.Selection.Changed += (sender, e) =>
		{
			logger.Info("SELECTION WAS CHANGED");
			Gtk.TreeIter selected;
			if (flowsTree.Selection.GetSelected(out selected))
			{
				logger.Info("SELECTED ITEM: {0}", store.GetValue(selected, FlowListStore.ID_COLUMN));
				ContentRequest.Content request = ContentRequest.GetContent((String)store.GetValue(selected, FlowListStore.ID_COLUMN),
														   ContentRequest.RequestType.request);

				if (request != null)
				{
					string requestString = System.Text.Encoding.UTF8.GetString(request.contentByte);
					requestTextView.Buffer.Text = requestString;
					requestTextView.WrapMode = WrapMode.WordChar;
					notebook.GetNthPage(REQUEST_TAB).Show();
				}
				else
				{
					notebook.GetNthPage(REQUEST_TAB).Hide();
				}


				ContentRequest.Content response = ContentRequest.GetContent((String)store.GetValue(selected, FlowListStore.ID_COLUMN),
				                                                            ContentRequest.RequestType.response);
				if (response != null)
				{
					DisplayContent(response);
					notebook.GetNthPage(RESPONSE_TAB).Show();
				}
				else
				{
					notebook.GetNthPage(RESPONSE_TAB).Hide();
				}
			}
		};

		TreeIter iter;
		if (store.GetIterFirst(out iter))
			flowsTree.Selection.SelectIter(iter);



	}

	protected void DisplayContent(ContentRequest.Content content)
	{
		try
		{
			logger.Info("Content Size:" + content.contentByte.Length);

			using (var reader = new StreamReader(new MemoryStream(content.contentByte), Encoding.Default))
			{
				// read the contents of the file into a string 
				Encoding utf8 = Encoding.UTF8;
				byte[] isoBytes = Encoding.Convert(reader.CurrentEncoding, utf8, content.contentByte);
				string msg = utf8.GetString(isoBytes);
				string responseString = msg;
				responseTextView.WrapMode = WrapMode.WordChar;
				responseTextView.Buffer.Text = responseString;
			}
		}
		catch (Exception e)
		{
			logger.Error("Could not be open as text:" + e.Message);
		}

	}

	protected void ExitAction(object sender, EventArgs e)
	{
		Application.Quit();
	}

}
