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
	partial class LaunchPadForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LaunchPadForm));
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnNewCustomerList = new System.Windows.Forms.Button();
			this.btnFireCustomerChange = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtCustomerId = new System.Windows.Forms.TextBox();
			this.lstGlobalCustomers = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.traceTextBox1 = new EventBrokerDemo.TraceTextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// textBox2
			// 
			this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox2.BackColor = System.Drawing.SystemColors.Control;
			this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox2.Location = new System.Drawing.Point(6, 19);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(308, 60);
			this.textBox2.TabIndex = 15;
			this.textBox2.Text = resources.GetString("textBox2.Text");
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.BackColor = System.Drawing.SystemColors.Control;
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Location = new System.Drawing.Point(11, 17);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(308, 45);
			this.textBox1.TabIndex = 14;
			this.textBox1.Text = "Enter a Customer ID here and click the Add Customer button. This will add a new c" +
				 "ustomer object to the State and fire a global Event notifying subscribers who ar" +
				 "e listening for that topic.";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.textBox2);
			this.groupBox2.Controls.Add(this.btnNewCustomerList);
			this.groupBox2.Location = new System.Drawing.Point(12, 157);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(325, 122);
			this.groupBox2.TabIndex = 19;
			this.groupBox2.TabStop = false;
			// 
			// btnNewCustomerList
			// 
			this.btnNewCustomerList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNewCustomerList.Location = new System.Drawing.Point(200, 85);
			this.btnNewCustomerList.Name = "btnNewCustomerList";
			this.btnNewCustomerList.Size = new System.Drawing.Size(115, 23);
			this.btnNewCustomerList.TabIndex = 11;
			this.btnNewCustomerList.Text = "Show List";
			this.btnNewCustomerList.Click += new System.EventHandler(this.btnNewCustomerList_Click);
			// 
			// btnFireCustomerChange
			// 
			this.btnFireCustomerChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFireCustomerChange.Location = new System.Drawing.Point(204, 71);
			this.btnFireCustomerChange.Name = "btnFireCustomerChange";
			this.btnFireCustomerChange.Size = new System.Drawing.Size(115, 23);
			this.btnFireCustomerChange.TabIndex = 13;
			this.btnFireCustomerChange.Text = "Add Customer";
			this.btnFireCustomerChange.Click += new System.EventHandler(this.btnFireCustomerChange_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 76);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 13);
			this.label2.TabIndex = 12;
			this.label2.Text = "Customer ID:";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Controls.Add(this.btnFireCustomerChange);
			this.groupBox1.Controls.Add(this.txtCustomerId);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(12, 28);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(325, 104);
			this.groupBox1.TabIndex = 18;
			this.groupBox1.TabStop = false;
			// 
			// txtCustomerId
			// 
			this.txtCustomerId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCustomerId.Location = new System.Drawing.Point(80, 73);
			this.txtCustomerId.Name = "txtCustomerId";
			this.txtCustomerId.Size = new System.Drawing.Size(119, 20);
			this.txtCustomerId.TabIndex = 10;
			// 
			// lstGlobalCustomers
			// 
			this.lstGlobalCustomers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lstGlobalCustomers.FormattingEnabled = true;
			this.lstGlobalCustomers.Location = new System.Drawing.Point(343, 28);
			this.lstGlobalCustomers.Name = "lstGlobalCustomers";
			this.lstGlobalCustomers.Size = new System.Drawing.Size(178, 251);
			this.lstGlobalCustomers.TabIndex = 21;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(342, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 13);
			this.label1.TabIndex = 22;
			this.label1.Text = "Customer List";
			// 
			// traceTextBox1
			// 
			this.traceTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.traceTextBox1.Location = new System.Drawing.Point(12, 294);
			this.traceTextBox1.Name = "traceTextBox1";
			this.traceTextBox1.Size = new System.Drawing.Size(509, 108);
			this.traceTextBox1.TabIndex = 25;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(9, 141);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 13);
			this.label3.TabIndex = 26;
			this.label3.Text = "Step 2";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(38, 13);
			this.label4.TabIndex = 27;
			this.label4.Text = "Step 1";
			// 
			// LaunchPadForm
			// 
			this.ClientSize = new System.Drawing.Size(533, 413);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.traceTextBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lstGlobalCustomers);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "LaunchPadForm";
			this.Text = "LaunchPadForm";
			this.Load += new System.EventHandler(this.LaunchPadForm_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private TextBox textBox2;
		private TextBox textBox1;
		private GroupBox groupBox2;
		private Button btnNewCustomerList;
		private Button btnFireCustomerChange;
		private Label label2;
		private GroupBox groupBox1;
		private TextBox txtCustomerId;
		private ListBox lstGlobalCustomers;
		private Label label1;
		private TraceTextBox traceTextBox1;
		private Label label3;
		private Label label4;
	}
}