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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;

namespace SmartPartQuickStart.BrowseCustomersWorkItem
{
	/// <summary>
	/// Provides a list fo customers.
	/// </summary>
	public partial class CustomerListView : TitledSmartPart
	{
		/// <summary>
		/// The controller will get injected into the smartpart
		/// when it is added to the workitem.
		/// </summary>
		private CustomersController controller = null;

		[CreateNew]
		public CustomersController Controller
		{
			set { controller = value; }
		}

		/// <summary>
		/// The customer list State will be injected into the view.
		/// </summary>
		private List<Customer> customers = null;

		[State]
		public List<Customer> Customers
		{
			set { customers = value; }
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public CustomerListView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Sets the datasource for the listbox.
		/// Wires up the SelectedIndexChanged event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			if (DesignMode == false)
			{
				controller.PopulateCustomersData();

				customerListBox.DataSource = customers;
				customerListBox.DisplayMember = "FullName";

				this.customerListBox.SelectedIndexChanged += 
					new EventHandler(customerListBox_SelectedIndexChanged);
			}
		}

		private void customerListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Call the controller to show customer details.
			controller.ShowCustomerDetails((Customer)this.customerListBox.SelectedValue);
		}

	}
}
