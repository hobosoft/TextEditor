using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using TextEditor.Undo;
using System.IO;
using VCI.IETM.Interface;

namespace TextEditor.Document
{
	public class VXmlDocument
	{
		TextBoxControl _editor;
		BookmarkManager _bookmarkManager;
		UndoStack undoStack = new UndoStack();
		IEditorProperties textEditorProperties = new DefaultEditorProperties();
		string m_sFile = null;

		#region Document Events
		/// <summary>
		/// Is fired when CommitUpdate is called
		/// </summary>
		public event EventHandler UpdateCommited;

		/// <summary>
		/// </summary>
		public event DocumentEventHandler DocumentAboutToBeChanged;

		/// <summary>
		/// </summary>
		public event DocumentEventHandler DocumentChanged;

		public event EventHandler TextContentChanged;
		#endregion

        #region properties


		public IEditorProperties EditorProperties
		{
			get
			{
				return textEditorProperties;
			}
			set
			{
				textEditorProperties = value;
			}
		}

		public BookmarkManager BookmarkManager
		{
			get
			{
				return _bookmarkManager;
			}
			protected set
			{
				_bookmarkManager = value;
			}
		}

		public UndoStack UndoStack
		{
			get
			{
				return undoStack;
			}
		}

		public VXmlHeader XmlHeader
		{
			get;
			set;
		}

		public VXmlNode NodeRoot
		{
			get;
			set;
		}

		public string InnerXml
		{
			get
			{
				StringBuilder sbText = new StringBuilder();
				if (XmlHeader != null)
				{
					sbText.Append(XmlHeader.InnerXml);
					sbText.Append("\n");
				}
				if (NodeRoot != null)
					sbText.Append(NodeRoot.InnerXml);

				return sbText.ToString();
			}
		}

		public bool NCalc
		{
			get;
			set;
		}

		public Size Size
		{
			get;
			protected set;
		}

		public bool ReadOnly
		{
			get;
			set;
		}
		#endregion

		public VXmlDocument(TextBoxControl editor)
		{
			_editor = editor;
			EditorProperties = _editor.EditorProperties;
			_bookmarkManager = new BookmarkManager(this);

			XmlHeader = new VXmlHeader(this);
			//NodeRoot = new VXmlNode("RootNode", this);
			ReadOnly = false;
			NCalc = true;
			m_sFile = "";
		}

		public void Load(string file)
		{
			System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

			try
			{
				_bookmarkManager.Clear();

				doc.Load(file);
				XmlHeader = new VXmlHeader(this, doc.FirstChild);

				NodeRoot = new VXmlNode(this);
				NodeRoot.Init(doc.DocumentElement);

				m_sFile = file;

				NCalc = true;
			}
			catch (System.Exception ex)
			{
				MessageBox.Show("打开XML文件失败，请确认文件是否正确。\n" + ex.Message);
			}
		}

		public void LoadXml(string xml)
		{
			System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

			try
			{
				doc.LoadXml(xml);
				XmlHeader = new VXmlHeader(this, doc.FirstChild);

				NodeRoot = new VXmlNode(this);
				NodeRoot.Init(doc.DocumentElement);

				m_sFile = "";

				NCalc = true;
			}
			catch (System.Exception ex)
			{
				MessageBox.Show("读取XML字符串失败，请确认xml字符串是否正确。\n" + ex.Message);
			}
		}

		public void Save()
		{
			SaveAs(m_sFile);
		}

		public void SaveAs(string file)
		{
			try
			{
				if (string.IsNullOrEmpty(file))
				{
					SaveFileDialog dlg = new SaveFileDialog();
					dlg.Filter = "Xml File (*.xml)|*.xml";
					if (dlg.ShowDialog() != DialogResult.OK)
						return;

					file = dlg.FileName;
				}

				Encoding ed = Encoding.UTF8;
				if (XmlHeader != null && !string.IsNullOrEmpty(XmlHeader.Encoding))
					ed = Encoding.GetEncoding(XmlHeader.Encoding);

				using (StreamWriter writer = new StreamWriter(file, false, ed))
				{
					if (XmlHeader != null)
						XmlHeader.Save(writer);

					if (NodeRoot != null)
						NodeRoot.Save(writer);
				}

				m_sFile = file;
			}
			catch (System.Exception ex)
			{
				//MessageBox.Show("保存XML文件失败，请确认文件是否正确。\n" + ex.Message);
				throw ex;
			}
		}

		public Size CalcSize(Graphics g, Font f, TextBoxControl editor)
		{
			Size size = new Size();

			if (XmlHeader != null)
			{
				Size szTemp = XmlHeader.CalcSize(g, f, editor);
				if (szTemp.Width > size.Width)
					size.Width = szTemp.Width;

				size.Height += szTemp.Height;
			}

			if (NodeRoot != null)
			{
				Size szTemp = NodeRoot.CalcSize(g, f, editor);
				if (szTemp.Width > size.Width)
					size.Width = szTemp.Width;

				size.Height += szTemp.Height;
			}

			Size = size;

			NCalc = false;

			return size;
		}

		public int LineCount()
		{
			int iCount = 0;
			if (XmlHeader != null)
				iCount += 1;
			if (NodeRoot != null)
				iCount += NodeRoot.LineCount();

			return iCount;
		}

		/// <summary>
		/// 通过行号获取行
		/// </summary>
		public Line GetLineByLineCount(int line) 
		{
			int CurrNumber = 0;
			if (XmlHeader != null)
			{
				CurrNumber += 1;
				if(line == CurrNumber)
				{
					return XmlHeader.FirstLine;
				}
			}
			if (NodeRoot != null)
			{
				return NodeRoot.GetLineByLineNumber(ref CurrNumber, line);
			}
			return null;
		}

		public void InsertXml(string xml)
		{
			System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

			try
			{
				doc.LoadXml(xml);

				if (NodeRoot == null)
				{
					NodeRoot = new VXmlNode(this);

					NodeRoot.Init(doc.DocumentElement);
				}
				else if (_editor.Caret.Line > 1)
				{
					Line line = this.GetLineByLineCount(_editor.Caret.Line);
					if (line.Node != null)
					{
						if (line == line.Node.FirstLine)
							line.Node.Init(doc.DocumentElement);
						else
						{
							VXmlNode node = line.Node;
							node.AddChildNode(doc.DocumentElement);
						}
					}
				}

				NCalc = true;
			}
			catch (System.Exception ex)
			{
				MessageBox.Show("读取XML字符串失败，请确认xml字符串是否正确。\n" + ex.Message);
			}
		}

		#region Event Funcation
		void OnDocumentAboutToBeChanged(DocumentEventArgs e)
		{
			if (DocumentAboutToBeChanged != null)
			{
				DocumentAboutToBeChanged(this, e);
			}
		}

		void OnDocumentChanged(DocumentEventArgs e)
		{
			if (DocumentChanged != null)
			{
				DocumentChanged(this, e);
			}
		}

		public void CommitUpdate()
		{
			if (UpdateCommited != null)
			{
				UpdateCommited(this, EventArgs.Empty);
			}
		}

		void OnTextContentChanged(EventArgs e)
		{
			if (TextContentChanged != null)
			{
				TextContentChanged(this, e);
			}
		}
		#endregion

	}
}
