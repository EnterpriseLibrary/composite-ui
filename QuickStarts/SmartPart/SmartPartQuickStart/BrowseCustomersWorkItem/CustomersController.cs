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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI;
using System.Windows.Forms;

namespace SmartPartQuickStart.BrowseCustomersWorkItem
{
	/// <summary>
	/// Controller used by the views.
	/// </summary>
	public class CustomersController : Controller
	{
		/// <summary>
		/// The customer State will be injected into the view.
		/// </summary>
		[State("Customers")]
		public List<Customer> customers
		{
		    get { return (List<Customer>)State["Customers"]; }
		    set
		    {
		        try
		        {
		            if ((value != null) &&
		                (State != null))
		            {
		                State["Customers"] = value;
		            }
		        }
		        catch (Exception ex)
		        {
		            MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
		        }
		    }
		}

		/// <summary>
		/// The controller is dependent on the BrowseCustomers WorkItem
		/// and will not run with out it.
		/// </summary>
		private BrowseCustomersWorkItem customerWorkItem = null;

		[ServiceDependency(Type = typeof(WorkItem))]
		public BrowseCustomersWorkItem CustomerWorkItem
		{
			set { customerWorkItem = value; }
		}

		/// <summary>
		/// Loads Mock data.
		/// </summary>
		public void PopulateCustomersData()
		{
			if (customers == null)
			{
				throw new ArgumentNullException("Customers");
			}

			customers.Add(new Customer("Jesper", "Aaberg", "One Microsoft Way, Redmond WA 98052", "CAB Rocks!"));
			customers.Add(new Customer("Martin", "Bankov", "One Microsoft Way, Redmond WA 98052", "This is awesome"));
			customers.Add(new Customer("Shu", "Ito", "One Microsoft Way, Redmond WA 98052", "N/A"));
			customers.Add(new Customer("Kim", "Ralls", "One Microsoft Way, Redmond WA 98052", "N/A"));
			customers.Add(new Customer("John", "Kane", "One Microsoft Way, Redmond WA 98052", "N/A"));
		}

		/// <summary>
		/// Shows the customer details.
		/// </summary>
		/// <param name="customer"></param>
		public void ShowCustomerDetails(Customer customer)
		{
			// To maintain separation of concerns.
			customerWorkItem.ShowCustomerDetails(customer);
		}
	}
}
