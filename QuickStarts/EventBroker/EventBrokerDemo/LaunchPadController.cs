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
using System.ComponentModel;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.Utility;

namespace EventBrokerDemo
{
	public class LaunchPadController : Controller
	{
		private List<string> customers;

		[State("customers")]
		public List<string> Customers
		{
			set { customers = value; }
		}

		// A sync key. As this QS shows some multi-threading behavior 
		// we need to synchronize some object access.
		private object syncRoot = new object();

		// Adds a customer to the state and fire the state changed event.
		public void AddCustomer(string customerId)
		{
			lock (syncRoot)
			{
				customers.Add(customerId);
			}
			State.RaiseStateChanged("customers", customerId);
		}

		/// <summary>
		/// This is the subscription for the CustomerAdded event
		/// We're using the default scope, which is Global
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		[EventSubscription("topic://EventBrokerQuickStart/CustomerAdded")]
		public void OnCustomerAdded(object sender, DataEventArgs<string> e)
		{
			this.AddCustomer(e.Data);
		}

		// Creates a new (generic) work item and sets up the customer list view and controller
		public void ShowCustomerList()
		{
			// Create a new WorkItem and add it as a child WorkItem
			WorkItem child = this.WorkItem.WorkItems.AddNew<WorkItem>();

			// Forward the state to the child work item
			child.State["customers"] = new List<string>();

			// Create view for the customer list and add them to the work item
			child.Items.AddNew<CustomerListView>().Show();
		}
	}
}