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
	/// Represents the context in which a build-up or tear-down operation runs.
	/// </summary>
	public interface IBuilderContext
	{
		/// <summary>
		/// Retrieves the head of the strategy chain.
		/// </summary>
		/// <returns>The strategy that's first in the chain; returns null if there are no
		/// strategies in the chain.</returns>
		IBuilderStrategy HeadOfChain { get; }

		/// <summary>
		/// The locator available to the strategies. A lifetime container is registered
		/// inside the locator, with a key of typeof(ILifetimeContainer).
		/// </summary>
		IReadWriteLocator Locator { get; }

		/// <summary>
		/// The policies for the current context. Any modifications will be transient (meaning,
		/// they will be forgotten when the outer BuildUp for this context is finished executing).
		/// </summary>
		PolicyList Policies { get; }

		/// <summary>
		/// Retrieves the next item in the strategy chain, relative to an existing item.
		/// </summary>
		/// <param name="currentStrategy">The strategy that is currently running</param>
		/// <returns>The next strategy in the chain; returns null if the given strategy
		/// was last in the chain.</returns>
		IBuilderStrategy GetNextInChain(IBuilderStrategy currentStrategy);
	}
}