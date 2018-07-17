using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace TextEditor
{
	internal class DrawHelper
	{
		struct TextFontPair
		{
			string word;
			Font font;
			public TextFontPair(string word, Font font)
			{
				this.word = word;
				this.font = font;
			}
			public override bool Equals(object obj)
			{
				TextFontPair myWordFontPair = (TextFontPair)obj;
				if (!word.Equals(myWordFontPair.word)) return false;
				return font.Equals(myWordFontPair.font);
			}

			public override int GetHashCode()
			{
				return word.GetHashCode() ^ font.GetHashCode();
			}
		}

		Dictionary<TextFontPair, int> measureCache = new Dictionary<TextFontPair, int>();

		// Important: Some flags combinations work on WinXP, but not on Win2000.
		// Make sure to test changes here on all operating systems.
		const TextFormatFlags textFormatFlags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.PreserveGraphicsClipping;

		const int MaximumWordLength = 1000;
		const int MaximumCacheSize = 2000;
		
		public int MeasureStringWidth(Graphics g, string word, Font font)
		{
			int width;
			
			if (word == null || word.Length == 0)
				return 0;
			if (word.Length > MaximumWordLength) {
				width = 0;
				for (int i = 0; i < word.Length; i += MaximumWordLength) {
					if (i + MaximumWordLength < word.Length)
						width += MeasureStringWidth(g, word.Substring(i, MaximumWordLength), font);
					else
						width += MeasureStringWidth(g, word.Substring(i, word.Length - i), font);
				}
				return width;
			}
			if (measureCache.TryGetValue(new TextFontPair(word, font), out width))
			{
				return width;
			}
			if (measureCache.Count > MaximumCacheSize) {
				measureCache.Clear();
			}
			
			// This code here provides better results than MeasureString!
			// Example line that is measured wrong:
			// txt.GetPositionFromCharIndex(txt.SelectionStart)
			// (Verdana 10, highlighting makes GetP... bold) -> note the space between 'x' and '('
			// this also fixes "jumping" characters when selecting in non-monospace fonts
			// [...]
			// Replaced GDI+ measurement with GDI measurement: faster and even more exact
			width = TextRenderer.MeasureText(g, word, font, new Size(short.MaxValue, short.MaxValue), textFormatFlags).Width;
			measureCache.Add(new TextFontPair(word, font), width);
			return width;
		}

		public int MeasureCharWidth(Graphics g, char c, Font font)
		{
			int width;
			
			if (c == (char)0)
				return 0;

			string sText = c.ToString();

			if (measureCache.TryGetValue(new TextFontPair(sText, font), out width))
			{
				return width;
			}
			if (measureCache.Count > MaximumCacheSize) {
				measureCache.Clear();
			}
			
			// This code here provides better results than MeasureString!
			// Example line that is measured wrong:
			// txt.GetPositionFromCharIndex(txt.SelectionStart)
			// (Verdana 10, highlighting makes GetP... bold) -> note the space between 'x' and '('
			// this also fixes "jumping" characters when selecting in non-monospace fonts
			// [...]
			// Replaced GDI+ measurement with GDI measurement: faster and even more exact
			width = TextRenderer.MeasureText(g, sText, font, new Size(short.MaxValue, short.MaxValue), textFormatFlags).Width;
			measureCache.Add(new TextFontPair(sText, font), width);
			return width;
		}

		public void DrawString(Graphics g, string text, Font font, Color color, int x, int y)
		{
			TextRenderer.DrawText(g, text, font, new Point(x, y), color, textFormatFlags);
		}

		public void DrawString(Graphics g, string text, Font font, Color color, Point pt)
		{
			TextRenderer.DrawText(g, text, font, pt, color, textFormatFlags);
		}

		/// <summary>
		/// 带背景色绘制的文本绘制方法
		/// </summary>
		public void DrawString(Graphics g, string text, Font font, Color foreColor, Color backColor, Point pt) 
		{
			TextRenderer.DrawText(g, text, font, pt, foreColor, backColor, textFormatFlags);
		}

		//#endregion
	}
}
