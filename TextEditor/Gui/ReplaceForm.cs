using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace TextEditor
{
	public partial class ReplaceForm : Form
	{
		TextBoxControl _editor;
		//bool firstSearch = true;
		//TextLocation startPlace;

		public ReplaceForm(TextBoxControl editor)
		{
			InitializeComponent();
			this._editor = editor;
		}

		private void btClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btFindNext_Click(object sender, EventArgs e)
		{
			try
			{
				if (!Find(tbFind.Text))
					MessageBox.Show("Not found");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		//public List<Range> FindAll(string pattern)
		//{
		//    var opt = cbMatchCase.Checked ? RegexOptions.None : RegexOptions.IgnoreCase;
		//    if (!cbRegex.Checked)
		//        pattern = Regex.Escape(pattern);
		//    if (cbWholeWord.Checked)
		//        pattern = "\\b" + pattern + "\\b";
		//    //
		//    var range = _editor.Selection.IsEmpty ? _editor.Range.Clone() : _editor.Selection.Clone();
		//    //
		//    var list = new List<Range>();
		//    foreach (var r in range.GetRangesByLines(pattern, opt))
		//        list.Add(r);

		//    return list;
		//}

		public bool Find(string pattern)
		{
			//RegexOptions opt = cbMatchCase.Checked ? RegexOptions.None : RegexOptions.IgnoreCase;
			//if (!cbRegex.Checked)
			//    pattern = Regex.Escape(pattern);
			//if (cbWholeWord.Checked)
			//    pattern = "\\b" + pattern + "\\b";
			////
			//Range range = _editor.Selection.Clone();
			//range.Normalize();
			////
			//if (firstSearch)
			//{
			//    startPlace = range.Start;
			//    firstSearch = false;
			//}
			////
			//range.Start = range.End;
			//if (range.Start >= startPlace)
			//    range.End = new TextLocation(_editor.GetLineLength(_editor.LinesCount - 1), _editor.LinesCount - 1);
			//else
			//    range.End = startPlace;
			////
			//foreach (var r in range.GetRangesByLines(pattern, opt))
			//{
			//    _editor.Selection.Start = r.Start;
			//    _editor.Selection.End = r.End;
			//    _editor.DoSelectionVisible();
			//    _editor.Invalidate();
			//    return true;
			//}
			//if (range.Start >= startPlace && startPlace > TextLocation.Empty)
			//{
			//    _editor.Selection.Start = new TextLocation(0, 0);
			//    return Find(pattern);
			//}
			return false;
		}

		private void tbFind_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
				btFindNext_Click(sender, null);
			if (e.KeyChar == '\x1b')
				Hide();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) // David
		{
			if (keyData == Keys.Escape)
			{
				this.Close();
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void ReplaceForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				Hide();
			}
			this._editor.Focus();
		}

		private void btReplace_Click(object sender, EventArgs e)
		{
			try
			{
				//if (_editor.SelectionManager.HasSomethingSelected && !_editor.SelectionManager.SelectionIsReadonly)
				//    _editor.InsertText(tbReplace.Text);
				btFindNext_Click(sender, null);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btReplaceAll_Click(object sender, EventArgs e)
		{
			try
			{
				//_editor.BeginUpdate();

				////search
				//var ranges = FindAll(tbFind.Text);
				////check readonly
				//var ro = false;
				//foreach (var r in ranges)
				//{
				//    if (r.ReadOnly)
				//    {
				//        ro = true;
				//        break;
				//    }
				//}
				////replace
				//if (!ro && ranges.Count > 0)
				//{
				//    _editor.TextSource.Manager.ExecuteCommand(new ReplaceTextCommand(_editor.TextSource, ranges, tbReplace.Text));
				//    _editor.Selection.Start = new TextLocation(0, 0);
				//}
				////
				//_editor.Invalidate();
				//MessageBox.Show(ranges.Count + " occurrence(s) replaced");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			_editor.EndUpdate();
		}

		protected override void OnActivated(EventArgs e)
		{
			tbFind.Focus();
			ResetSerach();
		}

		void ResetSerach()
		{
			//firstSearch = true;
		}

		private void cbMatchCase_CheckedChanged(object sender, EventArgs e)
		{
			ResetSerach();
		}
	}
}
