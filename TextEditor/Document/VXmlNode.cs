using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace TextEditor.Document
{
	public class VXmlNode
	{
		protected VXmlDocument _document;
		protected List<VXmlAttribute> _lstAttributes;
		protected List<VXmlNode> _lstChildNode;
		protected Line _lineFirst;
		protected Line _lineLast;
		protected List<Line> _lstLineText;
		protected List<LineSegment> _lstLineSegment;

		/// <summary>
		/// 匹配空格的正则表达式
		/// </summary>
		Regex regexSpace = new Regex(@"^\s");

		/// <summary>
		/// 匹配任意符号的正则表达式
		/// </summary>
		Regex regexPun = new Regex(@"^\p{P}|\p{S}");

		#region Property

		public VXmlDocument Document
		{
			get;
			protected set;
		}

		public VXmlNode Parent
		{
			get;
			set;
		}

		public virtual string Name
		{
			get;
			set;
		}
        public virtual string Label
        {
            get
            {
				string display = this.Document.LanguageReader == null? Name : this.Document.LanguageReader.GetDisplayText(Name);
               if (string.IsNullOrEmpty(display))
                   return Name;
               return display;
            }
        }
		public virtual string Value
		{
			get;
			set;
		}

		public virtual string InnerXml
		{
			get
			{
				StringBuilder sbInnerXml = new StringBuilder();

				if (_lineFirst != null)
				{
					sbInnerXml.Append(_lineFirst.Text);
					sbInnerXml.Append("\n");
				}

				if (_lstLineText != null)
				{
					foreach (Line line in _lstLineText)
					{
						sbInnerXml.Append(line.Text);
						sbInnerXml.Append("\n");
					}
				}
				foreach (VXmlNode node in _lstChildNode)
				{
					sbInnerXml.Append(node.InnerXml);
				}

				if (_lineLast != null)
				{
					sbInnerXml.Append(_lineLast.Text);
					sbInnerXml.Append("\n");
				}

				return sbInnerXml.ToString();
			}
		}

		public virtual int Layer
		{
			get;
			set;
		}

		public List<VXmlAttribute> Attributes
		{
			get { return _lstAttributes; }
		}

		public List<VXmlNode> Nodes
		{
			get { return _lstChildNode; }
		}

		public Line FirstLine
		{
			get { return _lineFirst; }
		}

		public Line LastLine
		{
			get { return _lineLast; }
		}

		//public Line[] Lines
		//{
		//    get { return _lstLine.ToArray(); }
		//}

		#endregion

		public VXmlNode(VXmlDocument doc)
		{
			_lstAttributes = new List<VXmlAttribute>();
			_lstChildNode = new List<VXmlNode>();
			_lstLineText = null;
			_lstLineSegment = new List<LineSegment>();
			Layer = 0;
			Document = doc;
		}

		public VXmlNode(string nodeName, VXmlDocument doc)
			: this(doc)
		{
			Name = nodeName;

			CreateTextLine();
		}

		public virtual void Init(System.Xml.XmlNode node)
		{
			try
			{
				Name = node.Name;
				foreach (System.Xml.XmlAttribute xmlattr in node.Attributes)
				{
					VXmlAttribute attr = new VXmlAttribute(this);
					attr.Name = xmlattr.Name;
					attr.Value = xmlattr.Value;
				}

				foreach (System.Xml.XmlNode xmlNode in node.ChildNodes)
				{
					if (xmlNode.NodeType == System.Xml.XmlNodeType.Text)
						this.Value = (xmlNode as System.Xml.XmlText).Value;
					else
					{
						VXmlNode nodeChild = null;
						switch (xmlNode.NodeType)
						{
							case System.Xml.XmlNodeType.Comment:
								nodeChild = new VXmlComment(Document);
								nodeChild.Layer = Layer + 1;
								nodeChild.Init(xmlNode);
								break;
							case System.Xml.XmlNodeType.CDATA:
								nodeChild = new VXmlCDataSection(Document);
								nodeChild.Layer = Layer + 1;
								nodeChild.Init(xmlNode);
								break;
							default:
								nodeChild = new VXmlNode(Document);
								nodeChild.Layer = Layer + 1;
								nodeChild.Init(xmlNode);
								break;
						}

						//nodeChild.Layer = Layer + 1;
						nodeChild.Parent = this;
						_lstChildNode.Add(nodeChild);
					}
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + node.OuterXml);
			}


			CreateTextLine();
		}

		public virtual void AddChildNode(System.Xml.XmlNode node)
		{
			try
			{
				VXmlNode childNode = new VXmlNode(Document);

				childNode.Init(node);
				this._lstChildNode.Add(childNode);
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + node.OuterXml);
			}
		}

		public virtual void Save(StreamWriter writer)
		{
			if (_lineFirst != null)
			{
				writer.WriteLine(_lineFirst.Text);
			}

			if (_lstLineText != null)
			{
				foreach (Line line in _lstLineText)
				{
					writer.WriteLine(line.Text);
				}
			}
			foreach (VXmlNode node in _lstChildNode)
			{
				node.Save(writer);
			}

			if (_lineLast != null)
			{
				writer.WriteLine(_lineLast.Text);
			}
		}

		/// <summary>
		/// 处理文本
		/// </summary>
		private List<string> DealText(string text)
		{
			List<string> array = new List<string>();
			StringBuilder spitstring = new StringBuilder();
			StringBuilder value = new StringBuilder(text);
			for (int i = 0; i < value.Length; i++)
			{
				// 拆分出空格
				if (regexSpace.IsMatch(value[i].ToString()))
				{
					if (!string.IsNullOrEmpty(spitstring.ToString()))
					{
						array.Add(spitstring.ToString());
						spitstring.Remove(0, spitstring.Length);
					}
					array.Add(" ");
				}
			    // 拆分出非空格的字母或词组
				else if (!string.IsNullOrEmpty(value[i].ToString()) && !regexPun.IsMatch(value[i].ToString()))
				{
					spitstring.Append(value[i]);
					if (i == value.Length - 1)
					{
						array.Add(spitstring.ToString());
					}
				}
				// 拆分出符号
				else if(regexPun.IsMatch(value[i].ToString()))
				{
					if (!string.IsNullOrEmpty(spitstring.ToString()))
					{
						array.Add(spitstring.ToString());
						spitstring.Remove(0, spitstring.Length);
					}
					array.Add(value[i].ToString());
				}
			}
			return array;
		}

		private void CreateTextLine()
		{
			if (_lstLineSegment != null)
				_lstLineSegment.Clear();

			for (int i = 0; i < Layer; i++)
			{
				_lstLineSegment.Add(new TabSegment());
			}

			if (string.IsNullOrEmpty(Name))
				return;

			_lineFirst.AddSegment(new LineSegment(SegType.LeftSign, "<"));
			_lineFirst.AddSegment(new NodeNameSegment());

			foreach (VXmlAttribute attr in _lstAttributes)
			{
				_lineFirst.AddSegment(new SpaceSegment());
				_lineFirst.AddSegment(new AttrNameSegment(attr));
				_lineFirst.AddSegment(new EqualSegment());
				_lineFirst.AddSegment(new QuotationSegment());
				List<string> dealString = DealText(attr.Value);
				foreach(string item in dealString)
				{
					if(item.Equals(" "))
					{
						_lineFirst.AddSegment(new LineSegment(SegType.AttrSplit, " "));
					}
					else if(!string.IsNullOrEmpty(item))
					{
						_lineFirst.AddSegment(new AttrValueSegment(attr));
					}
				}
				//_lineFirst.AddSegment(new LineSegment(SegType.AttrValue, attr.Value));
				_lineFirst.AddSegment(new QuotationSegment());
			}

			if (_lstChildNode.Count == 0)
			{
				if (string.IsNullOrEmpty(Value))
				{
					_lineFirst.AddSegment(new LineSegment(SegType.RightSign, "/>"));
					_lineLast = null;
				}
				else
				{
					_lineFirst.AddSegment(new LineSegment(SegType.RightSign, ">"));

					string sValue = Value;
					sValue = sValue.Replace('\r', '\n');
					sValue = sValue.Replace("\n\n", "\n");
					string[] texts = sValue.Split('\n');
					if (texts.Length == 1)
					{
						//_lineFirst.AddSegment(new LineSegment(SegType.Text, Value));
						List<string> dealString = DealText(Value);
						foreach (string text in dealString)
						{
							//if (text.Equals(" "))
							//{
							//    _lineFirst.AddSegment(new LineSegment(SegType.Text, text));
							//}
							//else if (text.Equals("="))
							//{
							//    _lineFirst.AddSegment(new LineSegment(SegType.Text, text));
							//}
							//else if (text.Equals("\""))
							//{
							//    _lineFirst.AddSegment(new QuotationSegment());
							//}
							//else if (text.Equals("\t"))
							//{
							//    _lineFirst.AddSegment(new TabSegment());
							//}
							//else 
							if (!string.IsNullOrEmpty(text))
							{
								_lineFirst.AddSegment(new LineSegment(SegType.Text, text));
							}
						}

						_lineFirst.AddSegment(new LineSegment(SegType.LeftSign, "</"));
						_lineFirst.AddSegment(new NodeNameSegment());
						_lineFirst.AddSegment(new LineSegment(SegType.RightSign, ">"));

						_lineLast = null;
					}
					else if (texts.Length > 1)
					{
						List<string> dealString1 = DealText(texts[0]);
						foreach (string item in dealString1)
						{
							if (item.Equals(" "))
							{
								_lineFirst.AddSegment(new SpaceSegment());
							}
							else if (!string.IsNullOrEmpty(item))
							{
								_lineFirst.AddSegment(new LineSegment(SegType.Text, item));
							}
						}
						//_lineFirst.AddSegment(new LineSegment(SegType.Text, texts[0]));

						_lstLineText = new List<Line>();
						for (int i = 1; i < texts.Length; i++)
						{
							string text = texts[i];

							if (i == texts.Length - 1)
							{
								if (text.Trim().Length == 0)
									break;
							}
							Line line = new Line(this);

							//line.AddSegment(new LineSegment(SegType.Text, text));
							List<string> dealString2 = DealText(text);
							foreach (string item in dealString2)
							{
								if (item.Equals(" "))
								{
									line.AddSegment(new SpaceSegment());
								}
								else if (!string.IsNullOrEmpty(item))
								{
									line.AddSegment(new LineSegment(SegType.Text, item));
								}
							}

							_lstLineText.Add(line);
						}

						_lineLast = new Line(this);
						for (int i = 0; i < Layer; i++)
						{
							_lineLast.AddSegment(new TabSegment());
						}
						_lineLast.AddSegment(new LineSegment(SegType.LeftSign, "</"));
						_lineLast.AddSegment(new NodeNameSegment());
						_lineLast.AddSegment(new LineSegment(SegType.RightSign, ">"));
					}
				}
			}
			else
			{
				_lineFirst.AddSegment(new LineSegment(SegType.RightSign, ">"));

				_lineLast = new Line(this);
				for (int i = 0; i < Layer; i++)
				{
					_lineLast.AddSegment(new TabSegment());
				}
				_lineLast.AddSegment(new LineSegment(SegType.LeftSign, "</"));
				_lineLast.AddSegment(new NodeNameSegment());
				_lineLast.AddSegment(new LineSegment(SegType.RightSign, ">"));
			}
		}

		public Line NewLine(Line line, bool after)
		{
			if (_lstLineText != null && _lstLineText.Contains(line))
			{
				int iIndex = _lstLineText.FindIndex(delegate(Line ln)
				{
					if (ln == line)
						return true;
					else
						return false;
				});
				if (iIndex > -1)
				{
					Line newLine = new Line(this);
					newLine.AddSegment(new LineSegment(SegType.Text, ""));
					if (after)
						_lstLineText.Insert(iIndex + 1, newLine);
					else
						_lstLineText.Insert(iIndex, newLine);

					return newLine;
				}
			}
			else if ((line == _lineFirst && _lineLast == null) || (line != _lineFirst && _lineLast != null && line == _lineLast))
			{
				//Line newLine = new Line(this);
				if (this.Parent != null)
				{
					VXmlNode node = this.Parent.AddChildNode();
					if (node != null)
						return node.FirstLine;
				}
				else
				{
					VXmlNode nodeRoot = new VXmlNode("", Document);
					nodeRoot.Layer = 0;
					nodeRoot.CreateTextLine();

					Document.NodeRoot = nodeRoot;

					return nodeRoot.FirstLine;
				}
			}

			return null;
		}

		/// <summary>
		/// 在当前行下插入新行
		/// </summary>
		public Line InsertLine(Line line, ref int caretCol) 
		{
			Line newLine = null;
			if (_lstLineText != null && _lstLineText.Contains(line)) 
			{
				newLine = new Line(this);
				int index = _lstLineText.IndexOf(line);
				_lstLineText.Insert(index + 1, newLine);
			}
			else if(this is VXmlHeader)
			{
				if(Document.NodeRoot == null)
				{
					VXmlNode nodeRoot = new VXmlNode("", Document);
					nodeRoot.Layer = 0;
					nodeRoot.CreateTextLine();

					Document.NodeRoot = nodeRoot;

					return nodeRoot.FirstLine;
				}
				return Document.NodeRoot.FirstLine;
			}
			else if (FirstLine == line)
			{
				if(_lstLineText == null)
				{
					newLine = new Line(this);
					_lstLineText = new List<Line>();
					_lstLineText.Add(newLine);
				}
				else
				{
					newLine = new Line(this);
					_lstLineText.Insert(0,newLine);
				}
			}
			if (newLine == null)
				return newLine;

			List<LineSegment> segmentList = new List<LineSegment>();
			foreach (LineSegment seg in line.Segments)
			{
				if (seg.SegType == SegType.Space)
				{
					segmentList.Add(seg);
					caretCol += 1;
				}
				else if(seg.SegType == SegType.Tab)
				{
					segmentList.Add(seg);
					caretCol += 4;
				}
				else
				{
					newLine.AddSegment(segmentList);
					return newLine;
				}
			}

			return newLine;
		}

		/// <summary>
		/// 设置最后一行
		/// </summary>
		public void SetLastLine(Line line) 
		{
			this._lineLast = line;
			line.Node = this;
		}

		/// <summary>
		/// 设置第一行
		/// </summary>
		public void SetFirstLine(Line line) 
		{
			this._lineFirst = line;
			line.Node = this;
		}

		public VXmlNode AddChildNode(string name = "")
		{
			VXmlNode nodeChild = null;
			nodeChild = new VXmlNode(name, Document);
			nodeChild.Layer = Layer + 1;

			nodeChild.Parent = this;

			nodeChild.CreateTextLine();
			_lstChildNode.Add(nodeChild);

			return nodeChild;
		}

		/// <summary>
		/// 添加子节点
		/// </summary>
		public VXmlNode InsertChildNode() 
		{
			VXmlNode nodeChild = null;
			nodeChild = new VXmlNode(Document);
			nodeChild.Layer = Layer + 1;

			nodeChild.Parent = this;

			_lstChildNode.Add(nodeChild);

			return nodeChild;
		}

		/// <summary>
		/// 添加子节点
		/// </summary>
		public VXmlNode InsertChildNode(int index) 
		{
			VXmlNode nodeChild = null;
			nodeChild = new VXmlNode(Document);
			nodeChild.Layer = Layer + 1;

			nodeChild.Parent = this;

			_lstChildNode.Insert(index,nodeChild);

			return nodeChild;
		}

		public virtual void Draw(Graphics g, Font f, ref Point ptPos, TextBoxControl editor, ref int iLine)
		{
			if (_lineFirst != null)
			{
				_lineFirst.Draw(editor, g, f, ptPos);

				ptPos.Y += editor.FontHeight;
			}

			if (_lstLineText != null)
			{
				foreach (Line line in _lstLineText)
				{
					line.Draw(editor, g, f, ptPos);

					ptPos.Y += editor.FontHeight;
				}
			}
			foreach (VXmlNode node in _lstChildNode)
			{
				node.Draw(g, f, ref ptPos, editor, ref iLine);
			}

			if (_lineLast != null)
			{
				_lineLast.Draw(editor, g, f, ptPos);

				ptPos.Y += editor.FontHeight;
			}
		}

		public virtual Size CalcSize(Graphics g, Font f, TextBoxControl editor)
		{
			Size size = new Size();
			if (_lineFirst != null)
			{
				Size sizeTemp = _lineFirst.CalcSize(g, f, editor);
				if (sizeTemp.Width > size.Width)
					size.Width = sizeTemp.Width;

				size.Height += sizeTemp.Height;
			}

			if(_lstLineText != null && _lstLineText.Count > 0 )
			{
				foreach(Line line in _lstLineText)
				{
					Size sizeTemp = line.CalcSize(g, f, editor);
					if (sizeTemp.Width > size.Width)
						size.Width = sizeTemp.Width;

					size.Height += sizeTemp.Height;
				}
			}

			foreach (VXmlNode node in _lstChildNode)
			{
				Size sizeTemp = node.CalcSize(g, f, editor);
				if (sizeTemp.Width > size.Width)
					size.Width = sizeTemp.Width;

				size.Height += sizeTemp.Height;
			}

			if (_lineLast != null)
			{
				Size sizeTemp = _lineLast.CalcSize(g, f, editor);

				if (sizeTemp.Width > size.Width)
					size.Width = sizeTemp.Width;

				size.Height += sizeTemp.Height;
			}

			return size;
		}

		public int LineCount()
		{
			int iCount = 0;
			if (_lineFirst != null)
				iCount += 1;
			if (_lstLineText != null)
			{
				iCount += _lstLineText.Count;
			}

			foreach (VXmlNode node in _lstChildNode)
			{
				iCount += node.LineCount();
			}

			if (_lineLast != null)
				iCount += 1;

			return iCount;
		}

		/// <summary>
		///  通过行号获取行
		/// </summary>
		public Line GetLineByLineNumber(ref int calNumber, int lineNumber) 
		{
			if (_lineFirst != null)
			{
				calNumber += 1;
				if (lineNumber == calNumber)
				{
					return _lineFirst;
				}
			}
			if(_lstLineText != null)
			{
				foreach(Line line in _lstLineText)
				{
					calNumber += 1;
					if(lineNumber == calNumber)
					{
						return line;
					}
				}
			}
			foreach(VXmlNode node in _lstChildNode)
			{
				 Line line = node.GetLineByLineNumber(ref calNumber,lineNumber);
				if(line == null)
				{
					continue;
				}
				return line;
			}
			if(_lineLast != null)
			{
				calNumber += 1;
				if(lineNumber == calNumber)
				{
					return _lineLast;
				}
			}
			return null;
		}

		public bool IsFirstLine(Line line)
		{
			if (line == _lineFirst)
				return true;
			else
				return false;
		}

		public bool IsLastLine(Line line)
		{
			if (line == _lineFirst && _lineLast == null)
				return true;
			else if (_lineLast != null && line == _lineLast)
				return true;
			else
				return false;
		}


		public bool IsMidLine(Line line)
		{
			if (line.Node == this && line != _lineFirst && line != _lineLast)
				return true;
			else
				return false;
		}

		/// <summary>
		/// 删除中间行
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public bool RemoveMidLine(Line line) 
		{
			if (_lstLineText == null)
				return false;
			if (!IsMidLine(line))
				return false;
			if (_lstLineText.Contains(line))
			{
				return _lstLineText.Remove(line);
			}

			return false;
		}

		/// <summary>
		/// 获取相同父节点下同级节点名称集合(包含自己)
		/// </summary>
		public List<string> GetSameLevelNodeName(bool includeSelf = true) 
		{
			List<string> nameList = new List<string>();
			if (this.Parent == null)
				return nameList;
			foreach(VXmlNode node in this.Parent.Nodes)
			{
				if(!nameList.Contains(node.Name))
				{
					if (!includeSelf && node == this)
						continue;
					nameList.Add(node.Name);
				}
			}
			return nameList;
		}

		/// <summary>
		/// 获取上一个节点的名称
		/// </summary>
		public string GetLastNodeName() 
		{
			int index = this.Parent.Nodes.IndexOf(this);
			if (index == 0)
				return "";
			return this.Parent.Nodes[index - 1].Name;
		}
	}
}
