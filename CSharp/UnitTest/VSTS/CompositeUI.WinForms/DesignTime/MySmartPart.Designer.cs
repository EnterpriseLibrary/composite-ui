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
	partial class MySmartPart
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
			this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
			this.tabSmartPartInfo1 = new Microsoft.Practices.CompositeUI.WinForms.TabSmartPartInfo();
			this.infoProvider = new Microsoft.Practices.CompositeUI.SmartParts.SmartPartInfoProvider();
			this.zoneSmartPartInfo1 = new Microsoft.Practices.CompositeUI.WinForms.ZoneSmartPartInfo();
			this.SuspendLayout();
			// 
			// monthCalendar1
			// 
			this.monthCalendar1.Location = new System.Drawing.Point(16, 14);
			this.monthCalendar1.Name = "monthCalendar1";
			this.monthCalendar1.TabIndex = 0;
			// 
			// tabSmartPartInfo1
			// 
			this.tabSmartPartInfo1.ActivateTab = true;
			this.tabSmartPartInfo1.Description = "";
			this.tabSmartPartInfo1.Position = Microsoft.Practices.CompositeUI.WinForms.TabPosition.End;
			this.tabSmartPartInfo1.Title = "";
			this.infoProvider.Items.Add(this.tabSmartPartInfo1);
			// 
			// zoneSmartPartInfo1
			// 
			this.zoneSmartPartInfo1.Description = "";
			this.zoneSmartPartInfo1.Title = "";
			this.zoneSmartPartInfo1.ZoneName = null;
			this.infoProvider.Items.Add(this.zoneSmartPartInfo1);
			// 
			// MySmartPart
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.monthCalendar1);
			this.Name = "MySmartPart";
			this.Size = new System.Drawing.Size(208, 187);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.MonthCalendar monthCalendar1;
		private TabSmartPartInfo tabSmartPartInfo1;
		private ZoneSmartPartInfo zoneSmartPartInfo1;
		private Microsoft.Practices.CompositeUI.SmartParts.SmartPartInfoProvider infoProvider;
	}
}
