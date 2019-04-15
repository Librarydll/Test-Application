using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication.Model
{
	public class Questions
	{
		public int Id { get; set; }	
		public string Question { get; set; }
		public string FirstAnswer { get; set; }
		public string SecondAnswer { get; set; }
		public string ThirdAnswer { get; set; }
		public string ForthAnswer { get; set; }
		public string FifthAnswer { get; set; }
		public string QuestionImagePath { get; set; }
		public int RightAnswer { get; set; }
	}
}
