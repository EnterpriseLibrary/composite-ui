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
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Simple default creation policy which selects the first public constructor
	/// of an object, using the builder to resolve/create any parameters the
	/// constructor requires.
	/// </summary>
	public class DefaultCreationPolicy : ICreationPolicy
	{
		/// <summary>
		/// See <see cref="ICreationPolicy.SelectConstructor"/> for more information.
		/// </summary>
		public ConstructorInfo SelectConstructor(IBuilderContext context, Type typeToBuild, string idToBuild)
		{
			ConstructorInfo[] constructors = typeToBuild.GetConstructors();

			if (constructors.Length > 0)
				return constructors[0];

			return null;
		}

		/// <summary>
		/// See <see cref="ICreationPolicy.GetParameters"/> for more information.
		/// </summary>
		public object[] GetParameters(IBuilderContext context, Type type, string id, ConstructorInfo constructor)
		{
			ParameterInfo[] parms = constructor.GetParameters();
			object[] parmsValueArray = new object[parms.Length];

			for (int i = 0; i < parms.Length; ++i)
				parmsValueArray[i] = context.HeadOfChain.BuildUp(context, parms[i].ParameterType, null, id);

			return parmsValueArray;
		}
	}
}
