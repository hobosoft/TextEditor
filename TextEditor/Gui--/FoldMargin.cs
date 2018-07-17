// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 3673 $</version>
// </file>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using VCI.XmlEditor.Document;

namespace VCI.XmlEditor
{
	/// <summary>
	/// This class views the line numbers and folding markers.
	/// </summary>
	public class FoldMargin : AbstractMargin
	{
		int selectedFoldLine = -1;

		public override Size Size
		{
			get
			{
				return new Size((int)(_editor.TextView.FontHeight),
								-1);
			}
		}

		public override bool IsVisible
		{
			get
			{
				return _editor.EditorProperties.EnableFolding;
			}
		}

		public FoldMargin(XmlEditorControl _editor)
			: base(_editor)
		{
		}

		public override void Paint(Graphics g, Rectangle rect)
		{
			if (rect.Width <= 0 || rect.Height <= 0)
			{
				return;
			}
			HighlightColor lineNumberPainterColor = _editor.Document.HighlightingStrategy.GetColorFor("LineNumbers");


			for (int y = 0; y < (DrawingPosition.Height + _editor.VisibleLineDrawingRemainder) / _editor.FontHeight + 1; ++y)
			{
				Rectangle markerRectangle = new Rectangle(DrawingPosition.X,
														  DrawingPosition.Top + y * _editor.FontHeight - _editor.VisibleLineDrawingRemainder,
														  DrawingPosition.Width,
														  _editor.FontHeight);

				if (rect.IntersectsWith(markerRectangle))
				{
					// draw dotted separator line
					if (_editor.Document.EditorProperties.ShowLineNumbers)
					{
						g.FillRectangle(BrushRegistry.GetBrush(_editor.Enabled ? lineNumberPainterColor.BackgroundColor : SystemColors.InactiveBorder),
										markerRectangle);

						g.DrawLine(BrushRegistry.GetDotPen(lineNumberPainterColor.Color),
								   base.drawingPosition.X,
								   markerRectangle.Y,
								   base.drawingPosition.X,
								   markerRectangle.Bottom);
					}
					else
					{
						g.FillRectangle(BrushRegistry.GetBrush(_editor.Enabled ? lineNumberPainterColor.BackgroundColor : SystemColors.InactiveBorder), markerRectangle);
					}

					int currentLine = _editor.Document.GetFirstLogicalLine(_editor.FirstPhysicalLine + y);
					if (currentLine < _editor.Document.TotalNumberOfLines)
					{
						PaintFoldMarker(g, currentLine, markerRectangle);
					}
				}
			}
		}

		bool SelectedFoldingFrom(List<FoldMarker> list)
		{
			if (list != null)
			{
				for (int i = 0; i < list.Count; ++i)
				{
					if (this.selectedFoldLine == list[i].StartLine)
					{
						return true;
					}
				}
			}
			return false;
		}

		void PaintFoldMarker(Graphics g, int lineNumber, Rectangle drawingRectangle)
		{
			HighlightColor foldLineColor = _editor.Document.HighlightingStrategy.GetColorFor("FoldLine");
			HighlightColor selectedFoldLine = _editor.Document.HighlightingStrategy.GetColorFor("SelectedFoldLine");

			List<FoldMarker> foldingsWithStart = _editor.Document.FoldingManager.GetFoldingsWithStart(lineNumber);
			List<FoldMarker> foldingsBetween = _editor.Document.FoldingManager.GetFoldingsContainsLineNumber(lineNumber);
			List<FoldMarker> foldingsWithEnd = _editor.Document.FoldingManager.GetFoldingsWithEnd(lineNumber);

			bool isFoldStart = foldingsWithStart.Count > 0;
			bool isBetween = foldingsBetween.Count > 0;
			bool isFoldEnd = foldingsWithEnd.Count > 0;

			bool isStartSelected = SelectedFoldingFrom(foldingsWithStart);
			bool isBetweenSelected = SelectedFoldingFrom(foldingsBetween);
			bool isEndSelected = SelectedFoldingFrom(foldingsWithEnd);

			int foldMarkerSize = (int)Math.Round(_editor.TextView.FontHeight * 0.47f);
			foldMarkerSize -= (foldMarkerSize) % 2;
			int foldMarkerYPos = drawingRectangle.Y + (int)((drawingRectangle.Height - foldMarkerSize) / 2);
			int xPos = drawingRectangle.X + (drawingRectangle.Width - foldMarkerSize) / 2 + foldMarkerSize / 2;


			if (isFoldStart)
			{
				bool isVisible = true;
				bool moreLinedOpenFold = false;
				foreach (FoldMarker foldMarker in foldingsWithStart)
				{
					if (foldMarker.IsFolded)
					{
						isVisible = false;
					}
					else
					{
						moreLinedOpenFold = foldMarker.EndLine > foldMarker.StartLine;
					}
				}

				bool isFoldEndFromUpperFold = false;
				foreach (FoldMarker foldMarker in foldingsWithEnd)
				{
					if (foldMarker.EndLine > foldMarker.StartLine && !foldMarker.IsFolded)
					{
						isFoldEndFromUpperFold = true;
					}
				}

				DrawFoldMarker(g, new RectangleF(drawingRectangle.X + (drawingRectangle.Width - foldMarkerSize) / 2,
												 foldMarkerYPos,
												 foldMarkerSize,
												 foldMarkerSize),
							   isVisible,
							   isStartSelected
							  );

				// draw line above fold marker
				if (isBetween || isFoldEndFromUpperFold)
				{
					g.DrawLine(BrushRegistry.GetPen(isBetweenSelected ? selectedFoldLine.Color : foldLineColor.Color),
							   xPos,
							   drawingRectangle.Top,
							   xPos,
							   foldMarkerYPos - 1);
				}

				// draw line below fold marker
				if (isBetween || moreLinedOpenFold)
				{
					g.DrawLine(BrushRegistry.GetPen(isEndSelected || (isStartSelected && isVisible) || isBetweenSelected ? selectedFoldLine.Color : foldLineColor.Color),
							   xPos,
							   foldMarkerYPos + foldMarkerSize + 1,
							   xPos,
							   drawingRectangle.Bottom);
				}
			}
			else
			{
				if (isFoldEnd)
				{
					int midy = drawingRectangle.Top + drawingRectangle.Height / 2;

					// draw fold end marker
					g.DrawLine(BrushRegistry.GetPen(isEndSelected ? selectedFoldLine.Color : foldLineColor.Color),
							   xPos,
							   midy,
							   xPos + foldMarkerSize / 2,
							   midy);

					// draw line above fold end marker
					// must be drawn after fold marker because it might have a different color than the fold marker
					g.DrawLine(BrushRegistry.GetPen(isBetweenSelected || isEndSelected ? selectedFoldLine.Color : foldLineColor.Color),
							   xPos,
							   drawingRectangle.Top,
							   xPos,
							   midy);

					// draw line below fold end marker
					if (isBetween)
					{
						g.DrawLine(BrushRegistry.GetPen(isBetweenSelected ? selectedFoldLine.Color : foldLineColor.Color),
								   xPos,
								   midy + 1,
								   xPos,
								   drawingRectangle.Bottom);
					}
				}
				else if (isBetween)
				{
					// just draw the line :)
					g.DrawLine(BrushRegistry.GetPen(isBetweenSelected ? selectedFoldLine.Color : foldLineColor.Color),
							   xPos,
							   drawingRectangle.Top,
							   xPos,
							   drawingRectangle.Bottom);
				}
			}
		}

