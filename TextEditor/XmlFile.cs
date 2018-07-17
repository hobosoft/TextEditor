using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using VCI.IETM.XMLEditControl;

namespace XmlEditor
{
	public class XmlFile
	{
		public XmlHeader XmlHeader
		{
			get;
			set;
		}

		public XmlNode NodeRoot
		{
			get;
			set;
		}

		public bool NCalc
		{
			get;
			set;
		}

		public Size Size
		{
			get;
			protected set;
		}

        /// <summary>
        /// 全文文本
        /// </summary>
        public virtual string Text
        {
            get;
            set;
        }

		public XmlFile()
		{
			XmlHeader = new XmlHeader();
			NodeRoot = new XmlNode("RootNode");

			NCalc = true;
		}

		public void Load(string file)
		{
			System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

			try
			{
				doc.Load(file);
				XmlHeader = new XmlHeader(doc.FirstChild);
                Text = doc.InnerXml;
				NodeRoot = new XmlNode();
				NodeRoot.Init(doc.DocumentElement);

				NCalc = true;
			}
			catch (System.Exception ex)
			{
				MessageBox.Show("打开XML文件失败，请确认文件是否正确。\n" + ex.Message);
			}
		}

        /// <summary>
        /// 加载xml文件，同时加载其对应的schema文件
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="xsd"></param>
		public void LoadXml(string xml,string xsd="")
		{
			System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

			try
			{
				doc.LoadXml(xml);
				XmlHeader = new XmlHeader(doc.FirstChild);
                Text = doc.InnerXml;
                NodeRoot = new XmlNode();
				NodeRoot.Init(doc.DocumentElement);

				NCalc = true;
			}
			catch (System.Exception ex)
			{
				MessageBox.Show("读取XML字符串失败，请确认xml字符串是否正确。\n" + ex.Message);
			}
		}

        /// <summary>
        /// 设置对应xml文件的schema文件
        /// </summary>
        /// <param name="xsd"></param>
        public void SetSchema(string xsd)
        {
            _editControl = new XMLEditControl();
            _editControl.SchemaFile = xsd;
            _editControl.Init();
        }

		public Size CalcSize(Graphics g, Font f)
		{
			Size size = new Size();

			if (XmlHeader != null)
			{
				Size szTemp = XmlHeader.CalcSize(g, f);
				if (szTemp.Width > size.Width)
					size.Width = szTemp.Width;

				size.Height += szTemp.Height;
			}

			if (NodeRoot != null)
			{
				Size szTemp = NodeRoot.CalcSize(g, f);
				if (szTemp.Width > size.Width)
					size.Width = szTemp.Width;

				size.Height += szTemp.Height;
			}

			Size = size;

			NCalc = false;

			return size;
		}

		public int LineCount()
		{
			int iCount = 0;
			if (XmlHeader != null)
				iCount += 1;
			if (NodeRoot != null)
				iCount += NodeRoot.LineCount();

			return iCount;
		}


        private XMLEditControl _editControl =new  XMLEditControl();
	}
}
