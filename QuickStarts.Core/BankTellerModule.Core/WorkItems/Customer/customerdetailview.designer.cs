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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerDetailView));
			this.customerBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.txtAddress1 = new System.Windows.Forms.TextBox();
			this.txtAddress2 = new System.Windows.Forms.TextBox();
			this.txtCity = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtState = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.txtZip = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.txtPhone1 = new System.Windows.Forms.TextBox();
			this.txtPhone2 = new System.Windows.Forms.TextBox();
			this.txtEmail = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.detailsCommands = new System.Windows.Forms.ToolStrip();
			this.showCommentsButton = new System.Windows.Forms.ToolStripButton();
			this.label1 = new System.Windows.Forms.Label();
			this.lastNameTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.firstNameTextBox = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).BeginInit();
			this.detailsCommands.SuspendLayout();
			this.SuspendLayout();
			// 
			// customerBindingSource
			// 
			this.customerBindingSource.DataSource = typeof(BankTellerCommon.Customer);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.label4.BackColor = System.Drawing.Color.LightBlue;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(4, 4);
			this.label4.Name = "label4";
			this.label4.Padding = new System.Windows.Forms.Padding(2);
			this.label4.Size = new System.Drawing.Size(438, 17);
			this.label4.TabIndex = 5;
			this.label4.Text = "Address";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(14, 60);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 13);
			this.label5.TabIndex = 6;
			this.label5.Text = "Address:";
			// 
			// txtAddress1
			// 
			this.txtAddress1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "Address1", true));
			this.txtAddress1.Location = new System.Drawing.Point(69, 57);
			this.txtAddress1.Name = "txtAddress1";
			this.txtAddress1.Size = new System.Drawing.Size(370, 20);
			this.txtAddress1.TabIndex = 7;
			// 
			// txtAddress2
			// 
			this.txtAddress2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "Address2", true));
			this.txtAddress2.Location = new System.Drawing.Point(69, 83);
			this.txtAddress2.Name = "txtAddress2";
			this.txtAddress2.Size = new System.Drawing.Size(370, 20);
			this.txtAddress2.TabIndex = 8;
			// 
			// txtCity
			// 
			this.txtCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "City", true));
			this.txtCity.Location = new System.Drawing.Point(68, 110);
			this.txtCity.Name = "txtCity";
			this.txtCity.Size = new System.Drawing.Size(149, 20);
			this.txtCity.TabIndex = 9;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(35, 113);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(27, 13);
			this.label6.TabIndex = 10;
			this.label6.Text = "City:";
			// 
			// txtState
			// 
			this.txtState.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "State", true));
			this.txtState.Location = new System.Drawing.Point(277, 110);
			this.txtState.Name = "txtState";
			this.txtState.Size = new System.Drawing.Size(62, 20);
			this.txtState.TabIndex = 11;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(240, 112);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(35, 13);
			this.label7.TabIndex = 12;
			this.label7.Text = "State:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(345, 113);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(25, 13);
			this.label8.TabIndex = 13;
			this.label8.Text = "Zip:";
			// 
			// txtZip
			// 
			this.txtZip.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "ZipCode", true));
			this.txtZip.Location = new System.Drawing.Point(372, 109);
			this.txtZip.Name = "txtZip";
			this.txtZip.Size = new System.Drawing.Size(67, 20);
			this.txtZip.TabIndex = 14;
			// 
			// label9
			// 
			this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.label9.BackColor = System.Drawing.Color.LightBlue;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(3, 142);
			this.label9.Name = "label9";
			this.label9.Padding = new System.Windows.Forms.Padding(2);
			this.label9.Size = new System.Drawing.Size(438, 17);
			this.label9.TabIndex = 15;
			this.label9.Text = "Contact";
			// 
			// txtPhone1
			// 
			this.txtPhone1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "Phone1", true));
			this.txtPhone1.Location = new System.Drawing.Point(69, 163);
			this.txtPhone1.Name = "txtPhone1";
			this.txtPhone1.Size = new System.Drawing.Size(148, 20);
			this.txtPhone1.TabIndex = 16;
			// 
			// txtPhone2
			// 
			this.txtPhone2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "Phone2", true));
			this.txtPhone2.Location = new System.Drawing.Point(286, 163);
			this.txtPhone2.Name = "txtPhone2";
			this.txtPhone2.Size = new System.Drawing.Size(153, 20);
			this.txtPhone2.TabIndex = 17;
			// 
			// txtEmail
			// 
			this.txtEmail.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "EmailAddress", true));
			this.txtEmail.Location = new System.Drawing.Point(69, 189);
			this.txtEmail.Name = "txtEmail";
			this.txtEmail.Size = new System.Drawing.Size(370, 20);
			this.txtEmail.TabIndex = 18;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(12, 166);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(50, 13);
			this.label10.TabIndex = 19;
			this.label10.Text = "Phone 1:";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(234, 166);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(50, 13);
			this.label11.TabIndex = 20;
			this.label11.Text = "Phone 2:";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(24, 192);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(38, 13);
			this.label12.TabIndex = 21;
			this.label12.Text = "E-mail:";
			// 
			// detailsCommands
			// 
			this.detailsCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showCommentsButton});
			this.detailsCommands.Location = new System.Drawing.Point(0, 0);
			this.detailsCommands.Name = "detailsCommands";
			this.detailsCommands.Size = new System.Drawing.Size(445, 25);
			this.detailsCommands.TabIndex = 23;
			this.detailsCommands.Text = "toolStrip1";
			// 
			// showCommentsButton
			// 
			this.showCommentsButton.Image = ((System.Drawing.Image)(resources.GetObject("showCommentsButton.Image")));
			this.showCommentsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.showCommentsButton.Name = "showCommentsButton";
			this.showCommentsButton.Size = new System.Drawing.Size(77, 22);
			this.showCommentsButton.Text = "Comments";
			this.showCommentsButton.Click += new System.EventHandler(this.OnShowComments);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(223, 33);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(61, 13);
			this.label1.TabIndex = 27;
			this.label1.Text = "Last Name:";
			// 
			// lastNameTextBox
			// 
			this.lastNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "LastName", true));
			this.lastNameTextBox.Location = new System.Drawing.Point(290, 31);
			this.lastNameTextBox.Name = "lastNameTextBox";
			this.lastNameTextBox.Size = new System.Drawing.Size(149, 20);
			this.lastNameTextBox.TabIndex = 26;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 33);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 13);
			this.label2.TabIndex = 25;
			this.label2.Text = "First Name:";
			// 
			// firstNameTextBox
			// 
			this.firstNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "FirstName", true));
			this.firstNameTextBox.Location = new System.Drawing.Point(69, 31);
			this.firstNameTextBox.Name = "firstNameTextBox";
			this.firstNameTextBox.Size = new System.Drawing.Size(153, 20);
			this.firstNameTextBox.TabIndex = 24;
			// 
			// CustomerDetailView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lastNameTextBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.firstNameTextBox);
			this.Controls.Add(this.detailsCommands);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.txtEmail);
			this.Controls.Add(this.txtPhone2);
			this.Controls.Add(this.txtPhone1);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.txtZip);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.txtState);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtCity);
			this.Controls.Add(this.txtAddress2);
			this.Controls.Add(this.txtAddress1);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Name = "CustomerDetailView";
			this.Size = new System.Drawing.Size(445, 223);
			((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).EndInit();
			this.detailsCommands.ResumeLayout(false);
			this.detailsCommands.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtAddress1;
		private System.Windows.Forms.TextBox txtAddress2;
		private System.Windows.Forms.TextBox txtCity;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtState;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtZip;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox txtPhone1;
		private System.Windows.Forms.TextBox txtPhone2;
		private System.Windows.Forms.TextBox txtEmail;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.BindingSource customerBindingSource;
		private System.Windows.Forms.ToolStrip detailsCommands;
		private System.Windows.Forms.ToolStripButton showCommentsButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox lastNameTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox firstNameTextBox;




	}
}
