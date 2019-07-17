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
using System.Text;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Implementation of <see cref="IBuilderStrategy"/> which will notify an object about
	/// the completion of a <see cref="IBuilder{T}.BuildUp"/> operation, or start of a
	/// <see cref="IBuilder{T}.TearDown"/> operation.
	/// </summary>
	/// <remarks>
	/// This strategy checks the object that is passing through the builder chain to see if it
	/// implements IBuilderAware and if it does, it will call <see cref="IBuilderAware.OnBuiltUp"/>
	/// and <see cref="IBuilderAware.OnTearingDown"/>. This strategy is meant to be used from the
	/// <see cref="BuilderStage.PostInitialization"/> stage.
	/// </remarks>
	public class BuilderAwareStrategy : BuilderStrategy
	{
		/// <summary>
		/// See <see cref="IBuilderStrategy.BuildUp"/> for more information.
		/// </summary>
		public override object BuildUp(IBuilderContext context, Type t, object existing, string id)
		{
			IBuilderAware awareObject = existing as IBuilderAware;

			if (awareObject != null)
			{
				TraceBuildUp(context, t, id, Properties.Resources.CallingOnBuiltUp);
				awareObject.OnBuiltUp(id);
			}

			return base.BuildUp(context, t, existing, id);
		}

		/// <summary>
		/// See <see cref="IBuilderStrategy.TearDown"/> for more information.
		/// </summary>
		public override object TearDown(IBuilderContext context, object item)
		{
			IBuilderAware awareObject = item as IBuilderAware;
			
			if (awareObject != null)
			{
				TraceTearDown(context, item, Properties.Resources.CallingOnTearingDown);
				awareObject.OnTearingDown();
			}
			
			return base.TearDown(context, item);
		}
	}
}
