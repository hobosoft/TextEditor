// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 3272 $</version>
// </file>

using System;
using TextEditor.Document;

namespace TextEditor.Actions 
{
	public class ToggleBookmark : AbstractEditAction
	{
		public override void Execute(TextBoxControl editor)
		{
			editor.BookmarkManager.ToggleMarkAt(editor.Caret.Position);
			//editor.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine, textArea.Caret.Line));
			editor.CommitUpdate();
			
		}
	}
	
	public class GotoPrevBookmark : AbstractEditAction
	{
		Predicate<Bookmark> predicate = null;
		
		public GotoPrevBookmark(Predicate<Bookmark> predicate)
		{
			this.predicate = predicate;
		}

		public override void Execute(TextBoxControl editor)
		{
			Bookmark mark = editor.BookmarkManager.GetPrevMark(editor.Caret.Line, predicate);
			if (mark != null) {
				editor.Caret.Position = mark.Location;
				editor.SelectionManager.ClearSelection();
				//editor.SetDesiredColumn();
			}
		}
	}
	
	public class GotoNextBookmark : AbstractEditAction
	{
		Predicate<Bookmark> predicate = null;
		
		public GotoNextBookmark(Predicate<Bookmark> predicate)
		{
			this.predicate = predicate;
		}

		public override void Execute(TextBoxControl editor)
		{
			Bookmark mark = editor.BookmarkManager.GetNextMark(editor.Caret.Line, predicate);
			if (mark != null) {
				editor.Caret.Position = mark.Location;
				editor.SelectionManager.ClearSelection();
				//editor.SetDesiredColumn();
			}
		}
	}
	
	public class ClearAllBookmarks : AbstractEditAction
	{
		Predicate<Bookmark> predicate = null;
		
		public ClearAllBookmarks(Predicate<Bookmark> predicate)
		{
			this.predicate = predicate;
		}

		public override void Execute(TextBoxControl editor)
		{
			editor.BookmarkManager.RemoveMarks(predicate);
			//editor.Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.WholeTextArea));
			editor.CommitUpdate();
		}
	}
}
