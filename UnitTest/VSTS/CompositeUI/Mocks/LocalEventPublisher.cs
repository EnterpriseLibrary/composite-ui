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
using System.ComponentModel;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace Microsoft.Practices.CompositeUI.Tests.Mocks
{
	[System.ComponentModel.DesignerCategory("Code")]
	public class LocalEventPublisher : Component
	{
		[EventPublication("LocalEvent", PublicationScope.WorkItem)]
		public event EventHandler Event;

		public void FireTheEventHandler()
		{
			if (Event != null)
			{
				Event(this, EventArgs.Empty);
			}
		}

		public bool EventIsNull
		{
			get { return Event == null; }
		}
	}


	public class LocalObjectEventPublisher
	{
		[EventPublicationAttribute("LocalEvent", PublicationScope.WorkItem)]
		public event EventHandler Event;

		public void FireTheEventHandler()
		{
			if (Event != null)
			{
				Event(this, EventArgs.Empty);
			}
		}

		public bool EventIsNull
		{
			get { return Event == null; }
		}
	}
}