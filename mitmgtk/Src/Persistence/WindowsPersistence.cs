using System;
using LiteDB;

namespace Mitmgtk
{
	public class WindowsPersistence : BaseClass
	{
		private const String TABLE = "windows";
		public String name { get; set; }
		public int height { get; set; }
		public int width { get; set; }
		public WindowsPersistence()
		{
		}

		public static WindowsPersistence factory(String name)
		{
			WindowsPersistence obj = findByName(name);
			if (obj == null) {
				obj = new WindowsPersistence();
				obj.name = name;
			}
			return obj;

		}
		public void save()
		{
			using (LiteDatabase db = getDB())
			{
				// Get customer collection
				var col = db.GetCollection<WindowsPersistence>(TABLE);

				if (findByName(this.name) == null)
				{
					col.Insert(this);

				}
				else
				{
					col.Update(this);

				}		
				col.EnsureIndex(x => x.name, true);


			}
		}

		public static WindowsPersistence findByName(String name)
		{
			using (LiteDatabase db =  getDB())
			{
				// Get customer collection
				var col = db.GetCollection<WindowsPersistence>(TABLE);

				var results = col.Find(x => x.name == name);
				WindowsPersistence tmp = null;
				foreach (WindowsPersistence result in results)
				{
					tmp = result;
				}	
				return tmp;

			}
		}

	}


}
