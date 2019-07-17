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
	/// Implementation of <see cref="IPropertySetterPolicy"/>.
	/// </summary>
	public class PropertySetterPolicy : IPropertySetterPolicy
	{
		private Dictionary<string, IPropertySetterInfo> properties = new Dictionary<string, IPropertySetterInfo>();

		/// <summary>
		/// See <see cref="IPropertySetterPolicy.Properties"/> for more information.
		/// </summary>
		public Dictionary<string, IPropertySetterInfo> Properties
		{
			get { return properties; }
		}
	}
}
