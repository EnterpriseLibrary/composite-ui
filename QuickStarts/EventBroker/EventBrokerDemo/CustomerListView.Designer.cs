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

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EventBrokerDemo
{
	partial class CustomerListView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private IContainer components = null;

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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.lstGlobalCustomers = new System.Windows.Forms.ListBox();
            this.btnAddGlobalCustomer = new System.Windows.Forms.Button();
            this.txtCustomerId = new System.Windows.Forms.TextBox();
            this.btnAddLocalCustomer = new System.Windows.Forms.Button();
            this.lstLocalCustomers = new System.Windows.Forms.ListBox();
            this.btnProcessLocal = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancelProcess = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstGlobalCustomers
            // 
            this.lstGlobalCustomers.FormattingEnabled = true;
            this.lstGlobalCustomers.Location = new System.Drawing.Point(12, 29);
            this.lstGlobalCustomers.Name = "lstGlobalCustomers";
            this.lstGlobalCustomers.Size = new System.Drawing.Size(268, 108);
            this.lstGlobalCustomers.TabIndex = 0;
            // 
            // btnAddGlobalCustomer
            // 
            this.btnAddGlobalCustomer.Location = new System.Drawing.Point(205, 258);
            this.btnAddGlobalCustomer.Name = "btnAddGlobalCustomer";
            this.btnAddGlobalCustomer.Size = new System.Drawing.Size(75, 23);
            this.btnAddGlobalCustomer.TabIndex = 1;
            this.btnAddGlobalCustomer.Text = "Add Global";
            this.btnAddGlobalCustomer.Click += new System.EventHandler(this.btnAddGlobalCustomer_Click);
            // 
            // txtCustomerId
            // 
            this.txtCustomerId.Location = new System.Drawing.Point(12, 260);
            this.txtCustomerId.Name = "txtCustomerId";
            this.txtCustomerId.Size = new System.Drawing.Size(101, 20);
            this.txtCustomerId.TabIndex = 2;
            // 
            // btnAddLocalCustomer
            // 
            this.btnAddLocalCustomer.Location = new System.Drawing.Point(124, 258);
            this.btnAddLocalCustomer.Name = "btnAddLocalCustomer";
            this.btnAddLocalCustomer.Size = new System.Drawing.Size(75, 23);
            this.btnAddLocalCustomer.TabIndex = 3;
            this.btnAddLocalCustomer.Text = "Add Local";
            this.btnAddLocalCustomer.Click += new System.EventHandler(this.btnAddLocalCustomer_Click);
            // 
            // lstLocalCustomers
            // 
            this.lstLocalCustomers.FormattingEnabled = true;
            this.lstLocalCustomers.Location = new System.Drawing.Point(12, 170);
            this.lstLocalCustomers.Name = "lstLocalCustomers";
            this.lstLocalCustomers.Size = new System.Drawing.Size(268, 82);
            this.lstLocalCustomers.TabIndex = 4;
            // 
            // btnProcessLocal
            // 
            this.btnProcessLocal.Location = new System.Drawing.Point(13, 329);
            this.btnProcessLocal.Name = "btnProcessLocal";
            this.btnProcessLocal.Size = new System.Drawing.Size(156, 23);
            this.btnProcessLocal.TabIndex = 5;
            this.btnProcessLocal.Text = "Process Local";
            this.btnProcessLocal.Click += new System.EventHandler(this.btnProcessLocal_Click);
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(12, 290);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(268, 33);
            this.textBox3.TabIndex = 17;
            this.textBox3.Text = "This button starts a background worker that moves the local customers to the glob" +
                "al customer list.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Global customers";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Local customers";
            // 
            // btnCancelProcess
            // 
            this.btnCancelProcess.Enabled = false;
            this.btnCancelProcess.Location = new System.Drawing.Point(205, 329);
            this.btnCancelProcess.Name = "btnCancelProcess";
            this.btnCancelProcess.Size = new System.Drawing.Size(75, 23);
            this.btnCancelProcess.TabIndex = 20;
            this.btnCancelProcess.Text = "Cancel";
            this.btnCancelProcess.Click += new System.EventHandler(this.btnCancelProcess_Click);
            // 
            // CustomerListView
            // 
            this.ClientSize = new System.Drawing.Size(292, 364);
            this.Controls.Add(this.btnCancelProcess);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.btnProcessLocal);
            this.Controls.Add(this.lstLocalCustomers);
            this.Controls.Add(this.btnAddLocalCustomer);
            this.Controls.Add(this.txtCustomerId);
            this.Controls.Add(this.btnAddGlobalCustomer);
            this.Controls.Add(this.lstGlobalCustomers);
            this.Name = "CustomerListView";
            this.Text = "CustomerListView";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private ListBox lstGlobalCustomers;
		private Button btnAddGlobalCustomer;
		private TextBox txtCustomerId;
		private Button btnAddLocalCustomer;
		private ListBox lstLocalCustomers;
		private Button btnProcessLocal;
		private TextBox textBox3;
		private Label label1;
		private Label label2;
		private Button btnCancelProcess;
	}
}