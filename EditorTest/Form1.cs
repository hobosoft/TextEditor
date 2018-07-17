using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace XmlEditorTest
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
       private string m_xmlfile = "";
		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "XML File (*.xml)|*.xml";

			if (dlg.ShowDialog() != DialogResult.OK)
				return;
            m_xmlfile = dlg.FileName;
			xmlEditorControl1.LoadFile(dlg.FileName);
		}

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2Collapsed = false;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XSD File (*.xsd)|*.xsd";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            //validationControl1.ValidateXMFile(m_xmlfile, dlg.FileName);
        }
	}
}
