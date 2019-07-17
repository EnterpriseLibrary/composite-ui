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
using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// An implementation helper class for <see cref="IBuilder{TStageEnum}"/>.
	/// </summary>
	/// <typeparam name="TStageEnum">The build stage enumeration.</typeparam>
	public class BuilderBase<TStageEnum> : IBuilder<TStageEnum>
	{
		private PolicyList policies = new PolicyList();
		private StrategyList<TStageEnum> strategies = new StrategyList<TStageEnum>();
		private Dictionary<object, object> lockObjects = new Dictionary<object, object>();

		/// <summary>
		/// Initializes a new instance of the <see cref="BuilderBase{T}"/> class.
		/// </summary>
		public BuilderBase()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BuilderBase{T}"/> class using the
		/// provided configurator.
		/// </summary>
		/// <param name="configurator">The configurator that will configure the builder.</param>
		public BuilderBase(IBuilderConfigurator<TStageEnum> configurator)
		{
			configurator.ApplyConfiguration(this);
		}

		/// <summary>
		/// See <see cref="IBuilder{TStageEnum}.Policies"/> for more information.
		/// </summary>
		public PolicyList Policies
		{
			get { return policies; }
		}

		/// <summary>
		/// See <see cref="IBuilder{TStageEnum}.Strategies"/> for more information.
		/// </summary>
		public StrategyList<TStageEnum> Strategies
		{
			get { return strategies; }
		}

		/// <summary>
		/// See <see cref="IBuilder{TStageEnum}.BuildUp{T}"/> for more information.
		/// </summary>
		public TTypeToBuild BuildUp<TTypeToBuild>(IReadWriteLocator locator,
															 string idToBuild, object existing, params PolicyList[] transientPolicies)
		{
			return (TTypeToBuild)BuildUp(locator, typeof(TTypeToBuild), idToBuild, existing, transientPolicies);
		}

		/// <summary>
		/// See <see cref="IBuilder{TStageEnum}.BuildUp"/> for more information.
		/// </summary>
		public virtual object BuildUp(IReadWriteLocator locator, Type typeToBuild,
											 string idToBuild, object existing, params PolicyList[] transientPolicies)
		{
			if (locator != null)
			{
				lock (GetLock(locator))
				{
					return DoBuildUp(locator, typeToBuild, idToBuild, existing, transientPolicies);
				}
			}
			else
			{
				return DoBuildUp(locator, typeToBuild, idToBuild, existing, transientPolicies);
			}

		}

		private object DoBuildUp(IReadWriteLocator locator, Type typeToBuild, string idToBuild, object existing,
			PolicyList[] transientPolicies)
		{
			IBuilderStrategyChain chain = strategies.MakeStrategyChain();
			ThrowIfNoStrategiesInChain(chain);

			IBuilderContext context = MakeContext(chain, locator, transientPolicies);
			IBuilderTracePolicy trace = context.Policies.Get<IBuilderTracePolicy>(null, null);

			if (trace != null)
				trace.Trace(Properties.Resources.BuildUpStarting, typeToBuild, idToBuild ?? "(null)");
			
			object result = chain.Head.BuildUp(context, typeToBuild, existing, idToBuild);

			if (trace != null)
				trace.Trace(Properties.Resources.BuildUpFinished, typeToBuild, idToBuild ?? "(null)");
			
			return result;
		}

		private IBuilderContext MakeContext(IBuilderStrategyChain chain,
														IReadWriteLocator locator, params PolicyList[] transientPolicies)
		{
			PolicyList policies = new PolicyList(this.policies);

			foreach (PolicyList policyList in transientPolicies)
				policies.AddPolicies(policyList);

			return new BuilderContext(chain, locator, policies);
		}

		private static void ThrowIfNoStrategiesInChain(IBuilderStrategyChain chain)
		{
			if (chain.Head == null)
				throw new InvalidOperationException(Properties.Resources.BuilderHasNoStrategies);
		}

		/// <summary>
		/// See <see cref="IBuilder{TStageEnum}.TearDown{T}"/> for more information.
		/// </summary>
		public TItem TearDown<TItem>(IReadWriteLocator locator, TItem item)
		{
			if (typeof(TItem).IsValueType == false && item == null)
				throw new ArgumentNullException("item");

			if (locator != null)
			{
				lock (GetLock(locator))
				{
					return DoTearDown<TItem>(locator, item);
				}
			}
			else
			{
				return DoTearDown<TItem>(locator, item);
			}
		}

		private TItem DoTearDown<TItem>(IReadWriteLocator locator, TItem item)
		{
			IBuilderStrategyChain chain = strategies.MakeReverseStrategyChain();
			ThrowIfNoStrategiesInChain(chain);

			Type type = item.GetType();
			IBuilderContext context = MakeContext(chain, locator);
			IBuilderTracePolicy trace = context.Policies.Get<IBuilderTracePolicy>(null, null);

			if (trace != null)
				trace.Trace(Properties.Resources.TearDownStarting, type);

			TItem result = (TItem)chain.Head.TearDown(context, item);

			if (trace != null)
				trace.Trace(Properties.Resources.TearDownFinished, type);

			return result;
		}

		private object GetLock(object locator)
		{
			lock (lockObjects)
			{
				if (lockObjects.ContainsKey(locator))
					return lockObjects[locator];

				object newLock = new object();
				lockObjects[locator] = newLock;
				return newLock;
			}
		}
	}
}
