using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace TextEditor.Document
{
	public class VXmlHeader : VXmlNode
	{
		public override string InnerXml
		{
			get
			{
				string sText = "";
				string sVersion = "", sEncoding = "", sStandalone = "";
				if (!string.IsNullOrEmpty(Version))
					sVersion = string.Format(" version=\"{0}\"", Version);
				if (!string.IsNullOrEmpty(Encoding))
					sEncoding = string.Format(" encoding=\"{0}\"", Encoding);
				if (!string.IsNullOrEmpty(Standalone))
					sStandalone = string.Format(" standalone=\"{0}\"", Standalone);
				sText = string.Format("<?xml{0}{1}{2}?>", sVersion, sEncoding, sStandalone);

				return sText;
			}
		}

		public string Version
		{
			get;
			set;
		}

		public string Encoding
		{
			get;
			set;
		}

		public string Standalone
		{
			get;
			set;
		}


		public VXmlHeader(VXmlDocument doc) : base(doc)
		{
			Version = "1.0";
			Encoding = "utf-8";
			Standalone = "";

			CreateTextLine();
		}

		public VXmlHeader(VXmlDocument doc, System.Xml.XmlNode header) : this(doc)
		{
			if (header.NodeType == System.Xml.XmlNodeType.XmlDeclaration)
			{
				System.Xml.XmlDeclaration dec = header as System.Xml.XmlDeclaration;
				Version = dec.Version;
				Encoding = dec.Encoding;
				Standalone = dec.Standalone;

				CreateTextLine();
			}
		}

		public override void Save(StreamWriter writer)
		{
			writer.WriteLine(InnerXml);
		}

		private void CreateTextLine()
		{
			_lineLast = null;
			if (_lstLineText != null)
				_lstLineText.Clear();

			_lineFirst = new Line(this);
			_lineFirst.AddSegment(new LineSegment(SegType.LeftSign, "<?"));
			_lineFirst.AddSegment(new LineSegment(SegType.NodeName, "xml"));
			if (!string.IsNullOrEmpty(Version))
			{
				_lineFirst.AddSegment(new SpaceSegment());
				_lineFirst.AddSegment(new LineSegment(SegType.AttrName, "version"));
				_lineFirst.AddSegment(new EqualSegment());
				_lineFirst.AddSegment(new LineSegment(SegType.AttrValue, string.Format("\"{0}\"", Version)));
			}

			if (!string.IsNullOrEmpty(Encoding))
			{
				_lineFirst.AddSegment(new SpaceSegment());
				_lineFirst.AddSegment(new LineSegment(SegType.AttrName, "encoding"));
				_lineFirst.AddSegment(new EqualSegment());
				_lineFirst.AddSegment(new LineSegment(SegType.AttrValue, string.Format("\"{0}\"", Encoding)));
			}

			if (!string.IsNullOrEmpty(Standalone))
			{
				_lineFirst.AddSegment(new SpaceSegment());
				_lineFirst.AddSegment(new LineSegment(SegType.AttrName, "standalone"));
				_lineFirst.AddSegment(new EqualSegment());
				_lineFirst.AddSegment(new LineSegment(SegType.AttrValue, string.Format("\"{0}\"", Standalone)));
			}
			_lineFirst.AddSegment(new LineSegment(SegType.RightSign, "?>"));
		}

		public override Size CalcSize(Graphics g, Font f, TextBoxControl editor)
		{
			if (_lineFirst != null)
				return _lineFirst.CalcSize(g, f, editor);
			else
				return new Size();
		}

		public override void Draw(Graphics g, Font f, ref Point ptPos, TextBoxControl editor, ref int iLine)
		{
			if (_lineFirst != null)
			{
				_lineFirst.Draw(editor, g, f, ptPos);

				ptPos.Y += editor.Font.Height;
			}
		}
	}
}
