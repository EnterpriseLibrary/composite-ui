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
	/// Represents a strategy in the <see cref="IBuilder{T}"/>'s chain of responsibility.
	/// Strategies are required to support both BuildUp and TearDown. Although you
	/// can implement this interface directly, you may also choose to use
	/// <see cref="BuilderStrategy"/> as the base class for your strategies, as
	/// this class provides useful helper methods and makes support BuildUp and TearDown
	/// optional.
	/// </summary>
	public interface IBuilderStrategy
	{
		/// <summary>
		/// Called during the chain of responsibility for a build operation.
		/// </summary>
		/// <param name="context">The context for the operation.</param>
		/// <param name="typeToBuild">The type of object that is being requested.</param>
		/// <param name="existing">The existing instance, if one was passed in, or
		/// if a previous strategy has already created the object.</param>
		/// <param name="idToBuild">The ID of the object that is being requested. May be
		/// null if the user requested an unnamed instance.</param>
		/// <returns>The built object.</returns>
		object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild);

		/// <summary>
		/// Called during the chain of responsibility for an unbuild operation.
		/// </summary>
		/// <param name="context">The context for the operation.</param>
		/// <param name="item">The item that is being unbuilt.</param>
		/// <returns>The unbuilt object.</returns>
		object TearDown(IBuilderContext context, object item);
	}
}