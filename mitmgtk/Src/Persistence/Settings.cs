using System;
using System.Configuration;
using LiteDB;

namespace Mitmgtk
{

	public partial class Settings : BaseClass
	{
		private const String TABLE = "settings";
		static private Settings _instance;
		static public Settings instance()
		{
			if (_instance == null)
			{
				_instance = Settings.getInstance();
				if (_instance == null)
				{
					_instance = new Settings();
				}
			}
			return _instance;

		}

		private static Settings getInstance()
		{
			using (LiteDatabase db = Settings.getDB())
			{
				// Get customer collection
				var col = db.GetCollection<Settings>(TABLE);

				var results = col.FindAll();
				Settings tmp = null;
				foreach (Settings result in results)
				{
					tmp = result;
				}
				return tmp;

			}
		}
		public void save()
		{
			using (LiteDatabase db = Settings.getDB())
			{
				// Get customer collection
				var col = db.GetCollection<Settings>(TABLE);

				if (Settings.getInstance() == null)
				{
					col.Insert(this);
				}
				else
				{
					col.Update(this);
				}
			}
		}


		public Settings()
		{
		}
	}

	public partial class Settings
	{

		const String LogEnabledKey = "logEnabled";
		const String defaultHostKey = "defaultHost";

		private String logEnableConstant { get; set; }
		private String defaultConstantConstant { get; set; }


		[BsonIgnore]
		public String defaultHost
		{
			get
			{
				if (this.defaultConstantConstant != null)
				{
					return this.defaultConstantConstant;
				}
				return ConfigurationManager.AppSettings[defaultHostKey];
			}
			set
			{
				this.defaultConstantConstant = value;
			}
		}

		[BsonIgnore]
		public Boolean logEnabled
		{
			get
			{
				if (this.logEnableConstant != null)
				{
					return Convert.ToBoolean(this.logEnableConstant);
				}
				String value = ConfigurationManager.AppSettings[LogEnabledKey];
				if (value != null)
				{
					return Convert.ToBoolean(value);
				}
				return false;
			}
			set
			{
				this.logEnableConstant = value.ToString();
			}

		}
	}
}