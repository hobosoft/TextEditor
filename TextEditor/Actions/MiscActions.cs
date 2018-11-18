// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 3537 $</version>
// </file>

using System;
using System.Linq;
using System.Diagnostics;
using System.Drawing;
using System.Text;

using TextEditor.Document;
using System.Collections.Generic;

namespace TextEditor.Actions
{
	/// <summary>
	/// 退格键
	/// </summary>
	public class BackspaceAction : AbstractEditAction
	{
		/// <remarks>
		/// Executes this edit action
		/// </remarks>
		/// <param name="editor">The <see cref="Ieditor"/> which is used for callback purposes</param>
		public override void Execute(TextBoxControl editor)
		{
			if (editor.SelectionManager.HasSomethingSelected)
			{
				//Delete.DeleteSelection(editor);
			}
			else
			{
				if (editor.Caret.Line > 0 && editor.Caret.Column > 0 && !editor.ReadOnly)
				{
					editor.BeginUpdate();

					Paragraph line = editor.GetParagraph(editor.Caret.Line);
					if (line != null && line.Remove(editor.Caret.Column - 1, 1))
						editor.Caret.Column -= 1;

					editor.EndUpdate();				
				}
			}
		}
	}

	/// <summary>
	/// Delete键
	/// </summary>
	public class DeleteAction : AbstractEditAction
	{
		internal static void DeleteSelection(TextBoxControl editor)
		{
			Debug.Assert(editor.SelectionManager.HasSomethingSelected);
			if (editor.SelectionManager.SelectionIsReadonly)
				return;
			editor.BeginUpdate();
			editor.Caret.Position = editor.SelectionManager.SelectionCollection[0].StartPosition;
			editor.SelectionManager.RemoveSelectedText();
			editor.ScrollToCaret();
			editor.EndUpdate();
		}

		/// <remarks>
		/// Executes this edit action
		/// </remarks>
		/// <param name="textArea">The <see cref="ItextArea"/> which is used for callback purposes</param>
		public override void Execute(TextBoxControl editor)
		{
			if (editor.SelectionManager.HasSomethingSelected)
			{
				DeleteSelection(editor);
			}
			else
			{
				if (editor.ReadOnly)
					return;

				if (editor.Caret.Line > 0 && editor.Caret.Column > 0 && !editor.ReadOnly)
				{
					editor.BeginUpdate();
					Paragraph pg = editor.GetParagraph(editor.Caret.Line);
					if (pg != null)
						pg.Remove(editor.Caret.Column, 1);

					editor.EndUpdate();
				}
			}
		}
	}

	/// <summary>
	/// 回车键
	/// </summary>
	public class ReturnAction : AbstractEditAction
	{
		/// <remarks>
		/// Executes this edit action
		/// </remarks>
		/// <param name="editor">The <see cref="TextArea"/> which is used for callback purposes</param>
		public override void Execute(TextBoxControl editor)
		{
			if (editor.ReadOnly)
			{
				return;
			}
			editor.BeginUpdate();
			editor.UndoStack.StartUndoGroup();
			try
			{
				//////////////////////////////////////
				/*  zx 2015/9/10 XML文本编辑器中按下回车键只有以下情形才有效
				 * 	1.在首行中右标记符>后点击回车键时，该节点添加一个中间行，光标位置后的元素全部添加到新建的中间行中
				 * 	2.光标落入文本元素中时，不管是在首行还是在中间行，该节点顺次添加一个中间行，光标以后的文本及该行中其他元素一并添加到新建的中间行中
				 * 	3.光标落入中间行中无任何有效元素的区域（光标前只存在空格和tab键时），该节点顺次添加中间行，光标以后的文本及该行中的其他元素一并添加到新建的中间行中
				 *////////////////////////////////////
				if (editor.HandleKeyPress('\n'))
					return;
				bool isEnd = false;
				bool isAllSpace = false;
				int curLineNr = editor.Caret.Line;
				Paragraph pg = editor.GetParagraph(curLineNr);
				pg.CheckIsIncludeVaildSegment(ref isEnd, ref isAllSpace);
				int insertLocation = 0;

				Block segment = pg.GetLineSegmentBeforeCaret(editor.Caret.Column, ref insertLocation);
				if (segment != null)
				{
					List<string> spitList = new List<string>();
					spitList = pg.SpitTextSegmentByCaret(editor.Caret.Column);
					// 当鼠标光标落在文本元素后时
					if(spitList.Count == 2 && (string.IsNullOrEmpty(spitList[0]) || string.IsNullOrEmpty(spitList[1])))
					{
						int caretCol = 0;
						Paragraph newPG = editor.InsertParagraph(pg, ref caretCol);
						if (newPG != null)
						{
							int index = pg.Blocks.ToList().IndexOf(segment);
							if (string.IsNullOrEmpty(spitList[0]))
								index  -= 1;
							List<Block> lstDelBlock = new List<Block>();
							for (int i = 0; i < pg.Blocks.Length; i++)
							{
								if (i > index)
								{
									lstDelBlock.Add(pg.Blocks[i]);
								}
							}
							pg.Remove(lstDelBlock);
							newPG.AddSegment(lstDelBlock.ToArray());
							editor.Caret.Position = new TextLocation(caretCol, editor.Caret.Line + 1);
							editor.Caret.UpdateCaretPosition();
						}
					}
					else if(spitList.Count == 2 && !string.IsNullOrEmpty(spitList[0]) && !string.IsNullOrEmpty(spitList[1]))
					{
						int caretCol = 0;
						Paragraph newPG = editor.InsertParagraph(pg, ref caretCol);
						if(newPG != null)
						{
							int index = pg.Blocks.ToList().IndexOf(segment);
							List<Block> lstDelBlock = new List<Block>();
							for (int i = 0; i < pg.Blocks.Length; i++)
							{
								if (i > index && pg.Blocks[i] != segment)
								{
									lstDelBlock.Add(pg.Blocks[i]);
								}
							}
							pg.Remove(lstDelBlock);
							pg.Remove(new List<Block>(){segment});
							Block segBefore = new Block(BlockType.Text,spitList[0]);
							pg.AddSegment(segBefore);
							Block segAfter = new Block(BlockType.Text,spitList[1]);
							newPG.AddSegment(segAfter);
							newPG.AddSegment(lstDelBlock.ToArray());
							editor.Caret.Position = new TextLocation(caretCol, editor.Caret.Line + 1);
							editor.Caret.UpdateCaretPosition();
						}
					}
				}
				else if (pg.Blocks.Length == 0 || isAllSpace)
				{
					int caretCol = 0;
					Paragraph newPG = editor.InsertParagraph(pg, ref caretCol);
					editor.Caret.Position = new TextLocation(caretCol, editor.Caret.Line + 1);
					editor.Caret.UpdateCaretPosition();
				}

			}
			finally
			{
				editor.UndoStack.EndUndoGroup();
				editor.EndUpdate();
			}
		}
	}

