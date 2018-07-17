using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TextEditor
{
	/// <summary>
	/// In this enumeration are all caret modes listed.
	/// </summary>
	public enum CaretMode
	{
		/// <summary>
		/// If the caret is in insert mode typed characters will be
		/// inserted at the caret position
		/// </summary>
		InsertMode,

		/// <summary>
		/// If the caret is in overwirte mode typed characters will
		/// overwrite the character at the caret position
		/// </summary>
		OverwriteMode
	}


	public class Caret : System.IDisposable
	{
		int line = 1;
		int column = 0;
		int desiredXPos = 0;
		CaretMode caretMode;

		static bool caretCreated = false;
		bool hidden = true;
		TextBoxControl _editor;
		Point currentPos = new Point(-1, -1);
		Ime ime = null;
		CaretImplementation caretImplementation;

		/// <value>
		/// The 'prefered' xPos in which the caret moves, when it is moved
		/// up/down. Measured in pixels, not in characters!
		/// </value>
		public int DesiredColumn
		{
			get
			{
				return desiredXPos;
			}
			set
			{
				desiredXPos = value;
			}
		}

		/// <value>
		/// The current caret mode.
		/// </value>
		public CaretMode CaretMode
		{
			get
			{
				return caretMode;
			}
			set
			{
				caretMode = value;
				OnCaretModeChanged(EventArgs.Empty);
			}
		}

		public int Line
		{
			get
			{
				return line;
			}
			set
			{
				line = value;
				ValidateCaretPos();
				UpdateCaretPosition();
				OnPositionChanged(EventArgs.Empty);
			}
		}

		public int Column
		{
			get
			{
				return column;
			}
			set
			{
				column = value;
				ValidateCaretPos();
				UpdateCaretPosition();
				OnPositionChanged(EventArgs.Empty);
			}
		}

		public TextLocation Position
		{
			get
			{
				return new TextLocation(column, line);
			}
			set
			{
				column = value.X;
				line = value.Y;
				ValidateCaretPos();
				UpdateCaretPosition();
				OnPositionChanged(EventArgs.Empty);
			}
		}

		public int Offset
		{
			get
			{
				return 0;// _editor.Document.PositionToOffset(Position);
			}
		}

		public Caret(TextBoxControl _editor)
		{
			this._editor = _editor;
			_editor.GotFocus += new EventHandler(GotFocus);
			_editor.LostFocus += new EventHandler(LostFocus);
			if (Environment.OSVersion.Platform == PlatformID.Unix)
				caretImplementation = new ManagedCaret(this);
			else
				caretImplementation = new Win32Caret(this);
		}

		public void Dispose()
		{
			_editor.GotFocus -= new EventHandler(GotFocus);
			_editor.LostFocus -= new EventHandler(LostFocus);
			_editor = null;
			caretImplementation.Dispose();
		}

		public TextLocation ValidatePosition(TextLocation pos)
		{
			int line = Math.Max(0, Math.Min(_editor.LineCount, pos.Y));
			int column = Math.Max(0, pos.X);

			//if (column == int.MaxValue || !_editor.TextEditorProperties.AllowCaretBeyondEOL)
			//{
			//    LineSegment lineSegment = _editor.Document.GetLineSegment(line);
			//    column = Math.Min(column, lineSegment.Length);
			//}
			return new TextLocation(column, line);
		}

		/// <remarks>
		/// If the caret position is outside the document text bounds
		/// it is set to the correct position by calling ValidateCaretPos.
		/// </remarks>
		public void ValidateCaretPos()
		{
			line = Math.Max(0, Math.Min(_editor.LineCount, line));
			column = Math.Max(0, column);

			//if (column == int.MaxValue || !_editor.TextEditorProperties.AllowCaretBeyondEOL)
			//{
			//    LineSegment lineSegment = _editor.Document.GetLineSegment(line);
			//    column = Math.Min(column, lineSegment.Length);
			//}
		}

		void CreateCaret()
		{
			while (!caretCreated)
			{
				switch (caretMode)
				{
					case CaretMode.InsertMode:
						caretCreated = caretImplementation.Create(2, _editor.FontHeight);
						break;
					case CaretMode.OverwriteMode:
						caretCreated = caretImplementation.Create((int)_editor.SpaceWidth, _editor.FontHeight);
						break;
				}
			}
			if (currentPos.X < 0)
			{
				ValidateCaretPos();
				//currentPos = ScreenPosition;
			}
			caretImplementation.SetPosition(currentPos.X, currentPos.Y);
			caretImplementation.Show();
		}

		public void RecreateCaret()
		{
			Log("RecreateCaret");
			DisposeCaret();
			if (!hidden)
			{
				CreateCaret();
			}
		}

		void DisposeCaret()
		{
			if (caretCreated)
			{
				caretCreated = false;
				caretImplementation.Hide();
				caretImplementation.Destroy();
			}
		}

		void GotFocus(object sender, EventArgs e)
		{
			//Log("GotFocus, IsInUpdate=" + _editor.MotherTextEditorControl.IsInUpdate);
			hidden = false;
			//if (!_editor.MotherTextEditorControl.IsInUpdate)
			{
				CreateCaret();
				UpdateCaretPosition();
			}
		}

		void LostFocus(object sender, EventArgs e)
		{
			Log("LostFocus");
			hidden = true;
			DisposeCaret();
		}

		//public Point ScreenPosition
		//{
		//    get
		//    {
		//        int xpos = _editor.TextView.GetDrawingXPos(this.line, this.column);
		//        return new Point(_editor.TextView.DrawingPosition.X + xpos,
		//                         _editor.TextView.DrawingPosition.Y
		//                         + (_editor.Document.GetVisibleLine(this.line)) * _editor.TextView.FontHeight
		//                         - _editor.TextView.TextArea.VirtualTop.Y);
		//    }
		//}
		//int oldLine = -1;
		bool outstandingUpdate;

		internal void OnEndUpdate()
		{
			if (outstandingUpdate)
				UpdateCaretPosition();
		}

		void PaintCaretLine(Graphics g)
		{
			//if (!_editor.Document.TextEditorProperties.CaretLine)
			//    return;

			if (_editor.ReadOnly)
				return;

			g.DrawLine(SystemPens.WindowText, currentPos.X, 0, currentPos.X, _editor.DisplayRectangle.Height);
		}

		public void UpdateCaretPosition()
		{
			//Log("UpdateCaretPosition");

			if (!_editor.ReadOnly)
			{
				_editor.Invalidate();
			}
			else
			{
			//    if (caretImplementation.RequireRedrawOnPositionChange)
			//    {
			//        _editor.UpdateLine(oldLine);
			//        if (line != oldLine)
			//            _editor.UpdateLine(line);
			//    }
			//    else
			//    {
			//        if (_editor.MotherTextAreaControl.TextEditorProperties.LineViewerStyle == LineViewerStyle.FullRow && oldLine != line)
			//        {
			//            _editor.UpdateLine(oldLine);
			//            _editor.UpdateLine(line);
			//        }
			//    }
			}
			//oldLine = line;


			if (hidden || _editor.ReadOnly)
			{
				outstandingUpdate = true;
				return;
			}
			else
			{
				outstandingUpdate = false;
			}
			ValidateCaretPos();
			int lineNr = this.line;
			Point ptPos = _editor.GetDrawingXPos(lineNr, this.column);
			//LineSegment lineSegment = _editor.Document.GetLineSegment(lineNr);
			Point pos = ptPos;
			if (ptPos.X >= 0)
			{
				CreateCaret();
				bool success = caretImplementation.SetPosition(pos.X, pos.Y - 1);
				if (!success)
				{
					caretImplementation.Destroy();
					caretCreated = false;
					UpdateCaretPosition();
				}
			}
			else
			{
				caretImplementation.Destroy();
			}

			// set the input method editor location
			if (ime == null)
			{
				ime = new Ime(_editor.Handle, _editor.Font);
			}
			else
			{
				ime.HWnd = _editor.Handle;
				ime.Font = _editor.Font;
			}
			ime.SetIMEWindowLocation(ptPos.X, ptPos.Y);

			//currentPos = pos;
		}

		[Conditional("DEBUG")]
		static void Log(string text)
		{
			//Console.WriteLine(text);
		}

		#region Caret implementation
		internal void PaintCaret(Graphics g)
		{
			caretImplementation.PaintCaret(g);
			PaintCaretLine(g);
		}

		abstract class CaretImplementation : IDisposable
		{
			public bool RequireRedrawOnPositionChange;

			public abstract bool Create(int width, int height);
			public abstract void Hide();
			public abstract void Show();
			public abstract bool SetPosition(int x, int y);
			public abstract void PaintCaret(Graphics g);
			public abstract void Destroy();

			public virtual void Dispose()
			{
				Destroy();
			}
		}

		class ManagedCaret : CaretImplementation
		{
			System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 300 };
			bool visible;
			bool blink = true;
			int x, y, width, height;
			TextBoxControl _editor;
			Caret parentCaret;

			public ManagedCaret(Caret caret)
			{
				base.RequireRedrawOnPositionChange = true;
				this._editor = caret._editor;
				this.parentCaret = caret;
				timer.Tick += CaretTimerTick;
			}

			void CaretTimerTick(object sender, EventArgs e)
			{
				blink = !blink;
				//if (visible)
				//    _editor.UpdateLine(parentCaret.Line);
			}

			public override bool Create(int width, int height)
			{
				this.visible = true;
				this.width = width - 2;
				this.height = height;
				timer.Enabled = true;
				return true;
			}
			public override void Hide()
			{
				visible = false;
			}
			public override void Show()
			{
				visible = true;
			}
			public override bool SetPosition(int x, int y)
			{
				this.x = x - 1;
				this.y = y;
				return true;
			}
			public override void PaintCaret(Graphics g)
			{
				if (visible && blink)
					g.DrawRectangle(Pens.Gray, x, y, width, height);
			}
			public override void Destroy()
			{
				visible = false;
				timer.Enabled = false;
			}
			public override void Dispose()
			{
				base.Dispose();
				timer.Dispose();
			}
		}

		class Win32Caret : CaretImplementation
		{
			[DllImport("User32.dll")]
			static extern bool CreateCaret(IntPtr hWnd, int hBitmap, int nWidth, int nHeight);

			[DllImport("User32.dll")]
			static extern bool SetCaretPos(int x, int y);

			[DllImport("User32.dll")]
			static extern bool DestroyCaret();

			[DllImport("User32.dll")]
			static extern bool ShowCaret(IntPtr hWnd);

			[DllImport("User32.dll")]
			static extern bool HideCaret(IntPtr hWnd);

			TextBoxControl _editor;

			public Win32Caret(Caret caret)
			{
				this._editor = caret._editor;
			}

			public override bool Create(int width, int height)
			{
				return CreateCaret(_editor.Handle, 0, width, height);
			}
			public override void Hide()
			{
				HideCaret(_editor.Handle);
			}
			public override void Show()
			{
				ShowCaret(_editor.Handle);
			}
			public override bool SetPosition(int x, int y)
			{
				return SetCaretPos(x, y);
			}
			public override void PaintCaret(Graphics g)
			{
			}
			public override void Destroy()
			{
				DestroyCaret();
			}
		}
		#endregion

		//bool firePositionChangedAfterUpdateEnd;

		void FirePositionChangedAfterUpdateEnd(object sender, EventArgs e)
		{
			OnPositionChanged(EventArgs.Empty);
		}

		protected virtual void OnPositionChanged(EventArgs e)
		{
			//if (this._editor.MotherTextEditorControl.IsInUpdate)
			//{
			//    if (firePositionChangedAfterUpdateEnd == false)
			//    {
			//        firePositionChangedAfterUpdateEnd = true;
			//        this._editor.Document.UpdateCommited += FirePositionChangedAfterUpdateEnd;
			//    }
			//    return;
			//}
			//else if (firePositionChangedAfterUpdateEnd)
			//{
			//    this._editor.Document.UpdateCommited -= FirePositionChangedAfterUpdateEnd;
			//    firePositionChangedAfterUpdateEnd = false;
			//}

			//List<FoldMarker> foldings = _editor.Document.FoldingManager.GetFoldingsFromPosition(line, column);
			//bool shouldUpdate = false;
			//foreach (FoldMarker foldMarker in foldings)
			//{
			//    shouldUpdate |= foldMarker.IsFolded;
			//    foldMarker.IsFolded = false;
			//}

			//if (shouldUpdate)
			//{
			//    _editor.Document.FoldingManager.NotifyFoldingsChanged(EventArgs.Empty);
			//}

			if (PositionChanged != null)
			{
				PositionChanged(this, e);
			}
			_editor.ScrollToCaret();
		}

		protected virtual void OnCaretModeChanged(EventArgs e)
		{
			if (CaretModeChanged != null)
			{
				CaretModeChanged(this, e);
			}
			caretImplementation.Hide();
			caretImplementation.Destroy();
			caretCreated = false;
			CreateCaret();
			caretImplementation.Show();
		}

		/// <remarks>
		/// Is called each time the caret is moved.
		/// </remarks>
		public event EventHandler PositionChanged;

		/// <remarks>
		/// Is called each time the CaretMode has changed.
		/// </remarks>
		public event EventHandler CaretModeChanged;
	}
}
