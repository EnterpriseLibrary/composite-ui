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

namespace Microsoft.Practices.CompositeUI.WinForms.Tests.DesignTime
{
	public class CustomWorkItem : WorkItem
	{
		internal MySmartPart customerInformation;
		internal Component1 anyComponent;
		internal MySmartPart customerDetails;
		private WindowWorkspace window;
		private IContainer components;
		
		public CustomWorkItem()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			this.customerInformation = new Microsoft.Practices.CompositeUI.WinForms.Tests.DesignTime.MySmartPart();
			this.components = new System.ComponentModel.Container();
			this.anyComponent = new Microsoft.Practices.CompositeUI.WinForms.Tests.DesignTime.Component1(this.components);
			this.customerDetails = new Microsoft.Practices.CompositeUI.WinForms.Tests.DesignTime.MySmartPart();
			this.window = new Microsoft.Practices.CompositeUI.WinForms.WindowWorkspace();
			// 
			// customerInformation
			// 
			this.customerInformation.Location = new System.Drawing.Point(0, 0);
			this.customerInformation.Name = "customerInformation";
			this.customerInformation.Size = new System.Drawing.Size(208, 187);
			this.customerInformation.TabIndex = 0;
			// 
			// customerDetails
			// 
			this.customerDetails.Location = new System.Drawing.Point(0, 0);
			this.customerDetails.Name = "customerDetails";
			this.customerDetails.Size = new System.Drawing.Size(208, 187);
			this.customerDetails.TabIndex = 0;
		}

		// This used to be called from InitializeComponent().
		// Now, it needs to be done during OnBuiltUp, as shown here.

		public override void OnBuiltUp(string id)
		{
			base.OnBuiltUp(id);

			// Do not modify this method, changes will be lost upon regeneration by the designer.
			this.Items.Add(this.customerInformation, "customerInformation");
			this.Items.Add(this.anyComponent, "anyComponent");
			this.Items.Add(this.customerDetails, "customerDetails");
			this.Items.Add(this.window, "window");
		}
	}
}
