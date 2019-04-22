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
		private BindableCollection<FrameworkElement> myVar;
		private List<Questions> testList = new List<Questions>();
		private UIElements elements = null;
		private string _questionText;
		private int _selectedIndex;
		private int _num = 1;
		private BindableCollection<Visibility> _extraAnswer = new BindableCollection<Visibility> { Visibility.Collapsed, Visibility.Collapsed };
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

		public int CountOfQuestions { get; set; } = 0;
		public int CurrentQuestion { get; set; } = 1;

		#endregion
		private BindableCollection<string> _textBoxList = new BindableCollection<string>() { "1", "2", "3", "4", "5" };
		public BindableCollection<string> TextBoxAnwerList
		{
			get { return _textBoxList; }
			set { _textBoxList = value; }
		}
		protected override void OnActivate()
		{
			ListBoxElements = new BindableCollection<FrameworkElement>();
			elements = new UIElements();
			Button btn = elements.CreateButton();
			var style = Application.Current.TryFindResource("AddImageButton") as Style;
			btn.Style = style;
			btn.Click += new RoutedEventHandler(AddRectengle);
			ListBoxElements.Add(btn);
		}
		private void AddRectengle(object sender, RoutedEventArgs e)
		{
			var _rect = elements.Create();
			ListBoxElements.Insert(ListBoxElements.Count - 1, _rect);

			CountOfQuestions += 1;
			CurrentQuestion = ListBoxElements.Count - 1;
			NotifyOfPropertyChange(() => GetCountOfQuestions);
			ClearScreen();
			DeleteBothAnswer();

		}

		public void SaveClick()
		{
			List<string> _localWrongAnswerList = new List<string>();

			var _tempich = TextBoxAnwerList.Skip(1);
			_localWrongAnswerList.AddRange(TextBoxAnwerList.Skip(1));

			var _path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Resources\\";
			string _temp = null;
			if (string.IsNullOrWhiteSpace(QuestionText))
				return;
			if (ListBoxElements.Count == 1)
				return;		
			Questions _tempClass = new Questions()
			{
				Id = CurrentQuestion,
				Question = QuestionText,
				RightAnswer = TextBoxAnwerList[0],
				WrongAnswer = _localWrongAnswerList
			};
			if (testList.Any(i => i.Id == CurrentQuestion))
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
			TextBoxAnwerList[0] = "";
			TextBoxAnwerList[1] = "";
			TextBoxAnwerList[2] = "";
			TextBoxAnwerList[3] = "";
			TextBoxAnwerList[4] = "";
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

			TextBoxAnwerList[0] = _temp.RightAnswer;
			TextBoxAnwerList[1] = _temp.WrongAnswer[0];
			TextBoxAnwerList[2] = _temp.WrongAnswer[1];
			if (!string.IsNullOrWhiteSpace(_temp.WrongAnswer[2]))
			{
				TextBoxAnwerList[3] = _temp.WrongAnswer[2];
				_num = 1;
				ShowAdditionalAnswer(1);
			}
			if (!string.IsNullOrWhiteSpace(_temp.WrongAnswer[3]))
			{
				_num = 2;
				ShowAdditionalAnswer(2);
				TextBoxAnwerList[4] = _temp.WrongAnswer[3];
			}
			_num = 1;
			if (string.IsNullOrWhiteSpace(_temp.WrongAnswer[1]))
				DeleteBothAnswer();
		}
	
		public void SaveTestClick()
		{
			QuestionSerialization serialization = new QuestionSerialization();
			if (isSaved == false)
				SaveAsTestClick();
			else
			{
				serialization.Serialize(fullSavedPath, testList);
			}
		}

		public void SaveAsTestClick()
		{
			QuestionSerialization serialization = new QuestionSerialization();
			SaveFileDialog saveFile = new SaveFileDialog
			{
				Title = "Save test",
				Filter = "Test files |*.tst",
				DefaultExt = "tst",
				AddExtension = true
			};

			if (saveFile.ShowDialog() == true)
			{
				var fileName = saveFile.SafeFileName;
				fullSavedPath = Path.GetFullPath(saveFile.FileName);
				serialization.Serialize(fullSavedPath, testList);
				isSaved = true;
			}
		}

		public void OpenTestClick()
		{
			QuestionSerialization serialization = new QuestionSerialization();
			OpenFileDialog open = new OpenFileDialog
			{
				Title = "Opne test",
				Filter = "Test files |*.tst",
			};
			if(open.ShowDialog()==true)
			{
				testList = serialization.Deserialize2(open.FileName);
				foreach (var item in testList)
				{
					var _rect = elements.Create();
					ListBoxElements.Insert(ListBoxElements.Count - 1, _rect);

					CountOfQuestions += 1;
					CurrentQuestion = ListBoxElements.Count - 1;
					NotifyOfPropertyChange(() => GetCountOfQuestions);
					var passedElement = (RectanlgeQuestions)ListBoxElements[CurrentQuestion - 1];
					elements.TextInsideRct(ref passedElement, item.Question);

					ClearScreen();
					DeleteBothAnswer();
				}

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
