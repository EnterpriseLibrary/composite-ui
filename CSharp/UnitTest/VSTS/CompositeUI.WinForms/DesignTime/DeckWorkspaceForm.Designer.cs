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

namespace Microsoft.Practices.CompositeUI.WinForms.Tests.DesignTime
{
	partial class DeckWorkspaceForm
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
			this.deckWorkspace1 = new Microsoft.Practices.CompositeUI.WinForms.DeckWorkspace();
			this.SuspendLayout();
			// 
			// deckWorkspace1
			// 
			this.deckWorkspace1.Location = new System.Drawing.Point(12, 43);
			this.deckWorkspace1.Name = "deckWorkspace1";
			this.deckWorkspace1.Size = new System.Drawing.Size(270, 201);
			this.deckWorkspace1.TabIndex = 0;
			this.deckWorkspace1.Text = "deckWorkspace1";
			// 
			// DeckWorkspaceForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.deckWorkspace1);
			this.Name = "DeckWorkspaceForm";
			this.Text = "DeckWorkspaceForm";
			this.ResumeLayout(false);

		}

		#endregion

		private DeckWorkspace deckWorkspace1;







	}
}