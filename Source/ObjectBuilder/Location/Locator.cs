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
	/// An implementation of <see cref="IReadableLocator"/> and <see cref="IReadWriteLocator"/>.
	/// </summary>
	public class Locator : ReadWriteLocator
	{
		private WeakRefDictionary<object, object> references = new WeakRefDictionary<object, object>();

		/// <summary>
		/// Constructor. Creates an root locator.
		/// </summary>
		public Locator()
			: this(null)
		{
		}

		/// <summary>
		/// Constructor. Creates a child locator.
		/// </summary>
		/// <param name="parentLocator">The parent locator.</param>
		public Locator(IReadableLocator parentLocator)
		{
            SetParentLocator(parentLocator);
		}

		/// <summary>
		/// See <see cref="IReadableLocator.Count"/> for more information.
		/// </summary>
		public override int Count
		{
			get { return references.Count; }
		}

		/// <summary>
		/// See <see cref="IReadWriteLocator.Add(object, object)"/> for more information.
		/// </summary>
		public override void Add(object key, object value)
		{
			if (key == null)
				throw new ArgumentNullException("key");
			if (value == null)
				throw new ArgumentNullException("value");

			references.Add(key, value);
		}

		/// <summary>
		/// See <see cref="IReadableLocator.Contains(object, SearchMode)"/> for more information.
		/// </summary>
		public override bool Contains(object key, SearchMode options)
		{
			if (key == null)
				throw new ArgumentNullException("key");
			if (!Enum.IsDefined(typeof(SearchMode), options))
				throw new ArgumentException(Properties.Resources.InvalidEnumerationValue, "options");

			if (references.ContainsKey(key))
				return true;

			if (options == SearchMode.Up && ParentLocator != null)
				return ParentLocator.Contains(key, options);

			return false;
		}

		/// <summary>
		/// See <see cref="IReadableLocator.Get(object, SearchMode)"/> for more information.
		/// </summary>
		public override object Get(object key, SearchMode options)
		{
			if (key == null)
				throw new ArgumentNullException("key");
			if (!Enum.IsDefined(typeof(SearchMode), options))
				throw new ArgumentException(Properties.Resources.InvalidEnumerationValue, "options");

			if (references.ContainsKey(key))
				return references[key];

			if (options == SearchMode.Up && ParentLocator != null)
				return ParentLocator.Get(key, options);

			return null;
		}

		/// <summary>
		/// See <see cref="IReadWriteLocator.Remove(object)"/> for more information.
		/// </summary>
		public override bool Remove(object key)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			return references.Remove(key);
		}

		/// <summary>
		/// See <see cref="IEnumerable{T}.GetEnumerator()"/> for more information.
		/// </summary>
		public override IEnumerator<KeyValuePair<object, object>> GetEnumerator()
		{
			return references.GetEnumerator();
		}
	}
}
