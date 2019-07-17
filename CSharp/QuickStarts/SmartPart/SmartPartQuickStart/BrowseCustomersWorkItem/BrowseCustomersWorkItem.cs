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

using System.Collections.Generic;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;

namespace SmartPartQuickStart.BrowseCustomersWorkItem
{
	/// <summary>
	/// Browse Cusotmer WorkItem.
	/// </summary>
	public class BrowseCustomersWorkItem : WorkItem
	{
		private CustomerMain customerMain;

		/// <summary>
		/// Starts the workitem.  You can put any logic 
		/// here that makes the workitem work as desired.
		/// </summary>
		protected override void OnRunStarted()
		{
			base.OnRunStarted();

			//Create the main view that will be shown on the main workspace
			//of the MainForm.
			IWorkspace workspace = this.Workspaces["MainFormWorkspace"];
			customerMain = this.Items.AddNew<CustomerMain>("CustomerMain");
			workspace.Show(customerMain);
		}

		/// <summary>
		/// Shows the details for a customer.
		/// </summary>
		/// <param name="customer"></param>
		public void ShowCustomerDetails(Customer customer)
		{
			//Set the state so the child workitem gets injected with it.
			State["Customer"] = customer;

			//Create a key for the workitem so we can check
			//later if the workitem has already been created.
			string key = customer.Id + "Details";

			ViewCustomerWorkItem.ViewCustomerWorkItem customerWorkItem = Items.Get<ViewCustomerWorkItem.ViewCustomerWorkItem>(key);

			if (customerWorkItem == null)
			{
				customerWorkItem =
					this.Items.AddNew<ViewCustomerWorkItem.ViewCustomerWorkItem>(key);
				customerWorkItem.Customer = (Customer)State["Customer"];
			}
			
			//Run the child workitem.
			customerWorkItem.Run(customerMain.customersDeckedWorkspace);
		}
	}
}
