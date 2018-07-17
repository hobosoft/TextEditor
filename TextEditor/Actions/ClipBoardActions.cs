// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 1965 $</version>
// </file>

using System;

namespace VCI.XmlEditor.Actions 
{
	public class Cut : AbstractEditAction
	{
		public override void Execute(XmlEditorControl editor)
		{
			if (editor.Document.ReadOnly)
			{
				return;
			}
			editor.ClipboardHandler.Cut(null, null);
		}
	}
	
	public class Copy : AbstractEditAction
	{
		public override void Execute(XmlEditorControl editor)
		{
			editor.AutoClearSelection = false;
			editor.ClipboardHandler.Copy(null, null);
		}
	}

	public class Paste : AbstractEditAction
	{
		public override void Execute(XmlEditorControl editor)
		{
			if (editor.Document.ReadOnly)
			{
				return;
			}
			editor.ClipboardHandler.Paste(null, null);
		}
	}
}
