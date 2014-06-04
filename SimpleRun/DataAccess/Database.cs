using System;
using System.IO;
using SQLite;
using SimpleRun.Models;

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

		protected Database(string path) : base(path)
		{
			CreateTable<Run>();
		}
	}
}

