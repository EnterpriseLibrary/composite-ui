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

namespace GPSModule
{
    partial class GPSView
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
            this.txtDistance = new System.Windows.Forms.TextBox();
            this.cmdGetDistance = new System.Windows.Forms.Button();
            this.txtLatitude = new System.Windows.Forms.TextBox();
            this.cmdGetLatitude = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtDistance
            // 
            this.txtDistance.Location = new System.Drawing.Point(94, 50);
            this.txtDistance.Name = "txtDistance";
            this.txtDistance.Size = new System.Drawing.Size(163, 20);
            this.txtDistance.TabIndex = 11;
            // 
            // cmdGetDistance
            // 
            this.cmdGetDistance.Location = new System.Drawing.Point(13, 48);
            this.cmdGetDistance.Name = "cmdGetDistance";
            this.cmdGetDistance.Size = new System.Drawing.Size(75, 23);
            this.cmdGetDistance.TabIndex = 10;
            this.cmdGetDistance.Text = "GetDistance";
            this.cmdGetDistance.Click += new System.EventHandler(this.cmdGetDistance_Click);
            // 
            // txtLatitude
            // 
            this.txtLatitude.Location = new System.Drawing.Point(94, 21);
            this.txtLatitude.Name = "txtLatitude";
            this.txtLatitude.Size = new System.Drawing.Size(163, 20);
            this.txtLatitude.TabIndex = 9;
            // 
            // cmdGetLatitude
            // 
            this.cmdGetLatitude.Location = new System.Drawing.Point(13, 19);
            this.cmdGetLatitude.Name = "cmdGetLatitude";
            this.cmdGetLatitude.Size = new System.Drawing.Size(75, 23);
            this.cmdGetLatitude.TabIndex = 8;
            this.cmdGetLatitude.Text = "GetLatitude";
            this.cmdGetLatitude.Click += new System.EventHandler(this.cmdGetLatitude_Click);
            // 
            // GPSView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtDistance);
            this.Controls.Add(this.cmdGetDistance);
            this.Controls.Add(this.txtLatitude);
            this.Controls.Add(this.cmdGetLatitude);
            this.Name = "GPSView";
            this.Size = new System.Drawing.Size(272, 86);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDistance;
        private System.Windows.Forms.Button cmdGetDistance;
        private System.Windows.Forms.TextBox txtLatitude;
        private System.Windows.Forms.Button cmdGetLatitude;
    }
}
