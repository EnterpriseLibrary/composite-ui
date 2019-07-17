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
	class GlobalEventPublisher : Component
	{
		[EventPublication("GlobalEvent", PublicationScope.Global)]
		public event EventHandler Event;

		public void OnEvent()
		{
			if (Event != null)
			{
				Event(this, new EventArgs());
			}
		}
	}

	class GlobalObjectEventPublisher
	{
		[EventPublication("GlobalEvent", PublicationScope.Global)]
		public event EventHandler Event;

		public void OnEvent()
		{
			if (Event != null)
			{
				Event(this, new EventArgs());
			}
		}
	}
}