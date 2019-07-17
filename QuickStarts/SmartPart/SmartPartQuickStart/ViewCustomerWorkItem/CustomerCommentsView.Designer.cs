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

using Microsoft.Practices.CompositeUI.SmartParts;
namespace SmartPartQuickStart.ViewCustomerWorkItem
{
	partial class CustomerCommentsView
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
			this.commentsTextBox = new System.Windows.Forms.TextBox();
			this.customerBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.smartPartInfo1 = new SmartPartInfo();
			((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// commentsTextBox
			// 
			this.commentsTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerBindingSource, "Comments", true));
			this.commentsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.commentsTextBox.Location = new System.Drawing.Point(0, 23);
			this.commentsTextBox.Multiline = true;
			this.commentsTextBox.Name = "commentsTextBox";
			this.commentsTextBox.Size = new System.Drawing.Size(514, 237);
			this.commentsTextBox.TabIndex = 0;
			// 
			// customerBindingSource
			// 
			this.customerBindingSource.DataSource = typeof(SmartPartQuickStart.Customer);
			// 
			// smartPartInfo1
			// 
			this.smartPartInfo1.Description = null;
			this.smartPartInfo1.Title = "Comments";
			// 
			// CustomerCommentsView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.commentsTextBox);
			this.Name = "CustomerCommentsView";
			this.Size = new System.Drawing.Size(514, 260);
			this.Title = "Comments";
			this.Controls.SetChildIndex(this.commentsTextBox, 0);
			((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox commentsTextBox;
		private System.Windows.Forms.BindingSource customerBindingSource;
		private SmartPartInfo smartPartInfo1;
	}
}
