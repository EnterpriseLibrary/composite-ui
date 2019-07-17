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
	/// Indicates the search mode to be used for locator queries
	/// </summary>
	public enum SearchMode
	{
		/// <summary>
		/// Search local first and then up the containment heirarchy as needed
		/// </summary>
		Up,

		/// <summary>
		/// Search local only
		/// </summary>
		Local
	}
}