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

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// An implementation of <see cref="IReadableLocator"/> that wraps an existing locator
	/// to ensure items are not written into the locator.
	/// </summary>
	public class ReadOnlyLocator : ReadableLocator
	{
		private IReadableLocator innerLocator;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="innerLocator">The inner locator to be wrapped.</param>
		public ReadOnlyLocator(IReadableLocator innerLocator)
		{
			if (innerLocator == null)
				throw new ArgumentNullException("innerLocator");

			this.innerLocator = innerLocator;
		}

		/// <summary>
		/// See <see cref="IReadableLocator.Count"/> for more information.
		/// </summary>
		public override int Count
		{
			get { return innerLocator.Count; }
		}

		/// <summary>
		/// See <see cref="IReadableLocator.Count"/> for more information.
		/// </summary>
		public override IReadableLocator ParentLocator
		{
			get
			{
				return new ReadOnlyLocator(innerLocator.ParentLocator);
			}
		}

		/// <summary>
		/// See <see cref="IReadableLocator.ReadOnly"/> for more information.
		/// </summary>
		public override bool ReadOnly
		{
			get { return true; }
		}

		/// <summary>
		/// See <see cref="IReadableLocator.Contains(object, SearchMode)"/> for more information.
		/// </summary>
		public override bool Contains(object key, SearchMode options)
		{
			return innerLocator.Contains(key, options);
		}

		/// <summary>
		/// See <see cref="IReadableLocator.Get(object, SearchMode)"/> for more information.
		/// </summary>
		public override object Get(object key, SearchMode options)
		{
			return innerLocator.Get(key, options);
		}

		/// <summary>
		/// See <see cref="IEnumerable{T}.GetEnumerator()"/> for more information.
		/// </summary>
		public override IEnumerator<KeyValuePair<object, object>> GetEnumerator()
		{
			return innerLocator.GetEnumerator();
		}
	}
}
