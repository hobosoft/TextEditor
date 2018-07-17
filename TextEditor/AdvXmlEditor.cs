using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using TextEditor.Gui;
using VCI.IETM.Interface;

namespace TextEditor
{
    public partial class AdvXmlEditor : UserControl
    {
        //FindForm _dlgFind;
        //GoToForm _dlgGoTo;
        //ReplaceForm _dlgReplace;
        private string m_schemafile = "";
        private ILanguageReader m_reader = null;

        public bool ReadOnly
        {
            get { return newXmlEditControl.ReadOnly; }
            set { newXmlEditControl.ReadOnly = value; }
        }

        public ILanguageReader LanguageReader
        {
            get { return m_reader; }
            set
            {
                m_reader = value;
                newXmlEditControl.LanguageReader = m_reader;
                InitLanguage();
            }
        }

        public AdvXmlEditor()
        {
            InitializeComponent();
        }

        public void NewDocument()
        {
            newXmlEditControl.NewDocument("");
        }

        public void NewDocumentWithSchema(string schema)
        {
            if (string.IsNullOrEmpty(schema))
            {
                OpenFileDialog dlg = new OpenFileDialog();
				dlg.Title = "请选择Schema";
                dlg.Filter = "Schema File (*.xsd)|*.xsd";

				if (dlg.ShowDialog() != DialogResult.OK)
				{
					newXmlEditControl.LoadFile("");
				}
				else
					newXmlEditControl.LoadFile(dlg.FileName);
            }
            else if (File.Exists(schema))
                newXmlEditControl.NewDocument(schema);
        }

        public void OpenDocument(string file = null)
        {
            if (string.IsNullOrEmpty(file))
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "XML File (*.xml)|*.xml";

                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                newXmlEditControl.LoadFile(dlg.FileName);
            }
            else if (File.Exists(file))
                newXmlEditControl.LoadFile(file);
        }

        public void OpenDocumentWithSchema(string file, string schema)
        {
            if (string.IsNullOrEmpty(file) || string.IsNullOrEmpty(schema))
            {
                OpenXmlFileDialog dlg = new OpenXmlFileDialog(file, schema);

                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                newXmlEditControl.LoadFile(dlg.XmlFile, dlg.SchemaFile);
            }
            else if (File.Exists(file))
            {
                newXmlEditControl.LoadFile(file, schema);
                m_schemafile = schema;
            }
        }

        public void Save(string file)
        {
            newXmlEditControl.Document.SaveAs(file);
        }

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            NewDocumentWithSchema("");
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            //Open();
            OpenDocumentWithSchema("", "");
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            Save("");
        }

        private void toolStripButtonFind_Click(object sender, EventArgs e)
        {
            newXmlEditControl.ExecuteDialogKey(Keys.Control | Keys.F);
        }

        private void toolStripButtonReplace_Click(object sender, EventArgs e)
        {
            newXmlEditControl.ExecuteDialogKey(Keys.Control | Keys.R);
        }

        private void toolStripButtonCut_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonValidation_Click(object sender, EventArgs e)
        {
            if (!File.Exists(m_schemafile))
            {
                MessageBox.Show("没有提供Schema文件，不能进行校验！");
                return;
            }
            //validationControl1.ValidateXMLContent(this.newXmlEditControl.Document.InnerXml, m_schemafile);
            this.splitContainer1.Panel2Collapsed = false;
        }

        /// <summary>
        /// 显示语言选项
        /// </summary>
        private void InitLanguage()
        {
            this.toolStripComboBoxLanguage.Items.Clear();
            this.toolStripComboBoxLanguage.Items.Add("");
            foreach (string language in m_reader.Languages)
            {
                this.toolStripComboBoxLanguage.Items.Add(language);
            }
            if (!string.IsNullOrEmpty(m_reader.CurrentLanguage))
            {
                this.toolStripComboBoxLanguage.Text = m_reader.CurrentLanguage;
            }
            this.toolStripComboBoxLanguage.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxLanguage_SelectedIndexChanged);

        }

        /// <summary>
        /// 切换语言
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripComboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = this.toolStripComboBoxLanguage.Text;
            m_reader.ChangeLanguage(key);
            newXmlEditControl.Refresh();
        }

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			string sXmlText = "<dmRef width=\"200\" height=\"5\"><icn code=\"asdfasdfasdf\" /></dmRef>";

			newXmlEditControl.Document.InsertXml(sXmlText);
		}
    }
}