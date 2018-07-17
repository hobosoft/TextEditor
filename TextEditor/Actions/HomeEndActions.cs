using System;
using System.Collections.Generic;
using System.Drawing;

using TextEditor.Document;

namespace TextEditor.Actions
{
	public class HomeAction : AbstractEditAction
	{
		public override void Execute(TextBoxControl editor)
		{
			TextLocation homeLocation = editor.GetLineHomeInfo(editor.Caret.Line);
			if(homeLocation != TextLocation.Empty)
			{
				editor.Caret.Position = homeLocation;
				editor.Caret.UpdateCaretPosition();
				if (editor.HorizontalScroll.Value != 0 && editor.HorizontalScroll.Visible)
				{
					editor.HorizontalScroll.Value = 0;
					editor.PerformLayout();
				}
			}
		}
	}

	public class EndAction : AbstractEditAction
	{
		public override void Execute(TextBoxControl editor)
		{
			int lineWidth = editor.GetLineWidth(editor.Caret.Line);
			int colNum = editor.CalColumnNumber(editor.Caret.Line, lineWidth);
			editor.Caret.Position = new TextLocation(colNum,editor.Caret.Line);
			editor.Caret.UpdateCaretPosition();
			if (lineWidth > editor.DrawRectangle.Width && editor.HorizontalScroll.Visible)
			{
				editor.HorizontalScroll.Value = Math.Min(editor.HorizontalScroll.Maximum, editor.HorizontalScroll.Value + lineWidth - editor.DrawRectangle.Width);
				editor.PerformLayout();
			}
		}
	}


	public class MoveToStartAction : AbstractEditAction
	{
		public override void Execute(TextBoxControl editor)
		{
			if (editor.Caret.Line != 0 || editor.Caret.Column != 0)
			{
				editor.Caret.Position = new TextLocation(0, 1);
				if(editor.VerticalScroll.Visible)
				{
					editor.VerticalScroll.Value = editor.VerticalScroll.Minimum;
				}
				if(editor.HorizontalScroll.Visible)
				{
					editor.HorizontalScroll.Value = editor.HorizontalScroll.Minimum;
				}
				editor.PerformLayout();
			}
		}
	}


	public class MoveToEndAction : AbstractEditAction
	{
		public override void Execute(TextBoxControl editor)
		{
			int iLine = editor.LineCount;
			Line line = editor.GetLine(iLine);

			TextLocation endPos = new TextLocation(line.Length, iLine);
			if (editor.Caret.Position != endPos)
			{
				editor.Caret.Position = endPos;
				if(editor.VerticalScroll.Visible)
				{
					editor.VerticalScroll.Value = editor.VerticalScroll.Maximum;
				}
				if(editor.HorizontalScroll.Visible)
				{
					int lineWidth = editor.GetLineWidth(iLine);
					if (lineWidth > editor.DrawRectangle.Width && editor.HorizontalScroll.Visible)
					{
						editor.HorizontalScroll.Value = Math.Min(editor.HorizontalScroll.Maximum, editor.HorizontalScroll.Value + lineWidth - editor.DrawRectangle.Width);
					}
					editor.PerformLayout();
				}
			}
		}
	}
}
