using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TextEditor.Actions;
using TextEditor.Document;
using TextEditor.Gui;
using TextEditor.Interface;
//using VCI.IETM.XMLEditControl;
//using TextEditor.Intellisense;
using TextEditor.Undo;

namespace TextEditor
{
	public delegate bool KeyEventHandler(char ch);
	public delegate bool DialogKeyProcessor(Keys keyData);

	public class TextBoxControl : Panel
	{
		#region Fields
		private const int WM_IME_SETCONTEXT = 0x0281;

		//private string m_sFileName = "";
		//VXmlDocument m_xmlDocument;
        //XMLEditController m_editController = null;
		//IntellisenseManager m_IntellisenseManager = null;

		private IEditorProperties m_editorProperties = new DefaultEditorProperties();
		private SelectionManager m_selectionManager = null;

		bool _bDirty = false;
        int m_iTabSize = 4;
		int m_iFontHeight = 20;
		int m_iMarkWidth = 20;

		//private TextArea textArea;
		private readonly Timer timer = new Timer();
		//private bool needRiseSelectionChangedDelayed;
		private bool needRiseVisibleRangeChangedDelayed;

		private DrawHelper m_drawHelper = new DrawHelper();

		private Caret m_caret;

		private int updateLevel = 0;

		private IColorStrategy m_colorStrategy = new XmlColorStrategy();

		/// <summary>
		/// 段落集合
		/// </summary>
		private List<Paragraph> m_lstParagraph;// = new List<Line>();


		/// <summary>
		/// This hashtable contains all editor keys, where
		/// the key is the key combination and the value the
		/// action.
		/// </summary>
		protected Dictionary<Keys, IEditAction> m_editactions = new Dictionary<Keys, IEditAction>();

		public event EditorEventHandler EditorChanged;


		#endregion

		#region events
		[Browsable(true)]
		[Description("It occurs after changing of visible range. This event occurs with a delay relative to VisibleRangeChanged, and fires only once.")]
		public event EventHandler VisibleRangeChangedDelayed;

		/// <summary>
		/// VisibleRangeChanged event.
		/// It occurs after changing of visible range.
		/// </summary>
		[Browsable(true)]
		[Description("It occurs after changing of visible range.")]
		public event EventHandler VisibleRangeChanged;

		public event KeyEventHandler KeyEventHandler;
		public event DialogKeyProcessor DoProcessDialogKey;

		#endregion

		#region Properties

		public string FileName
		{
			get;
			set;
		}

		//public Size TextSize
		//{
		//	get;
		//	private set;
		//}
		//public XMLEditController EditController
		//{
		//    get { return m_editController; }
		//}

		//private IntellisenseManager Intellisense
		//{
		//	get { return m_IntellisenseManager; }
		//}

		//[Browsable(false)]
		//public VXmlDocument Document
		//{
		//	get { return m_xmlDocument; }
		//}

		//[Browsable(false)]
		//public IEditorProperties EditorProperties
		//{
		//	get { return m_editorProperties; }
		//}

		Color m_coLineNumber = Color.FromArgb(43, 145, 175);
		[Browsable(false)]
		public Color LineNumberColor
		{
			get { return m_coLineNumber; }
			set { m_coLineNumber = value; }
		}


		Color m_cohighLight = SystemColors.Highlight;
		[Browsable(false)]
		public Color HighLightColor
		{
			get { return m_cohighLight; }
			set { m_cohighLight = value; }
		}

		Color m_cohighLightText = SystemColors.HighlightText;
		[Browsable(false)]
		public Color HighLightTextColor
		{
			get { return m_cohighLightText; }
			set { m_cohighLightText = value; }
		}

		Color m_coText = Color.Black;

		[Browsable(false)]
		public Color TextColor
		{
			get { return m_coText; }
			set { m_coText = value; }
		}
		//[Browsable(false)]
		//[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		//public IColorStrategy ColorStrategy
		//{
		//	get { return m_colorStrategy; }
		//	set { m_colorStrategy = value; }
		//}

		public bool ShowLineNumber
		{
			get;
			set;
		}

		[Browsable(false)]
		public int CharWidth
		{
			get;
			private set;
		}

		[Browsable(false)]
		public new int FontHeight
		{
			get
			{
				return m_iFontHeight;
			}
		}

		public int TabIndent
		{
			get { return m_iTabSize; }
			set { m_iTabSize = value; }
		}

		[Browsable(false)]
		public int SpaceWidth
		{
			get;
			private set;
		}

		private int m_iWidthLineNum = 0;
		[Browsable(false)]
		public int LineNumBarWidth
		{
			get
			{
				if (ShowLineNumber)
					return m_iWidthLineNum;
				else
					return 0;
			}
			private set
			{
				m_iWidthLineNum = value;
			}
		}

		[Browsable(false)]
		public int EqualWidth
		{
			get;
			private set;
		}

		[Browsable(false)]
		internal bool NeedRecalc
		{
			get;
			set;
		}

		internal DrawHelper DrawHelper
		{
			get { return m_drawHelper; }
		}

		[Browsable(false)]
		internal Caret Caret
		{
			get
			{
				return m_caret;
			}
		}

		public bool ReadOnly
		{
			get;
			set;
		}

		[Browsable(false)]
		internal Rectangle DrawRectangle
		{
			get;
			set;
		}


		[Browsable(false)]
		public bool IsInUpdate
		{
			get
			{
				return updateLevel > 0;
			}
		}

