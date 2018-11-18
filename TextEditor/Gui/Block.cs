using System.Drawing;
using System.Text;
using TextEditor.Document;

namespace TextEditor
{
	public class Block
	{
		private Line _line;
		private Paragraph _paragraph;

		public Line Line
		{
			get { return _line; }
			set { _line = value; }
		}

		public Paragraph Paragraph
		{
			get { return _paragraph; }
			set { _paragraph = value; }
		}


		public virtual string Text
		{
			get;
			set;
		}

		public virtual BlockType BlockType
		{
			get;
			set;
		}

		public virtual Color Color
		{
			get;
			set;
		}

		public virtual int Length
		{
			get { return Text.Length; }
		}

		public Block(BlockType type, string text)
		{
			Text = text;
			BlockType = type;
			Color = SystemColors.WindowText;
		}

		public Block(BlockType type)
			: this(type, "")
		{
		}

		public Block(string text)
			: this(BlockType.Text, text)
		{
		}

		public Block()
			: this("")
		{
		}

		/// <summary>
		/// 绘制
		/// </summary>
		public void Draw(TextBoxControl editor, Graphics g, Font f, ref Point ptTemp, int lineNumber) 
		{
			TextLocation start = editor.SelectionManager.SelectionStart;
			TextLocation end = editor.SelectionManager.SelectionEnd;
			// 若未被选中的状态(起始位置等于结束位置时)，则直接按照原来的逻辑进行计算
			if (start.X == end.X && start.Y == end.Y)
			{
				if (BlockType == BlockType.Space)
				{
					ptTemp.X += editor.SpaceWidth;
				}
				else if (BlockType == BlockType.Tab)
				{
					ptTemp.X += editor.SpaceWidth * Length;
				}
				else
				{
					int iWidth = editor.DrawHelper.MeasureStringWidth(g, Text, f);

					editor.DrawHelper.DrawString(g, Text, f, Color, ptTemp);
					ptTemp.X += iWidth;
				}
				return;
			}


			// 是否被选中
			bool isSelected = false;
			// 绘制的起始位置
			int startPos = -1;
			// 绘制的结束位置
			int endPos = -1;

			// 计算元素中选中部分和未选中部分
			// 若元素的类型为空格或者是Tab键时，计算它们所占的宽度

			int wordWidth = 0;
			if (BlockType == BlockType.Space)
			{
				wordWidth = editor.SpaceWidth;
			}
			else if (BlockType == BlockType.Tab)
			{
				wordWidth = editor.SpaceWidth * Length;
			}
			else
				wordWidth = editor.DrawHelper.MeasureStringWidth(g, Text, f);

			// 计算开始节点和结束节点坐标的位置
			int startColLocation = editor.CalColumnLocation(start.Y, start.X) 
				+ editor.Padding.Left 
				+ editor.Margin.Left
				+ editor.GetBarWidth()
				- editor.HorizontalScroll.Value;
			int endColLocation = editor.CalColumnLocation(end.Y, end.X) 
				+ editor.Padding.Left
				+ editor.Margin.Left 
				+ editor.GetBarWidth()
				- editor.HorizontalScroll.Value;

			if (start.Y == end.Y && start.Y == lineNumber)
			{
				int minX = startColLocation > endColLocation ? endColLocation : startColLocation;
				int maxX = startColLocation > endColLocation ? startColLocation : endColLocation;
				// 元素在选中范围内
				if (minX <= ptTemp.X && ptTemp.X + wordWidth <= maxX)
				{
					isSelected = true;
				}
				// 元素与选中范围左侧有交集
				else if (ptTemp.X < minX && minX < ptTemp.X + wordWidth && ptTemp.X + wordWidth <= maxX)
				{
					isSelected = true;
					startPos = minX;
					endPos = ptTemp.X + wordWidth;
				}
				// 元素与选中范围的右侧有交集
				else if (ptTemp.X >= minX && ptTemp.X < maxX && ptTemp.X + wordWidth > maxX)
				{
					isSelected = true;
					startPos = ptTemp.X;
					endPos = maxX;
				}
				// 选中范围在元素中
				else if (ptTemp.X < minX && ptTemp.X + wordWidth > maxX)
				{
					isSelected = true;
					startPos = minX;
					endPos = maxX;
				}
			}
			else
			{
				TextLocation minY = start.Y < end.Y ? start : end;
				TextLocation maxY = start.Y < end.Y ? end : start;
				int minYColLocation = start.Y < end.Y ? startColLocation : endColLocation;
				int maxYColLocation = start.Y < end.Y ? endColLocation : startColLocation;

				// 所在行在选择行之间时
				if (minY.Y < lineNumber && lineNumber < maxY.Y)
				{
					isSelected = true;
				}
				// 所在行在首行
				else if (minY.Y == lineNumber)
				{
					if (minYColLocation <= ptTemp.X)
					{
						isSelected = true;
					}
					else if (ptTemp.X <= minYColLocation && minYColLocation < ptTemp.X + wordWidth)
					{
						isSelected = true;
						startPos = minYColLocation;
						endPos = ptTemp.X + wordWidth;
					}
				}
				// 所在行在尾行
				else if (maxY.Y == lineNumber)
				{
					if (maxYColLocation >= ptTemp.X + wordWidth)
					{
						isSelected = true;
					}
					else if (ptTemp.X < maxYColLocation && maxYColLocation <= ptTemp.X + wordWidth)
					{
						isSelected = true;
						startPos = ptTemp.X;
						endPos = maxYColLocation;
					}
				}
			}
			Brush selectionBackgroundBrush = new SolidBrush(SystemColors.Highlight);
			if (BlockType == BlockType.Space)
			{
				if (isSelected)
				{
					g.FillRectangle(selectionBackgroundBrush, new RectangleF(ptTemp.X, ptTemp.Y, editor.SpaceWidth, editor.FontHeight));
				}
				ptTemp.X += editor.SpaceWidth;
			}
			else if (BlockType == BlockType.Tab)
			{
				if (isSelected)
				{
					g.FillRectangle(selectionBackgroundBrush, new RectangleF(ptTemp.X, ptTemp.Y, editor.SpaceWidth * 4, editor.FontHeight));
				}
				ptTemp.X += editor.SpaceWidth * editor.TabIndent;
			}
			else
			{
				if (isSelected && startPos != -1 && endPos != -1)
				{
					
					StringBuilder leftWords = new StringBuilder();
					StringBuilder middleWords = new StringBuilder();
					StringBuilder rightWords = new StringBuilder();
					int length = ptTemp.X;
					for (int i = 0; i < Text.Length; i++)
					{
						int word = editor.DrawHelper.MeasureStringWidth(g, Text[i].ToString(), editor.Font);
						// 小于等于起始位置的文本设置为左部分文本（可有可无）
						if(length + word <= startPos)
						{
							leftWords.Append(Text[i].ToString());
						}
						// 大于起始位置而小于等于结束位置的文本设置为中间文本（也就是选中部分）
						else if (length + word > startPos && length + word <= endPos)
						{	
							middleWords.Append(Text[i].ToString());
						}
						// 大于结束位置的文本定义为右侧文本（可有可无）
						else if(length + word > endPos)
						{
							rightWords.Append(Text[i].ToString());
						}
						length += word;
					}
					// 按段去绘制一个元素，最多可分为三部分（非选中、选中、非选中）
					editor.DrawHelper.DrawString(g, leftWords.ToString(), f,GetSegmentColor(editor), ptTemp);
					int leftWidth = string.IsNullOrEmpty(leftWords.ToString()) ? 0 : editor.DrawHelper.MeasureStringWidth(g,leftWords.ToString(),editor.Font);

					// 对高亮的绘制完全采用颜色填充的形式，而不采用直接设置背景色（直接设置背景色时，背景色和字体不是等宽的，而引起了背景色对前一个元素的尾字母产生了部分遮挡）
					int middleWidth = string.IsNullOrEmpty(middleWords.ToString()) ? 0 : editor.DrawHelper.MeasureStringWidth(g, middleWords.ToString(), editor.Font);
					g.FillRectangle(selectionBackgroundBrush, new RectangleF(ptTemp.X + leftWidth, ptTemp.Y, middleWidth,editor.FontHeight));
					editor.DrawHelper.DrawString(g, middleWords.ToString(), f, editor.HighLightTextColor, new Point(ptTemp.X + leftWidth, ptTemp.Y));

					editor.DrawHelper.DrawString(g, rightWords.ToString(), f, GetSegmentColor(editor), new Point(ptTemp.X + middleWidth + leftWidth, ptTemp.Y));
				}
				else if (isSelected && startPos == -1 && endPos == -1)
				{
					g.FillRectangle(selectionBackgroundBrush,new RectangleF(ptTemp.X,ptTemp.Y,wordWidth,editor.FontHeight));
					editor.DrawHelper.DrawString(g, Text, f, editor.HighLightTextColor, ptTemp);
				}
				else
				{
					editor.DrawHelper.DrawString(g, Text, f, GetSegmentColor(editor), ptTemp);
				}
				ptTemp.X += wordWidth;
			}
		}

