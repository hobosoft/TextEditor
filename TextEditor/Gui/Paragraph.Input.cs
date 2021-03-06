﻿
using System.Linq;
using System.Collections.Generic;
using TextEditor.Document;
using System.Text;
using System.Text.RegularExpressions;

namespace TextEditor
{
	/// <summary>
	/// 插入操作
	/// </summary>
	public enum InsertOperate 
	{
		/// <summary>
		/// 插入节点名称
		/// </summary>
		InsertNodeName = 0,

		/// <summary>
		/// 插入属性名称
		/// </summary>
		InsertAttrName = 1,

		/// <summary>
		/// 插入属性值
		/// </summary>
		InsertAttrValue = 2,

		/// <summary>
		/// 无
		/// </summary>
		None = 3
	}

	partial class Paragraph
	{
		/// <summary>
		/// 匹配任意字符
		/// </summary>
		Regex regexPun = new Regex(@"^\p{P}|\p{S}");

		/// <summary>
		/// 在指定位置插入字符
		/// </summary>
		public bool Insert(int offsert, string text, Caret caret) 
		{
			InsertOperate operate = InsertOperate.None;
			Block segment = null;
			return Insert(offsert, text, caret, ref operate, ref segment);
		}

		/// <summary>
		/// 在指定位置插入字符
		/// </summary>
		public bool Insert(int offsert, string text, Caret caret, ref InsertOperate operate, ref Block segment)
		{
			int insertLocation = 0;
			Block nearSegment = GetLineSegmentBeforeCaret(offsert, ref insertLocation);
			int index = 0;
			if (nearSegment != null)
				index = m_lstBlock.IndexOf(nearSegment);
			// 如果说当前行为首行的话，创建新的节点，并将新的节点做为当前节点的子节点，新节点自动换下一行并增加四个空格作为缩进
			// <first>
			// 	   <second>
			// 	   </second>
			// </first>
			// 这种排版主要为了规避当前展示时的对象结构为一行属于一个节点，而编辑时很有可能一行属于多个节点，自动进行格式设置，规避此种问题的出现

			// 若当前光标落在节点名、属性名、属性值、文本等可直接插入字符对这些内容进行补充
			if (nearSegment != null)
			{
				nearSegment.Text = nearSegment.Text.Insert(insertLocation, text);
				caret.Column += 1;
				return true;
			}

			return false;
		}

		/// <summary>
		/// 检测是否包含有效字符
		/// </summary>
		public void CheckIsIncludeVaildSegment(ref bool isEnd, ref bool isAllSpace)
		{
			int nCount = 0;
			foreach (Block ls in m_lstBlock)
			{
				if (ls.BlockType == BlockType.Space || ls.BlockType == BlockType.Tab)
				{
					nCount += 1;
				}
			}
			if (nCount == m_lstBlock.Count || (nCount == 0 && m_lstBlock.Count == 0))
				isAllSpace = true;
		}

		/// <summary>
		/// 获取空格元素（用于新行缩进）count标示缩进列数
		/// </summary>
		private int GetSpaceSegment(List<Block> segmentList)
		{
			int count = 0;
			foreach (Block seg in this.Blocks)
			{
				if (seg.BlockType == BlockType.Space)
				{
					segmentList.Add(seg);
					count += 1;
				}
				else if (seg.BlockType == BlockType.Tab)
				{
					segmentList.Add(seg);
					count += 4;
				}
				else
				{
					break;
				}
			}
			return count;
		}

		/// <summary>
		/// 获取在光标位置前面的元素或光标在其中的元素
		/// </summary>
		public Block GetLineSegmentBeforeCaret(int colNum, ref int insertLocation)
		{
			int count = 0;

			foreach (Block segment in m_lstBlock)
			{
				if (segment.BlockType == BlockType.Space)
				{
					count += 1;
					if (count == colNum)
					{
						// 这是处理何时添加属性名称的逻辑，待优化
						int index = m_lstBlock.IndexOf(segment);
						if (index + 1 < m_lstBlock.Count && string.IsNullOrEmpty(m_lstBlock[index + 1].Text))
						{
							return m_lstBlock[index + 1];
						}
						return segment;
					}
				}
				else if (segment.BlockType == BlockType.Tab)
				{
					count += 4;
					if (count == colNum)
						return segment;
				}
				else
				{
					int textLength = segment.Text.Length;
					if(count <= colNum && count + textLength >= colNum)
					{
						int index = m_lstBlock.IndexOf(segment);
						if (index + 1 < m_lstBlock.Count && string.IsNullOrEmpty(m_lstBlock[index + 1].Text))
						{
							return m_lstBlock[index + 1];
						}
						insertLocation = colNum - count;
						return segment;
					}
					count += textLength;
				}
			}
			return null;
		}

		/// <summary>
		/// 获取光标截取文本元素所产生的集合
		/// </summary>
		public List<string> SpitTextSegmentByCaret(int colNum) 
		{
			List<string> spitString = new List<string>();
			int count = 0;
			foreach (Block segment in m_lstBlock)
			{
				if (segment.BlockType == BlockType.Space)
				{
					count += 1;
				}
				else if (segment.BlockType == BlockType.Tab)
				{
					count += 4;
				}
				else
				{
					int textLength = segment.Text.Length;
					if (count <= colNum && count + textLength >= colNum && segment.BlockType == BlockType.Text)
					{
						StringBuilder before = new StringBuilder();
						StringBuilder after = new StringBuilder();
						for (int i = 0; i < segment.Text.Length; i++ )
						{
							if (count <colNum)
							{
								before.Append(segment.Text[i]);
							}
							else 
							{
								after.Append(segment.Text[i]);
							}
							count++;
						}
						spitString.Add(before.ToString());
						spitString.Add(after.ToString());
						return spitString;
					}
					count += textLength;
				}
			}
			return spitString;
		}

		/// <summary>
		/// 在指定位置删除字符(只有文本和属性值能够被删除，其他类型元素均不能被删除)
		/// </summary>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public bool Remove(int offset, int count)
		{
			int iIndex = 0;
			foreach (Block block in Blocks)
			{
				if (offset >= iIndex && offset < iIndex + block.Length)
				{
					if (block.BlockType != BlockType.Text)
						return false;

					if ((iIndex + block.Length - offset) >= count)
					{
						block.Text = block.Text.Remove(offset - iIndex, count);
						return true;
					}
					else
					{
						int iTemp = (iIndex + block.Length - offset);
						block.Text = block.Text.Remove(offset - iIndex, iTemp);
						count -= iTemp;
						continue;
					}
				}
				iIndex += block.Length;
			}

			return false;
		}

		/// <summary>
		/// 删除指定的元素
		/// </summary>
		public bool Remove(List<Block> lstBlock) 
		{
			foreach (Block block in lstBlock)
			{
				if(m_lstBlock.Contains(block))
				{
					m_lstBlock.Remove(block);
				}
			}
			return false;
		}
	}
}