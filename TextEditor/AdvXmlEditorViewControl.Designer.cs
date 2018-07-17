namespace VCI.IETM.XmlEditor
{
	partial class AdvXmlEditorViewControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.advXmlEditor = new VCI.IETM.XmlEditor.AdvXmlEditor();
			this.SuspendLayout();
			// 
			// advXmlEditor
			// 
			this.advXmlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.advXmlEditor.Location = new System.Drawing.Point(0, 0);
			this.advXmlEditor.Name = "advXmlEditor";
			this.advXmlEditor.Size = new System.Drawing.Size(564, 440);
			this.advXmlEditor.TabIndex = 0;
			// 
			// XmlEditorViewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.advXmlEditor);
			this.Name = "XmlEditorViewControl";
			this.Size = new System.Drawing.Size(564, 440);
			this.ResumeLayout(false);

		}

		#endregion

		private XmlEditor.AdvXmlEditor advXmlEditor;

	}
}
