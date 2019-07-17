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
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Commands;

namespace CommandsQuickStart
{
	// This is the controller for the main work item
	// and holds all commands handlers for this sample quickstart.
	public class MainController : Controller
	{
		[CommandHandler("ShowCustomer")]
		public void ShowCustomerHandler(object sender, EventArgs e)
		{
			MessageBox.Show("Show Customer");
		}

		[CommandHandler("EnableShowCustomer")]
		public void EnableShowCustomerHandler(object sender, EventArgs e)
		{
			WorkItem.Commands["ShowCustomer"].Status = CommandStatus.Enabled;
		}

		[CommandHandler("DisableShowCustomer")]
		public void DisableShowCustomer(object sender, EventArgs e)
		{
			WorkItem.Commands["ShowCustomer"].Status = CommandStatus.Disabled;
		}

		[CommandHandler("HideShowCustomer")]
		public void HideShowCustomer(object sender, EventArgs e)
		{
			WorkItem.Commands["ShowCustomer"].Status = CommandStatus.Unavailable;
		}
	}
}
