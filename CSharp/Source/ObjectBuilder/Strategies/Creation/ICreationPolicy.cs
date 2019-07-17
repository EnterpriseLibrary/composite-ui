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
	/// Represents a policy for <see cref="CreationStrategy"/>.
	/// </summary>
	public interface ICreationPolicy : IBuilderPolicy
	{
		/// <summary>
		/// Selects the constructor to be used to create the object.
		/// </summary>
		/// <param name="context">The builder context.</param>
		/// <param name="type">The type of object requested.</param>
		/// <param name="id">The ID of the object requested.</param>
		/// <returns>The constructor to use; returns null if no suitable constructor can be found.</returns>
		ConstructorInfo SelectConstructor(IBuilderContext context, Type type, string id);

		/// <summary>
		/// Gets the parameter values to be passed to the constructor.
		/// </summary>
		/// <param name="context">The builder context.</param>
		/// <param name="type">The type of object requested.</param>
		/// <param name="id">The ID of the object requested.</param>
		/// <param name="constructor">The constructor that will be used.</param>
		/// <returns>An array of parameters to pass to the constructor.</returns>
		object[] GetParameters(IBuilderContext context, Type type, string id, ConstructorInfo constructor);
	}
}