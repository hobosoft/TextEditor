using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextEditor.Document
{
	internal class Table
	{
		#region subclass
		internal class TTitle
		{

		}

		internal class TGroup
		{

		}

		internal class THeader
		{

		}

		internal class TBody
		{

		}

		internal class TFoot
		{

		}

		internal class Row
		{
			List<Entry> _lstEntry = new List<Entry>();
		}

		internal class Entry
		{

		}
		#endregion

		#region fields
		List<TGroup> _lstGroup = new List<TGroup>();
		#endregion

		#region properties
		public TGroup[] TableGroups
		{
			get { return _lstGroup.ToArray(); }
		}

		public TTitle Title
		{
			get;
			set;
		}

		public string tabstyle { get;set; }
        public string tocentry { get;set; }
        public string frame { get;set; }
        public string colsep { get;set; }
        public string rowsep { get;set; }
        public string orient { get;set; }
        public string pgwide { get;set; }
        public string applicRefId { get;set; }
		public string id { get; set; }

		#region changeAttGroup
        public string changeType { get;set; }
        public string changeMark { get;set; }
		public string reasonForUpdateRefIds { get; set; }
		#endregion

		#region authorityAttGroup
        public string authorityName { get; set; }
		public string authorityDocument { get; set; }
		#endregion

		#region securityAttGroup
		/// <summary>
		/// 取值范围“00”-“99”
		/// </summary>
        string securityClassification { get; set; }

        #region commercialSecurityAttGroup
			public string commercialClassification { get;set;}
			public string caveat { get;set;}
		#endregion
		#endregion

		#endregion


		public Table()
		{
			//Title = new TTitle();
			_lstGroup.Add(new TGroup());
		}
	}
}
