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
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace EventBrokerDemo
{
	public partial class ProgressView : Form
	{
		public ProgressView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Listens for on Process complete event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		[EventSubscription("topic://EventBrokerQuickStart/ProcessCompleted", Thread=ThreadOption.UserInterface)]
		public void OnProcessCompleted(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Listens for the Progress Changed event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		[EventSubscription("topic://EventBrokerQuickStart/ProgressChanged", Thread=ThreadOption.UserInterface)]
		public void OnProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			progressWork.Value = e.ProgressPercentage;
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}