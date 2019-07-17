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

namespace SmartPartQuickStart.ViewCustomerWorkItem
{
	/// <summary>
	/// Controller for the customer comments view.
	/// </summary>
	public class CustomerController : Controller
	{
		/// <summary>
		/// Has to have a customer workitem to work.
		/// </summary>
		private ViewCustomerWorkItem customerWorkItem = null;

		[ServiceDependency(Type = typeof(WorkItem))]
		public ViewCustomerWorkItem CustomerWorkItem
		{
			set { customerWorkItem = value; }
		}

		/// <summary>
		/// Calls back to the workitem to create and show
		/// the comments view.
		/// </summary>
		public void ShowCustomerComments()
		{
			customerWorkItem.ShowCustomerComments();
		}
	}
}