		public override void HandleMouseMove(Point mousepos, MouseButtons mouseButtons)
		{
			bool showFolding = _editor.Document.EditorProperties.EnableFolding;
			int physicalLine = +(int)((mousepos.Y + _editor.VirtualTop.Y) / _editor.TextView.FontHeight);
			int realline = _editor.Document.GetFirstLogicalLine(physicalLine);

			if (!showFolding || realline < 0 || realline + 1 >= _editor.Document.TotalNumberOfLines)
			{
				return;
			}

			List<FoldMarker> foldMarkers = _editor.Document.FoldingManager.GetFoldingsWithStart(realline);
			int oldSelection = selectedFoldLine;
			if (foldMarkers.Count > 0)
			{
				selectedFoldLine = realline;
			}
			else
			{
				selectedFoldLine = -1;
			}
			if (oldSelection != selectedFoldLine)
			{
				_editor.Refresh(this);
			}
		}

		public override void HandleMouseDown(Point mousepos, MouseButtons mouseButtons)
		{
			bool showFolding = _editor.Document.EditorProperties.EnableFolding;
			int physicalLine = +(int)((mousepos.Y + _editor.VirtualTop.Y) / _editor.TextView.FontHeight);
			int realline = _editor.Document.GetFirstLogicalLine(physicalLine);

			// focus the textarea if the user clicks on the line number view
			_editor.Focus();

			if (!showFolding || realline < 0 || realline + 1 >= _editor.Document.TotalNumberOfLines)
			{
				return;
			}

			List<FoldMarker> foldMarkers = _editor.Document.FoldingManager.GetFoldingsWithStart(realline);
			foreach (FoldMarker fm in foldMarkers)
			{
				fm.IsFolded = !fm.IsFolded;
			}
			_editor.Document.FoldingManager.NotifyFoldingsChanged(EventArgs.Empty);
		}

		public override void HandleMouseLeave(EventArgs e)
		{
			if (selectedFoldLine != -1)
			{
				selectedFoldLine = -1;
				_editor.Refresh(this);
			}
		}

		#region Drawing functions
		void DrawFoldMarker(Graphics g, RectangleF rectangle, bool isOpened, bool isSelected)
		{
			HighlightColor foldMarkerColor = _editor.Document.HighlightingStrategy.GetColorFor("FoldMarker");
			HighlightColor foldLineColor = _editor.Document.HighlightingStrategy.GetColorFor("FoldLine");
			HighlightColor selectedFoldLine = _editor.Document.HighlightingStrategy.GetColorFor("SelectedFoldLine");

			Rectangle intRect = new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);
			g.FillRectangle(BrushRegistry.GetBrush(foldMarkerColor.BackgroundColor), intRect);
			g.DrawRectangle(BrushRegistry.GetPen(isSelected ? selectedFoldLine.Color : foldLineColor.Color), intRect);

			int space = (int)Math.Round(((double)rectangle.Height) / 8d) + 1;
			int mid = intRect.Height / 2 + intRect.Height % 2;

			// draw minus
			g.DrawLine(BrushRegistry.GetPen(foldMarkerColor.Color),
					   rectangle.X + space,
					   rectangle.Y + mid,
					   rectangle.X + rectangle.Width - space,
					   rectangle.Y + mid);

			// draw plus
			if (!isOpened)
			{
				g.DrawLine(BrushRegistry.GetPen(foldMarkerColor.Color),
						   rectangle.X + mid,
						   rectangle.Y + space,
						   rectangle.X + mid,
						   rectangle.Y + rectangle.Height - space);
			}
		}
		#endregion
	}
}
