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
			this.summaryTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// summaryTextBox
			// 
			this.summaryTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.summaryTextBox.Location = new System.Drawing.Point(0, 23);
			this.summaryTextBox.Multiline = true;
			this.summaryTextBox.Name = "summaryTextBox";
			this.summaryTextBox.Size = new System.Drawing.Size(588, 316);
			this.summaryTextBox.TabIndex = 1;
			// 
			// CustomerSummaryView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.summaryTextBox);
			this.Name = "CustomerSummaryView";
			this.Title = "Summary";
			this.Controls.SetChildIndex(this.summaryTextBox, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox summaryTextBox;
	}
}
