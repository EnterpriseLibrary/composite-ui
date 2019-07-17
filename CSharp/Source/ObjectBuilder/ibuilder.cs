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
	/// Represents the main interface for an object builder.
	/// </summary>
	/// <typeparam name="TStageEnum">The enum that represents the build stages for this builder.</typeparam>
	public interface IBuilder<TStageEnum>
	{
		/// <summary>
		/// Permanent policies used for build-up operations.
		/// </summary>
		PolicyList Policies { get; }

		/// <summary>
		/// Strategies used for build-up and tear-down operations.
		/// </summary>
		StrategyList<TStageEnum> Strategies { get; }

		/// <summary>
		/// Performs a build operation.
		/// </summary>
		/// <remarks>This operation uses the strategies and permanent policies already configured
		/// into the builder, combined with the optional transient policies, and starts a build
		/// operation. Transient policies override any built-in policies, when present.</remarks>
		/// <param name="locator">The locator to be used for this build operation.</param>
		/// <param name="typeToBuild">The type to build.</param>
		/// <param name="idToBuild">The ID of the object to build.</param>
		/// <param name="existing">The existing object to run the build chain on, if one exists.
		/// If null is passed, a new object instance will typically be created by some strategy
		/// in the chain (such as the <see cref="CreationStrategy"/>).</param>
		/// <param name="transientPolicies">The transient policies to apply to this build. These
		/// policies take precedence over any permanent policies built into the builder.</param>
		/// <returns>The built object.</returns>
		object BuildUp(IReadWriteLocator locator, Type typeToBuild, string idToBuild, object existing,
						 params PolicyList[] transientPolicies);

		/// <summary>
		/// Performs a build operation.
		/// </summary>
		/// <remarks>This operation uses the strategies and permanent policies already configured
		/// into the builder, combined with the optional transient policies, and starts a build
		/// operation. Transient policies override any built-in policies, when present.</remarks>
		/// <typeparam name="TTypeToBuild">The type to build.</typeparam>
		/// <param name="locator">The locator to be used for this build operation.</param>
		/// <param name="idToBuild">The ID of the object to build.</param>
		/// <param name="existing">The existing object to run the build chain on, if one exists.
		/// If null is passed, a new object instance will typically be created by some strategy
		/// in the chain (such as the <see cref="CreationStrategy"/>).</param>
		/// <param name="transientPolicies">The transient policies to apply to this build. These
		/// policies take precedence over any permanent policies built into the builder.</param>
		/// <returns>The built object.</returns>
		TTypeToBuild BuildUp<TTypeToBuild>(IReadWriteLocator locator, string idToBuild, object existing,
													params PolicyList[] transientPolicies);

		/// <summary>
		/// Performs an unbuild operation.
		/// </summary>
		/// <typeparam name="TItem">The type to unbuild. If not provided, it will be inferred from the
		/// type of item.</typeparam>
		/// <param name="locator">The locator to be used for this unbuild operation.</param>
		/// <param name="item">The item to unbuild.</param>
		/// <returns>The unbuilt item.</returns>
		TItem TearDown<TItem>(IReadWriteLocator locator, TItem item);
	}
}