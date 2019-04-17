using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using Dapper;
using System.Data;

namespace TestApplication.Model
{
	public static class HelperSQLIte
	{
		public static string DbFile
		{
			get
			{
				return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Resources\\QuestionDB.sqlite";
			}
		}
		public static string GetTestName { get; set; }
		public static SQLiteConnection SimpleDbConnection()
		{
			return new SQLiteConnection("Data Source=" + DbFile);
		}
		public static void CreateTable(string tableName)
		{
			using (var cnn = SimpleDbConnection())
			{
				cnn.Open();
				cnn.Execute(
				GetQueryCreateTable(tableName));
			}
		}

		public static void SaveData(List<Questions> test)
		{
			if (string.IsNullOrWhiteSpace(HelperSQLIte.GetTestName))
				return;
			using (var cnn = SimpleDbConnection())
			{
				cnn.Open();
				foreach (var item in test)
				{
					string sql = @"INSERT INTO " +GetTestName +
				@" Values (@Id,@Question,@FirstAnswer,@SecondAnswer,@ThirdAnswer,@ForthAnswer,@FifthAnswer,@QuestionImagePath,@RightAnswer)";
					cnn.Execute(sql, item);
				}

			}
		}

		public static IList<string> GetTableNames()
		{
			List<string> result = new List<string>();	
			using (var cnn = SimpleDbConnection())
			{
				IDataReader reader = cnn.ExecuteReader("SELECT name FROM sqlite_master WHERE type='table'");
				while (reader.Read())
					result.Add(reader["name"].ToString());
			}
			return result;
		}
		public static string GetQueryCreateTable(string tableName)
		{
			return @"CREATE TABLE " + tableName + " " +
					"(Id int NOT NULL PRIMARY KEY," +
					"Question nvarchar(200) NULL," +
					"FirstAnswer nvarchar(200) NULL," +
					"SecondAnswer nvarchar(200) NULL," +
					"ThirdAnswer nvarchar(200) NULL," +
					"ForthAnswer nvarchar(200) NULL," +
					"FifthAnswer nvarchar(200) NULL," +
					"QuestionImage string(200) NULL," +
					"RightAnswer smallint NOT NULL" +
					")";
		}
	
	}
}
