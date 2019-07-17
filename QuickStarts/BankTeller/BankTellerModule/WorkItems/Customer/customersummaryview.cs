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
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Commands;
using Microsoft.Practices.CompositeUI.UIElements;
using BankTellerCommon;
using Microsoft.Practices.ObjectBuilder;

namespace BankTellerModule
{
	[SmartPart]
	public partial class CustomerSummaryView : UserControl
	{
		private CustomerSummaryController controller;

		public CustomerSummaryView()
		{
			InitializeComponent();
		}

		[CreateNew]
		public CustomerSummaryController Controller
		{
			set { controller = value; }
		}

		private void OnSave(object sender, EventArgs e)
		{
			controller.Save();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			controller.WorkItem.UIExtensionSites.RegisterSite(UIExtensionConstants.CUSTOMERCONTEXT, this.customerContextMenu);
		}

		internal void FocusFirstTab()
		{
			this.tabbedWorkspace1.SelectedTab = this.tabbedWorkspace1.TabPages[0];
		}
	}
}
