// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 915 $</version>
// </file>

using System;

namespace TextEditor
{
	/// <summary>
	/// This delegate is used for document events.
	/// </summary>
	public delegate void EditorEventHandler(object sender, EditorEventArgs e);

	/// <summary>
	/// This class contains more information on a document event
	/// </summary>
	public class EditorEventArgs : EventArgs
	{
		TextBoxControl _control;
		int offset;
		int length;
		string text;

		/// <returns>
		/// always a valid Document which is related to the Event.
		/// </returns>
		public TextBoxControl Control
		{
			get
			{
				return _control;
			}
		}

		/// <returns>
		/// -1 if no offset was specified for this event
		/// </returns>
		public int Offset
		{
			get
			{
				return offset;
			}
		}

		/// <returns>
		/// null if no text was specified for this event
		/// </returns>
		public string Text
		{
			get
			{
				return text;
			}
		}

		/// <returns>
		/// -1 if no length was specified for this event
		/// </returns>
		public int Length
		{
			get
			{
				return length;
			}
		}

		/// <summary>
		/// Creates a new instance off <see cref="DocumentEventArgs"/>
		/// </summary>
		public EditorEventArgs(TextBoxControl ctrl)
			: this(ctrl, -1, -1, null)
		{
		}

		/// <summary>
		/// Creates a new instance off <see cref="DocumentEventArgs"/>
		/// </summary>
		public EditorEventArgs(TextBoxControl ctrl, int offset)
			: this(ctrl, offset, -1, null)
		{
		}

		/// <summary>
		/// Creates a new instance off <see cref="DocumentEventArgs"/>
		/// </summary>
		public EditorEventArgs(TextBoxControl ctrl, int offset, int length)
			: this(ctrl, offset, length, null)
		{
		}

		/// <summary>
		/// Creates a new instance off <see cref="DocumentEventArgs"/>
		/// </summary>
		public EditorEventArgs(TextBoxControl ctrl, int offset, int length, string text)
		{
			this._control = ctrl;
			this.offset = offset;
			this.length = length;
			this.text = text;
		}

		public override string ToString()
		{
			return String.Format("[EditorEventArgs: Offset = {0}, Text = {1}, Length = {2}]", Offset, Text, Length);
		}
	}
}
