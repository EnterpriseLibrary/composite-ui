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
	partial class CustomerQueueView
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnNextCustomer = new System.Windows.Forms.Button();
			this.listCustomers = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.btnNextCustomer);
			this.panel1.Controls.Add(this.listCustomers);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Location = new System.Drawing.Point(4, 4);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(5);
			this.panel1.Size = new System.Drawing.Size(182, 394);
			this.panel1.TabIndex = 0;
			// 
			// btnNextCustomer
			// 
			this.btnNextCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.btnNextCustomer.Location = new System.Drawing.Point(8, 28);
			this.btnNextCustomer.Name = "btnNextCustomer";
			this.btnNextCustomer.Size = new System.Drawing.Size(166, 26);
			this.btnNextCustomer.TabIndex = 5;
			this.btnNextCustomer.Text = "Accept Customer";
			this.btnNextCustomer.Click += new System.EventHandler(this.OnAcceptCustomer);
			// 
			// listCustomers
			// 
			this.listCustomers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.listCustomers.FormattingEnabled = true;
			this.listCustomers.IntegralHeight = false;
			this.listCustomers.Location = new System.Drawing.Point(8, 61);
			this.listCustomers.Name = "listCustomers";
			this.listCustomers.Size = new System.Drawing.Size(166, 321);
			this.listCustomers.TabIndex = 4;
			this.listCustomers.ValueMember = "Count";
			this.listCustomers.SelectedIndexChanged += new System.EventHandler(this.OnCustomerSelectionChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(122, 20);
			this.label1.TabIndex = 3;
			this.label1.Text = "My Customers";
			// 
			// CustomerQueueView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Name = "CustomerQueueView";
			this.Size = new System.Drawing.Size(189, 401);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnNextCustomer;
		private System.Windows.Forms.ListBox listCustomers;
		private System.Windows.Forms.Label label1;

	}
}
