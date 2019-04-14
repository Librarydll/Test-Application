using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TestApplication.MyClasses
{
	public class UIElements
	{
		public Button CreateButton()
		{
			Button btn = new Button()
			{
				Height = 40,
				Width = 163,
				Content = "Create new question",
				Margin = new Thickness(10),
				Padding = new Thickness(10)
			};
			return btn;
		}

		public void TextInsideRct(ref RectanlgeQuestions rectanlge, string text)
		{
			if (text.Length > 35)
				rectanlge.TextPlace = text.Substring(0, 35) + "...";
			else
			{
				rectanlge.HorizontalAlignment = HorizontalAlignment.Center;
				rectanlge.VerticalAlignment = VerticalAlignment.Center;
				rectanlge.TextPlace = text;
			}
		}

		public RectanlgeQuestions Create()
		{
			RectanlgeQuestions rectanlge = new RectanlgeQuestions
			{
				TextPlace = "Your question"
			};
			return rectanlge;
		}


	}
}
