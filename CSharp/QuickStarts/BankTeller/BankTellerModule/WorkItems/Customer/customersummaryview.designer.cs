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

namespace BankTellerModule
{
	partial class CustomerSummaryView
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
			this.components = new System.ComponentModel.Container();
			this.tabbedWorkspace1 = new Microsoft.Practices.CompositeUI.WinForms.TabWorkspace();
			this.tabSummary = new System.Windows.Forms.TabPage();
			this.tabAccounts = new System.Windows.Forms.TabPage();
			this.SaveButton = new System.Windows.Forms.Button();
			this.customerContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.customerDetailView1 = new BankTellerModule.CustomerDetailView();
			this.customerAccountsView1 = new BankTellerModule.CustomerAccountsView();
			this.customerHeaderView1 = new BankTellerModule.CustomerHeaderView();
			this.tabbedWorkspace1.SuspendLayout();
			this.tabSummary.SuspendLayout();
			this.tabAccounts.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabbedWorkspace1
			// 
			this.tabbedWorkspace1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabbedWorkspace1.Controls.Add(this.tabSummary);
			this.tabbedWorkspace1.Controls.Add(this.tabAccounts);
			this.tabbedWorkspace1.Location = new System.Drawing.Point(4, 96);
			this.tabbedWorkspace1.Name = "tabbedWorkspace1";
			this.tabbedWorkspace1.SelectedIndex = 0;
			this.tabbedWorkspace1.Size = new System.Drawing.Size(469, 282);
			this.tabbedWorkspace1.TabIndex = 1;
			// 
			// tabSummary
			// 
			this.tabSummary.AutoScroll = true;
			this.tabSummary.BackColor = System.Drawing.Color.White;
			this.tabSummary.Controls.Add(this.customerDetailView1);
			this.tabSummary.Location = new System.Drawing.Point(4, 22);
			this.tabSummary.Name = "tabSummary";
			this.tabSummary.Padding = new System.Windows.Forms.Padding(5);
			this.tabSummary.Size = new System.Drawing.Size(461, 256);
			this.tabSummary.TabIndex = 0;
			this.tabSummary.Text = "Summary";
			// 
			// tabAccounts
			// 
			this.tabAccounts.BackColor = System.Drawing.Color.White;
			this.tabAccounts.Controls.Add(this.customerAccountsView1);
			this.tabAccounts.Location = new System.Drawing.Point(4, 22);
			this.tabAccounts.Name = "tabAccounts";
			this.tabAccounts.Padding = new System.Windows.Forms.Padding(5);
			this.tabAccounts.Size = new System.Drawing.Size(461, 256);
			this.tabAccounts.TabIndex = 1;
			this.tabAccounts.Text = "Accounts";
			// 
			// SaveButton
			// 
			this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.SaveButton.Location = new System.Drawing.Point(396, 383);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(75, 23);
			this.SaveButton.TabIndex = 2;
			this.SaveButton.Text = "Save";
			this.SaveButton.Click += new System.EventHandler(this.OnSave);
			// 
			// customerContextMenu
			// 
			this.customerContextMenu.Name = "customerContextMenu";
			this.customerContextMenu.Size = new System.Drawing.Size(61, 4);
			// 
			// customerDetailView1
			// 
			this.customerDetailView1.AutoScroll = true;
			this.customerDetailView1.Location = new System.Drawing.Point(8, 8);
			this.customerDetailView1.MinimumSize = new System.Drawing.Size(445, 271);
			this.customerDetailView1.Name = "customerDetailView1";
			this.customerDetailView1.Size = new System.Drawing.Size(445, 271);
			this.customerDetailView1.TabIndex = 0;
			// 
			// customerAccountsView1
			// 
			this.customerAccountsView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.customerAccountsView1.Location = new System.Drawing.Point(5, 5);
			this.customerAccountsView1.Name = "customerAccountsView1";
			this.customerAccountsView1.Size = new System.Drawing.Size(451, 246);
			this.customerAccountsView1.TabIndex = 0;
			// 
			// customerHeaderView1
			// 
			this.customerHeaderView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.customerHeaderView1.Location = new System.Drawing.Point(4, 4);
			this.customerHeaderView1.Name = "customerHeaderView1";
			this.customerHeaderView1.Size = new System.Drawing.Size(465, 85);
			this.customerHeaderView1.TabIndex = 0;
			// 
			// CustomerSummaryView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ContextMenuStrip = this.customerContextMenu;
			this.Controls.Add(this.SaveButton);
			this.Controls.Add(this.tabbedWorkspace1);
			this.Controls.Add(this.customerHeaderView1);
			this.Name = "CustomerSummaryView";
			this.Size = new System.Drawing.Size(476, 412);
			this.tabbedWorkspace1.ResumeLayout(false);
			this.tabSummary.ResumeLayout(false);
			this.tabAccounts.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Microsoft.Practices.CompositeUI.WinForms.TabWorkspace tabbedWorkspace1;
		private System.Windows.Forms.TabPage tabSummary;
		private CustomerDetailView customerDetailView1;
		private System.Windows.Forms.TabPage tabAccounts;
		private CustomerAccountsView customerAccountsView1;
		private CustomerHeaderView customerHeaderView1;
		private System.Windows.Forms.Button SaveButton;
		private System.Windows.Forms.ContextMenuStrip customerContextMenu;
	}
}
