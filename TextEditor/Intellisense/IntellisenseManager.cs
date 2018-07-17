using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
//using VCI.IETM.XMLEditControl;
using TextEditor.Document;

namespace TextEditor.Intellisense
{
	internal class IntellisenseManager
	{
		#region Members
		private TextBoxControl m_editor = null;
		private IntellisenseListBox m_IntellisenseBox = null;
		//private bool m_LastCharWasAScopeOperator = false;
		#endregion

		#region Properties
		#endregion

		#region Constructors
		public IntellisenseManager(TextBoxControl editor)
		{
			m_editor = editor;
            m_editor.InsertCharEvent += new InsertCharHandle(m_editor_InsertCharEvent);
			m_IntellisenseBox = new IntellisenseListBox();

			#region Setup intellisense box
			//Setup intellisense box
			editor.Controls.Add(m_IntellisenseBox);
			m_IntellisenseBox.Size = new Size(250, 150);
			m_IntellisenseBox.Visible = false;
			m_IntellisenseBox.KeyDown += new System.Windows.Forms.KeyEventHandler(IntellisenseBox_KeyDown);// += new KeyEventHandler(IntellisenseBox_KeyDown);
			m_IntellisenseBox.DoubleClick += new EventHandler(IntellisenseBox_DoubleClick);
			#endregion

		}


		#endregion

		#region Methods

        private void m_editor_InsertCharEvent(InsertCharEventArgs e)
        {
            UpdateIntellisense(false, "", "", e);
        }

