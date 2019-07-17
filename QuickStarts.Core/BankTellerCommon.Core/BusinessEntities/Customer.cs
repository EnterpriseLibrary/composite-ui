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

namespace BankTellerCommon
{
	[Serializable]
	public class Customer
	{
		private string address1;
		private string address2;
		private string city;
		private string comments;
		private string emailAddress;
		private string firstName;
		private int id;
		private string lastName;
		private string phone1;
		private string phone2;
		private string state;
		private string zipCode;

		public Customer(int id, string firstName, string lastName, string address1, string address2,
					string city, string state, string zipCode, string emailAddress, string phone1, string phone2,
					string comments)
		{
			this.id = id;
			this.firstName = firstName;
			this.lastName = lastName;
			this.address1 = address1;
			this.address2 = address2;
			this.city = city;
			this.state = state;
			this.zipCode = zipCode;
			this.emailAddress = emailAddress;
			this.phone1 = phone1;
			this.phone2 = phone2;
			this.comments = comments;
		}

		public string Address1
		{
			get { return address1; }
			set { address1 = value; }
		}

		public string Address2
		{
			get { return address2; }
			set { address2 = value; }
		}

		public string City
		{
			get { return city; }
			set { city = value; }
		}

		public string Comments
		{
			get { return comments; }
			set { comments = value; }
		}

		public string EmailAddress
		{
			get { return emailAddress; }
			set { emailAddress = value; }
		}

		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		public int ID
		{
			get { return id; }
		}

		public string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}

		public string Phone1
		{
			get { return phone1; }
			set { phone1 = value; }
		}

		public string Phone2
		{
			get { return phone2; }
			set { phone2 = value; }
		}

		public string State
		{
			get { return state; }
			set { state = value; }
		}

		public string ZipCode
		{
			get { return zipCode; }
			set { zipCode = value; }
		}

		public override string ToString()
		{
			return string.Format("{0}, {1}", LastName, FirstName);
		}

	}
}
