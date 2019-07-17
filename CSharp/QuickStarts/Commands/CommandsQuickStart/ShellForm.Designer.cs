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

namespace CommandsQuickStart
{
    partial class ShellForm
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
			  this.Exit = new System.Windows.Forms.ToolStripMenuItem();
			  this.mainMenuStrip.SuspendLayout();
			  this.SuspendLayout();
			  // 
			  // mainMenuStrip
			  // 
			  this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File});
			  this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
			  this.mainMenuStrip.Name = "mainMenuStrip";
			  this.mainMenuStrip.Size = new System.Drawing.Size(292, 24);
			  this.mainMenuStrip.TabIndex = 0;
			  this.mainMenuStrip.Text = "menuStrip1";
			  // 
			  // File
			  // 
			  this.File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Exit});
			  this.File.Name = "File";
			  this.File.Size = new System.Drawing.Size(35, 20);
			  this.File.Text = "&File";
			  // 
			  // Exit
			  // 
			  this.Exit.Name = "Exit";
			  this.Exit.Size = new System.Drawing.Size(152, 22);
			  this.Exit.Text = "E&xit";
			  this.Exit.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			  // 
			  // ShellForm
			  // 
			  this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			  this.ClientSize = new System.Drawing.Size(292, 273);
			  this.Controls.Add(this.mainMenuStrip);
			  this.MainMenuStrip = this.mainMenuStrip;
			  this.Name = "ShellForm";
			  this.Text = "CommandsQuickStart";
			  this.mainMenuStrip.ResumeLayout(false);
			  this.mainMenuStrip.PerformLayout();
			  this.ResumeLayout(false);
			  this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem File;
        private System.Windows.Forms.ToolStripMenuItem Exit;
    }
}

