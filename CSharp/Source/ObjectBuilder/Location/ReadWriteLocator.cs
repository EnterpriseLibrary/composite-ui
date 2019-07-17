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

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// A abstract implementation of <see cref="IReadWriteLocator"/>.
	/// </summary>
	public abstract class ReadWriteLocator : ReadableLocator, IReadWriteLocator
	{
		/// <summary>
		/// See <see cref="IReadableLocator.ReadOnly"/> for more information.
		/// </summary>
		public override bool ReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// See <see cref="IReadWriteLocator.Add(object, object)"/> for more information.
		/// </summary>
		public abstract void Add(object key, object value);

		/// <summary>
		/// See <see cref="IReadWriteLocator.Remove(object)"/> for more information.
		/// </summary>
		public abstract bool Remove(object key);
	}
}
