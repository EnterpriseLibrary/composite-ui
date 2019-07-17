using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace MyModule
{
	[SmartPart]
	public partial class MyView : UserControl, IMyView 
	{
		public MyView()
		{
			InitializeComponent();
		}

		#region IMyView Members

		public string Message
		{
			get
			{
				return this.label1.Text;
			}
			set
			{
				this.label1.Text = value;
			}
		}

		#endregion
	}
}
