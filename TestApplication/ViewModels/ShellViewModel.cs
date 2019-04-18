using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TestApplication.MyClasses;
using Microsoft.Win32;
using System.Data;
using TestApplication.Model;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Xml;
namespace TestApplication.ViewModels
{
	public class ShellViewModel : Screen
	{

		#region Field
		private IWindowManager manager = new WindowManager();
		private BindableCollection<FrameworkElement> myVar;
		private List<Questions> testList = new List<Questions>();
		private UIElements elements = null;
		private string _firstAnswerTextBox;
		private string _secondAnswerTextBox;
		private string _thirdAnswerTextBox;
		private string _forthAnswerTextBox;
		private string _fifthAnswerTextBox;
		private string _questionText;
		private int _selectedIndex;
		private int _num =1;
		private string _questionImagepath;
		private Visibility _visibilityborder;
		private BindableCollection<Visibility> _extraAnswer = new BindableCollection<Visibility> { Visibility.Collapsed, Visibility.Collapsed };
		private BindableCollection<bool> _radioButtonIsChecked = new BindableCollection<bool> { true, false, false, false, false };
		private bool isSaved = false;
		private string fullSavedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		#endregion
		#region Properties
		public BindableCollection<FrameworkElement> ListBoxElements
		{
			get { return myVar; }
			set
			{
				myVar = value;
				NotifyOfPropertyChange(() => ListBoxElements);
			}
		}
		public string FirstAnswerTextBox
		{
			get { return _firstAnswerTextBox; }
			set
			{
				_firstAnswerTextBox = value;
				NotifyOfPropertyChange(() => FirstAnswerTextBox);
			}
		}

		public string SecondAnswerTextBox
		{
			get { return _secondAnswerTextBox; }
			set
			{
				_secondAnswerTextBox = value; NotifyOfPropertyChange(() => SecondAnswerTextBox);
			}
		}

		public string ThirdAnswerTextBox
		{
			get { return _thirdAnswerTextBox; }
			set
			{
				_thirdAnswerTextBox = value; NotifyOfPropertyChange(() => ThirdAnswerTextBox);
			}
		}

		public string ForthAnswerTextBox
		{
			get { return _forthAnswerTextBox; }
			set
			{
				_forthAnswerTextBox = value; NotifyOfPropertyChange(() => ForthAnswerTextBox);
			}
		}

		public string FifthAnswerTextBox
		{
			get { return _fifthAnswerTextBox; }
			set
			{
				_fifthAnswerTextBox = value; NotifyOfPropertyChange(() => FifthAnswerTextBox);
			}
		}

		public string QuestionText
		{
			get
			{
				return _questionText;
			}
			set
			{
				_questionText = value;
				NotifyOfPropertyChange(() => QuestionText);
			}
		}

		public string QuestionImagePath
		{
			get { return _questionImagepath; }
			set
			{
				_questionImagepath = value;
				NotifyOfPropertyChange(() => QuestionImagePath);
			}
		}

		public Visibility VisibilityBorder
		{
			get { return _visibilityborder; }
			set { _visibilityborder = value; NotifyOfPropertyChange(() => VisibilityBorder); }
		}

		public int ListBoxSelectedIndex//Какой элемент был выбран в листбохе
		{
			get
			{
				ShowCurrentQuestion();
				return _selectedIndex;
			}
			set
			{
				if (value == ListBoxElements.Count - 1 && ListBoxElements.Count != 1)
				{
					CurrentQuestion = value;
					_selectedIndex = value;

				}
				else
				{
					CurrentQuestion = value + 1;
					_selectedIndex = value + 1;
				}
				_selectedIndex = value;
				NotifyOfPropertyChange(() => ListBoxSelectedIndex);
			}
		}

		public BindableCollection<Visibility> ExtraAnswer
		{
			get { return _extraAnswer; }
			set { _extraAnswer = value; NotifyOfPropertyChange(() => ExtraAnswer); }
		}

		public string GetCountOfQuestions
		{
			get
			{
				if (CountOfQuestions > 1)
					return CountOfQuestions.ToString() + " questions are created";

				return CountOfQuestions.ToString() + " question is created";
			}

		}

		public BindableCollection<bool> RadioButtonIsChecked
		{
			get { return _radioButtonIsChecked; }
			set { _radioButtonIsChecked = value; NotifyOfPropertyChange(() => RadioButtonIsChecked); }
		}

		public int CountOfQuestions { get; set; } = 0;
		public int CurrentQuestion { get; set; } = 1;

		#endregion

		protected override void OnActivate()
		{
			ListBoxElements = new BindableCollection<FrameworkElement>();
			elements = new UIElements();
			Button btn = elements.CreateButton();
			var style = Application.Current.TryFindResource("AddImageButton") as Style;
			btn.Style = style;
			btn.Click += new RoutedEventHandler(AddRectengle);
			ListBoxElements.Add(btn);
			_visibilityborder = Visibility.Collapsed;
		}

		public void InsertImageToBorder(string temp)
        {
			int index = Convert.ToInt16(temp);
            ConverterImage converter = new ConverterImage();
			
			OpenFileDialog open = new OpenFileDialog
			{
				Title = "Image Scource",
				Multiselect = false,
				Filter = "Image File |*.jpg;*.png"
			};

			if (open.ShowDialog() == true)
            {
				QuestionImagePath = open.FileName;
				VisibilityBorder = Visibility.Visible;
            }

        }
		private void AddRectengle(object sender,RoutedEventArgs e)
		{
			var _rect = elements.Create();
			ListBoxElements.Insert(ListBoxElements.Count-1, _rect);

			CountOfQuestions +=1;
			CurrentQuestion = ListBoxElements.Count-1;
			NotifyOfPropertyChange(() => GetCountOfQuestions);
			ClearScreen();
			DeleteBothAnswer();
			QuestionImagePath = "";

		}

