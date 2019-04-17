using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
namespace TestApplication.MyClasses
{
	public class ExtensionRegisterHelper
	{
		public static void SetFileAssociation(string extension,string progID)
		{
			SetValue(Registry.ClassesRoot, extension, progID);

			string assemblyFullPath = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("/", @"\");

			StringBuilder sbShellEntry = new StringBuilder();
			sbShellEntry.AppendFormat("\"{0}\" \"%1\"", assemblyFullPath);
			SetValue(Registry.ClassesRoot, progID + @"\shell\open\command", sbShellEntry.ToString());
			StringBuilder sbDefaultIconEntry = new StringBuilder();
			sbDefaultIconEntry.AppendFormat("\"{0}\",0", assemblyFullPath);
			SetValue(Registry.ClassesRoot, progID + @"\DefaultIcon", sbDefaultIconEntry.ToString());

			SetValue(Registry.ClassesRoot, @"Applications" + Path.GetFileName(assemblyFullPath), "", "NoOpenWith");
		}

		private static void SetValue(RegistryKey root,string subkey,object keyValue)
		{
			SetValue(root, subkey, keyValue, null);
		}
		private static void SetValue(RegistryKey root, string subkey, object keyValue,string valueName)
		{
			bool hasSubKey = ((subkey != null) && (subkey.Length > 0));
			RegistryKey key = root;
			try
			{
				if (hasSubKey)
				key = root.CreateSubKey(subkey);
				key.SetValue(valueName, keyValue);
			}
			finally
			{
				if (hasSubKey && (key != null)) key.Close();
			}

		}
	}
}
