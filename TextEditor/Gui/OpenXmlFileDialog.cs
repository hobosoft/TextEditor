using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TextEditor.Gui
{
	public partial class OpenXmlFileDialog : Form
	{
		public OpenXmlFileDialog(string file, string schema)
		{
			InitializeComponent();
			textBoxXML.Text = file;
			textBoxSchema.Text = schema;
		}

		private void buttonXml_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Xml File (*.xml)|*.xml|All File (*.*)|*.*";
			if (dlg.ShowDialog() != DialogResult.OK)
				return;

			textBoxXML.Text = dlg.FileName;
		}

		private void buttonSchema_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Schema File (*.xsd)|*.xsd|All File (*.*)|*.*";
			if (dlg.ShowDialog() != DialogResult.OK)
				return;

			textBoxSchema.Text = dlg.FileName;
		}

		public string XmlFile
		{
			get { return textBoxXML.Text; }
		}

		public string SchemaFile
		{
			get { return textBoxSchema.Text; }
		}
	}
}
