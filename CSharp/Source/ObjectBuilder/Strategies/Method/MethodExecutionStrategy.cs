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
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Implementation of <see cref="IBuilderStrategy"/> which calls methods on an object.
	/// </summary>
	/// <remarks>
	/// This strategy looks for policies in the context registered under the interface type
	/// <see cref="IMethodPolicy"/>, and calls the methods expressed in the policy. If there
	/// is no policy registered, it does not call any methods.
	/// </remarks>
	public class MethodExecutionStrategy : BuilderStrategy
	{
		/// <summary>
		/// Override of <see cref="IBuilderStrategy.BuildUp"/>. Calls methods on the object.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <param name="typeToBuild">The type being built.</param>
		/// <param name="existing">The object on which the methods will be called.</param>
		/// <param name="idToBuild">The ID of the object being built.</param>
		/// <returns></returns>
		public override object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			ApplyPolicy(context, existing, idToBuild);
			return base.BuildUp(context, typeToBuild, existing, idToBuild);
		}

		private void ApplyPolicy(IBuilderContext context, object obj, string id)
		{
			if (obj == null)
				return;

			Type type = obj.GetType();
			IMethodPolicy policy = context.Policies.Get<IMethodPolicy>(type, id);

			if (policy == null)
				return;

			foreach (IMethodCallInfo methodCallInfo in policy.Methods.Values)
			{
				MethodInfo methodInfo = methodCallInfo.SelectMethod(context, type, id);

				if (methodInfo != null)
				{
					object[] parameters = methodCallInfo.GetParameters(context, type, id, methodInfo);
					Guard.ValidateMethodParameters(methodInfo, parameters, obj.GetType());

					if (TraceEnabled(context))
						TraceBuildUp(context, type, id, Properties.Resources.CallingMethod, methodInfo.Name, ParametersToTypeList(parameters));

					methodInfo.Invoke(obj, parameters);
				}
			}
		}
	}
}