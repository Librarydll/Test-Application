﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using System.IO;
using TestApplication.MyClasses;
using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using TestApplication.Model;

namespace TestApplication.ViewModels
{
	public class ShellViewModel : Screen
	{
		#region Field
		private IWindowManager manager = new WindowManager();
		private BindableCollection<FrameworkElement> myVar;
		private List<TestClass> testList = new List<TestClass>();
		private TestClass testClass = new TestClass();
		private UIElements elements = null;
		private string _firstAnswerTextBox;
		private string _secondAnswerTextBox;
		private string _thirdAnswerTextBox;
		private string _forthAnswerTextBox;
		private string _fifthAnswerTextBox;
		private string _questionText;
		private int _selectedIndex;
		private int _num =1;
		private byte[] imageBytes = null;
		private Uri _questionImage;
		private Visibility _visibilityborder;
		private BindableCollection<Visibility> _extraAnswer = new BindableCollection<Visibility> { Visibility.Collapsed, Visibility.Collapsed };
		private BindableCollection<bool> _radioButtonIsChecked = new BindableCollection<bool> { true, false, false, false, false };
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

		public Uri QuestionImage
		{
			get { return _questionImage; }
			set
			{
				_questionImage = value;
				NotifyOfPropertyChange(() => QuestionImage);
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
                imageBytes = converter.ConvertImageToByteArray(open.FileName);
                QuestionImage = new Uri(open.FileName);
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
		}

	
		
		public void SaveClick()
        {
			if (QuestionImage == null && (string.IsNullOrWhiteSpace(QuestionText)))
				return;
			if (ListBoxElements.Count == 1)
				return;
			TestClass _tempClass = new TestClass()
			{
				Id = CurrentQuestion,
				Question = QuestionText,
				First = FirstAnswerTextBox,
				Second = SecondAnswerTextBox,
				Third = ThirdAnswerTextBox,
				Forth = ForthAnswerTextBox,
				Fifth = FifthAnswerTextBox,
				RightAnswer= RadioButtonIsChecked.IndexOf(RadioButtonIsChecked.Where(i=>i==true).FirstOrDefault())+1,
				Image = imageBytes			
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

			FirstAnswerTextBox = _temp.First;
			SecondAnswerTextBox = _temp.Second;
			ThirdAnswerTextBox = _temp.Third;
			if (!string.IsNullOrWhiteSpace(_temp.Forth))
			{
				ForthAnswerTextBox = _temp.Forth;
				_num = 1;
				ShowAdditionalAnswer(1);
			}
			if (!string.IsNullOrWhiteSpace(_temp.Fifth))
			{
				_num = 2;
				ShowAdditionalAnswer(2);
				FifthAnswerTextBox = _temp.Fifth;
			}
			_num = 1;
			if (string.IsNullOrWhiteSpace(_temp.Forth))
				DeleteBothAnswer();

		}
  
		public void CanceSelectedlImage()
		{
			VisibilityBorder = Visibility.Collapsed;
			QuestionImage = null;
		}

		public void CreateTestClick() => manager.ShowDialog(new CreateTestWindowViewModel());		

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
