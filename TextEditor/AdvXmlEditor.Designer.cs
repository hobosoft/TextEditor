namespace TextEditor
{
	partial class AdvXmlEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvXmlEditor));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonFind = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonReplace = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonCut = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonPaste = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonUndo = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonRedo = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonValidation = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonHelp = new System.Windows.Forms.ToolStripButton();
			this.toolStripComboBoxLanguage = new System.Windows.Forms.ToolStripComboBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			//this.validationControl1 = new VCI.IETM.XMLEditControl.ValidationControl();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.newXmlEditControl = new TextEditor.TextBoxControl();
			this.toolStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNew,
            this.toolStripButtonOpen,
            this.toolStripButtonSave,
            this.toolStripSeparator1,
            this.toolStripButtonFind,
            this.toolStripButtonReplace,
            this.toolStripButtonCut,
            this.toolStripButtonPaste,
            this.toolStripSeparator2,
            this.toolStripButtonUndo,
            this.toolStripButtonRedo,
            this.toolStripSeparator3,
            this.toolStripButtonValidation,
            this.toolStripButtonHelp,
            this.toolStripComboBoxLanguage,
            this.toolStripButton1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(914, 25);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButtonNew
			// 
			this.toolStripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNew.Image")));
			this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonNew.Name = "toolStripButtonNew";
			this.toolStripButtonNew.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonNew.Text = "New";
			this.toolStripButtonNew.Click += new System.EventHandler(this.toolStripButtonNew_Click);
			// 
			// toolStripButtonOpen
			// 
			this.toolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpen.Image")));
			this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonOpen.Name = "toolStripButtonOpen";
			this.toolStripButtonOpen.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonOpen.Text = "Open";
			this.toolStripButtonOpen.Click += new System.EventHandler(this.toolStripButtonOpen_Click);
			// 
			// toolStripButtonSave
			// 
			this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
			this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonSave.Name = "toolStripButtonSave";
			this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonSave.Text = "Save";
			this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonFind
			// 
			this.toolStripButtonFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonFind.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFind.Image")));
			this.toolStripButtonFind.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonFind.Name = "toolStripButtonFind";
			this.toolStripButtonFind.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonFind.Text = "Find";
			this.toolStripButtonFind.Click += new System.EventHandler(this.toolStripButtonFind_Click);
			// 
			// toolStripButtonReplace
			// 
			this.toolStripButtonReplace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonReplace.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonReplace.Image")));
			this.toolStripButtonReplace.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonReplace.Name = "toolStripButtonReplace";
			this.toolStripButtonReplace.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonReplace.Text = "Replace";
			this.toolStripButtonReplace.Click += new System.EventHandler(this.toolStripButtonReplace_Click);
			// 
			// toolStripButtonCut
			// 
			this.toolStripButtonCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonCut.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCut.Image")));
			this.toolStripButtonCut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonCut.Name = "toolStripButtonCut";
			this.toolStripButtonCut.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonCut.Text = "Cut";
			this.toolStripButtonCut.Click += new System.EventHandler(this.toolStripButtonCut_Click);
			// 
			// toolStripButtonPaste
			// 
			this.toolStripButtonPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonPaste.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPaste.Image")));
			this.toolStripButtonPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonPaste.Name = "toolStripButtonPaste";
			this.toolStripButtonPaste.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonPaste.Text = "Paste";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonUndo
			// 
			this.toolStripButtonUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonUndo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUndo.Image")));
			this.toolStripButtonUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonUndo.Name = "toolStripButtonUndo";
			this.toolStripButtonUndo.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonUndo.Text = "Undo";
			// 
			// toolStripButtonRedo
			// 
			this.toolStripButtonRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonRedo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRedo.Image")));
			this.toolStripButtonRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonRedo.Name = "toolStripButtonRedo";
			this.toolStripButtonRedo.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonRedo.Text = "Redo";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonValidation
			// 
			this.toolStripButtonValidation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonValidation.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonValidation.Image")));
			this.toolStripButtonValidation.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonValidation.Name = "toolStripButtonValidation";
			this.toolStripButtonValidation.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonValidation.Text = "选择一个xsd文件来校验当前xml";
			this.toolStripButtonValidation.Click += new System.EventHandler(this.toolStripButtonValidation_Click);
			// 
			// toolStripButtonHelp
			// 
			this.toolStripButtonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonHelp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonHelp.Image")));
			this.toolStripButtonHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonHelp.Name = "toolStripButtonHelp";
			this.toolStripButtonHelp.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonHelp.Text = "Help";
			// 
			// toolStripComboBoxLanguage
			// 
			this.toolStripComboBoxLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.toolStripComboBoxLanguage.Name = "toolStripComboBoxLanguage";
			this.toolStripComboBoxLanguage.Size = new System.Drawing.Size(121, 25);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 25);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.newXmlEditControl);
			// 
			// splitContainer1.Panel2
			// 
			//this.splitContainer1.Panel2.Controls.Add(this.validationControl1);
			this.splitContainer1.Size = new System.Drawing.Size(914, 682);
			this.splitContainer1.SplitterDistance = 564;
			this.splitContainer1.TabIndex = 4;
			// 
			// validationControl1
			// 
			//this.validationControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			//this.validationControl1.Location = new System.Drawing.Point(0, 0);
			//this.validationControl1.Name = "validationControl1";
			//this.validationControl1.SelectError = null;
			//this.validationControl1.Size = new System.Drawing.Size(914, 114);
			//this.validationControl1.TabIndex = 0;
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "toolStripButton1";
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// newXmlEditControl
			// 
			this.newXmlEditControl.AutoScroll = true;
			this.newXmlEditControl.AutoScrollMinSize = new System.Drawing.Size(281, 17);
			this.newXmlEditControl.BackColor = System.Drawing.SystemColors.Window;
			this.newXmlEditControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.newXmlEditControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.newXmlEditControl.DrawRectangle = new System.Drawing.Rectangle(62, 4, 847, 554);
			this.newXmlEditControl.FileName = "";
			this.newXmlEditControl.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.newXmlEditControl.LanguageReader = null;
			this.newXmlEditControl.LineCount = 1;
			this.newXmlEditControl.Location = new System.Drawing.Point(0, 0);
			this.newXmlEditControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.newXmlEditControl.Name = "newXmlEditControl";
			this.newXmlEditControl.NeedRecalc = false;
			this.newXmlEditControl.ReadOnly = false;
			this.newXmlEditControl.ShowLineNumber = true;
			this.newXmlEditControl.Size = new System.Drawing.Size(914, 564);
			this.newXmlEditControl.TabIndex = 3;
			this.newXmlEditControl.TabSize = 4;
			// 
			// AdvXmlEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "AdvXmlEditor";
			this.Size = new System.Drawing.Size(914, 707);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton toolStripButtonNew;
		private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
		private System.Windows.Forms.ToolStripButton toolStripButtonSave;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolStripButtonFind;
		private System.Windows.Forms.ToolStripButton toolStripButtonReplace;
		private System.Windows.Forms.ToolStripButton toolStripButtonCut;
		private System.Windows.Forms.ToolStripButton toolStripButtonPaste;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolStripButtonUndo;
		private System.Windows.Forms.ToolStripButton toolStripButtonRedo;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton toolStripButtonValidation;
		private System.Windows.Forms.ToolStripButton toolStripButtonHelp;
		private TextBoxControl newXmlEditControl;
        private System.Windows.Forms.SplitContainer splitContainer1;
        //private IETM.XMLEditControl.ValidationControl validationControl1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxLanguage;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
	}
}
