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

namespace BankShell
{
	partial class BankShellForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
			this.File = new System.Windows.Forms.ToolStripMenuItem();
			this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.sideBarWorkspace = new Microsoft.Practices.CompositeUI.WinForms.DeckWorkspace();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.contentWorkspace = new Microsoft.Practices.CompositeUI.WinForms.DeckWorkspace();
			this.mainMenuStrip.SuspendLayout();
			this.mainStatusStrip.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenuStrip
			// 
			this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File});
			this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
			this.mainMenuStrip.Name = "mainMenuStrip";
			this.mainMenuStrip.Size = new System.Drawing.Size(739, 24);
			this.mainMenuStrip.TabIndex = 0;
			this.mainMenuStrip.Text = "mainmenu";
			// 
			// File
			// 
			this.File.Name = "File";
			this.File.Size = new System.Drawing.Size(35, 20);
			this.File.Text = "&File";
			// 
			// mainStatusStrip
			// 
			this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.mainStatusStrip.Location = new System.Drawing.Point(0, 513);
			this.mainStatusStrip.Name = "mainStatusStrip";
			this.mainStatusStrip.Size = new System.Drawing.Size(739, 22);
			this.mainStatusStrip.TabIndex = 1;
			this.mainStatusStrip.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
			// 
			// sideBarWorkspace
			// 
			this.sideBarWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sideBarWorkspace.Location = new System.Drawing.Point(0, 0);
			this.sideBarWorkspace.Name = "sideBarWorkspace";
			this.sideBarWorkspace.Size = new System.Drawing.Size(222, 489);
			this.sideBarWorkspace.TabIndex = 0;
			this.sideBarWorkspace.Text = "deckWorkspace1";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 24);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.sideBarWorkspace);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.contentWorkspace);
			this.splitContainer1.Size = new System.Drawing.Size(739, 489);
			this.splitContainer1.SplitterDistance = 222;
			this.splitContainer1.TabIndex = 2;
			this.splitContainer1.Text = "splitContainer1";
			// 
			// contentWorkspace
			// 
			this.contentWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
			this.contentWorkspace.Location = new System.Drawing.Point(0, 0);
			this.contentWorkspace.Name = "contentWorkspace";
			this.contentWorkspace.Size = new System.Drawing.Size(513, 489);
			this.contentWorkspace.TabIndex = 0;
			this.contentWorkspace.Text = "deckedWorkspace1";
			// 
			// BankShellForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(739, 535);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.mainStatusStrip);
			this.Controls.Add(this.mainMenuStrip);
			this.MainMenuStrip = this.mainMenuStrip;
			this.Name = "BankShellForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Bank Shell (netfx4)";
			this.mainMenuStrip.ResumeLayout(false);
			this.mainMenuStrip.PerformLayout();
			this.mainStatusStrip.ResumeLayout(false);
			this.mainStatusStrip.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mainMenuStrip;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private Microsoft.Practices.CompositeUI.WinForms.DeckWorkspace sideBarWorkspace;
		private System.Windows.Forms.SplitContainer splitContainer1;
		public Microsoft.Practices.CompositeUI.WinForms.DeckWorkspace contentWorkspace;
		private System.Windows.Forms.ToolStripMenuItem File;
		internal System.Windows.Forms.StatusStrip mainStatusStrip;
	}
}

