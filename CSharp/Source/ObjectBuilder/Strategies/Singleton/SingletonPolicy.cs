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

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Implementation of <see cref="ISingletonPolicy"/>.
	/// </summary>
	public class SingletonPolicy : ISingletonPolicy
	{
		private bool isSingleton;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="isSingleton">Whether the object should be a singleton.</param>
		public SingletonPolicy(bool isSingleton)
		{
			this.isSingleton = isSingleton;
		}

		/// <summary>
		/// See <see cref="ISingletonPolicy.IsSingleton"/> for more information.
		/// </summary>
		public bool IsSingleton
		{
			get { return isSingleton; }
		}
	}
}
