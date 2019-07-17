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
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Encapsulates a method call.
	/// </summary>
	public interface IMethodCallInfo
	{
		/// <summary>
		/// Gets the parameter values to be passed to the method.
		/// </summary>
		/// <param name="context">The builder context.</param>
		/// <param name="type">The type of object requested.</param>
		/// <param name="id">The ID of the object requested.</param>
		/// <param name="method">The method that will be used.</param>
		/// <returns>An array of parameters to pass to the method.</returns>
		object[] GetParameters(IBuilderContext context, Type type, string id, MethodInfo method);

		/// <summary>
		/// Selects the method to be called.
		/// </summary>
		/// <param name="context">The builder context.</param>
		/// <param name="type">The type of object requested.</param>
		/// <param name="id">The ID of the object requested.</param>
		/// <returns>The method to use; return null if no suitable method can be found.</returns>
		MethodInfo SelectMethod(IBuilderContext context, Type type, string id);
	}
}
