//===============================================================================
// Microsoft patterns & practices
// CompositeUI Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace ModuleLoaderQuickStart
{
    partial class MainForm
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
			this.MainWorkspace = new Microsoft.Practices.CompositeUI.WinForms.DeckWorkspace();
			this.SuspendLayout();
			// 
			// MainWorkspace
			// 
			this.MainWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainWorkspace.Location = new System.Drawing.Point(0, 0);
			this.MainWorkspace.Name = "MainWorkspace";
			this.MainWorkspace.Size = new System.Drawing.Size(267, 84);
			this.MainWorkspace.TabIndex = 0;
			this.MainWorkspace.Text = "MainWorkSpace";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(267, 84);
			this.Controls.Add(this.MainWorkspace);
			this.Name = "MainForm";
			this.Text = "ModuleLoader MainForm";
			this.ResumeLayout(false);

        }

        #endregion

			private Microsoft.Practices.CompositeUI.WinForms.DeckWorkspace MainWorkspace;



			}
}

