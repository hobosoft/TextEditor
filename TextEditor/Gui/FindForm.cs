﻿using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace TextEditor
{
    public partial class FindForm : Form
    {
        //bool firstSearch = true;
		//TextLocation startPlace;
        TextBoxControl _editor;

        public FindForm(TextBoxControl editor)
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
            FindNext(tbFind.Text);
        }

        public virtual void FindNext(string pattern)
        {
            try
            {
				//RegexOptions opt = cbMatchCase.Checked ? RegexOptions.None : RegexOptions.IgnoreCase;
				//if (!cbRegex.Checked)
				//    pattern = Regex.Escape(pattern);
				//if (cbWholeWord.Checked)
				//    pattern = "\\b" + pattern + "\\b";
				////
				//ISelection range = _editor.SelectionManager. Clone();
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
				//    _editor.Selection = r;
				//    _editor.DoSelectionVisible();
				//    _editor.Invalidate();
				//    return;
				//}
				////
				//if (range.Start >= startPlace && startPlace > Place.Empty)
				//{
				//    _editor.Selection.Start = new TextLocation(0, 0);
				//    FindNext(pattern);
				//    return;
				//}
                MessageBox.Show("Not found");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                btFindNext.PerformClick();
                e.Handled = true;
                return;
            }
            if (e.KeyChar == '\x1b')
            {
                Hide();
                e.Handled = true;
                return;
            }
        }

        private void FindForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            this._editor.Focus();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
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