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
	partial class CustomerDetailView
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
			System.Windows.Forms.Label lastNameLabel;
			System.Windows.Forms.Label idLabel;
			System.Windows.Forms.Label fullNameLabel;
			System.Windows.Forms.Label firstNameLabel;
			System.Windows.Forms.Label addressLabel;
			this.customerBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.lastNameTextBox = new System.Windows.Forms.TextBox();
			this.idTextBox = new System.Windows.Forms.TextBox();
			this.fullNameTextBox = new System.Windows.Forms.TextBox();
			this.firstNameTextBox = new System.Windows.Forms.TextBox();
			this.addressTextBox = new System.Windows.Forms.TextBox();
			this.commentsButton = new System.Windows.Forms.Button();
			lastNameLabel = new System.Windows.Forms.Label();
			idLabel = new System.Windows.Forms.Label();
			fullNameLabel = new System.Windows.Forms.Label();
			firstNameLabel = new System.Windows.Forms.Label();
			addressLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// lastNameLabel
			// 
			lastNameLabel.AutoSize = true;
			lastNameLabel.Location = new System.Drawing.Point(27, 154);
			lastNameLabel.Name = "lastNameLabel";
			lastNameLabel.Size = new System.Drawing.Size(57, 13);
			lastNameLabel.TabIndex = 10;
			lastNameLabel.Text = "Last Name:";
			// 
			// idLabel
			// 
			idLabel.AutoSize = true;
			idLabel.Location = new System.Drawing.Point(27, 127);
			idLabel.Name = "idLabel";
			idLabel.Size = new System.Drawing.Size(15, 13);
			idLabel.TabIndex = 8;
			idLabel.Text = "Id:";
			// 
			// fullNameLabel
			// 
			fullNameLabel.AutoSize = true;
			fullNameLabel.Location = new System.Drawing.Point(27, 100);
			fullNameLabel.Name = "fullNameLabel";
			fullNameLabel.Size = new System.Drawing.Size(53, 13);
			fullNameLabel.TabIndex = 6;
			fullNameLabel.Text = "Full Name:";
			// 
			// firstNameLabel
			// 
			firstNameLabel.AutoSize = true;
			firstNameLabel.Location = new System.Drawing.Point(27, 73);
			firstNameLabel.Name = "firstNameLabel";
			firstNameLabel.Size = new System.Drawing.Size(56, 13);
			firstNameLabel.TabIndex = 4;
			firstNameLabel.Text = "First Name:";
			// 
			// addressLabel
			// 
			addressLabel.AutoSize = true;
			addressLabel.Location = new System.Drawing.Point(27, 46);
			addressLabel.Name = "addressLabel";
			addressLabel.Size = new System.Drawing.Size(44, 13);
			addressLabel.TabIndex = 2;
			addressLabel.Text = "Address:";
			// 
			// customerBindingSource
			// 
			this.customerBindingSource.DataSource = typeof(SmartPartQuickStart.Customer);
			// 
			// lastNameTextBox
			// 
			this.lastNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "LastName", true));
			this.lastNameTextBox.Location = new System.Drawing.Point(91, 151);
			this.lastNameTextBox.Name = "lastNameTextBox";
			this.lastNameTextBox.Size = new System.Drawing.Size(100, 20);
			this.lastNameTextBox.TabIndex = 11;
			// 
			// idTextBox
			// 
			this.idTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "Id", true));
			this.idTextBox.Location = new System.Drawing.Point(91, 124);
			this.idTextBox.Name = "idTextBox";
			this.idTextBox.Size = new System.Drawing.Size(100, 20);
			this.idTextBox.TabIndex = 9;
			// 
			// fullNameTextBox
			// 
			this.fullNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "FullName", true));
			this.fullNameTextBox.Location = new System.Drawing.Point(91, 97);
			this.fullNameTextBox.Name = "fullNameTextBox";
			this.fullNameTextBox.Size = new System.Drawing.Size(100, 20);
			this.fullNameTextBox.TabIndex = 7;
			// 
			// firstNameTextBox
			// 
			this.firstNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "FirstName", true));
			this.firstNameTextBox.Location = new System.Drawing.Point(91, 70);
			this.firstNameTextBox.Name = "firstNameTextBox";
			this.firstNameTextBox.Size = new System.Drawing.Size(100, 20);
			this.firstNameTextBox.TabIndex = 5;
			// 
			// addressTextBox
			// 
			this.addressTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "Address", true));
			this.addressTextBox.Location = new System.Drawing.Point(91, 43);
			this.addressTextBox.Name = "addressTextBox";
			this.addressTextBox.Size = new System.Drawing.Size(100, 20);
			this.addressTextBox.TabIndex = 3;
			// 
			// commentsButton
			// 
			this.commentsButton.Location = new System.Drawing.Point(18, 203);
			this.commentsButton.Name = "commentsButton";
			this.commentsButton.Size = new System.Drawing.Size(75, 23);
			this.commentsButton.TabIndex = 12;
			this.commentsButton.Text = "Comments";
			this.commentsButton.Click += new System.EventHandler(this.commentsButton_Click);
			// 
			// CustomerDetailView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.commentsButton);
			this.Controls.Add(this.lastNameTextBox);
			this.Controls.Add(lastNameLabel);
			this.Controls.Add(addressLabel);
			this.Controls.Add(this.addressTextBox);
			this.Controls.Add(firstNameLabel);
			this.Controls.Add(this.firstNameTextBox);
			this.Controls.Add(fullNameLabel);
			this.Controls.Add(this.fullNameTextBox);
			this.Controls.Add(idLabel);
			this.Controls.Add(this.idTextBox);
			this.Name = "CustomerDetailView";
			this.Size = new System.Drawing.Size(303, 229);
			this.Title = "Customer Details";
			this.Controls.SetChildIndex(this.idTextBox, 0);
			this.Controls.SetChildIndex(idLabel, 0);
			this.Controls.SetChildIndex(this.fullNameTextBox, 0);
			this.Controls.SetChildIndex(fullNameLabel, 0);
			this.Controls.SetChildIndex(this.firstNameTextBox, 0);
			this.Controls.SetChildIndex(firstNameLabel, 0);
			this.Controls.SetChildIndex(this.addressTextBox, 0);
			this.Controls.SetChildIndex(addressLabel, 0);
			this.Controls.SetChildIndex(lastNameLabel, 0);
			this.Controls.SetChildIndex(this.lastNameTextBox, 0);
			this.Controls.SetChildIndex(this.commentsButton, 0);
			((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.BindingSource customerBindingSource;
		private System.Windows.Forms.TextBox lastNameTextBox;
		private System.Windows.Forms.TextBox idTextBox;
		private System.Windows.Forms.TextBox fullNameTextBox;
		private System.Windows.Forms.TextBox firstNameTextBox;
		private System.Windows.Forms.TextBox addressTextBox;
		private System.Windows.Forms.Button commentsButton;

	}
}