		public void SaveClick()
        {
			var _path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Resources\\";
			string _temp = null;
			if (QuestionImagePath == null && (string.IsNullOrWhiteSpace(QuestionText)))
				return;
			if (ListBoxElements.Count == 1)
				return;
			if (!string.IsNullOrWhiteSpace(QuestionImagePath))
			{
				_temp = _path + CurrentQuestion + ".jpg";
				var converter = new ConverterImage();
				System.Drawing.Image img = System.Drawing.Image.FromFile(QuestionImagePath);
				converter.SaveImage(_temp, System.Drawing.Imaging.ImageFormat.Jpeg, img);
			}
			Questions _tempClass = new Questions()
			{
				Id = CurrentQuestion,
				Question = QuestionText,
				FirstAnswer = FirstAnswerTextBox,
				SecondAnswer = SecondAnswerTextBox,
				ThirdAnswer = ThirdAnswerTextBox,
				ForthAnswer = ForthAnswerTextBox,
				FifthAnswer = FifthAnswerTextBox,
				RightAnswer= RadioButtonIsChecked.IndexOf(RadioButtonIsChecked.Where(i=>i==true).FirstOrDefault())+1,
				QuestionImagePath = _temp
			};
			if(testList.Any(i=>i.Id == CurrentQuestion))
			{
				var subList = testList.Where(x => x.Id == CurrentQuestion).ToList();
				testList.RemoveAll(x => x.Id == CurrentQuestion);
				testList.Add(_tempClass);
			}
			else
			{
				testList.Add(_tempClass);
			}

			var passedElement = (RectanlgeQuestions)ListBoxElements[CurrentQuestion - 1];
			elements.TextInsideRct(ref passedElement, QuestionText);
		}

		private void ClearScreen()
        {
            QuestionText = string.Empty;
            FirstAnswerTextBox = string.Empty;
            SecondAnswerTextBox = string.Empty;
            ThirdAnswerTextBox = string.Empty;
            ForthAnswerTextBox = string.Empty;
            FifthAnswerTextBox = string.Empty;

        }

		private void ShowCurrentQuestion()
		{
			ClearScreen();
			DeleteBothAnswer();
			   _num = 1;
			if (!testList.Any(i => i.Id == CurrentQuestion))
				return;

			var _temp = testList.Where(i => i.Id == CurrentQuestion).FirstOrDefault();

			QuestionText = _temp.Question;

			FirstAnswerTextBox = _temp.FirstAnswer;
			SecondAnswerTextBox = _temp.SecondAnswer;
			ThirdAnswerTextBox = _temp.ThirdAnswer;
			if (!string.IsNullOrWhiteSpace(_temp.ForthAnswer))
			{
				ForthAnswerTextBox = _temp.ForthAnswer;
				_num = 1;
				ShowAdditionalAnswer(1);
			}
			if (!string.IsNullOrWhiteSpace(_temp.FifthAnswer))
			{
				_num = 2;
				ShowAdditionalAnswer(2);
				FifthAnswerTextBox = _temp.FifthAnswer;
			}
			_num = 1;
			if (string.IsNullOrWhiteSpace(_temp.ForthAnswer))
				DeleteBothAnswer();
		}

		public void CanceSelectedlImage()
		{
			VisibilityBorder = Visibility.Collapsed;
			QuestionImagePath = null;
		}

		public  void SaveTestClick()
		{
			SDClass<Questions> sDClass = new SDClass<Questions>();
			if (isSaved == false)
				SaveAsTestClick();
			else
			{
				sDClass.SerializeXml(testList, fullSavedPath);
			}
		}

		public void SaveAsTestClick()
		{
			SDClass<Questions> sDClass = new SDClass<Questions>();
			SaveFileDialog saveFile = new SaveFileDialog
			{
				Title = "Save test",
				Filter = "Test files |*.tst",
				DefaultExt = "test",
				AddExtension = true
			};

			if(saveFile.ShowDialog()==true)
			{
				var fileName = saveFile.SafeFileName;
				fullSavedPath = Path.GetFullPath(saveFile.FileName);
				sDClass.SerializeXml(testList, fullSavedPath);
				isSaved = true;
			}
		}
		#region AddingAndDeletingAnswer
		public void ShowAdditionalAnswer(int num)
		{
			if (_num == 1)
			{
				_extraAnswer[0] = Visibility.Visible;
			}

			if (_num == 2)
			{
				_extraAnswer[1] = Visibility.Visible;
			}
			if (_num != 2)
				_num++;
		}

		public void DeleteAnswer()
		{
			if (_num == 1)
			{
				_extraAnswer[0] = Visibility.Collapsed;
			}
			if (_num == 2)
			{
				_extraAnswer[1] = Visibility.Collapsed;
				_num--;
			}
		}
		private void DeleteBothAnswer()
		{
			_extraAnswer[0] = Visibility.Collapsed;
			_extraAnswer[1] = Visibility.Collapsed;

		}
		#endregion

	}
}
