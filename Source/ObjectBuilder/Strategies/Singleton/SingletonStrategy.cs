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
	/// Implementation of <see cref="IBuilderStrategy"/> which allows objects to be
	/// singletons.
	/// </summary>
	/// <remarks>
	/// This strategy looks for policies in the context registered under the interface type
	/// <see cref="ISingletonPolicy"/>. It uses the locator in the build context to rememeber
	/// singleton objects, and the lifetime container contained in the locator to ensure they
	/// are not garbage collected. Upon the second request for an object, it will short-circuit
	/// the strategy chain and return the singleton instance (and will not re-inject the
	/// object).
	/// </remarks>
	public class SingletonStrategy : BuilderStrategy
	{
		/// <summary>
		/// Implementation of <see cref="IBuilderStrategy.BuildUp"/>.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <param name="typeToBuild">The type of the object being built.</param>
		/// <param name="existing">The existing instance of the object.</param>
		/// <param name="idToBuild">The ID of the object being built.</param>
		/// <returns>The built object.</returns>
		public override object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			DependencyResolutionLocatorKey key = new DependencyResolutionLocatorKey(typeToBuild, idToBuild);

			if (context.Locator != null && context.Locator.Contains(key, SearchMode.Local))
			{
				TraceBuildUp(context, typeToBuild, idToBuild, "");
				return context.Locator.Get(key);
			}

			return base.BuildUp(context, typeToBuild, existing, idToBuild);
		}
	}
}