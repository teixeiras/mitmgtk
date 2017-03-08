using System;
namespace Mitmgtk
{
	public partial class Setup : Gtk.Dialog
	{
		public Setup()
		{

			/* MessageDialog md = new MessageDialog(this, 
                DialogFlags.DestroyWithParent, MessageType.Question, 
                ButtonsType.Close, "Are you sure to quit?");
            md.Run();
            md.Destroy();*/
			
			this.Build();
		}
	}
}
