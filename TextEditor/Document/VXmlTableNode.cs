using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TextEditor.Document
{
	public class VXmlTableNode : VXmlNode
	{
		public VXmlTableNode(VXmlDocument doc)
			: base(doc)
		{

		}

		public virtual void Draw(Graphics g, Font f, ref Point ptPos, NewXmlEditControl editor, ref int iLine)
		{
		}
	}
}
