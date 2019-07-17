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

namespace SmartPartQuickStart.BrowseCustomersWorkItem
{
	partial class CustomerMain
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.customerListView1 = new SmartPartQuickStart.BrowseCustomersWorkItem.CustomerListView();
			this.customersDeckedWorkspace = new Microsoft.Practices.CompositeUI.WinForms.DeckWorkspace();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.customerListView1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.customersDeckedWorkspace);
			this.splitContainer1.Size = new System.Drawing.Size(655, 529);
			this.splitContainer1.SplitterDistance = 219;
			this.splitContainer1.TabIndex = 1;
			this.splitContainer1.Text = "splitContainer1";
			// 
			// customerListView1
			// 
			this.customerListView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.customerListView1.Description = "";
			this.customerListView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.customerListView1.Location = new System.Drawing.Point(0, 0);
			this.customerListView1.Name = "customerListView1";
			this.customerListView1.Size = new System.Drawing.Size(219, 529);
			this.customerListView1.TabIndex = 0;
			this.customerListView1.Title = "Customers";
			// 
			// customersDeckedWorkspace
			// 
			this.customersDeckedWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
			this.customersDeckedWorkspace.Location = new System.Drawing.Point(0, 0);
			this.customersDeckedWorkspace.Name = "customersDeckedWorkspace";
			this.customersDeckedWorkspace.Size = new System.Drawing.Size(432, 529);
			this.customersDeckedWorkspace.TabIndex = 0;
			// 
			// CustomerMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Name = "CustomerMain";
			this.Size = new System.Drawing.Size(655, 529);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private CustomerListView customerListView1;
		internal Microsoft.Practices.CompositeUI.WinForms.DeckWorkspace customersDeckedWorkspace;
	}
}
