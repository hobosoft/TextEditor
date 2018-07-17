using System;
using System.Collections.Generic;
using System.Drawing;

namespace TextEditor
{
	public interface IColorStrategy
	{
		Color Sign
		{
			get;
			set;
		}

		Color Name
		{
			get;
			set;
		}

		Color AttrName
		{
			get;
			set;
		}

		Color EqualSign
		{
			get;
			set;
		}

		Color QuotationSign
		{
			get;
			set;
		}

		Color AttrValue
		{
			get;
			set;
		}

		Color AttrRefValue
		{
			get;
			set;
		}


		Color Text
		{
			get;
			set;
		}


		Color Comment
		{
			get;
			set;
		}

		Color CommentSign
		{
			get;
			set;
		}

		Color CData
		{
			get;
			set;
		}

		Color LineNumber { get; set; }
		Color FoldMarker { get; set; }
		Color FoldLine { get; set; }
		Color BookMarker { get; set; }
		Color HighLightText { get; set; }
		Color HighLight { get; set; }
	}
}
