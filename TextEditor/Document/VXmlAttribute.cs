using System;
using System.Collections.Generic;
using System.Text;

namespace TextEditor.Document
{
	public class VXmlAttribute
	{

		public VXmlAttribute(VXmlNode node) 
		{
			Node = node;
			node.Attributes.Add(this);
		}

		/// <summary>
		/// 所属XML节点
		/// </summary>
		public VXmlNode Node
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			set;
		}

		public virtual string Label
		{
			get
			{
				string display = Name;
				if (Node == null)
				{
					return Name;
				}
				else 
				{
					display = Name;// Node.Document.LanguageReader == null ? Name : Node.Document.LanguageReader.GetDisplayText(Name);
				}
				if (string.IsNullOrEmpty(display))
					return Name;
				return display;
			}
		}

		public string Value
		{
			get;
			set;
		}
	}
}
