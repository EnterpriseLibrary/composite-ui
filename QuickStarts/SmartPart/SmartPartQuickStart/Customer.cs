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
using System.ComponentModel;

namespace SmartPartQuickStart
{
	/// <summary>
	/// The Model for data.
	/// </summary>
	public class Customer
	{
		private string firstName;
		private string lastName;
		private string address;
		private string comments;
		private string id = Guid.NewGuid().ToString();

		/// <summary>
		/// Fires when the customer data changes.
		/// </summary>
		public event EventHandler CustomerInfoChanged;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="firstName">First Name of customer.</param>
		/// <param name="lastName">Last Name of customer.</param>
		/// <param name="address">Address of customer.</param>
		/// <param name="comments">Comments associated with customer.</param>
		public Customer(string firstName, string lastName, string address, string comments)
		{
			this.firstName = firstName;
			this.lastName = lastName;
			this.address = address;
			this.comments = comments;
		}

		/// <summary>
		/// The unique id of the customer.
		/// </summary>
		public string Id
		{
			get { return id; }
		}

		/// <summary>
		/// The concatination of the first name and last name.
		/// </summary>
		public string FullName
		{
			get { return firstName + " " + lastName; }
		}

		/// <summary>
		/// Last Name of customer.
		/// </summary>
		public string LastName
		{
			get { return lastName; }
			set
			{
				lastName = value;
			}
		}

		/// <summary>
		/// First Name of customer.
		/// </summary>
		public string FirstName
		{
			get { return firstName; }
			set
			{
				firstName = value;
			}
		}

		/// <summary>
		/// Address of customer.
		/// </summary>
		public string Address
		{
			get { return address; }
			set
			{
				address = value;
			}
		}

		/// <summary>
		/// Comments associated with customer.
		/// </summary>
		public string Comments
		{
			get { return comments; }
			set { comments = value; }
		}

		/// <summary>
		/// Fires the customer chnaged event.
		/// </summary>
		public void FireCustomerInfoChanged()
		{
			if (this.CustomerInfoChanged != null)
			{
				this.CustomerInfoChanged(this, EventArgs.Empty);
			}
		}

	}
}
