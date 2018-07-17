using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace TextEditor.Gui
{
	internal class XmlColorStrategy : IColorStrategy
	{
		#region Filder
		Color m_coSign = Color.Blue;
		Color m_coName = Color.Blue;
		Color m_coAttrName = Color.Red;
		Color m_coEqualSign = Color.Blue;
		Color m_coQuotationSign = Color.Black;
		Color m_coAttrValue = Color.Blue;
		Color m_coAttrRefValue = Color.Blue;
		Color m_coValue = Color.Black;
		Color m_coComment = Color.Green;
		Color m_coCommentSign = Color.Green;

		Color m_coCDATA = Color.Brown;
		Color m_coCDATASign = Color.Brown;

		Color m_coLineNumber = Color.FromArgb(43, 145, 175);
		Color m_coFoldMarker = Color.Gray;
		Color m_coFoldLine = Color.Gray;
		Color m_coBookMarker = Color.Brown;
		Color m_highLight = SystemColors.Highlight;
		Color m_highLightText = SystemColors.HighlightText;
		#endregion

		[Browsable(false)]
		public Color Sign
		{
			get { return m_coSign; }
			set { m_coSign = value; }
		}
		[Browsable(false)]
		public Color Name
		{
			get { return m_coName; }
			set { m_coName = value; }
		}
		[Browsable(false)]
		public Color AttrName
		{
			get { return m_coAttrName; }
			set { m_coAttrName = value; }
		}
		[Browsable(false)]
		public Color AttrValue
		{
			get { return m_coAttrValue; }
			set { m_coAttrValue = value; }
		}

		public Color EqualSign
		{
			get { return m_coEqualSign; }
			set { m_coEqualSign = value; }
		}

		public Color QuotationSign
		{
			get { return m_coQuotationSign; }
			set { m_coQuotationSign = value; }
		}

		[Browsable(false)]
		public Color AttrRefValue
		{
			get { return m_coAttrRefValue; }
			set { m_coAttrRefValue = value; }
		}

		[Browsable(false)]
		public Color Text
		{
			get { return m_coValue; }
			set { m_coValue = value; }
		}

		[Browsable(false)]
		public Color Comment
		{
			get { return m_coComment; }
			set { m_coComment = value; }
		}
		[Browsable(false)]
		public Color CommentSign
		{
			get { return m_coCommentSign; }
			set { m_coCommentSign = value; }
		}
		[Browsable(false)]
		public Color CData
		{
			get { return m_coCDATA; }
			set { m_coCDATA = value; }
		}

		[Browsable(false)]
		public Color LineNumber
		{
			get { return m_coLineNumber; }
			set { m_coLineNumber = value; }
		}
		[Browsable(false)]
		public Color FoldMarker
		{
			get { return m_coFoldMarker; }
			set { m_coFoldMarker = value; }
		}

		[Browsable(false)]
		public Color FoldLine
		{
			get { return m_coFoldLine; }
			set { m_coFoldLine = value; }
		}

		[Browsable(false)]
		public Color BookMarker
		{
			get { return m_coBookMarker; }
			set { m_coBookMarker = value; }
		}

		[Browsable(false)]
		public Color HighLight
		{
			get { return m_highLight; }
			set { m_highLight = value; }
		}

		[Browsable(false)]
		public Color HighLightText
		{
			get { return m_highLightText; }
			set { m_highLightText = value; }
		}

	}
}
