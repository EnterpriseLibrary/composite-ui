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
	/// Implementation of <see cref="ITypeMappingPolicy"/> which does simple type/ID
	/// mapping.
	/// </summary>
	public class TypeMappingPolicy : ITypeMappingPolicy
	{
		private DependencyResolutionLocatorKey pair;

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeMappingPolicy"/> class using
		/// the provided type and ID.
		/// </summary>
		/// <param name="type">The new type to be returned during Map.</param>
		/// <param name="id">The new ID to be returned during Map.</param>
		public TypeMappingPolicy(Type type, string id)
		{
			pair = new DependencyResolutionLocatorKey(type, id);
		}

		/// <summary>
		/// See <see cref="ITypeMappingPolicy.Map"/> for more information.
		/// </summary>
		public DependencyResolutionLocatorKey Map(DependencyResolutionLocatorKey incomingTypeIDPair)
		{
			return pair;
		}
	}
}
