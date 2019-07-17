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

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Represents a pair of Type and ID.
	/// </summary>
	public sealed class DependencyResolutionLocatorKey
	{
		private Type type;
		private string id;

		/// <summary>
		/// Initializes a new instance of the <see cref="DependencyResolutionLocatorKey"/> class
		/// using a null type and null ID.
		/// </summary>
		public DependencyResolutionLocatorKey()
			: this(null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DependencyResolutionLocatorKey"/> class
		/// using the provided type and ID.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="id">The ID.</param>
		public DependencyResolutionLocatorKey(Type type, string id)
		{
			this.type = type;
			this.id = id;
		}

		/// <summary>
		/// Returns the ID.
		/// </summary>
		public string ID
		{
			get { return id; }
		}

		/// <summary>
		/// Returns the type.
		/// </summary>
		public Type Type
		{
			get { return type; }
		}

		/// <summary>
		/// Overridden so that TypeIDPair can be used as the key in dictionaries.
		/// </summary>
		public override bool Equals(object obj)
		{
			DependencyResolutionLocatorKey other = obj as DependencyResolutionLocatorKey;

			if (other == null)
				return false;

			return (Equals(type, other.type) && Equals(id, other.id));
		}

		/// <summary>
		/// Overridden so that TypeIDPair can be used as the key in dictionaries.
		/// </summary>
		public override int GetHashCode()
		{
			int hashForType = type == null ? 0 : type.GetHashCode();
			int hashForID = id == null ? 0 : id.GetHashCode();
			return hashForType ^ hashForID;
		}
	}
}