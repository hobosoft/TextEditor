using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextEditor
{
	public class Paragraph
	{
		List<Block> m_lstSegment = new List<Block>();
		List<Line> m_lstLine = new List<Line>();

		public Paragraph()
		{
			m_lstLine.Add(new Line());
		}

		public string Text
		{
			get;
			set;
		}

		private void Parser()
		{

		}


	}
}
