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
using System.Globalization;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder
{
	internal static class Guard
	{
		public static void TypeIsAssignableFromType(Type assignee, Type providedType, Type classBeingBuilt)
		{
			if (!assignee.IsAssignableFrom(providedType))
				throw new IncompatibleTypesException(string.Format(CultureInfo.CurrentCulture,
					Properties.Resources.TypeNotCompatible, assignee, providedType, classBeingBuilt));
		}

		public static void ValidateMethodParameters(MethodBase methodInfo, object[] parameters, Type typeBeingBuilt)
		{
			ParameterInfo[] paramInfos = methodInfo.GetParameters();

			for (int i = 0; i < paramInfos.Length; i++)
				if( parameters[i] != null )
					Guard.TypeIsAssignableFromType(paramInfos[i].ParameterType, parameters[i].GetType(), typeBeingBuilt);
		}
	}
}
