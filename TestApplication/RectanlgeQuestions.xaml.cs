using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Caliburn.Micro;
namespace TestApplication
{
	/// <summary>
	/// Interaction logic for RectanlgeQuestions.xaml
	/// </summary>
	public partial class RectanlgeQuestions : UserControl ,INotifyPropertyChanged
	{
		public RectanlgeQuestions()
		{
			InitializeComponent();
		}
		private string _textPlace;

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string prop="")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}

		public string TextPlace
		{
			get
			{
				return _textPlace;
			}

			set
			{
				_textPlace = value;
				OnPropertyChanged("TextPlace");
			}
		}

	}
}
