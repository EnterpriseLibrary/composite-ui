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
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;
using BankTellerCommon;

namespace BankTellerModule
{
	// This SmartPart implements ISmartPartInfoProvider because it is displayed
	// dynamically in its tabbed workspace. The SmartPartInfo lets us tell the
	// tabbed workspace what the name of our tab should be.

	[SmartPart]
	public partial class CustomerCommentsView : UserControl
	{
		// The Customer state is stored in our parent work item

		private Customer customer = null;

		[State]
		public Customer Customer
		{
			set { customer = value; }
		}

		public CustomerCommentsView()
		{
			InitializeComponent();
		}

		private void CustomerCommentsView_Load(object sender, EventArgs e)
		{
			if (!DesignMode)
			{
				customerBindingSource.Add(customer);
			}
		}
	}
}
