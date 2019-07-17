//===============================================================================
// Microsoft patterns & practices
// CompositeUI Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.CompositeUI.Utility;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeUI.Tests
{
	public class MockBuilderContext : IBuilderContext
	{
		IReadWriteLocator locator = new Locator();
		BuilderStrategyChain chain = new BuilderStrategyChain();
		PolicyList policies = new PolicyList();

		public MockBuilderContext(params IBuilderStrategy[] strategies)
		{
			Guard.ArgumentNotNull(strategies, "strategies");

			foreach (IBuilderStrategy strategy in strategies)
				chain.Add(strategy);
		}

		public IBuilderStrategy HeadOfChain
		{
			get { return chain.Head; }
		}

		public IBuilderStrategy GetNextInChain(IBuilderStrategy currentStrategy)
		{
			return chain.GetNext(currentStrategy);
		}

		public IReadWriteLocator Locator
		{
			get { return locator; }
		}

		public PolicyList Policies
		{
			get { return policies; }
		}
	}
}