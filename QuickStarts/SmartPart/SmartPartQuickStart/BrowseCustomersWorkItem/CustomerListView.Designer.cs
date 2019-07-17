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
	partial class CustomerListView
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
			this.customerListBox = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// customerListBox
			// 
			this.customerListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.customerListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.customerListBox.FormattingEnabled = true;
			this.customerListBox.Location = new System.Drawing.Point(0, 23);
			this.customerListBox.Name = "customerListBox";
			this.customerListBox.Size = new System.Drawing.Size(240, 403);
			this.customerListBox.TabIndex = 1;
			// 
			// CustomerListView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.customerListBox);
			this.Name = "CustomerListView";
			this.Size = new System.Drawing.Size(240, 433);
			this.Title = "Customers";
			this.Controls.SetChildIndex(this.customerListBox, 0);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox customerListBox;
	}
}
