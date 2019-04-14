using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using System.Windows.Input;

namespace TestApplication.Model
{
	public class DataEntry
	{
		private SqlConnection _connection = null;

		public DataEntry(SqlConnection connection)
		{
			_connection = connection;
		}

		//public void DataSave(string tableName)
		//{
		//	using (IDbConnection connection = new SqlConnection(Helper.CnnVal("SampleDb"))
		//	{
		//		connection.Query<TestClass>($"INSERT INTO '{tableName}' VALUES");
		//	}		
		//}
		//private string GetQuery(List<string> question ,Dictionary<int,List<string>> answer)
		//{
		//	_connection.Open();
		//	using (SqlCommand command = new SqlCommand())
		//	{
		//		command.Connection = _connection;
		//		command.CommandText = CommandType.Text;
		//		command.CommandText
		//	}




		//		string _query = null;
		//	for(int i=0; ;i++)
		//	{
		//		var _tempAnswers = answer.Where(g => g.Key == i).Select(m => m.Value).FirstOrDefault();
		//		_query += "(" + question[i] + "," + _tempAnswers[0] + "," + _tempAnswers[1] + ",";
		//		if (_tempAnswers.Count >= 3)
		//			_query += _tempAnswers[2];
		//		else
		//			_query+= ""
		//		if (_tempAnswers.Count >= 4)
		//		{
		//			_query += _tempAnswers[3];
		//		}
		//		if (_tempAnswers.Count == 5)
		//		{
		//			_query += _tempAnswers[4];
		//		}

		//	}

		//}
	}
}
