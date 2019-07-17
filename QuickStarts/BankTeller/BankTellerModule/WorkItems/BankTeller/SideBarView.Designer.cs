namespace BankTellerModule
{
	partial class SideBarView
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.smartPartPlaceholder1 = new Microsoft.Practices.CompositeUI.WinForms.SmartPartPlaceholder();
			this.customerQueueView1 = new BankTellerModule.CustomerQueueView();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.smartPartPlaceholder1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.customerQueueView1);
			this.splitContainer1.Size = new System.Drawing.Size(199, 500);
			this.splitContainer1.SplitterDistance = 55;
			this.splitContainer1.TabIndex = 0;
			// 
			// smartPartPlaceholder1
			// 
			this.smartPartPlaceholder1.SmartPartName = "UserInfo";
			this.smartPartPlaceholder1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.smartPartPlaceholder1.Location = new System.Drawing.Point(0, 0);
			this.smartPartPlaceholder1.Name = "smartPartPlaceholder1";
			this.smartPartPlaceholder1.Size = new System.Drawing.Size(199, 55);
			this.smartPartPlaceholder1.TabIndex = 0;
			this.smartPartPlaceholder1.Text = "smartPartPlaceholder1";
			// 
			// customerQueueView1
			// 
			this.customerQueueView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.customerQueueView1.Location = new System.Drawing.Point(0, 0);
			this.customerQueueView1.Name = "customerQueueView1";
			this.customerQueueView1.Size = new System.Drawing.Size(199, 441);
			this.customerQueueView1.TabIndex = 0;
			// 
			// SideBarView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Name = "SideBarView";
			this.Size = new System.Drawing.Size(199, 500);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private Microsoft.Practices.CompositeUI.WinForms.SmartPartPlaceholder smartPartPlaceholder1;
		private CustomerQueueView customerQueueView1;
	}
}
