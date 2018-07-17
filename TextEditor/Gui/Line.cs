using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TextEditor.Document;
using System.Text;

namespace TextEditor
{
	public partial class Line
	{
		/// <summary>
		/// 所属节点
		/// </summary>
		//private VXmlNode _node;
		/// <summary>
		/// 行包含的段
		/// </summary>
		private List<Block> _lstSegment;

		/// <summary>
		/// 行号
		/// </summary>
		private int _nLineNumber = -1;

		/// <summary>
		/// 所属XML节点
		/// </summary>
		//public VXmlNode Node
		//{
		//	get { return _node; }
		//	set { _node = value; }
		//}

		/// <summary>
		/// 行文本
		/// </summary>
		public string Text
		{
			get
			{
				StringBuilder sbText = new StringBuilder();
				foreach (Block seg in _lstSegment)
				{
					sbText.Append(seg.Text);
				}

				return sbText.ToString();
			}
			//set;
		}

		/// <summary>
		/// 行包含的段
		/// </summary>
		public Block[] Segments
		{
			get { return _lstSegment.ToArray(); }
		}

		/// <summary>
		/// 行号
		/// </summary>
		public int LineNumber 
		{
			get 
			{
				return _nLineNumber;
			}
			//set 
			//{
			//    mLineNumber = value;
			//}
		}

		/// <summary>
		/// 行字符长度
		/// </summary>
		public int Length
		{
			get
			{
				int iLength = 0;
				foreach (Block seg in Segments)
				{
					if(!string.IsNullOrEmpty(seg.Text))
					{
						iLength += seg.Length;
					}
				}

				return iLength;
			}
		}

		/// <summary>
		/// 行高度
		/// </summary>
		public int Height
		{
			get;
			protected set;
		}

		/// <summary>
		/// 行最大宽度
		/// </summary>
		public int Width
		{
			get;
			protected set;
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="node"></param>
		public Line()
		{
			_lstSegment = new List<Block>();
			_lstSegment.Add(new Block(""));
        }

		/// <summary>
		/// 添加段
		/// </summary>
		/// <param name="seg"></param>
		public void AddSegment(Block seg)
		{
			if (seg == null)
				return;

			seg.Line = this;
			_lstSegment.Add(seg);
		}

		/// <summary>
		/// 添加段
		/// </summary>
		public void AddSegment(List<Block> segList) 
		{
			if (segList == null || segList.Count == 0)
				return;
			foreach(Block seg in segList)
			{
				seg.Line = this;
				_lstSegment.Add(seg);
			}
		}

		/// <summary>
		/// 添加段
		/// </summary>
		public void AddSegment(Block seg, int index) 
		{
			if (seg == null)
				return;

			seg.Line = this;
			_lstSegment.Insert(index,seg);
		}

		/// <summary>
		/// 计算显示时行占用的空间
		/// </summary>
		/// <param name="g"></param>
		/// <param name="f"></param>
		/// <param name="editor"></param>
		/// <returns></returns>
		public Size CalcSize(Graphics g, Font f, TextBoxControl editor)
		{
			Size size = new Size();
			foreach (Block seg in Segments)
			{
				switch (seg.SegType)
				{
					case BlockType.Text:
						{
							size.Width += editor.DrawHelper.MeasureStringWidth(g, seg.Text, f);
						}
						break;
					case BlockType.Tab:
						size.Width += editor.SpaceWidth * editor.TabIndent;
						break;
					case BlockType.Space:
					case BlockType.AttrSplit:
						size.Width += editor.SpaceWidth;
						break;
					default:
						continue;
				}
			}

			//if (size.Width > 0)
			size.Height = editor.FontHeight;

			Width = size.Width;
			Height = size.Height;

			return size;
		}



		/// <summary>
		/// 绘制行
		/// </summary>
		public void Draw(TextBoxControl editor, Graphics g, Font f, Point ptPos)
		{
			Point ptTemp = ptPos;
			RectangleF rcClip = g.ClipBounds;

			// 绘制行号
			//_nLineNumber = editor.LineCount;
			if (editor.ShowLineNumber && (ptTemp.Y + editor.FontHeight > rcClip.Top && ptTemp.Y < rcClip.Bottom && ptTemp.X < rcClip.Right))
			{
				g.ResetClip();
				int numWidth = editor.DrawHelper.MeasureStringWidth(g, _nLineNumber.ToString(), f);

				Point ptNum = new Point(editor.Padding.Left + editor.GetBarWidth() - editor.Margin.Right - numWidth, ptPos.Y);

				editor.DrawHelper.DrawString(g, _nLineNumber.ToString(), f, editor.LineNumberColor, ptNum);
				g.SetClip(rcClip);
			}

			// 绘制元素
			foreach (Block seg in Segments)
			{
				if (ptTemp.Y + editor.FontHeight > rcClip.Top && ptTemp.Y < rcClip.Bottom && ptTemp.X < rcClip.Right)
				{
					seg.Draw(editor, g, f, ref ptTemp, _nLineNumber);
				}
			}
		}

		public TextAnchor CreateAnchor(int column)
		{
			if (column < 0 || column > Length)
				throw new ArgumentOutOfRangeException("column");
			TextAnchor anchor = new TextAnchor(this, column);
			//AddAnchor(anchor);
			return anchor;
		}


		public void SetLineNumber(int iLineNumber)
		{
			_nLineNumber = iLineNumber;
		}

	}
}
