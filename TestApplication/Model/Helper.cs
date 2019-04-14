using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using System.Data.SqlClient;
namespace TestApplication.Model
{
	public static class Helper
	{
		public static string GetTestName { get;  set; }
		public static string CnnVal(string name)
		{
			return ConfigurationManager.ConnectionStrings[name].ConnectionString;
		}

		public static IList<string> GetTableNames(string name)
		{
			List<string> result = new List<string>();
			using (IDbConnection connection = new SqlConnection(CnnVal(name)))
			{
				IDataReader reader = connection.ExecuteReader("SELECT NAME FROM sys." + "tables");
				while (reader.Read())
					result.Add(reader["name"].ToString());
				return result;
			}
		}

		public static string GetCreateQuery(string newTableName)
		{

			return @"CREATE TABLE " + newTableName + " " +
					"(Id int NOT NULL PRIMARY KEY," +
					"Question nvarchar(max) NULL," +
					"FirstAnswer nvarchar(max) NULL," +
					"SecondAnswer nvarchar(max) NULL," +
					"ThirdAnswer nvarchar(max) NULL," +
					"ForthAnswer nvarchar(max) NULL," +
					"FifthAnswer nvarchar(max) NULL," +
					"QuestionImage varbinary(max) NULL," +
					"RightAnswer smallint NOT NULL" +
					")";
		}

		public static async Task InsertData(List<TestClass> test)
		{
			if (string.IsNullOrWhiteSpace(GetTestName))
				return;
			using (IDbConnection connection = new SqlConnection(CnnVal("TestDB")))
			{
				foreach (var item in test)
				{
					string sql = "INSERT INTO "  +GetTestName +
				" Values (@Id,@Question,@FirstAnswer,@SecondAnswer,@ThirdAnswer,@ForthAnswer,@FifthAnswer,@QuestionImage,@RightAnswer)";
					await connection.ExecuteAsync(sql, item);
				}
			}
		}
	}
}
