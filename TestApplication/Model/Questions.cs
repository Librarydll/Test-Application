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
		public string RightAnswer { get; set; }
		public List<string> WrongAnswer { get; set; }
	}
}
