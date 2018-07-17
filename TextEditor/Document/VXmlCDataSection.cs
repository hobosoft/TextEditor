using System;
using System.Collections.Generic;
using System.Text;

namespace TextEditor.Document
{
	public class VXmlCDataSection : VXmlNode
	{
		public VXmlCDataSection(VXmlDocument doc)
			: base(doc)
		{

		}

		public override void Init(System.Xml.XmlNode xmlNode)
		{
			if (xmlNode.NodeType != System.Xml.XmlNodeType.CDATA)
				return;

			System.Xml.XmlCDataSection cdata = xmlNode as System.Xml.XmlCDataSection;

			Name = cdata.Name;

			Value = cdata.Value;

			CreateTextLine();
		}

		private void CreateTextLine()
		{
			_lineFirst = new Line(this);

			for (int i = 0; i < Layer; i++)
			{
				_lineFirst.AddSegment(new TabSegment());
			}
			_lineFirst.AddSegment(new LineSegment(SegType.CDATASign, "<[!CDATA["));
			_lineFirst.AddSegment(new LineSegment(SegType.CDATA, Value));
			_lineFirst.AddSegment(new LineSegment(SegType.CDATASign, "]]>"));
			_lineLast = null;
		}
	}
}