		/// <summary>
		/// 获取颜色
		/// </summary>
		public Color GetSegmentColor(TextBoxControl editor) 
		{
			 switch(BlockType)
			 {
				 //case SegType.LeftSign:
				 //case SegType.RightSign:
				 //case SegType.Reverse:
					// return editor.ColorStrategy.Sign;
				 //case SegType.NodeName:
					// return editor.ColorStrategy.Name;
				 //case SegType.AttrName:
					// return editor.ColorStrategy.AttrName;
				 //case SegType.Quotation:
					// return editor.ColorStrategy.QuotationSign;
				 //case SegType.Equal:
					// return editor.ColorStrategy.EqualSign;
				 //case SegType.AttrValue:
					// return editor.ColorStrategy.AttrValue;
				 case BlockType.Text:
					 return editor.TextColor;
				 //case SegType.Comment:
				 //case SegType.CommentSign:
					// return editor.ColorStrategy.Comment;
				 //case SegType.CDATA:
				 //case SegType.CDATASign:
					// return editor.ColorStrategy.CData;
			 }
			 return Color.Empty;
		}
	}


	public class SpaceSegment : Block
	{
		public SpaceSegment() : base()
		{
			Text = " ";
			BlockType = BlockType.Space;
		}

		public override string Text
		{
			get { return " "; }
			set { ;}
		}

		public override int Length
		{
			get { return 1; }
		}

		public override BlockType BlockType
		{
			get { return BlockType.Space; }
			set { ;}
		}
	}

	public class TabSegment : Block
	{
		public TabSegment() : base()
		{
			Text = "\t";
			BlockType = BlockType.Tab;
		}

		public override string Text
		{
			get { return "\t"; }
			set { ;}
		}

		public override int Length
		{
			get
			{
				//if (_node != null)
				//    _node
				return 4;
			}
		}


		public override BlockType BlockType
		{
			get { return BlockType.Tab; }
			set { ;}
		}
	}

	public class EnterSegment : Block
	{
		public EnterSegment()
			: base()
		{
			Text = "\n";
			BlockType = BlockType.Enter;
		}

		public override string Text
		{
			get { return "\n"; }
			set { ;}
		}

		public override int Length
		{
			get { return 1; }
		}


		public override BlockType BlockType
		{
			get { return BlockType.Enter; }
			set { ;}
		}
	}

	public enum BlockType
	{
		Text,		// 文本
		Tab,		// Tab键
		Space,		// 空格
		Symbol,     // 符号
		Enter,		// 回车
	}
}