		[Browsable(false)]
		internal SelectionManager SelectionManager
		{
			get
			{
				return m_selectionManager;
			}
		}

		[Browsable(false)]
		internal BookmarkManager BookmarkManager
		{
			get;
			set;
		}

		#region 事件锁定
		/// <summary>
		/// 锁定修改事件
		/// </summary>
		public void LockChangedEvent()
		{
			IsLockEvent = true;
		}

		/// <summary>
		/// 解锁修改事件，修改事件开始
		/// </summary>
		public void UnlockChangedEvent()
		{
			IsLockEvent = false;
		}

		/// <summary>
		/// 事件锁定
		/// </summary>
		[Browsable(false)]
		public bool IsLockEvent
		{
			get;
			private set;
		}
		#endregion

		private UndoStack m_undoStack = new UndoStack();

		[Browsable(false)]
		public UndoStack UndoStack
		{
			get
			{
				return m_undoStack;
			}
		}


		#endregion

		public TextBoxControl() : base()
		{
			//m_xmlDocument = new VXmlDocument(this);
			FileName = "";

			NeedRecalc = true;
			ShowLineNumber = false;
			ReadOnly = false;

			//InitializeComponent();
			CalcParamentsByFont();

			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			base.AutoScroll = true;
			timer.Tick += timer_Tick;

			m_lstParagraph = new List<Paragraph>();
			m_lstParagraph.Add(new Paragraph());

			m_caret = new Caret(this);
			m_caret.PositionChanged += new EventHandler(SearchMatchingBracket);

			//m_IntellisenseManager = new IntellisenseManager(this);
			m_selectionManager = new SelectionManager(this);

			GenerateDefaultActions();

			//Rectangle rcDraw = ClientRectangle;
			//rcDraw.X += Padding.Left + Margin.Left;
			//rcDraw.Width -= Padding.Left + Margin.Left;

			//rcDraw.X += LineNumBarWidth;
			//rcDraw.Width -= LineNumBarWidth;

			//DrawRectangle = rcDraw;
		}



		public void LoadFile(string file)
		{
			SelectionManager.ClearSelection();

			try
			{
				//m_xmlDocument.Load(file);
				FileName = file;
			}
			catch (System.Exception ex)
			{
				string sError = string.Format("Open {0} Field!\n{1}", file, ex.Message);
				MessageBox.Show(sError);
				FileName = "";

				throw ex;
			}

			NeedRecalc = true;

			Caret.Position = new TextLocation(0,1);
			m_lstParagraph.Clear();
			this.Invalidate();
		}


		public Point GetDrawingXPos(int line, int col)
		{
			Point ptPos = new Point();

			ptPos.X = CalColumnLocation(line,col);

			// 行号从一开始，便于计算，转化成下标
			ptPos.Y = (line - 1)  * FontHeight;

			ptPos.X -= HorizontalScroll.Value;
			ptPos.X += this.Padding.Left + this.Margin.Left;
			ptPos.X += LineNumBarWidth;

			ptPos.Y -= VerticalScroll.Value;
			ptPos.Y += Padding.Top + Margin.Top;

			return ptPos;
		}

		public int GetBarWidth()
		{
			int iWidth = 0;
			iWidth += LineNumBarWidth;

			return iWidth;
		}

		public void ScrollToCaret()
		{

		}

		#region Paint
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			//g.SmoothingMode = SmoothingMode.AntiAlias;
			//g.SetClip(e.ClipRectangle);

			if (NeedRecalc)
			{
				this.Recalc();
			}

			Brush paddingBrush = new SolidBrush(Color.Transparent);

			//draw padding area
			//top
			g.FillRectangle(paddingBrush, 0, -VerticalScroll.Value, ClientSize.Width, Math.Max(0, Padding.Top - 1));
			//bottom
			var bottomPaddingStartY = AutoScrollMinSize.Height + Padding.Top;
			g.FillRectangle(paddingBrush, 0, bottomPaddingStartY - VerticalScroll.Value, ClientSize.Width, ClientSize.Height);
			//right
			var rightPaddingStartX = AutoScrollMinSize.Width + Padding.Left + 1;
			g.FillRectangle(paddingBrush, rightPaddingStartX - HorizontalScroll.Value, 0, ClientSize.Width, ClientSize.Height);
			//left
			g.FillRectangle(paddingBrush, 0, 0, Padding.Left - 1, ClientSize.Height);
			if (HorizontalScroll.Value <= Padding.Left)
				g.FillRectangle(paddingBrush, HorizontalScroll.Value - 2, 0, Math.Max(0, Padding.Left - 1), ClientSize.Height);

			//draw indent area
			Rectangle rcArea = this.ClientRectangle;

			if (ShowLineNumber)
			{
				int iWidth = rcArea.Width;
				rcArea.Width = Padding.Left + LineNumBarWidth;
				e.Graphics.FillRectangle(SystemBrushes.Window, rcArea);
				e.Graphics.DrawLine(SystemPens.ButtonShadow, rcArea.Right, rcArea.Top, rcArea.Right, rcArea.Bottom);

				rcArea.Width = iWidth - Padding.Left + LineNumBarWidth;
				rcArea.Offset(Padding.Left + LineNumBarWidth, 0);
			}

			//if (LeftIndent > minLeftIndent)
			//    e.Graphics.DrawLine(servicePen, LeftIndentLine, 0, LeftIndentLine, ClientSize.Height);

