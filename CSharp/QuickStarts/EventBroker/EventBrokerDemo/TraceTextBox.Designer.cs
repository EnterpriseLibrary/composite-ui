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
	partial class TraceTextBox
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.logBox = new TextBox();
			this.panel1 = new Panel();
			this.enableLogging = new CheckBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// logBox
			// 
			this.logBox.Dock = DockStyle.Fill;
			this.logBox.Location = new Point(0, 25);
			this.logBox.Multiline = true;
			this.logBox.Name = "logBox";
			this.logBox.ScrollBars = ScrollBars.Both;
			this.logBox.Size = new Size(513, 96);
			this.logBox.TabIndex = 0;
			this.logBox.WordWrap = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.enableLogging);
			this.panel1.Dock = DockStyle.Top;
			this.panel1.Location = new Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(513, 25);
			this.panel1.TabIndex = 1;
			// 
			// enableLogging
			// 
			this.enableLogging.AutoSize = true;
			this.enableLogging.Checked = true;
			this.enableLogging.CheckState = CheckState.Checked;
			this.enableLogging.Location = new Point(4, 4);
			this.enableLogging.Name = "enableLogging";
			this.enableLogging.Size = new Size(90, 17);
			this.enableLogging.TabIndex = 0;
			this.enableLogging.Text = "&Enable tracing";
			this.enableLogging.CheckedChanged += new EventHandler(this.enableLogging_CheckedChanged);
			// 
			// TraceTextBox
			// 
			this.AutoScaleDimensions = new SizeF(6F, 13F);
			this.Controls.Add(this.logBox);
			this.Controls.Add(this.panel1);
			this.Name = "TraceTextBox";
			this.Size = new Size(513, 121);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private TextBox logBox;
		private Panel panel1;
		private CheckBox enableLogging;
	}
}