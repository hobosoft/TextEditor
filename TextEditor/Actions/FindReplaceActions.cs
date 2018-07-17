using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TextEditor.Actions
{
	public class FindAction : AbstractEditAction
	{
		FindForm _dlgFind;

		public override void Execute(TextBoxControl editor)
		{
			if (_dlgFind == null)
				_dlgFind = new FindForm(editor);

			if (!_dlgFind.Visible)
				_dlgFind.Show();

			//TextLocation homeLocation = editor.GetLineHomeInfo(editor.Caret.Line);
			//if (homeLocation != TextLocation.Empty)
			//{
			//    editor.Caret.Position = homeLocation;
			//    editor.Caret.UpdateCaretPosition();
			//    if (editor.HorizontalScroll.Value != 0 && editor.HorizontalScroll.Visible)
			//    {
			//        editor.HorizontalScroll.Value = 0;
			//        editor.PerformLayout();
			//    }
			//}
		}
	}

	public class ReplaceAction : AbstractEditAction
	{
		ReplaceForm _dlgReplace;

		public override void Execute(TextBoxControl editor)
		{
			if (_dlgReplace == null)
				_dlgReplace = new ReplaceForm(editor);

			if (!_dlgReplace.Visible)
				_dlgReplace.Show();

			//TextLocation homeLocation = editor.GetLineHomeInfo(editor.Caret.Line);
			//if (homeLocation != TextLocation.Empty)
			//{
			//    editor.Caret.Position = homeLocation;
			//    editor.Caret.UpdateCaretPosition();
			//    if (editor.HorizontalScroll.Value != 0 && editor.HorizontalScroll.Visible)
			//    {
			//        editor.HorizontalScroll.Value = 0;
			//        editor.PerformLayout();
			//    }
			//}
		}
	}

	public class GotoAction : AbstractEditAction
	{
		public override void Execute(TextBoxControl editor)
		{
			GoToForm dlg = new GoToForm();
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				
			}
			//TextLocation homeLocation = editor.GetLineHomeInfo(editor.Caret.Line);
			//if (homeLocation != TextLocation.Empty)
			//{
			//    editor.Caret.Position = homeLocation;
			//    editor.Caret.UpdateCaretPosition();
			//    if (editor.HorizontalScroll.Value != 0 && editor.HorizontalScroll.Visible)
			//    {
			//        editor.HorizontalScroll.Value = 0;
			//        editor.PerformLayout();
			//    }
			//}
		}
	}
}
