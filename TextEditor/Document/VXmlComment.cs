using System;
using System.Collections.Generic;
using System.Text;

namespace TextEditor.Document
{
	public class VXmlComment : VXmlNode
	{
		public VXmlComment(VXmlDocument doc)
			: base(doc)
		{

		}

		public override void Init(System.Xml.XmlNode xmlNode)
		{
			if (xmlNode.NodeType != System.Xml.XmlNodeType.Comment)
				return;

			System.Xml.XmlComment comment = xmlNode as System.Xml.XmlComment;
			Name = comment.Name;

			Value = comment.Value;

			CreateTextLine();
		}

		private void CreateTextLine()
		{
			_lineFirst = new Line(this);

			for (int i = 0; i < Layer; i++)
			{
				_lineFirst.AddSegment(new TabSegment());
			}
			_lineFirst.AddSegment(new LineSegment(SegType.CommentSign, "<!--"));
			_lineFirst.AddSegment(new LineSegment(SegType.Comment, Value));
			_lineFirst.AddSegment(new LineSegment(SegType.CommentSign, "-->"));
			_lineLast = null;
		}
	}
}
