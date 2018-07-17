namespace XmlEditorTest
{
	partial class Form2
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBoxControl1 = new TextEditor.TextBoxControl();
			this.SuspendLayout();
			// 
			// textBoxControl1
			// 
			this.textBoxControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxControl1.AutoScroll = true;
			this.textBoxControl1.AutoScrollMinSize = new System.Drawing.Size(266, 17);
			this.textBoxControl1.BackColor = System.Drawing.SystemColors.Window;
			this.textBoxControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxControl1.FileName = "";
			this.textBoxControl1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.textBoxControl1.Location = new System.Drawing.Point(12, 12);
			this.textBoxControl1.Name = "textBoxControl1";
			this.textBoxControl1.ReadOnly = false;
			this.textBoxControl1.ShowLineNumber = true;
			this.textBoxControl1.Size = new System.Drawing.Size(538, 381);
			this.textBoxControl1.TabIndex = 0;
			this.textBoxControl1.TabIndent = 4;
			// 
			// Form2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(562, 405);
			this.Controls.Add(this.textBoxControl1);
			this.Name = "Form2";
			this.Text = "Form2";
			this.ResumeLayout(false);

		}

		#endregion

		private TextEditor.TextBoxControl textBoxControl1;
	}
}