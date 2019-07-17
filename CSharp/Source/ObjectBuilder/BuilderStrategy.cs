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
using System.Globalization;
using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// An implementation of <see cref="IBuilderStrategy"/>.
	/// </summary>
	public abstract class BuilderStrategy : IBuilderStrategy
	{
		/// <summary>
		/// Generic version of BuildUp, to aid unit-testing.
		/// </summary>
		public TItem BuildUp<TItem>(IBuilderContext context, TItem existing, string idToBuild)
		{
			return (TItem)BuildUp(context, typeof(TItem), existing, idToBuild);
		}

		/// <summary>
		/// See <see cref="IBuilderStrategy.BuildUp"/> for more information.
		/// </summary>
		public virtual object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			IBuilderStrategy next = context.GetNextInChain(this);

			if (next != null)
				return next.BuildUp(context, typeToBuild, existing, idToBuild);
			else
				return existing;
		}

		/// <summary>
		/// See <see cref="IBuilderStrategy.TearDown"/> for more information.
		/// </summary>
		public virtual object TearDown(IBuilderContext context, object item)
		{
			IBuilderStrategy next = context.GetNextInChain(this);

			if (next != null)
				return next.TearDown(context, item);
			else
				return item;
		}

		/// <summary>
		/// Creates a trace list of parameter types from a list of <see cref="IParameter"/> objects.
		/// </summary>
		/// <param name="parameters">The parameters</param>
		/// <returns>The type list in string form</returns>
		protected string ParametersToTypeList(params object[] parameters)
		{
			List<string> types = new List<string>();

			foreach (object parameter in parameters)
				types.Add(parameter.GetType().Name);

			return string.Join(", ", types.ToArray());
		}

		/// <summary>
		/// Traces debugging information, if there is an appropriate policy.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <param name="typeToBuild">The type being built.</param>
		/// <param name="idToBuild">The ID being built.</param>
		/// <param name="format">The format of the message.</param>
		/// <param name="args">The message arguments.</param>
		protected void TraceBuildUp(IBuilderContext context, Type typeToBuild, string idToBuild, string format, params object[] args)
		{
			IBuilderTracePolicy policy = context.Policies.Get<IBuilderTracePolicy>(null, null);

			if (policy != null)
			{
				string message = string.Format(CultureInfo.CurrentCulture, format, args);
				policy.Trace(Properties.Resources.BuilderStrategyTraceBuildUp, GetType().Name, typeToBuild.Name, idToBuild ?? "(null)", message);
			}
		}

		/// <summary>
		/// Traces debugging information, if there is an appropriate policy.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <param name="item">Item being torn down.</param>
		/// <param name="format">The format of the message.</param>
		/// <param name="args">The message arguments.</param>
		protected void TraceTearDown(IBuilderContext context, object item, string format, params object[] args)
		{
			IBuilderTracePolicy policy = context.Policies.Get<IBuilderTracePolicy>(null, null);

			if (policy != null)
			{
				string message = string.Format(CultureInfo.CurrentCulture, format, args);
				policy.Trace(Properties.Resources.BuilderStrategyTraceTearDown, GetType().Name, item.GetType().Name, message);
			}
		}

		/// <summary>
		/// Determines if tracing is enabled
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <returns>Returns true if tracing is enabled; false otherwise.</returns>
		protected bool TraceEnabled(IBuilderContext context)
		{
			return context.Policies.Get<IBuilderTracePolicy>(null, null) != null;
		}
	}
}
