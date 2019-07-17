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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Microsoft.Practices.CompositeUI.WinForms.Tests.DesignTime
{
	[System.ComponentModel.DesignerCategory("Component")]
	public partial class Component1 : Component
	{
		public Component1()
		{
			InitializeComponent();
		}

		public Component1(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}
	}
}
