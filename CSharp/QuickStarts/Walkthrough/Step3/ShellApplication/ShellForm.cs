using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.WinForms;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace ShellApplication
{
	[SmartPart]
	public partial class ShellForm : Form
	{
		public ShellForm()
		{
			InitializeComponent();
		}
	}
}