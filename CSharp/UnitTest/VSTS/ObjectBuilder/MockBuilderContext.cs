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

namespace Microsoft.Practices.ObjectBuilder.Tests
{
	internal class MockBuilderContext : BuilderContext
	{
		public IReadWriteLocator InnerLocator;
		public BuilderStrategyChain InnerChain = new BuilderStrategyChain();
		public PolicyList InnerPolicies = new PolicyList();
		public LifetimeContainer lifetimeContainer = new LifetimeContainer();

		public MockBuilderContext()
			: this(new Locator())
		{
		}

		public MockBuilderContext(IReadWriteLocator locator)
		{
			InnerLocator = locator;
			SetLocator(InnerLocator);
			StrategyChain = InnerChain;
			SetPolicies(InnerPolicies);

			if (!Locator.Contains(typeof(ILifetimeContainer)))
				Locator.Add(typeof(ILifetimeContainer), lifetimeContainer);
		}
	}
}