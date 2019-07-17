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
	partial class CustomerAccountsView
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
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.accountNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.accountTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.currentBalanceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CustomerAccountBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.toolCommands = new System.Windows.Forms.ToolStrip();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.CustomerAccountBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.Columns.Add(this.accountNumberDataGridViewTextBoxColumn);
			this.dataGridView1.Columns.Add(this.accountTypeDataGridViewTextBoxColumn);
			this.dataGridView1.Columns.Add(this.currentBalanceDataGridViewTextBoxColumn);
			this.dataGridView1.DataSource = this.CustomerAccountBindingSource;
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.Location = new System.Drawing.Point(0, 25);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.Size = new System.Drawing.Size(445, 246);
			this.dataGridView1.TabIndex = 0;
			this.dataGridView1.Text = "dataGridView1";
			// 
			// accountNumberDataGridViewTextBoxColumn
			// 
			this.accountNumberDataGridViewTextBoxColumn.DataPropertyName = "AccountNumber";
			this.accountNumberDataGridViewTextBoxColumn.HeaderText = "Account Number";
			this.accountNumberDataGridViewTextBoxColumn.Name = "AccountNumber";
			this.accountNumberDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// accountTypeDataGridViewTextBoxColumn
			// 
			this.accountTypeDataGridViewTextBoxColumn.DataPropertyName = "AccountType";
			this.accountTypeDataGridViewTextBoxColumn.HeaderText = "Account Type";
			this.accountTypeDataGridViewTextBoxColumn.Name = "AccountType";
			this.accountTypeDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// currentBalanceDataGridViewTextBoxColumn
			// 
			this.currentBalanceDataGridViewTextBoxColumn.DataPropertyName = "CurrentBalance";
			this.currentBalanceDataGridViewTextBoxColumn.HeaderText = "Current Balance";
			this.currentBalanceDataGridViewTextBoxColumn.Name = "CurrentBalance";
			this.currentBalanceDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// CustomerAccountBindingSource
			// 
			this.CustomerAccountBindingSource.DataSource = typeof(BankTellerCommon.CustomerAccount);
			// 
			// toolCommands
			// 
			this.toolCommands.Location = new System.Drawing.Point(0, 0);
			this.toolCommands.Name = "toolCommands";
			this.toolCommands.Size = new System.Drawing.Size(445, 25);
			this.toolCommands.TabIndex = 1;
			// 
			// CustomerAccountsView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.toolCommands);
			this.Name = "CustomerAccountsView";
			this.Size = new System.Drawing.Size(445, 271);
			this.Load += new System.EventHandler(this.CustomerAccountsView_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.CustomerAccountBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridViewTextBoxColumn accountNumberDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn accountTypeDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn currentBalanceDataGridViewTextBoxColumn;
		private System.Windows.Forms.BindingSource CustomerAccountBindingSource;
		private System.Windows.Forms.ToolStrip toolCommands;



	}
}
