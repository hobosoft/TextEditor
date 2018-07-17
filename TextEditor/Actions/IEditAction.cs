using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace TextEditor.Actions
{
	public interface IEditAction
	{
		/// <value>
		/// An array of keys on which this edit action occurs.
		/// </value>
		Keys[] Keys
		{
			get;
			set;
		}

		/// <remarks>
		/// When the key which is defined per XML is pressed, this method will be launched.
		/// </remarks>
		void Execute(TextBoxControl editor);
	}

	/// <summary>
	/// To define a new key for the XmlEditorControl, you must write a class which
	/// implements this interface.
	/// </summary>
	public abstract class AbstractEditAction : IEditAction
	{
		Keys[] keys = null;

		/// <value>
		/// An array of keys on which this edit action occurs.
		/// </value>
		public Keys[] Keys
		{
			get
			{
				return keys;
			}
			set
			{
				keys = value;
			}
		}

		/// <remarks>
		/// When the key which is defined per XML is pressed, this method will be launched.
		/// </remarks>
		public abstract void Execute(TextBoxControl editor);
	}		
}
