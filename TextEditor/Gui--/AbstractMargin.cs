// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 1965 $</version>
// </file>

using System;
using System.Drawing;
using System.Windows.Forms;

using VCI.XmlEditor.Document;

namespace VCI.XmlEditor
{
	public delegate void MarginMouseEventHandler(AbstractMargin sender, Point mousepos, MouseButtons mouseButtons);
	public delegate void MarginPaintEventHandler(AbstractMargin sender, Graphics g, Rectangle rect);
	
	/// <summary>
	/// This class views the line numbers and folding markers.
	/// </summary>
	public abstract class AbstractMargin
	{
		Cursor cursor = Cursors.Default;
		
		[CLSCompliant(false)]
		protected Rectangle drawingPosition = new Rectangle(0, 0, 0, 0);
		[CLSCompliant(false)]
		protected XmlEditorControl _editor;
		
		public Rectangle DrawingPosition {
			get {
				return drawingPosition;
			}
			set {
				drawingPosition = value;
			}
		}

		public XmlEditorControl TextArea
		{
			get {
				return _editor;
			}
		}
		
		public IDocument Document {
			get {
				return _editor.Document;
			}
		}

		public IEditorProperties EditorProperties
		{
			get {
				return _editor.Document.EditorProperties;
			}
		}
		
		public virtual Cursor Cursor {
			get {
				return cursor;
			}
			set {
				cursor = value;
			}
		}
		
		public virtual Size Size {
			get {
				return new Size(-1, -1);
			}
		}
		
		public virtual bool IsVisible {
			get {
				return true;
			}
		}

		protected AbstractMargin(XmlEditorControl textArea)
		{
			this._editor = textArea;
		}
		
		public virtual void HandleMouseDown(Point mousepos, MouseButtons mouseButtons)
		{
			if (MouseDown != null) {
				MouseDown(this, mousepos, mouseButtons);
			}
		}
		public virtual void HandleMouseMove(Point mousepos, MouseButtons mouseButtons)
		{
			if (MouseMove != null) {
				MouseMove(this, mousepos, mouseButtons);
			}
		}
		public virtual void HandleMouseLeave(EventArgs e)
		{
			if (MouseLeave != null) {
				MouseLeave(this, e);
			}
		}
		
		public virtual void Paint(Graphics g, Rectangle rect)
		{
			if (Painted != null) {
				Painted(this, g, rect);
			}
		}
		
		public event MarginPaintEventHandler Painted;
		public event MarginMouseEventHandler MouseDown;
		public event MarginMouseEventHandler MouseMove;
		public event EventHandler            MouseLeave;
	}
}
