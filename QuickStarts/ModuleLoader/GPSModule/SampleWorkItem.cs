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

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace GPSModule
{
	// This is a very simple work item that just shows a view
	public class SampleWorkItem : WorkItem
	{
		protected override void OnRunStarted()
		{
			base.OnRunStarted();
			IWorkspace workspace = Workspaces["MainWorkspace"];
			workspace.Show(Items.AddNew<GPSView>());
		}
	}
}
