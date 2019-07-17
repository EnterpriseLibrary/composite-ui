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

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EventBrokerDemo
{
	partial class ProgressView
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
			this.progressWork = new ProgressBar();
			this.SuspendLayout();
			// 
			// progressWork
			// 
			this.progressWork.Anchor = ((AnchorStyles) (((AnchorStyles.Top | AnchorStyles.Left)
			                                             | AnchorStyles.Right)));
			this.progressWork.Location = new Point(12, 12);
			this.progressWork.Name = "progressWork";
			this.progressWork.Size = new Size(444, 23);
			this.progressWork.TabIndex = 0;
			// 
			// ProgressView
			// 
			this.AutoScaleDimensions = new SizeF(6F, 13F);
			this.ClientSize = new Size(468, 47);
			this.Controls.Add(this.progressWork);
			this.Name = "ProgressView";
			this.Text = "ProgressView";
			this.ResumeLayout(false);

		}

		#endregion

		private ProgressBar progressWork;
	}
}