			//g.FillRectangle(SystemBrushes.Window, this.ClientRectangle);

			Rectangle rcClip = rcArea;// ClientRectangle;
									  //if (ShowLineNumber)
									  //{
									  //    rcClip.X += this.Padding.Left + this.Margin.Left + NumberWidth;
									  //    rcClip.Width -= (this.Padding.Left + this.Margin.Left + NumberWidth);
									  //}
									  //else
									  //{
									  //    rcClip.X += this.Padding.Left + this.Margin.Left;
									  //    rcClip.Width -= (this.Padding.Left + this.Margin.Left);
									  //}

			//rcClip.Y += this.Padding.Top + this.Margin.Top;
			//rcClip.Height -= (this.Padding.Top + this.Margin.Top + Padding.Bottom + Margin.Bottom);
			LineCount = 0;
			g.SetClip(rcClip);


			if (m_lstParagraph.Count > 0)
			{
				Point ptPos = new Point(this.Padding.Left + this.Margin.Left - HorizontalScroll.Value, this.Padding.Top + this.Margin.Top - VerticalScroll.Value);

				ptPos.X += LineNumBarWidth;

				foreach (Paragraph paragraph in m_lstParagraph)
				{
					paragraph.Draw(this, g, Font, ptPos);

					ptPos.Y = (int)(ptPos.Y + paragraph.Height + 0.5f);
				}
				//if (m_xmlDocument == null)
				//	return;
				//if (m_xmlDocument.XmlHeader != null)
				//	m_xmlDocument.XmlHeader.Draw(g, Font, ref ptPos, this, ref iLine);

				//if (m_xmlDocument.NodeRoot != null)
				//	m_xmlDocument.NodeRoot.Draw(g, Font, ref ptPos, this, ref iLine);
			}

			//g.SmoothingMode = SmoothingMode.None;
			g.ResetClip();

			base.OnPaint(e);
		}

		protected override void OnScroll(ScrollEventArgs se)
		{
			base.OnScroll(se);
			OnVisibleRangeChanged();
			Invalidate();
		}

		protected override void OnClientSizeChanged(EventArgs e)
		{
			base.OnClientSizeChanged(e);


			//if (WordWrap)
			//{
			//    RecalcWordWrap(0, lines.Count - 1);
			//    Invalidate();
			//}
			Rectangle rcDraw = ClientRectangle;
			rcDraw.X += Padding.Left + Margin.Left;
			rcDraw.Width -= (Padding.Left + Margin.Left + Padding.Right + Margin.Right);

			rcDraw.X += LineNumBarWidth;
			rcDraw.Width -= LineNumBarWidth;

			rcDraw.Y += Padding.Top + Margin.Top;
			rcDraw.Height -= (Padding.Top + Margin.Top + Padding.Bottom + Margin.Bottom);
			DrawRectangle = rcDraw;

            OnVisibleRangeChanged();
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
            Invalidate();
            base.OnMouseWheel(e);
            OnVisibleRangeChanged();
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);

			//using (Graphics g = this.CreateGraphics())
			//{
			//    SpaceWidth = DrawHelper.MeasureStringWidth(g, " ", Font);
			//    //FontHeight = Font.Height;

			//    int iWidth = DrawHelper.MeasureStringWidth(g, "M", Font);
			//    CharWidth = (int) Math.Round(CharWidth*1f /*0.85*/) - 1 /*0*/;

