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

// The example companies, organizations, products, domain names, e-mail addresses,
// logos, people, places, and events depicted herein are fictitious.  No association
// with any real company, organization, product, domain name, email address, logo,
// person, places, or events is intended or should be inferred.

using BankTellerCommon;
using Microsoft.Practices.CompositeUI;

namespace BankTellerModule
{
	// The service that simulates a data provider for customer business
	// entities (model data). Any class annotated with the [Service] attribute
	// is automatically registered in the root Work Item during module
	// initialization.

	[Service]
	public class CustomerQueueService
	{
		private Customer[] customers;
		private int idx = 0;

		public CustomerQueueService()
		{
			customers = new Customer[] {
				new Customer(1, "Brian", "Smith", "16074 NE 36th Way", "", "Redmond", "WA", "98052", "someone@example.com", "425-555-0100", "", "April 12, 2004 - Inquired about business line of credit"),
				new Customer(2, "David", "Jones", "1 Microsoft Way", "", "Redmond", "WA", "98052", "", "425-555-0101", "425-555-0102", "")
			};
		}

		public bool HasMore
		{
			get
			{
				return idx < customers.Length;
			}
		}

		public Customer GetNext()
		{
			if (!HasMore)
				return null;

			return customers[idx++];
		}
	}
}
