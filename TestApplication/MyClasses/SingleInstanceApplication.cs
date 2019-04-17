using System;
using System.Windows;
using Microsoft.VisualBasic.ApplicationServices;

namespace TestApplication.MyClasses
{
	public class SingleInstanceApplication : WindowsFormsApplicationBase
	{

		public SingleInstanceApplication()
		{
			this.IsSingleInstance = true;
		}

		protected override bool OnStartup(Microsoft.VisualBasic.ApplicationServices.StartupEventArgs eventArgs)
		{
			try
			{
				string extension = ".test";
				string title = "TestApplication";
				string extensionDescription = "A Test Document";
				ExtensionRegisterHelper.SetFileAssociation(extension, title + "." + extensionDescription);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			return false;
		}
	}
}
