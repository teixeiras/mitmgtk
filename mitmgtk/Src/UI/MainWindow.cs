using System;
using Gtk;
using Gdk;
using Mitmgtk;

public partial class MainWindow : Gtk.Window
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
}
