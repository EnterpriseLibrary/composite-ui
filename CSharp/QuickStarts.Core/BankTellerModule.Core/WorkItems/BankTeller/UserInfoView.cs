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

using System.Threading;
using System.Windows.Forms;

namespace BankTellerModule
{
	public partial class UserInfoView : UserControl
	{
		public UserInfoView()
		{
			InitializeComponent();

			Principal.Text = Thread.CurrentPrincipal.Identity.Name;
		}
	}
}
