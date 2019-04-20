using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TestApplication.Model;

namespace TestApplication.MyClasses
{
	public class QuestionSerialization
	{

		public void Serialize(string path,List<Questions> questionList)
		{

			using (StreamWriter stream =new StreamWriter(path))
			{
				foreach (var item in questionList)
				{
					stream.WriteLine("=" + item.Question);
					stream.WriteLine("+" + item.RightAnswer);
					foreach (var wrong in item.WrongAnswer)
					{
						stream.WriteLine("-" + wrong);
					}
				}
			}
		}
	}
}
