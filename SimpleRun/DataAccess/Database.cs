using System;
using System.IO;
using SQLite.Net;
using SimpleRun.Models;

#if __Android__
using SQLite.Net.Platform.XamarinAndroid;
#else
using SQLite.Net.Platform.XamarinIOS;
#endif


namespace SimpleRun.DataAccess
{
	public class Database : SQLiteConnection
	{
		static public Database Main { get; private set; }

		static public string DBPath {
			get {
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "simplerun.db");
			}
		}

		static Database()
		{
			Main = new Database(DBPath);
		}
			
#if __Android__
		protected Database(string path) : base(new SQLitePlatformAndroid(), path)
		{
			CreateTable<Run>();
			CreateTable<RunPosition>();
		}
#else
		protected Database(string path) : base(new SQLitePlatformIOS(), path)
		{
			CreateTable<Run>();
			CreateTable<RunPosition>();
			CreateTable<Settings>();
		}
#endif
	}
}

