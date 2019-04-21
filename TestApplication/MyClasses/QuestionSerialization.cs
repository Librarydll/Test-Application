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
		List<Questions> questionsList = new List<Questions>();
		public void Serialize(string path, List<Questions> questionList)
		{

			using (StreamWriter stream = new StreamWriter(path))
			{
				foreach (var item in questionList)
				{
					stream.WriteLine("=" + item.Question);
					stream.WriteLine("+" + item.RightAnswer);
					foreach (var wrong in item.WrongAnswer)
					{
						stream.WriteLine("-" + wrong);
					}
					stream.WriteLine("<!>");
				}
			}
		}
		public List<Questions> Deserialize(string path)
		{
			Questions _tempQuestions = null;
			string tempRight = "";
			using (StreamReader stream = new StreamReader(path))
			{
				var tempStream = stream;
				string line;
				while ((line = stream.ReadLine()) != null)
				{
					if (line.StartsWith("="))
					{
						_tempQuestions = new Questions
						{
							Question = line.Replace("=", "")
						};
					}
					if (line.StartsWith("+"))
					{
						_tempQuestions.RightAnswer = line.Replace("+", "");
						tempRight = line.Replace("+", "");
					}
					if (line.StartsWith("-"))
					{
						_tempQuestions.WrongAnswer.Add(line.Replace("-", ""));

					}
					if (line.StartsWith("<!>"))
					{
						var temp = _tempQuestions.WrongAnswer.Where(i => !string.IsNullOrWhiteSpace(i)).ToList();
						temp.Add(tempRight);
						if (temp.Count > 2)
							RandomHelper.Shuffle(temp);
						_tempQuestions.WrongAnswer.Clear();
						_tempQuestions.WrongAnswer.AddRange(temp);
						questionsList.Add(_tempQuestions);
					}
				}
			}
			return questionsList;
		}
	}
}
