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

using Microsoft.Practices.CompositeUI.BuilderStrategies;
using Microsoft.Practices.CompositeUI.Commands;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeUI.Tests
{
	public class TestableRootWorkItem : WorkItem
	{
		public TestableRootWorkItem()
		{
			InitializeRootWorkItem(CreateBuilder());

			Services.AddNew<CommandAdapterMapService, ICommandAdapterMapService>();
			Services.AddNew<TraceSourceCatalogService, ITraceSourceCatalogService>();

			TestableAddServices();

			BuildUp();
		}

		protected virtual void TestableAddServices()
		{
		}

		public Builder Builder
		{
			get { return InnerBuilder; }
		}

		public IReadWriteLocator Locator
		{
			get { return InnerLocator; }
		}

		private Builder CreateBuilder()
		{
			Builder builder = new Builder();

			builder.Strategies.AddNew<EventBrokerStrategy>(BuilderStage.Initialization);
			builder.Strategies.AddNew<CommandStrategy>(BuilderStage.Initialization);
            builder.Strategies.AddNew<ObjectBuiltNotificationStrategy>(BuilderStage.PostInitialization);

            builder.Policies.SetDefault<ObjectBuiltNotificationPolicy>(new ObjectBuiltNotificationPolicy());
            builder.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			return builder;
		}
	}
}
