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

using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Implementation of <see cref="IBuilderStrategyChain"/>.
	/// </summary>
	public class BuilderStrategyChain : IBuilderStrategyChain
	{
		private List<IBuilderStrategy> strategies;

		/// <summary>
		/// Initializes a new instance of the <see cref="BuilderStrategyChain"/> class.
		/// </summary>
		public BuilderStrategyChain()
		{
			strategies = new List<IBuilderStrategy>();
		}

		/// <summary>
		/// See <see cref="IBuilderStrategyChain.Head"/> for more information.
		/// </summary>
		public IBuilderStrategy Head
		{
			get
			{
				if (strategies.Count > 0)
					return strategies[0];
				else
					return null;
			}
		}

		/// <summary>
		/// See <see cref="IBuilderStrategyChain.Add"/> for more information.
		/// </summary>
		public void Add(IBuilderStrategy strategy)
		{
			strategies.Add(strategy);
		}

		/// <summary>
		/// See <see cref="IBuilderStrategyChain.AddRange"/> for more information.
		/// </summary>
		public void AddRange(IEnumerable strategies)
		{
			foreach (IBuilderStrategy strategy in strategies)
				Add(strategy);
		}

		/// <summary>
		/// See <see cref="IBuilderStrategyChain.GetNext"/> for more information.
		/// </summary>
		public IBuilderStrategy GetNext(IBuilderStrategy currentStrategy)
		{
			for (int idx = 0; idx < strategies.Count - 1; idx++)
				if (ReferenceEquals(currentStrategy, strategies[idx]))
					return strategies[idx + 1];

			return null;
		}
	}
}
