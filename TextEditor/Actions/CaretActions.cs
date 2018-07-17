/*********************************************************************************************
*文件名：	CaretActions.cs
*创建日期：	2015-09-02
*作者：	张鑫
*版本：	1.0
*说明：	光标Action
*----------------------------------------------------------------------------------------------
*修改记录：
*日期			版本	修改人	修改内容
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace TextEditor.Actions
{
	/// <summary>
	/// 光标向左移动一个字母的位置
	/// </summary>
	public class CaretLeft : AbstractEditAction
	{
		public override void Execute(TextBoxControl editor)
		{
			if (editor.Caret.Position.X == 0)
			{
				if (editor.Caret.Position.Y > 1)
				{
					int lineWidth = editor.GetLineWidth(editor.Caret.Position.Y - 1);
					if (lineWidth > editor.DrawRectangle.Width && editor.HorizontalScroll.Visible)
					{
						editor.HorizontalScroll.Value = Math.Min(editor.HorizontalScroll.Maximum, editor.HorizontalScroll.Value + lineWidth - editor.DrawRectangle.Width);
					}
					// 由于光标的起点为光标顶端点，在计算相对于垂直滚动条位置时需减2
					if ((editor.Caret.Position.Y - 2) * editor.FontHeight < editor.VerticalScroll.Value && editor.VerticalScroll.Visible)
					{
						editor.VerticalScroll.Value = Math.Max(editor.VerticalScroll.Minimum, editor.VerticalScroll.Value - editor.FontHeight);
					}
					editor.PerformLayout();
					editor.Caret.Position = new TextLocation(editor.CalColumnNumber(editor.Caret.Position.Y - 1,lineWidth), editor.Caret.Position.Y - 1);
					editor.Caret.UpdateCaretPosition();
				}
			}
			else
			{
				int location = editor.GetMovePositionByKey(true);
				int hScroll2 = editor.HorizontalScroll.Value - editor.DrawRectangle.Width;
				if (location < editor.HorizontalScroll.Value && editor.HorizontalScroll.Visible)
				{
					editor.HorizontalScroll.Value = Math.Max(editor.VerticalScroll.Minimum, hScroll2 > 0 ? hScroll2 : 0);
				}
				editor.PerformLayout();
				editor.Caret.Position = new TextLocation(editor.CalColumnNumber(editor.Caret.Position.Y,location), editor.Caret.Position.Y);
				editor.Caret.UpdateCaretPosition();
			}
		}
	}

	/// <summary>
	/// 光标向右移动一个字母的位置
	/// </summary>
	public class CaretRight : AbstractEditAction 
	{
		public override void Execute(TextBoxControl editor)
		{
			int lineWidth = editor.GetLineWidth(editor.Caret.Position.Y);
			if (lineWidth <= editor.CalColumnLocation(editor.Caret.Position.Y, editor.Caret.Position.X) && editor.Caret.Position.Y != editor.LineCount)
			{
				if (editor.HorizontalScroll.Value != 0 && editor.HorizontalScroll.Visible)
				{
					editor.HorizontalScroll.Value = 0;
				}
				if ((editor.Caret.Position.Y + 1) * editor.FontHeight > editor.DrawRectangle.Height && editor.VerticalScroll.Visible)
				{
					editor.VerticalScroll.Value = Math.Min(editor.VerticalScroll.Maximum, editor.VerticalScroll.Value + editor.FontHeight);
				}
				editor.PerformLayout();
				editor.Caret.Position = new TextLocation(0, editor.Caret.Position.Y + 1);
				editor.Caret.UpdateCaretPosition();
			}
			else 
			{
				int location = editor.GetMovePositionByKey(false);
				if (location > editor.DrawRectangle.Width && location > editor.HorizontalScroll.Value + editor.DrawRectangle.Width && editor.HorizontalScroll.Visible)
				{
					editor.HorizontalScroll.Value = Math.Min(editor.VerticalScroll.Maximum, editor.HorizontalScroll.Value + editor.DrawRectangle.Width);
				}
				editor.PerformLayout();
				editor.Caret.Position = new TextLocation(editor.CalColumnNumber(editor.Caret.Position.Y,location), editor.Caret.Position.Y);
				editor.Caret.UpdateCaretPosition();
			}
		}
	}

	/// <summary>
	/// 光标向上
	/// </summary>
	public class CaretUp : AbstractEditAction 
	{
		public override void Execute(TextBoxControl editor)
		{
			if(editor.Caret.Position.Y == 1)
			{
				return;
			}
			int lineWidth = editor.GetLineWidth(editor.Caret.Position.Y - 1);
			if (editor.CalColumnLocation(editor.Caret.Position.Y,editor.Caret.Position.X) > lineWidth)
			{
				if (editor.Caret.Position.Y > 1)
				{
					editor.Caret.Position = new TextLocation(editor.CalColumnNumber(editor.Caret.Position.Y - 1,lineWidth), editor.Caret.Position.Y - 1);
					editor.Caret.UpdateCaretPosition();
				}
			}
			else 
			{
				int oldLocation = editor.CalColumnLocation(editor.Caret.Position.Y,editor.Caret.Position.X);
				int location = oldLocation > 0 ? oldLocation : 0;
				editor.Caret.Position = new TextLocation(editor.CalColumnNumber(editor.Caret.Position.Y - 1, location), editor.Caret.Position.Y - 1);
				editor.Caret.UpdateCaretPosition();
			}

			if(editor.VerticalScroll.Visible && editor.VerticalScroll.Value > (editor.Caret.Line - 1) * editor.FontHeight)
			{
				editor.VerticalScroll.Value = Math.Max(editor.VerticalScroll.Minimum, editor.VerticalScroll.Value - editor.FontHeight);
				editor.PerformLayout();
			}
		}
	}

	/// <summary>
	/// 光标向下
	/// </summary>
	public class CaretDown : AbstractEditAction 
	{
		public override void Execute(TextBoxControl editor)
		{
			if(editor.Caret.Position.Y == editor.LineCount)
			{
				return;
			}
			int lineWidth = editor.GetLineWidth(editor.Caret.Position.Y + 1);
			if (editor.CalColumnLocation(editor.Caret.Position.Y ,editor.Caret.Position.X) > lineWidth)
			{
				editor.Caret.Position = new TextLocation(editor.CalColumnNumber( editor.Caret.Position.Y + 1,lineWidth), editor.Caret.Position.Y + 1);
				editor.Caret.UpdateCaretPosition();
			}
			else 
			{
				int oldLocation = editor.CalColumnLocation(editor.Caret.Position.Y, editor.Caret.Position.X);
				int location = oldLocation > 0 ? oldLocation : 0;
				editor.Caret.Position = new TextLocation(editor.CalColumnNumber(editor.Caret.Position.Y + 1, location), editor.Caret.Position.Y + 1);
				editor.Caret.UpdateCaretPosition();
			}
			if (editor.VerticalScroll.Visible && editor.VerticalScroll.Value + editor.DrawRectangle.Height < (editor.Caret.Line - 1) * editor.FontHeight) 
			{
				editor.VerticalScroll.Value = Math.Min(editor.VerticalScroll.Maximum, editor.VerticalScroll.Value + editor.FontHeight);
				editor.PerformLayout();
			}
		}
	}

	/// <summary>
	/// 到达元素右侧
	/// </summary>
	public class WordRight : CaretRight 
	{
		public override void Execute(TextBoxControl editor)
		{
			base.Execute(editor);
			List<TextLocation> locationList = editor.GetDoubleClickSelectSegmentInfo(editor.Caret.Position.Y,editor.Caret.Position.X);
			if(locationList.Count == 2)
			{
				editor.Caret.Position = new TextLocation(locationList[1].X, editor.Caret.Position.Y);
				editor.Caret.UpdateCaretPosition();
			}
		}
	}

	/// <summary>
	/// 到达单词左侧
	/// </summary>
	public class WordLeft : CaretLeft 
	{
		public override void Execute(TextBoxControl editor)
		{
			base.Execute(editor);
			List<TextLocation> locationList = editor.GetDoubleClickSelectSegmentInfo(editor.Caret.Position.Y, editor.Caret.Position.X + 1);
			if (locationList.Count == 2)
			{
				editor.Caret.Position = new TextLocation(locationList[0].X, editor.Caret.Position.Y);
				editor.Caret.UpdateCaretPosition();
			}
		}
	}

	/// <summary>
	/// 滚动条向上
	/// </summary>
	public class ScrollLineUp : AbstractEditAction 
	{
		public override void Execute(TextBoxControl editor)
		{
			if (!editor.VerticalScroll.Visible)
				return;
			editor.VerticalScroll.Value = Math.Max(editor.VerticalScroll.Minimum,editor.VerticalScroll.Value - editor.FontHeight);
			editor.PerformLayout();
		}
	}

	/// <summary>
	/// 滚动条向下
	/// </summary>
	public class ScrollLineDown : AbstractEditAction 
	{
		public override void Execute(TextBoxControl editor)
		{
			if (!editor.VerticalScroll.Visible)
				return;
			editor.VerticalScroll.Value = Math.Min(editor.VerticalScroll.Maximum ,editor.VerticalScroll.Value + editor.FontHeight);
			editor.PerformLayout();
		}
	}
}
