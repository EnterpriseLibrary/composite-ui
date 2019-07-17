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
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.CompositeUI;
using BankTellerCommon;

namespace BankTellerModule
{
	[SmartPart]
	public partial class CustomerAccountsView : UserControl
	{
		private Customer customer;
		private CustomerAccountService accountService;

		// The Customer state is stored in our parent work item
		[State]
		public Customer Customer
		{
			set { customer = value; }
		}

		// Make sure our required CustomerAccountService is available
		[ServiceDependency]
		public CustomerAccountService AccountService
		{
			set { accountService = value; }
		}

		public CustomerAccountsView()
		{
			InitializeComponent();
		}

		private void CustomerAccountsView_Load(object sender, EventArgs e)
		{
			if (customer != null)
			{
				dataGridView1.DataSource = accountService.GetByCustomerID(customer.ID);
			}
		}
	}
}
