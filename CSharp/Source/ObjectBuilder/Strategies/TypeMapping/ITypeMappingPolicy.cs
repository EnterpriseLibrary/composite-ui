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
	/// Represents a policy for <see cref="TypeMappingStrategy"/>.
	/// </summary>
	public interface ITypeMappingPolicy : IBuilderPolicy
	{
		/// <summary>
		/// Maps one Type/ID pair to another.
		/// </summary>
		/// <param name="incomingTypeIDPair">The incoming Type/ID pair.</param>
		/// <returns>The new Type/ID pair.</returns>
		DependencyResolutionLocatorKey Map(DependencyResolutionLocatorKey incomingTypeIDPair);
	}
}