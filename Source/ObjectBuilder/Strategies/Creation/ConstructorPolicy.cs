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
	/// A creation policy where the constructor to choose is derived from the parameters
	/// provided by the user.
	/// </summary>
	public class ConstructorPolicy : ICreationPolicy
	{
		ConstructorInfo constructor;
		List<IParameter> parameters = new List<IParameter>();

		/// <summary>
		/// Initializes a new instance of the <see cref="ConstructorPolicy"/> object.
		/// </summary>
		public ConstructorPolicy() { }

		/// <summary>
		/// Initializes an instance of <see cref="ConstructorPolicy"/> using the provided
		/// parameters. Will use reflection to discover the constructor to call.
		/// </summary>
		/// <param name="parameters">The parameters to pass to the constructor.</param>
		public ConstructorPolicy(params IParameter[] parameters)
		{
			foreach (IParameter parameter in parameters)
				AddParameter(parameter);
		}

		/// <summary>
		/// Initializes an instance of <see cref="ConstructorPolicy"/> using the provided
		/// <see cref="ConstructorInfo"/> and parameters.
		/// </summary>
		/// <param name="constructor">The constructor to call.</param>
		/// <param name="parameters">The parameters to pass to the constructor.</param>
		public ConstructorPolicy(ConstructorInfo constructor, params IParameter[] parameters)
			: this(parameters)
		{
			this.constructor = constructor;
		}

		/// <summary>
		/// Adds a parameter to the list of parameters used to find the constructor.
		/// </summary>
		/// <param name="parameter">The parameter to add.</param>
		public void AddParameter(IParameter parameter)
		{
			parameters.Add(parameter);
		}

		/// <summary>
		/// See <see cref="ICreationPolicy.SelectConstructor"/> for more information.
		/// </summary>
		public ConstructorInfo SelectConstructor(IBuilderContext context, Type type, string id)
		{
			if (constructor != null)
				return constructor;

			List<Type> types = new List<Type>();

			foreach (IParameter parm in parameters)
				types.Add(parm.GetParameterType(context));

			return type.GetConstructor(types.ToArray());
		}

		/// <summary>
		/// See <see cref="ICreationPolicy.GetParameters"/> for more information.
		/// </summary>
		public object[] GetParameters(IBuilderContext context, Type type, string id, ConstructorInfo constructor)
		{
			List<object> results = new List<object>();

			foreach (IParameter parm in parameters)
				results.Add(parm.GetValue(context));

			return results.ToArray();
		}
	}
}
