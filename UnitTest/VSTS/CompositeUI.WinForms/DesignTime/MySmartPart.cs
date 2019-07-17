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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Microsoft.Practices.CompositeUI.WinForms.Tests.DesignTime
{
	[SmartPart]
	public partial class MySmartPart : UserControl, ISmartPartInfoProvider
	{
		public MySmartPart()
		{
			InitializeComponent();
		}

		public ISmartPartInfo GetSmartPartInfo(Type smartPartInfoType)
		{
			// The containing smart part must add the ISmartPartInfo to its implemented interfaces in order for contained smart part infos to work.
			Microsoft.Practices.CompositeUI.SmartParts.ISmartPartInfoProvider ensureProvider = this;
			return this.infoProvider.GetSmartPartInfo(smartPartInfoType);
		}
	}
}
