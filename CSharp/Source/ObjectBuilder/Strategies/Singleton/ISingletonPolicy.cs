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
	/// Represents a policy for <see cref="SingletonStrategy"/>.
	/// </summary>
	public interface ISingletonPolicy : IBuilderPolicy
	{
		/// <summary>
		/// Returns true if the object should be a singleton.
		/// </summary>
		bool IsSingleton { get; }
	}
}