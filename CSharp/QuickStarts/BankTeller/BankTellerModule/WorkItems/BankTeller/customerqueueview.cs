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
using Microsoft.Practices.CompositeUI.Commands;
using Microsoft.Practices.CompositeUI.SmartParts;
using BankTellerCommon;
using System.Collections.Generic;
using Microsoft.Practices.CompositeUI.Utility;
using Microsoft.Practices.ObjectBuilder;

namespace BankTellerModule
{
	[SmartPart]
	public partial class CustomerQueueView : UserControl
	{
		private CustomerQueueController myController;

		// We need our controller, so that we can work with a customer when
		// the user clicks on one
		[CreateNew]
		public CustomerQueueController MyController
		{
			set { myController = value; }
		}

		public CustomerQueueView()
		{
			InitializeComponent();
		}

		// In addition to offering a button to get the next customer from the
		// queue, we also support a menu item to do the same thing. Because
		// the signature for both methods (button click handle, command handler)
		// we use this single method to do both.

		[CommandHandler(CommandConstants.ACCEPT_CUSTOMER)]
		public void OnAcceptCustomer(object sender, EventArgs e)
		{
			Customer customer = myController.GetNextCustomerInQueue();

			if (customer == null)
			{
				MessageBox.Show(this, "There are no more customers in the queue.", "Bank Teller", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			listCustomers.Items.Add(customer);
		}

		private void OnCustomerSelectionChanged(object sender, EventArgs e)
		{
			Customer customer = listCustomers.SelectedItem as Customer;

			if (customer != null)
			{
				myController.WorkWithCustomer(customer);
			}
		}
	}
}
