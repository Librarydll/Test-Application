using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using TestApplication.Model;
using System.Windows;
using System.Threading;

namespace TestApplication.ViewModels
{
	public class CreateTestWindowViewModel : Screen
	{
		private string _testName;
		public string TestName
		{
			get { return _testName; }
			set { _testName = value; NotifyOfPropertyChange(() => TestName); }
		}

		public bool CanCreateTestClick(string testName)
		{
			if (string.IsNullOrWhiteSpace(testName))
				return false;
			return true;
		}

		public  void CreateTestClick(string testName)
		{

			var tableNames = HelperSQLIte.GetTableNames();

			if (tableNames.Any(i => i == TestName))
			{
				MessageBox.Show("The test name has already existed", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			HelperSQLIte.GetTestName = TestName;
			HelperSQLIte.CreateTable(testName);
			MessageBox.Show("The test has just successfully created", "About creating", MessageBoxButton.OK, MessageBoxImage.Information);
			TryClose();

		}


	}
}
