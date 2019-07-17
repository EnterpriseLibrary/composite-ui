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

namespace BankTellerModule
{
	// The CustomerDetailController is the controller used by CustomerDetailView.
	// The detail view contains a "comments" button which dynamically shows the
	// comments for the customer. The controller forwards this request on to the
	// containing work item (CustomerWorkItem) to process, since the action takes
	// place outside the confines of the detail view.

	public class CustomerDetailController : Controller
	{
		public new CustomerWorkItem WorkItem
		{
			get { return base.WorkItem as CustomerWorkItem; }
		}

		public void ShowCustomerComments()
		{
			WorkItem.ShowCustomerComments();
		}
	}
}
