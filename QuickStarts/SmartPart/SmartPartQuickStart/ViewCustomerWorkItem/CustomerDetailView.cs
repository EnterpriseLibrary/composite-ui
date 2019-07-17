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

namespace SmartPartQuickStart.ViewCustomerWorkItem
{
	/// <summary>
	/// Displays the details of the customer.
	/// </summary>
	public partial class CustomerDetailView : TitledSmartPart
	{
		/// <summary>
		/// The customer State will be injected into the view.
		/// </summary>
		private Customer customer = null;

		[State("Customer")]
		public Customer Customer
		{
			set { customer = value; }
		}

		/// <summary>
		/// The controller will be injected into the view.
		/// </summary>
		private CustomerController controller = null;

		[CreateNew]
		public CustomerController Controller
		{
			set { controller = value; }
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public CustomerDetailView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Adds the customer to the binding source.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (this.customer != null)
			{
				this.customerBindingSource.Add(this.customer);
			}
		}

		private void commentsButton_Click(object sender, EventArgs e)
		{
			//Calls to the controller to show the comments associated with
			//the customer.
			controller.ShowCustomerComments();
		}
	}
}
