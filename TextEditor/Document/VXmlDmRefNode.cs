using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TextEditor.Document
{
	public class VXmlDmRefNode : VXmlNode
	{

		#region Property

		#endregion


		public VXmlDmRefNode(VXmlDocument doc)
			: base(doc)
		{

		}

		public static VXmlDmRefNode CreateFromString(string xml, VXmlDocument doc)
		{
			VXmlDmRefNode node = new VXmlDmRefNode(doc);

			return node;
		}

		public virtual void Draw(Graphics g, Font f, ref Point ptPos, NewXmlEditControl editor, ref int iLine)
		{
			//if (_lineFirst != null)
			//{
			//    _lineFirst.Draw(editor, g, f, ptPos);

			//    ptPos.Y += editor.FontHeight;
			//}

			//if (_lstLineText != null)
			//{
			//    foreach (Line line in _lstLineText)
			//    {
			//        line.Draw(editor, g, f, ptPos);

			//        ptPos.Y += editor.FontHeight;
			//    }
			//}
			//foreach (VXmlNode node in _lstChildNode)
			//{
			//    node.Draw(g, f, ref ptPos, editor, ref iLine);
			//}

			//if (_lineLast != null)
			//{
			//    _lineLast.Draw(editor, g, f, ptPos);

			//    ptPos.Y += editor.FontHeight;
			//}
		}
	}
}