		/// <summary>
		/// Shows the intellisense box.
		/// </summary>
		public void ShowIntellisenseBox(string keyWord)
		{
			//if (m_editor.EditController == null)
			{
				return;
			}

			//If our box has no elements, do not show it...
            //if (!UpdateIntellisense(false, keyWord, ""))
            //{
            //    return;
            //}

			ShowIntellisenseBoxWithoutUpdate();
		}
		internal void ShowIntellisenseBoxWithoutUpdate()
		{
            //if (m_editor.EditController == null)
			{
				return;
			}

			//our box has some elements, choose the first
			try
			{
				m_IntellisenseBox.SelectedIndex = 0;
			}
			catch { }


			//Get top-left coordinate for our intellisenseBox
			Point topLeft = m_editor.GetDrawingXPos(m_editor.Caret.Line, m_editor.Caret.Column);// .GetPositionFromCharIndex(m_editor.SelectionStart);
			topLeft.Offset(0, 18);

			#region Place the intellisense box, to fit the space...
			if (m_editor.Size.Height < (topLeft.Y + m_IntellisenseBox.Height))
			{
				topLeft.Offset(0, -18 - 18 - m_IntellisenseBox.Height);
			}

			if (m_editor.Size.Width < (topLeft.X + m_IntellisenseBox.Width))
			{
				topLeft.Offset(35 + 15 - m_IntellisenseBox.Width, 0);
			}

			if (topLeft.X < 0)
			{
				topLeft.X = 0;
			}

			if (topLeft.Y < 0)
			{
				topLeft.Y = 0;
			}
			#endregion

			m_IntellisenseBox.Location = topLeft;
			m_IntellisenseBox.Visible = true;
			m_editor.Focus();
		}
		/// <summary>
		/// Hides the intellisense box.
		/// </summary>
		public void HideIntellisenseBox()
		{
			m_IntellisenseBox.Items.Clear();
			m_IntellisenseBox.Visible = false;
		}
		/// <summary>
		/// Navigates up in the intellisense box.
		/// </summary>
		public void NavigateUp(int elements)
		{
			#region Some checkings for the intellisense box
			//Do nothing if the intellisense is not visible...
			if (!m_IntellisenseBox.Visible)
			{
				return;
			}
			//If our box has no elements, do not show it...
			if (m_IntellisenseBox.Items.Count <= 0)
			{
				return;
			}
			#endregion

			if (m_IntellisenseBox.SelectedIndex > elements - 1)
			{
				m_IntellisenseBox.SelectedIndex -= elements;
			}
			else
			{
				m_IntellisenseBox.SelectedIndex = 0;
			}
		}
		/// <summary>
		/// Navigates down in the intellisense box.
		/// </summary>
		public void NavigateDown(int elements)
		{
			#region Some checkings for the intellisense box
			//Do nothing if the intellisense is not visible...
			if (!m_IntellisenseBox.Visible)
			{
				return;
			}
			//If our box has no elements, do not show it...
			if (m_IntellisenseBox.Items.Count <= 0)
			{
				return;
			}
			#endregion

			if (m_IntellisenseBox.SelectedIndex < m_IntellisenseBox.Items.Count - elements - 1)
			{
				m_IntellisenseBox.SelectedIndex += elements;
			}
			else
			{
				m_IntellisenseBox.SelectedIndex = m_IntellisenseBox.Items.Count - 1;
			}
		}
		/// <summary>
		/// Navigates to the first element in the intellisense box.
		/// </summary>
		public void NavigateHome()
		{
			#region Some checkings for the intellisense box
			//Do nothing if the intellisense is not visible...
			if (!m_IntellisenseBox.Visible)
			{
				return;
			}
			//If our box has no elements, do not show it...
			if (m_IntellisenseBox.Items.Count <= 0)
			{
				return;
			}
			#endregion

			m_IntellisenseBox.SelectedIndex = 0;
		}
		/// <summary>
		/// Navigates to the last element in the intellisense box.
		/// </summary>
		public void NavigateEnd()
		{
			#region Some checkings for the intellisense box
			//Do nothing if the intellisense is not visible...
			if (!m_IntellisenseBox.Visible)
			{
				return;
			}
			//If our box has no elements, do not show it...
			if (m_IntellisenseBox.Items.Count <= 0)
			{
				return;
			}
			#endregion

			m_IntellisenseBox.SelectedIndex = m_IntellisenseBox.Items.Count - 1;
		}
		/// <summary>
		/// Calls, when a backspace typed.
		/// </summary>
		public void TypeBackspace()
		{
			#region Some checkings for the intellisense box
			//Do nothing if the intellisense is not visible...
			if (!m_IntellisenseBox.Visible)
			{
				return;
			}
			#endregion

			//m_LastCharWasAScopeOperator = false;
            //UpdateIntellisense(false, "", "\b");
		}
		/// <summary>
		/// Calls, when an alphanumerical character typed.
		/// </summary>
		/// <param name="c"></param>
		public void TypeAlphaNumerical(char c)
		{
			//CheckScopeOperator(c);
            //UpdateIntellisense(false, "", c.ToString());
		}
		/// <summary>
		/// Calls, when a non-alphanumerical character typed.
		/// </summary>
		/// <param name="c"></param>
		public void TypeNonAlphaNumerical(char c)
		{
			//CheckScopeOperator(c);

			//Search the last letter(s) for separator, and update if found...
			//string lastWord = RichTextboxHelper.GetLastWord(m_editor);
			////last word doesn't contains the processed char yet...
			//lastWord += c;

			//foreach (string separator in m_editor.CodeWords_ScopeOperators)
			//{
			//    int index = lastWord.LastIndexOf(separator);
			//    if (index > -1)
			//    {
			//        if (index == (lastWord.Length - separator.Length))
			//        {
			//            //found...
			//            UpdateIntellisense(true, lastWord.Substring(0, index), c.ToString());
			//            return;
			//        }
			//    }
			//}
		}
        //private VXmlNode _node = null;
        private InsertOperate _operate = InsertOperate.None;
        private InsertCharEventArgs _args = null;
		/// <summary>
		/// Updates the intellisense box's elements to show the right object list.
		/// </summary>
		/// <param name="forceNextLevel"></param>
		/// <param name="word"></param>
		/// <param name="justRead"></param>
		/// <returns></returns>
        public bool UpdateIntellisense(bool forceNextLevel, string keyWord, string justRead, InsertCharEventArgs args)
        {
            //_node = args.Node;
            _args = args;
            _operate = args.Operate;
            //Clear all elements
            m_IntellisenseBox.Items.Clear();
            bool needShow = false;
            switch (args.Operate)
            {
                case InsertOperate.InsertNodeName:

					//List<string> lstElement = m_editor.EditController.GetElementLists(_node.Parent.Name, _node.GetLastNodeName(), _node.GetSameLevelNodeName());
					//if (lstElement.Count > 0)
					//{
					//    foreach (string element in lstElement)
					//    {
					//        IntellisenseListItem li = new IntellisenseListItem();
					//        li.Text = m_editor.LanguageReader.GetDisplayText(element);
					//        li.Image = Properties.Resources.Element;
					//        li.Tag = element;
					//        m_IntellisenseBox.Items.Add(li);
					//    }
					//    needShow = true;
					//}
                    break;
                case InsertOperate.InsertAttrName:
                   
                    ///已经存在的属性
					//List<string> existAttr = new List<string>();

					//foreach (VXmlAttribute attr in _node.Attributes)
					//{
					//    existAttr.Add(attr.Name);
					//}
					//List<string> lstAttr = m_editor.EditController.GetAttributeList(_node.Name, existAttr);
					//if (lstAttr.Count > 0)
					//{
					//    foreach (string attr in lstAttr)
					//    {
					//        IntellisenseListItem li = new IntellisenseListItem();
					//        li.Text = m_editor.LanguageReader.GetDisplayText(attr);
					//        li.Tag = attr;
					//        li.Image = Properties.Resources.Attribute;

					//        m_IntellisenseBox.Items.Add(li);
					//    }
					//    needShow = true;
					//}

                    break;
                case InsertOperate.InsertAttrValue:
					//string attrName = (args.Segment as AttrValueSegment).Attribute.Name;
					//List<string> lstValue = m_editor.EditController.GetAttributeEnumList(attrName);
					//if (lstValue.Count > 0)
					//{
					//    foreach (string attrValue in lstValue)
					//    {
					//        IntellisenseListItem li = new IntellisenseListItem();
					//        li.Text = m_editor.LanguageReader.GetDisplayText(attrValue);
					//        li.Tag = attrValue;
					//        li.Image = Properties.Resources.Attribute;

					//        m_IntellisenseBox.Items.Add(li);
					//    }
					//    needShow = true;
					//}

                    break;
            }
            if (needShow)
                //Show box
                ShowIntellisenseBoxWithoutUpdate();
            else
                HideIntellisenseBox();
            return true;
        }

