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
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace SmartPartQuickStart.ViewCustomerWorkItem
{
	/// <summary>
	/// Displays the comments associated with the customer.
	/// </summary>
	public partial class CustomerCommentsView : TitledSmartPart
	{
		/// <summary>
		/// The customer State will be injected into the view.
		/// </summary>
		private Customer customer = null;

		[State]
		public Customer Customer
		{
			set { customer = value; }
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public CustomerCommentsView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Adds the customer to the bindings source.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (customer != null)
			{
				this.customerBindingSource.Add(customer);
			}
		}
	}
}
