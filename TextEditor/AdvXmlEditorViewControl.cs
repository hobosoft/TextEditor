using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VCI.Common.Interfaces;
using VCI.Client.UIEngine.Controls;
using VCI.Client.UIEngine.Objects;
using VCI.IETM.Objects;
using VCI.IETM.Interface;
using VCI.IETM.XMLSerializer;

namespace VCI.IETM.XmlEditor
{
	public partial class AdvXmlEditorViewControl : CustomViewControl
	{
		public AdvXmlEditorViewControl()
		{
			InitializeComponent();

			advXmlEditor.ReadOnly = true;
		}

        public override void InitData(IDataNode dataSource, Dictionary<string, string> paras)
        {
            if (dataSource == null)
            {
                return;
            }

            bool bReadOnly = true;
            string sReadonly = "";
            if (paras.TryGetValue("readonly", out sReadonly))
            {
                bReadOnly = System.Convert.ToBoolean(sReadonly);
            }

            advXmlEditor.ReadOnly = bReadOnly;

            IEntity ent = dataSource.getEntity();
            if (ent is BusinessFile)
            {
                BusinessFile bf = ent as BusinessFile;
                string file = bf.GetDownloadPath();
                if (!string.IsNullOrEmpty(file))
                    advXmlEditor.OpenDocument(file);
            }
            else if (ent is IXmlData)
            {
                IXmlData dm = ent as IXmlData;

                if (dm.XMLFile == null)
                {
                    MessageBox.Show("选中的业务数据，没有获取到文件，不能编辑内容！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                dm.DownLoadFiles();
                dm.DownLoadXSDFiles();
                string file = dm.XMLFile.GetDownloadPath();
                if (!string.IsNullOrEmpty(file))
                {
                    if (!string.IsNullOrEmpty(dm.XSDFilePath))
                    {
                        advXmlEditor.OpenDocumentWithSchema(file, dm.XSDFilePath);
                    }
                    else
                    {
                        advXmlEditor.OpenDocument(file);
                    }
                    advXmlEditor.LanguageReader = dm.LanguageReader;
                }

            }
        }

		public override void ClearData()
		{
			;
		}

		public override IEntity GetCurSelectEntity()
		{
			return null;
		}

		public override IEntity[] GetSelectEntity()
		{
			return null;
		}
	}
}