		/// <summary>
		/// Confirms the selection from the intellisense, and write the selected text back to the textbox.
		/// </summary>
		public void ConfirmIntellisense()
		{
			string wordSelected;

            try
            {

                wordSelected = ((IntellisenseListItem)m_IntellisenseBox.SelectedItem).Tag.ToString();
                //switch (_operate)
                //{
                //    case InsertOperate.InsertNodeName:
                //        _node.Name = wordSelected;
                //        break;
                //    case InsertOperate.InsertAttrName:
                //        (_args.Segment as AttrValueSegment).Attribute.Name = wordSelected;
                //        break;
                //    case InsertOperate.InsertAttrValue:
                //        (_args.Segment as AttrValueSegment).Attribute.Value = wordSelected;
                //        break;
                //}

            }
            catch
            {
                return;
            }

			Line line = m_editor.GetLine(m_editor.Caret.Line);
            if (line != null)
            {
                switch (wordSelected)
                {
                    case "dmRef":

                        break;
                    case "pmRef":

                        break;
                }

				line.Insert(m_editor.Caret.Column, wordSelected, m_editor.Caret);
			}
			////Get the actual position
			//int currentPosition = m_editor.SelectionStart;
			////Get the start position of the last word
			//int lastWordPosition = GetLastWordStartPosition(m_editor, m_editor.CodeWords_ScopeOperators);

			////Set selection
			//m_editor.SelectionStart = lastWordPosition;
			//m_editor.SelectionLength = currentPosition - lastWordPosition;

			////Change the word
			//m_editor.SelectedText = wordSelected;

			//Hide the intellisense
			HideIntellisenseBox();
		}
		#endregion

		private void IntellisenseBox_KeyDown(object sender, KeyEventArgs e)
		{
			//Let the textbox handle keypresses inside the intellisense box
			OnKeyDown(e);
		}
		private void IntellisenseBox_DoubleClick(object sender, EventArgs e)
		{
			ConfirmIntellisense();
		}

		protected void OnKeyDown(KeyEventArgs e)
		{
			//#region Show Intellisense
			//if (e.KeyData == mp_IntellisenseKey)
			//{
			//    m_IntellisenseManager.ShowIntellisenseBox();
			//    e.Handled = true;
			//    this.Focus();
			//    return;
			//}
			//#endregion

			if (m_IntellisenseBox.Visible)
			{
				#region ESCAPE - Hide Intellisense
				if (e.KeyCode == Keys.Escape)
				{
					HideIntellisenseBox();
					e.Handled = true;
				}
				#endregion

				#region Navigation - Up, Down, PageUp, PageDown, Home, End
				else if (e.KeyCode == Keys.Up)
				{
					NavigateUp(1);
					e.Handled = true;
				}
				else if (e.KeyCode == Keys.Down)
				{
					NavigateDown(1);
					e.Handled = true;
				}
				else if (e.KeyCode == Keys.PageUp)
				{
					NavigateUp(10);
					e.Handled = true;
				}
				else if (e.KeyCode == Keys.PageDown)
				{
					NavigateDown(10);
					e.Handled = true;
				}
				else if (e.KeyCode == Keys.Home)
				{
					NavigateHome();
					e.Handled = true;
				}
				else if (e.KeyCode == Keys.End)
				{
					NavigateEnd();
					e.Handled = true;
				}
				#endregion

				#region Typing - Back
				else if (e.KeyCode == Keys.Back)
				{
					TypeBackspace();
				}
				#endregion

				#region Typing - Brackets
				else if (e.KeyCode == Keys.D9)
				{
					// Trap the open bracket key, displaying a cheap and
					// cheerful tooltip if the word just typed is in our tree
					// (the parameters are stored in the tag property of the node)
				}
				else if (e.KeyCode == Keys.D8)
				{
					// Close bracket key, hide the tooltip textbox
				}
				#endregion

				#region Typing - TAB and Enter
				else if (e.KeyCode == Keys.Tab)
				{
					ConfirmIntellisense();
					e.Handled = true;
				}
				else if (e.KeyCode == Keys.Enter)
				{
					ConfirmIntellisense();
					e.Handled = true;
				}
				#endregion
			}

			m_editor.Focus();
			//m_editor.KeyDown(e);
		}

	}
}
