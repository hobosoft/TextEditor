namespace TextEditor.Gui
{
	partial class OpenXmlFileDialog
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
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.labelXML = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxXML = new System.Windows.Forms.TextBox();
			this.buttonXml = new System.Windows.Forms.Button();
			this.textBoxSchema = new System.Windows.Forms.TextBox();
			this.buttonSchema = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(366, 70);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 0;
			this.buttonCancel.Text = "取消";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(285, 70);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 0;
			this.buttonOK.Text = "确定";
			this.buttonOK.UseVisualStyleBackColor = true;
			// 
			// labelXML
			// 
			this.labelXML.AutoSize = true;
			this.labelXML.Location = new System.Drawing.Point(12, 12);
			this.labelXML.Name = "labelXML";
			this.labelXML.Size = new System.Drawing.Size(53, 12);
			this.labelXML.TabIndex = 1;
			this.labelXML.Text = "XML File";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(71, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "Schema File";
			// 
			// textBoxXML
			// 
			this.textBoxXML.Location = new System.Drawing.Point(89, 9);
			this.textBoxXML.Name = "textBoxXML";
			this.textBoxXML.Size = new System.Drawing.Size(309, 21);
			this.textBoxXML.TabIndex = 2;
			// 
			// buttonXml
			// 
			this.buttonXml.Location = new System.Drawing.Point(404, 7);
			this.buttonXml.Name = "buttonXml";
			this.buttonXml.Size = new System.Drawing.Size(37, 23);
			this.buttonXml.TabIndex = 3;
			this.buttonXml.Text = "...";
			this.buttonXml.UseVisualStyleBackColor = true;
			this.buttonXml.Click += new System.EventHandler(this.buttonXml_Click);
			// 
			// textBoxSchema
			// 
			this.textBoxSchema.Location = new System.Drawing.Point(89, 36);
			this.textBoxSchema.Name = "textBoxSchema";
			this.textBoxSchema.Size = new System.Drawing.Size(309, 21);
			this.textBoxSchema.TabIndex = 2;
			// 
			// buttonSchema
			// 
			this.buttonSchema.Location = new System.Drawing.Point(404, 34);
			this.buttonSchema.Name = "buttonSchema";
			this.buttonSchema.Size = new System.Drawing.Size(37, 23);
			this.buttonSchema.TabIndex = 3;
			this.buttonSchema.Text = "...";
			this.buttonSchema.UseVisualStyleBackColor = true;
			this.buttonSchema.Click += new System.EventHandler(this.buttonSchema_Click);
			// 
			// OpenXmlFileDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(453, 105);
			this.Controls.Add(this.buttonSchema);
			this.Controls.Add(this.buttonXml);
			this.Controls.Add(this.textBoxSchema);
			this.Controls.Add(this.textBoxXML);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.labelXML);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OpenXmlFileDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "选择XML文件及Schema";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Label labelXML;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxXML;
		private System.Windows.Forms.Button buttonXml;
		private System.Windows.Forms.TextBox textBoxSchema;
		private System.Windows.Forms.Button buttonSchema;
	}
}