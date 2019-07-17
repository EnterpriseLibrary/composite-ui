//===============================================================================
// Microsoft patterns & practices
// CompositeUI Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace Microsoft.Practices.CompositeUI.WinForms.Tests.DesignTime
{
	partial class Component1
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
			this.mySmartPart1 = new Microsoft.Practices.CompositeUI.WinForms.Tests.DesignTime.MySmartPart();
			this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
			this.listBox1 = new System.Windows.Forms.ListBox();
			// 
			// mySmartPart1
			// 
			this.mySmartPart1.Location = new System.Drawing.Point(0, 0);
			this.mySmartPart1.Name = "mySmartPart1";
			this.mySmartPart1.Size = new System.Drawing.Size(208, 187);
			this.mySmartPart1.TabIndex = 0;
			// 
			// monthCalendar1
			// 
			this.monthCalendar1.Location = new System.Drawing.Point(0, 0);
			this.monthCalendar1.Name = "monthCalendar1";
			this.monthCalendar1.Size = new System.Drawing.Size(178, 155);
			this.monthCalendar1.TabIndex = 0;
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(0, 0);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(120, 95);
			this.listBox1.TabIndex = 0;

		}

		#endregion

		private MySmartPart mySmartPart1;
		private System.Windows.Forms.MonthCalendar monthCalendar1;
		private System.Windows.Forms.ListBox listBox1;
	}
}
