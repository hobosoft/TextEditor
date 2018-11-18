using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TextEditor
{
	public partial class Paragraph
	{
		private List<Block> m_lstBlock = new List<Block>();
		private List<Line> m_lstLine = new List<Line>();
		private string m_sText;
		private float m_fHeight = 0.0f;

		private static string[] splitSymbols = new string[] {" ", ",", ".", "?", "\"", ";", ":", "\'", "{", "}", "[", "]", "(", ")", "+", "-", "*", "/", "<", ">", "，", "。", "？", "；", "【", "】", "|" };

		public Paragraph()
		{
			m_lstLine.Add(new Line());
		}

		public string Text
		{
			get { return m_sText; }
			set
			{
				if (m_sText == value)
					return;

				m_sText = value;

				Parser();
			}
		}

		public int Length
		{
			get
			{
				int length = 0;
				foreach (Block block in m_lstBlock)
				{
					length += block.Length;
				}

				return length;
			}
		}

		public float Height
		{
			get { return m_fHeight; }
		}

		public Block[] Blocks
		{
			get { return m_lstBlock.ToArray(); }
		}

		public int LineCount
		{
			get { return m_lstLine.Count; }
		}

		public Line[] Lines
		{
			get { return m_lstLine.ToArray(); }
		}

		private void Parser()
		{
			m_lstLine.Clear();

			if (string.IsNullOrEmpty(m_sText))
				return;

			string[] blocks = m_sText.Split(splitSymbols, StringSplitOptions.RemoveEmptyEntries);

			foreach (string text in blocks)
			{

			}
		}

		public SizeF CalcSize(Graphics g, Font f, TextBoxControl editor)
		{
			SizeF size = new SizeF();

			return size;
		}

		public void Draw(TextBoxControl editor, Graphics g, Font f, Point ptPos)
		{
			Point ptTemp = ptPos;
			RectangleF rcClip = g.ClipBounds;

			foreach (Line line in m_lstLine)
			{
				line.Draw(editor, g, f, ptTemp);
				ptTemp.Y += line.Height;
			}
			ptPos = ptTemp;
		}

		/// <summary>
		/// 添加段
		/// </summary>
		/// <param name="block"></param>
		public void AddSegment(Block block)
		{
			if (block == null)
				return;

			block.Paragraph = this;
			m_lstBlock.Add(block);
		}

		/// <summary>
		/// 添加段
		/// </summary>
		public void AddSegment(Block[] blocks)
		{
			if (blocks == null || blocks.Length == 0)
				return;

			foreach (Block seg in blocks)
			{
				seg.Paragraph = this;
				m_lstBlock.Add(seg);
			}
		}

		/// <summary>
		/// 添加段
		/// </summary>
		public void AddSegment(Block block, int index)
		{
			if (block == null)
				return;

			block.Paragraph = this;
			m_lstBlock.Insert(index, block);
		}

	}
}
