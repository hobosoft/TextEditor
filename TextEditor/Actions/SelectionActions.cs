// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 2659 $</version>
// </file>

using System;
using System.Drawing;
using VCI.XmlEditor.Document;

namespace VCI.XmlEditor.Actions
{
	public class ShiftCaretRight : CaretRight
	{
		public override void Execute(XmlEditorControl editor)
		{
			TextLocation oldCaretPos = editor.Caret.Position;
			base.Execute(editor);
			editor.AutoClearSelection = false;
			editor.SelectionManager.ExtendSelection(oldCaretPos, editor.Caret.Position);
		}
	}
	
	public class ShiftCaretLeft : CaretLeft
	{
		public override void Execute(XmlEditorControl editor)
		{
			TextLocation oldCaretPos = editor.Caret.Position;
			base.Execute(editor);
			editor.AutoClearSelection = false;
			editor.SelectionManager.ExtendSelection(oldCaretPos, editor.Caret.Position);
		}
	}
	
	public class ShiftCaretUp : CaretUp
	{
		public override void Execute(XmlEditorControl editor)
		{
			TextLocation oldCaretPos = editor.Caret.Position;
			base.Execute(editor);
			editor.AutoClearSelection = false;
			editor.SelectionManager.ExtendSelection(oldCaretPos, editor.Caret.Position);
		}
	}
	
	public class ShiftCaretDown : CaretDown
	{
		public override void Execute(XmlEditorControl editor)
		{
			TextLocation oldCaretPos = editor.Caret.Position;
			base.Execute(editor);
			editor.AutoClearSelection = false;
			editor.SelectionManager.ExtendSelection(oldCaretPos, editor.Caret.Position);
		}
	}
	
	public class ShiftWordRight : WordRight
	{
		public override void Execute(XmlEditorControl editor)
		{
			TextLocation oldCaretPos = editor.Caret.Position;
			base.Execute(editor);
			editor.AutoClearSelection = false;
			editor.SelectionManager.ExtendSelection(oldCaretPos, editor.Caret.Position);
		}
	}
	
	public class ShiftWordLeft : WordLeft
	{
		public override void Execute(XmlEditorControl editor)
		{
			TextLocation oldCaretPos = editor.Caret.Position;
			base.Execute(editor);
			editor.AutoClearSelection = false;
			editor.SelectionManager.ExtendSelection(oldCaretPos, editor.Caret.Position);
		}
	}
	
	public class ShiftHome : Home
	{
		public override void Execute(XmlEditorControl editor)
		{
			TextLocation oldCaretPos = editor.Caret.Position;
			base.Execute(editor);
			editor.AutoClearSelection = false;
			editor.SelectionManager.ExtendSelection(oldCaretPos, editor.Caret.Position);
		}
	}
	
	public class ShiftEnd : End
	{
		public override void Execute(XmlEditorControl editor)
		{
			TextLocation oldCaretPos = editor.Caret.Position;
			base.Execute(editor);
			editor.AutoClearSelection = false;
			editor.SelectionManager.ExtendSelection(oldCaretPos, editor.Caret.Position);
		}
	}
	
	public class ShiftMoveToStart : MoveToStart
	{
		public override void Execute(XmlEditorControl editor)
		{
			TextLocation oldCaretPos = editor.Caret.Position;
			base.Execute(editor);
			editor.AutoClearSelection = false;
			editor.SelectionManager.ExtendSelection(oldCaretPos, editor.Caret.Position);
		}
	}
	
	public class ShiftMoveToEnd : MoveToEnd
	{
		public override void Execute(XmlEditorControl editor)
		{
			TextLocation oldCaretPos = editor.Caret.Position;
			base.Execute(editor);
			editor.AutoClearSelection = false;
			editor.SelectionManager.ExtendSelection(oldCaretPos, editor.Caret.Position);
		}
	}
	
	public class ShiftMovePageUp : MovePageUp
	{
		public override void Execute(XmlEditorControl editor)
		{
			TextLocation oldCaretPos  = textArea.Caret.Position;
			base.Execute(textArea);
			textArea.AutoClearSelection = false;
			textArea.SelectionManager.ExtendSelection(oldCaretPos, textArea.Caret.Position);
		}
	}
	
	public class ShiftMovePageDown : MovePageDown
	{
		public override void Execute(XmlEditorControl editor)
		{
			TextLocation oldCaretPos = editor.Caret.Position;
			base.Execute(editor);
			editor.AutoClearSelection = false;
			editor.SelectionManager.ExtendSelection(oldCaretPos, editor.Caret.Position);
		}
	}
	
	public class SelectWholeDocument : AbstractEditAction
	{
		public override void Execute(XmlEditorControl editor)
		{
			editor.AutoClearSelection = false;
			TextLocation startPoint = new TextLocation(0, 0);
			TextLocation endPoint = editor.Document.OffsetToPosition(editor.Document.TextLength);
			if (editor.SelectionManager.HasSomethingSelected)
			{
				if (editor.SelectionManager.SelectionCollection[0].StartPosition == startPoint &&
					editor.SelectionManager.SelectionCollection[0].EndPosition == endPoint)
				{
					return;
				}
			}
			editor.Caret.Position = editor.SelectionManager.NextValidPosition(endPoint.Y);
			editor.SelectionManager.ExtendSelection(startPoint, endPoint);
			// after a SelectWholeDocument selection, the caret is placed correctly,
			// but it is not positioned internally.  The effect is when the cursor
			// is moved up or down a line, the caret will take on the column that
			// it was in before the SelectWholeDocument
			editor.SetDesiredColumn();
		}
	}
	
	public class ClearAllSelections : AbstractEditAction
	{
		public override void Execute(XmlEditorControl editor)
		{
			editor.SelectionManager.ClearSelection();
		}
	}
}