				CalcParamentsByFont();
			//}
			
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			return ExecuteDialogKey(keyData) || base.ProcessDialogKey(keyData);
		}

		#endregion

					/// <summary>
		/// This method executes a dialog key
		/// </summary>
		public bool ExecuteDialogKey(Keys keyData)
		{
			// try, if a dialog key processor was set to use this
			if (DoProcessDialogKey != null && DoProcessDialogKey(keyData)) {
				return true;
			}
			
			// if not (or the process was 'silent', use the standard edit actions
			IEditAction action =  GetEditAction(keyData);
			//AutoClearSelection = true;
			if (action != null)
			{
				BeginUpdate();
				try
				{
					object obj = new object();
					lock (obj)
					{
						action.Execute(this);
						//if (SelectionManager.HasSomethingSelected && AutoClearSelection /*&& caretchanged*/)
						//{
						//    if (EditorProperties.DocumentSelectionMode == DocumentSelectionMode.Normal)
						//    {
						//        SelectionManager.ClearSelection();
						//    }
						//}
					}
				}
				finally
				{
					EndUpdate();
					Caret.UpdateCaretPosition();
				}
				return true;
			}
			return false;
		}

		public void BeginUpdate()
		{
			updateLevel++;
		}

		public void EndUpdate()
		{
			Debug.Assert(updateLevel > 0);
			updateLevel = Math.Max(0, updateLevel - 1);
		}


		//AbstractMargin updateMargin = null;
		//public void Refresh(AbstractMargin margin)
		//{
		//    updateMargin = margin;
		//    Invalidate(updateMargin.DrawingPosition);
		//    Update();
		//    updateMargin = null;
		//}

		#region Event virtual Function
		public void SimulateKeyPress(char ch)
		{
			if (SelectionManager.HasSomethingSelected)
			{
				if (SelectionManager.SelectionIsReadonly)
					return;
			}
			//else if (IsReadOnly(Caret.Offset))
			//{
			//    return;
			//}

			if (ch < ' ')
			{
				return;
			}

			//if (!hiddenMouseCursor && TextEditorProperties.HideMouseCursor)
			//{
			//    if (this.ClientRectangle.Contains(PointToClient(Cursor.Position)))
			//    {
			//        mouseCursorHidePosition = Cursor.Position;
			//        hiddenMouseCursor = true;
			//        Cursor.Hide();
			//    }
			//}
			//CloseToolTip();

			BeginUpdate();
			UndoStack.StartUndoGroup();
			try
			{
				// INSERT char
				if (!HandleKeyPress(ch))
				{
					switch (Caret.CaretMode)
					{
						case CaretMode.InsertMode:
							InsertChar(ch);
							break;
						case CaretMode.OverwriteMode:
							ReplaceChar(ch);
							break;
						default:
							Debug.Assert(false, "Unknown caret mode " + Caret.CaretMode);
							break;
					}
				}

				int currentLineNr = Caret.Line;
				
				EndUpdate();
			}
			finally
			{
				UndoStack.EndUndoGroup();
			}
		}

		/// <remarks>
		/// Inserts a single character at the caret position
		/// </remarks>
		public void InsertChar(char ch)
		{
			bool updating = IsInUpdate;
			if (!updating)
			{
				BeginUpdate();
			}

			// filter out forgein whitespace chars and replace them with standard space (ASCII 32)
			if (Char.IsWhiteSpace(ch) && ch != '\t' && ch != '\n')
			{
				ch = ' ';
			}

			UndoStack.StartUndoGroup();
			//if (EditorProperties.DocumentSelectionMode == DocumentSelectionMode.Normal &&
			//	SelectionManager.SelectionCollection.Count > 0)
			if (SelectionManager.SelectionCollection.Count > 0)
			{
				Caret.Position = SelectionManager.SelectionCollection[0].StartPosition;
				SelectionManager.RemoveSelectedText();
			}
			Paragraph caretLine = GetParagraph(Caret.Line);
			int dc = Caret.Column;
			InsertOperate operate = InsertOperate.None;
			Block segment = null;
			if (caretLine.Insert(dc, ch.ToString(), Caret, ref operate, ref segment) && operate != InsertOperate.None) 
			{
				int ptx = CalColumnLocation(Caret.Line,Caret.Column);
				Point point = new Point(ptx,Caret.Line * FontHeight);
				InsertCharEventArgs args = new InsertCharEventArgs() { CaretPoint = point, Operate = operate,Segment = segment};
                if (InsertCharEvent != null)
                {
                    InsertCharEvent(args);
                }
			}

			UndoStack.EndUndoGroup();
			if (!updating)
			{
				EndUpdate();
			}

			Invalidate();
		}

		/// <summary>
		/// 插入字符操作事件，用于智能感知
		/// </summary>
		public event InsertCharHandle InsertCharEvent;

		/// <remarks>
		/// Replaces a char at the caret position
		/// </remarks>
		public void ReplaceChar(char ch)
		{
			//bool updating = motherTextEditorControl.IsInUpdate;
			//if (!updating)
			//{
			//    BeginUpdate();
			//}
			//if (Document.TextEditorProperties.DocumentSelectionMode == DocumentSelectionMode.Normal && SelectionManager.SelectionCollection.Count > 0)
			//{
			//    Caret.Position = SelectionManager.SelectionCollection[0].StartPosition;
			//    SelectionManager.RemoveSelectedText();
			//}

			//int lineNr = Caret.Line;
			//LineSegment line = Document.GetLineSegment(lineNr);
			//int offset = Document.PositionToOffset(Caret.Position);
			//if (offset < line.Offset + line.Length)
			//{
			//    Document.Replace(offset, 1, ch.ToString());
			//}
			//else
			//{
			//    Document.Insert(offset, ch.ToString());
			//}
			//if (!updating)
			//{
			//    EndUpdate();
			//    UpdateLineToEnd(lineNr, Caret.Column);
			//}
			//++Caret.Column;
			//			++Caret.DesiredColumn;
		}

		/// <summary>
		/// This method is called on each Keypress
		/// </summary>
		/// <returns>
		/// True, if the key is handled by this method and should NOT be
		/// inserted in the textarea.
		/// </returns>
		protected internal virtual bool HandleKeyPress(char ch)
		{
			if (KeyEventHandler != null)
			{
				return KeyEventHandler(ch);
			}
			return false;
		}


		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			SimulateKeyPress(e.KeyChar);
			e.Handled = true;
		}


		/// <summary>
		/// Occurs when VisibleRange is changed
		/// </summary>
		public virtual void OnVisibleRangeChanged()
		{
			//needRecalcFoldingLines = true;

			needRiseVisibleRangeChangedDelayed = true;
			ResetTimer(timer);
			if (VisibleRangeChanged != null)
				VisibleRangeChanged(this, new EventArgs());
		}


		public virtual void OnVisibleRangeChangedDelayed()
		{
			if (VisibleRangeChangedDelayed != null)
				VisibleRangeChangedDelayed(this, new EventArgs());
		}

		/// <summary>
		/// 鼠标按下
		/// </summary>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			this.Focus();

			if (e.Button == MouseButtons.Left && DrawRectangle.Contains(e.Location))
			{
				Point pt = e.Location;
				// 计算行数的时候减去控件上边缘存留的空间
				int line = (pt.Y + VerticalScroll.Value - Padding.Top - Margin.Top) / FontHeight + 1;
				int ptX = pt.X - Padding.Left - Margin.Left + HorizontalScroll.Value;
				ptX -= LineNumBarWidth;

				int col = CalColumnNumber(line, ptX);
				Caret.Position = new TextLocation(col, line);
				SelectionManager.ExtendSelection(Caret.Position,Caret.Position);
				Caret.UpdateCaretPosition();
			}
			base.OnMouseDown(e);
		}

		/// <summary>
		/// 鼠标移动
		/// </summary>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left && DrawRectangle.Contains(e.Location) && SelectionManager.SelectionStart.X != -1)
			{
				Point pt = e.Location;
				int line = (pt.Y + VerticalScroll.Value - Padding.Top - Margin.Top) / FontHeight + 1;

				int ptX = pt.X - Padding.Left - Margin.Left + HorizontalScroll.Value;
				ptX -= LineNumBarWidth;

				int col = CalColumnNumber(line, ptX);

				TextLocation endLocation = new TextLocation(col, line);
				SelectionManager.ExtendSelection(SelectionManager.SelectionStart, endLocation);
				Caret.Position = endLocation;
				Caret.UpdateCaretPosition();

			}
			base.OnMouseMove(e);
		}

		/// <summary>
		/// 鼠标双击
		/// </summary>
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left && DrawRectangle.Contains(e.Location))
			{
				Point pt = e.Location;
				int line = (pt.Y + VerticalScroll.Value - Padding.Top - Margin.Top) / FontHeight + 1;

				int ptX = pt.X - Padding.Left - Margin.Left + HorizontalScroll.Value;
				ptX -= LineNumBarWidth;

				int colNum = CalColumnNumber(line,ptX);
				List<TextLocation> locationList = GetDoubleClickSelectSegmentInfo(line, colNum);
				if (locationList.Count == 2)
				{
					SelectionManager.ExtendSelection(locationList[0], locationList[1]);
					Caret.Position = locationList[1];
					Caret.UpdateCaretPosition();
				}

			}
			base.OnMouseDoubleClick(e);
		}

		/// <summary>
		/// 计算列号（当前行中的第几个字符）
		/// </summary>
		public int CalColumnNumber(int lineNum, int ptX) 
		{
			Paragraph line = GetParagraph(lineNum);
			int drawPos = 0;
			int colNumber = 0;

			if (line == null)
				return colNumber;
			if (ptX == 0)
				return colNumber;

			using (Graphics g = this.CreateGraphics())
			{
				foreach (Block segment in line.Blocks)
				{
					int newdrawPos = 0;
					if (segment.BlockType == BlockType.Space)
					{
						newdrawPos = drawPos + SpaceWidth;
						if (newdrawPos >= ptX)
						{
							return IsNearerToAThanB(ptX, drawPos, newdrawPos) ? colNumber : colNumber + 1;
						}
						colNumber += 1;
					}
					else if (segment.BlockType == BlockType.Tab)
					{
						newdrawPos = drawPos + SpaceWidth * segment.Length;
						if (newdrawPos >= ptX)
						{
							return IsNearerToAThanB(ptX, drawPos, newdrawPos) ? colNumber : colNumber + segment.Length;
						}
						colNumber += 4;
					}
					else
					{
						string text = segment.Text;
						int textLength = DrawHelper.MeasureStringWidth(g, text, Font);
						if (drawPos + textLength >= ptX && drawPos < ptX)
						{
							for (int i = 0; i < text.Length; i++)
							{
								int wordWidth = DrawHelper.MeasureStringWidth(g, text[i].ToString(), Font);
								newdrawPos = drawPos + wordWidth;
								if (newdrawPos >= ptX)
								{
									return IsNearerToAThanB(ptX, drawPos, newdrawPos) ? colNumber : colNumber + 1;
								}
								drawPos += wordWidth;
								colNumber += 1;
							}
						}
						else
						{
							newdrawPos = drawPos + textLength;
							colNumber += text.Length;
						}

					}
					drawPos = newdrawPos;
				}
			}
			return colNumber;
		}

		/// <summary>
		/// 计算列位置
		/// </summary>
		public int CalColumnLocation(int lineNum, int colNum) 
		{
			Paragraph line = GetParagraph(lineNum);
			int drawPos = 0;
			int number = 0;

			if (line == null)
				return number;
			if (colNum == 0)
				return 0;

			using (Graphics g = this.CreateGraphics())
			{
				foreach(Block segment in line.Blocks)
				{
					int newdrawPos = 0;
					if (segment.BlockType == BlockType.Space)
					{
						newdrawPos = drawPos + SpaceWidth;
						number += 1;
						if (number == colNum)
							return newdrawPos;
					}
					else if (segment.BlockType == BlockType.Tab)
					{
						newdrawPos = drawPos + SpaceWidth * segment.Length;
						number += 4;
						if (number == colNum)
							return newdrawPos;
					}
					else 
					{
						string text = segment.Text;
						int textLength = DrawHelper.MeasureStringWidth(g, text, Font);
						if (number + text.Length >= colNum && number < colNum)
						{
							for (int i = 0; i < text.Length; i++)
							{
								int wordWidth = DrawHelper.MeasureStringWidth(g, text[i].ToString(), Font);
								newdrawPos = drawPos + wordWidth;
								number += 1;
								if (number == colNum)
								{
									return newdrawPos;
								}
								drawPos += wordWidth;

							}
						}
						else 
						{
							newdrawPos = drawPos + textLength;
							number += text.Length;
						}

					}
					drawPos = newdrawPos;
				}
			}
			return drawPos;
		}

		/// <summary>
		/// 获取选中的元素的信息
		/// </summary>
		/// <returns></returns>
		public List<TextLocation> GetDoubleClickSelectSegmentInfo(int lineNum, int colNum) 
		{
			List<TextLocation> locationList = new List<TextLocation>();
			Paragraph line = GetParagraph(lineNum);
			int ptX = CalColumnLocation(lineNum, colNum);
			int drawPos = 0;
			int colNumber = 0;

			if (line == null)
				return locationList;

			using (Graphics g = this.CreateGraphics())
			{
				foreach (Block segment in line.Blocks)
				{
					int newdrawPos = 0;
					if (segment.BlockType == BlockType.Space)
					{
						newdrawPos = drawPos + SpaceWidth;

						if (newdrawPos >= ptX)
						{
							locationList.Add(new TextLocation(colNumber, lineNum));
							locationList.Add(new TextLocation(colNumber + 1,lineNum));
							return locationList;
						}
						colNumber += 1;
					}
					else if (segment.BlockType == BlockType.Tab)
					{
						newdrawPos = drawPos + SpaceWidth * segment.Length;
						if (newdrawPos >= ptX)
						{
							locationList.Add(new TextLocation(colNumber, lineNum));
							locationList.Add(new TextLocation(colNumber + 4, lineNum));
							return locationList;
						}
						colNumber += 4;
					}
					else
					{
						string text = segment.Text;
						int textLength = DrawHelper.MeasureStringWidth(g, text, Font);
						if (drawPos + textLength >= ptX && drawPos < ptX)
						{
							newdrawPos = drawPos + textLength;
							locationList.Add(new TextLocation(colNumber, lineNum));
							locationList.Add(new TextLocation(colNumber + text.Length, lineNum));
							return locationList;
						}
						else
						{
							newdrawPos = drawPos + textLength;
							colNumber += text.Length;
						}

					}
					drawPos = newdrawPos;
				}
			}
			return locationList;
		}

		/// <summary>
		/// 获取home信息
		/// </summary>
		public TextLocation GetLineHomeInfo(int lineNum) 
		{
			Paragraph line = GetParagraph(lineNum);
			int colNumber = 0;
			if (line == null)
				return TextLocation.Empty;
			foreach(Block segment in line.Blocks)
			{
				if (segment.BlockType == BlockType.Space)
				{
					colNumber += 1;
				}
				else if (segment.BlockType == BlockType.Tab)
				{
					colNumber += 4;
				}
				else
				{
					return new TextLocation(colNumber, lineNum);
				}
			}
			return TextLocation.Empty;
		}

		/// <summary>
		/// 获取通过键盘控制鼠标移动的位置
		/// </summary>
		/// <returns></returns>
		public int GetMovePositionByKey(bool isLeft) 
		{
			int location = CalColumnLocation(Caret.Position.Y ,Caret.Position.X);
			Paragraph line = GetParagraph(Caret.Position.Y);
			int drawPos = 0;

			if (line == null)
				return location;

			bool isGetLocation = false;
			using (Graphics g = this.CreateGraphics())
			{
				foreach (Block segment in line.Blocks)
				{
					int newdrawPos = 0;
					if (segment.BlockType == BlockType.Space)
					{
						newdrawPos = drawPos + SpaceWidth;
						if (isGetLocation)
						{
							return newdrawPos;
						}
						if (newdrawPos >= location)
						{
							isGetLocation = true;
							if (isLeft)
							{
								return drawPos;
							}
							if (location == 0)
							{
								return newdrawPos;
							}
						}

					}
					else if (segment.BlockType == BlockType.Tab)
					{
						newdrawPos = drawPos + SpaceWidth * segment.Length;
						if (isGetLocation)
						{
							return newdrawPos;
						}
						if (newdrawPos >= location)
						{
							isGetLocation = true;
							if (isLeft)
							{
								return drawPos;
							}
							if (location == 0)
							{
								return newdrawPos;
							}
						}

					}
					else
					{
						string text = segment.Text;
						int textLength = DrawHelper.MeasureStringWidth(g, text, Font);
						if (drawPos + textLength >= location && drawPos <= location)
						{
							for (int i = 0; i < text.Length; i++)
							{
								int wordWidth = DrawHelper.MeasureStringWidth(g, text[i].ToString(), Font);
								newdrawPos = drawPos + wordWidth;
								if (isGetLocation)
								{
									return newdrawPos;
								}
								if (newdrawPos >= location)
								{
									isGetLocation = true;
									if (isLeft)
									{
										return drawPos;
									}
									if (location == 0) 
									{
										return newdrawPos;
									}
								}

								drawPos += wordWidth;
							}
						}
						else
						{
							newdrawPos = drawPos + textLength;
						}

					}
					if(newdrawPos != 0)
						drawPos = newdrawPos;
				}
			}

			return location;
		}

		/// <summary>
		/// 获取行宽
		/// </summary>
		public int GetLineWidth(int lineNumber) 
		{
			Paragraph line = GetParagraph(lineNumber);
			int drawPos = 0;
			if(line == null)
			{
				return drawPos;
			}

			using (Graphics g = this.CreateGraphics())
			{
				foreach (Block segment in line.Blocks)
				{
					if (segment.BlockType == BlockType.Space)
					{
						drawPos += SpaceWidth;
					}
					else if (segment.BlockType == BlockType.Tab)
					{
						drawPos += SpaceWidth * segment.Length;
					}
					else
					{
						int textLength = DrawHelper.MeasureStringWidth(g, segment.Text, Font);
						drawPos += textLength;
					}
				}
			}
			return drawPos;
		}

		static bool IsNearerToAThanB(int num, int a, int b)
		{
			return Math.Abs(a - num) < Math.Abs(b - num);
		}
		#endregion

		private IntPtr m_hImc;
		protected override void WndProc(ref Message m)
		{
			if (this.ImeMode != ImeMode.Disable && this.ImeMode != ImeMode.Off && m.Msg == WM_IME_SETCONTEXT && m.WParam.ToInt32() != 0)
			{
				Ime.ImmAssociateContext(Handle, m_hImc);
			}
			base.WndProc(ref m);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			m_hImc = Ime.ImmGetContext(Handle);
		}

		#region Private Function
		private void CalcParamentsByFont()
		{
			using (Graphics g = this.CreateGraphics())
			{
				Size szNumber = TextRenderer.MeasureText(g, "8", Font, new Size(100, 100), TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.PreserveGraphicsClipping);

				int iNumberWidth = szNumber.Width * 5 + Margin.Left + Margin.Right;
				LineNumBarWidth = iNumberWidth;

				Size szChar = TextRenderer.MeasureText(g, "M", Font, new Size(100, 100), TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.PreserveGraphicsClipping);

				CharWidth = szChar.Width;

				int height1 = TextRenderer.MeasureText("_", Font).Height;
				int height2 = (int)Math.Ceiling(Font.GetHeight());
				m_iFontHeight = Math.Max(height1, height2) + 1;
				m_iMarkWidth = m_iFontHeight;

				//FontHeight = Font.Height;

				SpaceWidth = DrawHelper.MeasureStringWidth(g, " ", Font);

				EqualWidth = DrawHelper.MeasureStringWidth(g, "=", Font);
			}
		}

		private void Recalc()
		{
			if (!NeedRecalc)
				return;

			NeedRecalc = false;

			Graphics g = this.CreateGraphics();
			try
			{
				SizeF size = CalcSize(g, Font);

				//Size size = m_xmlDocument.Size;

				NeedRecalc = false;
				//adjust AutoScrollMinSize
				int minWidth = (int)(size.Width + 2 + Padding.Left + Padding.Right);// + NumberWidth;
				minWidth += LineNumBarWidth;

				int minHeight = (int)(size.Height + 2 + Padding.Top + Padding.Bottom);
				AutoScrollMinSize = new Size(minWidth, minHeight);
			}
			finally
			{
				g.Dispose();
			}
		}


		/// <summary>
		/// 计算大小
		/// </summary>
		public SizeF CalcSize(Graphics g, Font f)
		{
			SizeF size = new SizeF();

			foreach (Paragraph paragraph in m_lstParagraph)
			{
				//if (line.State != VisibleState.Hidden)
				{
					SizeF lineSize = paragraph.CalcSize(g, f, this);

					size.Height += lineSize.Height;
					if (size.Width < lineSize.Width)
					{
						size.Width = lineSize.Width;
					}
				}
			}

			float minWidth = size.Width + 2 + Padding.Left + Padding.Right;// + NumberWidth;
			minWidth += LineNumBarWidth;

			float minHeight = size.Height + 2 + Padding.Top + Padding.Bottom;

			SizeF minSize = new SizeF(minWidth, minHeight);
			AutoScrollMinSize = minSize.ToSize();// new Size((int)Math.Round(minWidth), (int)Math.Round(minHeight));

			return size;
		}

		private void ResetTimer(Timer timer)
		{
			timer.Stop();
			if (IsHandleCreated)
				timer.Start();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			timer.Enabled = false;
			//if (needRiseSelectionChangedDelayed)
			//{
			//    needRiseSelectionChangedDelayed = false;
			//    OnSelectionChangedDelayed();
			//}
			if (needRiseVisibleRangeChangedDelayed)
			{
				needRiseVisibleRangeChangedDelayed = false;
				OnVisibleRangeChangedDelayed();
			}
		}

		void SearchMatchingBracket(object sender, EventArgs e)
		{
			//if (!TextEditorProperties.ShowMatchingBracket)
			//{
			//    textView.Highlight = null;
			//    return;
			//}
			//int oldLine1 = -1, oldLine2 = -1;
			//if (textView.Highlight != null && textView.Highlight.OpenBrace.Y >= 0 && textView.Highlight.OpenBrace.Y < Document.TotalNumberOfLines)
			//{
			//    oldLine1 = textView.Highlight.OpenBrace.Y;
			//}
			//if (textView.Highlight != null && textView.Highlight.CloseBrace.Y >= 0 && textView.Highlight.CloseBrace.Y < Document.TotalNumberOfLines)
			//{
			//    oldLine2 = textView.Highlight.CloseBrace.Y;
			//}
			//textView.Highlight = FindMatchingBracketHighlight();
			//if (oldLine1 >= 0)
			//    UpdateLine(oldLine1);
			//if (oldLine2 >= 0 && oldLine2 != oldLine1)
			//    UpdateLine(oldLine2);
			//if (textView.Highlight != null)
			//{
			//    int newLine1 = textView.Highlight.OpenBrace.Y;
			//    int newLine2 = textView.Highlight.CloseBrace.Y;
			//    if (newLine1 != oldLine1 && newLine1 != oldLine2)
			//        UpdateLine(newLine1);
			//    if (newLine2 != oldLine1 && newLine2 != oldLine2 && newLine2 != newLine1)
			//        UpdateLine(newLine2);
			//}
		}

		public bool IsEditAction(Keys keyData)
		{
			return m_editactions.ContainsKey(keyData);
		}

		internal IEditAction GetEditAction(Keys keyData)
		{
			if (!IsEditAction(keyData))
			{
				return null;
			}
			return (IEditAction)m_editactions[keyData];
		}

		void GenerateDefaultActions() 
		{
			m_editactions[Keys.Left] = new CaretLeft();
			m_editactions[Keys.Right] = new CaretRight();
			m_editactions[Keys.Up] = new CaretUp();
			m_editactions[Keys.Down] = new CaretDown();
			m_editactions[Keys.Left | Keys.Control] = new WordLeft();
			m_editactions[Keys.Right | Keys.Control] = new WordRight();
			m_editactions[Keys.Down | Keys.Control] = new ScrollLineDown();
			m_editactions[Keys.Up | Keys.Control] = new ScrollLineUp();
			m_editactions[Keys.PageDown] = new MovePageDownAction();
			m_editactions[Keys.PageUp] = new MovePageUpAction();
			m_editactions[Keys.Home] = new HomeAction();
			m_editactions[Keys.End] = new EndAction();
			m_editactions[Keys.Home | Keys.Control] = new MoveToStartAction();
			m_editactions[Keys.End | Keys.Control] = new MoveToEndAction();


			m_editactions[Keys.Back] = new BackspaceAction();
			m_editactions[Keys.Delete] = new DeleteAction();
			m_editactions[Keys.Return] = new ReturnAction();

			m_editactions[Keys.Control | Keys.G] = new GotoAction();
			m_editactions[Keys.Control | Keys.F] = new FindAction();
			m_editactions[Keys.Control | Keys.R] = new ReplaceAction();

			m_editactions[Keys.Control | Keys.B] = new ToggleBookmark();
			//m_editactions[Keys.Control | Keys.P] = new GotoPrevBookmark();
			//m_editactions[Keys.Control | Keys.N] = new GotoNextBookmark();
	
		}

		#endregion

        #region Private EditController
      

        /// <summary>
        /// 从智能感知界面获取了一个信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_editController_OnSelectedItems(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
		#endregion

		/// <summary>
		/// 更新提交
		/// </summary>
		public event EventHandler UpdateCommited;

		public void CommitUpdate()
		{
			if (IsLockEvent && UpdateCommited != null)
			{
				UpdateCommited(this, EventArgs.Empty);
			}
		}


		/// <summary>
		///  获取行的数量
		/// </summary>
		public int LineCount
		{
			get { return m_lstParagraph.Count; }
			private set {; }
		}

		public bool IsDirty
		{
			get
			{
				return _bDirty;
			}
		}

		/// <summary>
		/// 通过行号获取行
		/// </summary>
		public Paragraph GetParagraph(int line)
		{
			int iLineCount = 0;

			foreach (Paragraph pg in m_lstParagraph)
			{
				if (iLineCount < line && (iLineCount + pg.LineCount) >= line)
				{
					int index = (line - iLineCount);
					return pg;//.Lines[index];
				}

				iLineCount += pg.LineCount;
			}
			//if (m_lstParagraph.Count >= line && line > 0)
			//{
			//	return m_lstParagraph[line - 1];
			//}
			return null;
		}

		public bool InsertParagraph(int index, Paragraph pg)
		{
			if (m_lstParagraph.Contains(pg))
				return false;

			if (index >= m_lstParagraph.Count)
				m_lstParagraph.Add(pg);
			else
				m_lstParagraph.Insert(index, pg);

			OnEditorChanged(new EditorEventArgs(this));

			NeedRecalc = true;

			return true;

		}

		/// <summary>
		/// 在当前行下插入新行
		/// </summary>
		public Paragraph InsertParagraph(Paragraph pg, ref int caretCol)
		{
			Paragraph newPg = null;
			if (m_lstParagraph.Contains(pg))
			{
				newPg = new Paragraph();
				int index = m_lstParagraph.IndexOf(pg);
				this.InsertParagraph(index + 1, newPg);
			}
			return newPg;
		}

		void OnEditorChanged(EditorEventArgs e)
		{
			if (!IsLockEvent)
			{
				_bDirty = true;
				if (EditorChanged != null)
				{
					EditorChanged(this, e);
				}
			}
		}
	}

	public delegate void InsertCharHandle(InsertCharEventArgs e);

	/// <summary>
	/// 插入事件参数类
	/// </summary>
	public class InsertCharEventArgs : EventArgs 
	{
		/// <summary>
		/// 当前光标所在坐标
		/// </summary>
		public Point CaretPoint
		{
			get;
			set;
		}

		/// <summary>
		/// 操作
		/// </summary>
		public InsertOperate Operate
		{
			get;
			set;
		}
		
		/// <summary>
		/// 当前编辑的段（只有在插入属性名和属性值时会返回该段，在编辑节点时该属性为空）
		/// </summary>
		public Block Segment
		{
			get;
			set;
		}

	}
}
