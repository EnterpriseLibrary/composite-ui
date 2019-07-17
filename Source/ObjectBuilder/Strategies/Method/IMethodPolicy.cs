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

using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Represents a policy for <see cref="MethodExecutionStrategy"/>.
	/// </summary>
	public interface IMethodPolicy : IBuilderPolicy
	{
		/// <summary>
		/// A collection of methods to be called on the object instance.
		/// </summary>
		Dictionary<string, IMethodCallInfo> Methods { get; }
	}
}