	public class MovePageDownAction : AbstractEditAction
	{
		/// <remarks>
		/// Executes this edit action
		/// </remarks>
		/// <param name="editor">The <see cref="ItextArea"/> which is used for callback purposes</param>
		public override void Execute(TextBoxControl editor)
		{
			int curLineNr = editor.Caret.Line;
			int iPageLines = editor.ClientSize.Height/ editor.FontHeight;
			int requestedLineNumber = Math.Min((curLineNr + iPageLines), (editor.LineCount - 1));

			if (curLineNr != requestedLineNumber)
			{
				int lineWidth = editor.GetLineWidth(requestedLineNumber);
				if (editor.CalColumnLocation(editor.Caret.Position.Y, editor.Caret.Position.X) > lineWidth)
				{
					editor.Caret.Position = new TextLocation(editor.CalColumnNumber(requestedLineNumber, lineWidth), requestedLineNumber);
					editor.Caret.UpdateCaretPosition();
				}
				else
				{
					int oldLocation = editor.CalColumnLocation(editor.Caret.Position.Y, editor.Caret.Position.X);
					int location = oldLocation > 0 ? oldLocation : 0;
					editor.Caret.Position = new TextLocation(editor.CalColumnNumber(requestedLineNumber, location), requestedLineNumber);
					editor.Caret.UpdateCaretPosition();
				}
				if (editor.VerticalScroll.Visible)
				{
					editor.VerticalScroll.Value = Math.Min(editor.VerticalScroll.Value + iPageLines * editor.FontHeight, editor.VerticalScroll.Maximum);
					editor.PerformLayout();
				}
			}
		}
	}

	public class MovePageUpAction : AbstractEditAction
	{
		/// <remarks>
		/// Executes this edit action
		/// </remarks>
		/// <param name="editor">The <see cref="Ieditor"/> which is used for callback purposes</param>
		public override void Execute(TextBoxControl editor)
		{
			int curLineNr = editor.Caret.Line;
			int iPageLines = editor.ClientSize.Height / editor.FontHeight;
			int requestedLineNumber = Math.Max((curLineNr - iPageLines), 1);

			if (curLineNr != requestedLineNumber)
			{
				int lineWidth = editor.GetLineWidth(requestedLineNumber);
				if (editor.CalColumnLocation(editor.Caret.Position.Y, editor.Caret.Position.X) > lineWidth)
				{
					if (editor.Caret.Position.Y > 1)
					{
						editor.Caret.Position = new TextLocation(editor.CalColumnNumber(requestedLineNumber, lineWidth), requestedLineNumber);
						editor.Caret.UpdateCaretPosition();
					}
				}
				else
				{
					int oldLocation = editor.CalColumnLocation(editor.Caret.Position.Y, editor.Caret.Position.X);
					int location = oldLocation > 0 ? oldLocation : 0;
					editor.Caret.Position = new TextLocation(editor.CalColumnNumber(requestedLineNumber, location), requestedLineNumber);
					editor.Caret.UpdateCaretPosition();
				}
				if(editor.VerticalScroll.Visible)
				{
					editor.VerticalScroll.Value = Math.Max(0, editor.VerticalScroll.Value - iPageLines * editor.FontHeight);
					editor.PerformLayout();
				}
			}
		}
	}
}
