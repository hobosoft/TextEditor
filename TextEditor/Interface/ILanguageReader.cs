using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextEditor.Interface
{
	public interface ILanguageReader
	{
		string CurrentLanguage {get;set;}
		void ChangeLanguage(string key);
		string GetDisplayText(string key);

		string[] Languages
		{
			get;
		}
	}
}
