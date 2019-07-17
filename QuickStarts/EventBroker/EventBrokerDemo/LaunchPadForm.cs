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
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;

namespace EventBrokerDemo
{
	public partial class LaunchPadForm : Form
	{
		// Specify we need the LaunchPadController
		private LaunchPadController controller;

		[CreateNew]
		public LaunchPadController Controller
		{
			set { controller = value; }
		}

		// Specify we need the state object under the "customers" key
		private List<string> customers;

		[State("customers")]
		public List<string> Customers
		{
			set { customers = value; }
		}

		public LaunchPadForm()
		{
			InitializeComponent();
		}

		// At this point, the form is sited into the work item, and our controller and view
		// are already injected by CAB. So we can hook the State changed event.
		protected override void OnLoad(EventArgs e)
		{
			this.controller.State.StateChanged += new EventHandler<StateChangedEventArgs>(State_StateChanged);
		}

		// Update the view accordingly to the changes on the model (the state)
		void State_StateChanged(object sender, StateChangedEventArgs e)
		{
			this.lstGlobalCustomers.DataSource = null;
			this.lstGlobalCustomers.DataSource = customers;
		}

		// Here we handle view events, forwarding the needed calls to our controller.

		private void btnFireCustomerChange_Click(object sender, EventArgs e)
		{
			if (txtCustomerId.Text.Length > 0)
			{
				controller.AddCustomer(txtCustomerId.Text);
				txtCustomerId.Text = "";
			}
			txtCustomerId.Focus();
		}

		private void btnNewCustomerList_Click(object sender, EventArgs e)
		{
			controller.ShowCustomerList();
		}

		private void LaunchPadForm_Load(object sender, EventArgs e)
		{

		}
	}
}