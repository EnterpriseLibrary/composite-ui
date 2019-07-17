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

using System.Collections.Generic;
namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// An implementation of <see cref="IMethodPolicy"/>.
	/// </summary>
	public class MethodPolicy : IMethodPolicy
	{
		private Dictionary<string, IMethodCallInfo> methods = new Dictionary<string, IMethodCallInfo>();

		/// <summary>
		/// See <see cref="IMethodPolicy.Methods"/> for more information.
		/// </summary>
		public Dictionary<string, IMethodCallInfo> Methods
		{
			get { return methods; }
		}
	}
}
