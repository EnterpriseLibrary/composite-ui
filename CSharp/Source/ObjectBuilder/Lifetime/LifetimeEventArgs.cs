//===============================================================================
// Microsoft patterns & practices
// Object Builder Application Block
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

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// The event data sent for the events of <see cref="ILifetimeContainer"/>.
	/// </summary>
	public class LifetimeEventArgs : EventArgs
	{
		private object item;

		/// <summary>
		/// The item that the event it about.
		/// </summary>
		public object Item
		{
			get { return item; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LifetimeEventArgs"/> class using
		/// the provided item.
		/// </summary>
		/// <param name="item">The item.</param>
		public LifetimeEventArgs(object item)
		{
			this.item = item;
		}
	}
}
