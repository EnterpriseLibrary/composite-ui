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

namespace SmartPartQuickStart.ViewCustomerWorkItem
{
	partial class CustomerTabView
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
			this.tabWorkspace1 = new Microsoft.Practices.CompositeUI.WinForms.TabWorkspace();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.customerDetailView1 = new SmartPartQuickStart.ViewCustomerWorkItem.CustomerDetailView();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.smartPartPlaceholder1 = new Microsoft.Practices.CompositeUI.WinForms.SmartPartPlaceholder();
			this.tabWorkspace1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabWorkspace1
			// 
			this.tabWorkspace1.Controls.Add(this.tabPage1);
			this.tabWorkspace1.Controls.Add(this.tabPage2);
			this.tabWorkspace1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabWorkspace1.Location = new System.Drawing.Point(0, 0);
			this.tabWorkspace1.Name = "tabWorkspace1";
			this.tabWorkspace1.SelectedIndex = 0;
			this.tabWorkspace1.Size = new System.Drawing.Size(464, 316);
			this.tabWorkspace1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.customerDetailView1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(456, 290);
			this.tabPage1.TabIndex = 1;
			this.tabPage1.Text = "Customer Details";
			// 
			// customerDetailView1
			// 
			this.customerDetailView1.Description = "";
			this.customerDetailView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.customerDetailView1.Location = new System.Drawing.Point(3, 3);
			this.customerDetailView1.Name = "customerDetailView1";
			this.customerDetailView1.Size = new System.Drawing.Size(450, 284);
			this.customerDetailView1.TabIndex = 0;
			this.customerDetailView1.Title = "Customer Details";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.smartPartPlaceholder1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(456, 290);
			this.tabPage2.TabIndex = 2;
			this.tabPage2.Text = "Summary";
			// 
			// smartPartPlaceholder1
			// 
			this.smartPartPlaceholder1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.smartPartPlaceholder1.SmartPartName = "CustomerSummary";
			this.smartPartPlaceholder1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.smartPartPlaceholder1.Location = new System.Drawing.Point(3, 3);
			this.smartPartPlaceholder1.Name = "smartPartPlaceholder1";
			this.smartPartPlaceholder1.Size = new System.Drawing.Size(450, 284);
			this.smartPartPlaceholder1.TabIndex = 0;
			this.smartPartPlaceholder1.Text = "smartPartPlaceholder1";
			// 
			// CustomerTabView
			// 
			this.Controls.Add(this.tabWorkspace1);
			this.Name = "CustomerTabView";
			this.Size = new System.Drawing.Size(464, 316);
			this.tabWorkspace1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Microsoft.Practices.CompositeUI.WinForms.TabWorkspace tabWorkspace1;
		private System.Windows.Forms.TabPage tabPage1;
		private CustomerDetailView customerDetailView1;
		private System.Windows.Forms.TabPage tabPage2;
		private Microsoft.Practices.CompositeUI.WinForms.SmartPartPlaceholder smartPartPlaceholder1;


	}
}
