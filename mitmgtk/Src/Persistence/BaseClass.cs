using System;
using LiteDB;
namespace Mitmgtk
{
	public class BaseClass
	{
		[BsonId]
		public ObjectId id { get; set; }

		protected static LiteDatabase getDB()
		{
			return new LiteDatabase("db.db");
		}


		public BaseClass()
		{
		}	
		
	}
}
