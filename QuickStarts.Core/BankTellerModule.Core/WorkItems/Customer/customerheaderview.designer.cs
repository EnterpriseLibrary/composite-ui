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
	partial class CustomerHeaderView
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
			this.lblFirstName = new System.Windows.Forms.Label();
			this.lblLastName = new System.Windows.Forms.Label();
			this.lblCustomerID = new System.Windows.Forms.Label();
			this.txtCustomerID = new System.Windows.Forms.TextBox();
			this.txtFirstName = new System.Windows.Forms.TextBox();
			this.txtLastName = new System.Windows.Forms.TextBox();
			this.customerBindingSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// lblFirstName
			// 
			this.lblFirstName.AutoSize = true;
			this.lblFirstName.Location = new System.Drawing.Point(3, 35);
			this.lblFirstName.Name = "lblFirstName";
			this.lblFirstName.Size = new System.Drawing.Size(56, 13);
			this.lblFirstName.TabIndex = 0;
			this.lblFirstName.Text = "First Name:";
			// 
			// lblLastName
			// 
			this.lblLastName.AutoSize = true;
			this.lblLastName.Location = new System.Drawing.Point(3, 62);
			this.lblLastName.Name = "lblLastName";
			this.lblLastName.Size = new System.Drawing.Size(57, 13);
			this.lblLastName.TabIndex = 1;
			this.lblLastName.Text = "Last Name:";
			// 
			// lblCustomerID
			// 
			this.lblCustomerID.AutoSize = true;
			this.lblCustomerID.Location = new System.Drawing.Point(3, 9);
			this.lblCustomerID.Name = "lblCustomerID";
			this.lblCustomerID.Size = new System.Drawing.Size(64, 13);
			this.lblCustomerID.TabIndex = 2;
			this.lblCustomerID.Text = "Customer ID:";
			// 
			// txtCustomerID
			// 
			this.txtCustomerID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCustomerID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "ID", true));
			this.txtCustomerID.Location = new System.Drawing.Point(73, 6);
			this.txtCustomerID.Name = "txtCustomerID";
			this.txtCustomerID.ReadOnly = true;
			this.txtCustomerID.Size = new System.Drawing.Size(176, 20);
			this.txtCustomerID.TabIndex = 3;
			// 
			// txtFirstName
			// 
			this.txtFirstName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFirstName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "FirstName", true));
			this.txtFirstName.Location = new System.Drawing.Point(73, 32);
			this.txtFirstName.Name = "txtFirstName";
			this.txtFirstName.ReadOnly = true;
			this.txtFirstName.Size = new System.Drawing.Size(176, 20);
			this.txtFirstName.TabIndex = 4;
			// 
			// txtLastName
			// 
			this.txtLastName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtLastName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "LastName", true));
			this.txtLastName.Location = new System.Drawing.Point(73, 59);
			this.txtLastName.Name = "txtLastName";
			this.txtLastName.ReadOnly = true;
			this.txtLastName.Size = new System.Drawing.Size(176, 20);
			this.txtLastName.TabIndex = 5;
			// 
			// customerBindingSource
			// 
			this.customerBindingSource.DataSource = typeof(BankTellerCommon.Customer);
			// 
			// CustomerHeaderView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtLastName);
			this.Controls.Add(this.txtFirstName);
			this.Controls.Add(this.txtCustomerID);
			this.Controls.Add(this.lblCustomerID);
			this.Controls.Add(this.lblLastName);
			this.Controls.Add(this.lblFirstName);
			this.Name = "CustomerHeaderView";
			this.Size = new System.Drawing.Size(256, 85);
			this.Load += new System.EventHandler(this.CustomerHeaderView_Load);
			((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblCustomerID;
		private System.Windows.Forms.Label lblFirstName;
		private System.Windows.Forms.Label lblLastName;
		private System.Windows.Forms.TextBox txtCustomerID;
		private System.Windows.Forms.TextBox txtFirstName;
		private System.Windows.Forms.TextBox txtLastName;
		private System.Windows.Forms.BindingSource customerBindingSource;
	}
}
