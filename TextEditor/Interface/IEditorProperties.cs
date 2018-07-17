// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="none" email=""/>
//     <version>$Revision: 3440 $</version>
// </file>

using System;
using System.Drawing;
using System.Text;

namespace TextEditor.Interface
{
	/// <summary>
	/// Describes the caret marker
	/// </summary>
	public enum LineViewerStyle
	{
		/// <summary>
		/// No line viewer will be displayed
		/// </summary>
		None,

		/// <summary>
		/// The row in which the caret is will be marked
		/// </summary>
		FullRow
	}

	/// <summary>
	/// Describes the indent style
	/// </summary>
	public enum IndentStyle
	{
		/// <summary>
		/// No indentation occurs
		/// </summary>
		None,

		/// <summary>
		/// The indentation from the line above will be
		/// taken to indent the curent line
		/// </summary>
		Auto,

		/// <summary>
		/// Inteligent, context sensitive indentation will occur
		/// </summary>
		Smart
	}

	/// <summary>
	/// Describes the bracket highlighting style
	/// </summary>
	public enum BracketHighlightingStyle
	{

		/// <summary>
		/// Brackets won't be highlighted
		/// </summary>
		None,

		/// <summary>
		/// Brackets will be highlighted if the caret is on the bracket
		/// </summary>
		OnBracket,

		/// <summary>
		/// Brackets will be highlighted if the caret is after the bracket
		/// </summary>
		AfterBracket
	}

	/// <summary>
	/// Describes the selection mode of the text area
	/// </summary>
	public enum DocumentSelectionMode
	{
		/// <summary>
		/// The 'normal' selection mode.
		/// </summary>
		Normal,

		/// <summary>
		/// Selections will be added to the current selection or new
		/// ones will be created (multi-select mode)
		/// </summary>
		Additive
	}

	public enum BracketMatchingStyle
	{
		Before,
		After
	}

	public interface IEditorProperties
	{
		bool CaretLine
		{
			get;
			set;
		}

		bool AutoInsertCurlyBracket
		{ // is wrapped in text editor control
			get;
			set;
		}

		bool HideMouseCursor
		{ // is wrapped in text editor control
			get;
			set;
		}

		bool AllowCaretBeyondEOL
		{
			get;
			set;
		}

		bool ShowMatchingBracket
		{ // is wrapped in text editor control
			get;
			set;
		}

		bool CutCopyWholeLine
		{
			get;
			set;
		}

		System.Drawing.Text.TextRenderingHint TextRenderingHint
		{ // is wrapped in text editor control
			get;
			set;
		}

		bool MouseWheelScrollDown
		{
			get;
			set;
		}

		bool MouseWheelTextZoom
		{
			get;
			set;
		}

		string LineTerminator
		{
			get;
			set;
		}

		LineViewerStyle LineViewerStyle
		{ // is wrapped in text editor control
			get;
			set;
		}

		bool ShowInvalidLines
		{ // is wrapped in text editor control
			get;
			set;
		}

		int VerticalRulerRow
		{ // is wrapped in text editor control
			get;
			set;
		}

		bool ShowSpaces
		{ // is wrapped in text editor control
			get;
			set;
		}

		bool ShowTabs
		{ // is wrapped in text editor control
			get;
			set;
		}

		bool ShowEOLMarker
		{ // is wrapped in text editor control
			get;
			set;
		}

		bool ConvertTabsToSpaces
		{ // is wrapped in text editor control
			get;
			set;
		}

		bool ShowHorizontalRuler
		{ // is wrapped in text editor control
			get;
			set;
		}

		bool ShowVerticalRuler
		{ // is wrapped in text editor control
			get;
			set;
		}

		Encoding Encoding
		{
			get;
			set;
		}

		bool EnableFolding
		{ // is wrapped in text editor control
			get;
			set;
		}

		bool ShowLineNumbers
		{ // is wrapped in text editor control
			get;
			set;
		}

		/// <summary>
		/// The width of a tab.
		/// </summary>
		int TabIndent
		{ // is wrapped in text editor control
			get;
			set;
		}

		/// <summary>
		/// The amount of spaces a tab is converted to if ConvertTabsToSpaces is true.
		/// </summary>
		int IndentationSize
		{
			get;
			set;
		}

		IndentStyle IndentStyle
		{ // is wrapped in text editor control
			get;
			set;
		}

		DocumentSelectionMode DocumentSelectionMode
		{
			get;
			set;
		}

		Font Font
		{ // is wrapped in text editor control
			get;
			set;
		}

		FontContainer FontContainer
		{
			get;
		}

		BracketMatchingStyle BracketMatchingStyle
		{ // is wrapped in text editor control
			get;
			set;
		}

		bool SupportReadOnlySegments
		{
			get;
			set;
		}
	}
}
