//===============================================================================
// Microsoft patterns & practices
// CompositeUI Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace SmartPartQuickStart.ViewCustomerWorkItem
{
	/// <summary>
	/// The SmartPart attribute tells the SmartPartMonitor to add this view 
	/// to the WorkItem.
	/// </summary>
	[SmartPart]
	public partial class CustomerTabView : UserControl
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public CustomerTabView()
		{
			InitializeComponent();
		}

	}
}
