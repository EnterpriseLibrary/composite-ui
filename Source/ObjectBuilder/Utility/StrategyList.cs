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
	/// A custom collection type for multi-stage strategies.
	/// </summary>
	/// <typeparam name="TStageEnum">The enumeration type that describes the policies.</typeparam>
	public class StrategyList<TStageEnum>
	{
		private readonly static Array stageValues = Enum.GetValues(typeof(TStageEnum));
		private Dictionary<TStageEnum, List<IBuilderStrategy>> stages;
		private object lockObject = new object();

		/// <summary>
		/// Initializes a new instance of the <see cref="StrategyList{T}"/> class.
		/// </summary>
		public StrategyList()
		{
			stages = new Dictionary<TStageEnum, List<IBuilderStrategy>>();

			foreach (TStageEnum stage in stageValues)
				stages[stage] = new List<IBuilderStrategy>();
		}

		/// <summary>
		/// Add a strategy to the list.
		/// </summary>
		/// <param name="strategy">The strategy to be added.</param>
		/// <param name="stage">The stage to add the strategy to.</param>
		public void Add(IBuilderStrategy strategy, TStageEnum stage)
		{
			lock (lockObject)
			{
				stages[stage].Add(strategy);
			}
		}

		/// <summary>
		/// Creates a new strategy and adds it to the list.
		/// </summary>
		/// <typeparam name="TStrategy">The strategy type to be created. Must have a parameterless constructor.</typeparam>
		/// <param name="stage">The stage to add the strategy to.</param>
		public void AddNew<TStrategy>(TStageEnum stage)
			where TStrategy : IBuilderStrategy, new()
		{
			lock (lockObject)
			{
				stages[stage].Add(new TStrategy());
			}
		}

		/// <summary>
		/// Clears the strategy list.
		/// </summary>
		public void Clear()
		{
			lock (lockObject)
			{
				foreach (TStageEnum stage in stageValues)
					stages[stage].Clear();
			}
		}

		/// <summary>
		/// Creates a reverse strategy chain based on the strategies in the list. Useful
		/// for unbuild operations (which run strategies in reverse of build operations).
		/// </summary>
		/// <returns>The new strategy chain.</returns>
		public IBuilderStrategyChain MakeReverseStrategyChain()
		{
			lock (lockObject)
			{
				List<IBuilderStrategy> tempList = new List<IBuilderStrategy>();
				foreach (TStageEnum stage in stageValues)
					tempList.AddRange(stages[stage]);

				tempList.Reverse();

				BuilderStrategyChain result = new BuilderStrategyChain();
				result.AddRange(tempList);
				return result;
			}
		}

		/// <summary>
		/// Creates a strategy chain based on the strategies in the list. Useful for
		/// build operations.
		/// </summary>
		/// <returns>The new strategy chain.</returns>
		public IBuilderStrategyChain MakeStrategyChain()
		{
			lock (lockObject)
			{
				BuilderStrategyChain result = new BuilderStrategyChain();

				foreach (TStageEnum stage in stageValues)
					result.AddRange(stages[stage]);

				return result;
			}
		}
	}
}
