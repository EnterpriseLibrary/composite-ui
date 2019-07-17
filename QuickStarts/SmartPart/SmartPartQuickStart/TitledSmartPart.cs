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

namespace SmartPartQuickStart
{
	/// <summary>
	/// Base class for building other smartparts
	/// </summary>
	[SmartPart]
	public partial class TitledSmartPart : UserControl
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public TitledSmartPart()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Text that will show in the title label.
		/// </summary>
		public string Title
		{
			get
			{
				return titleLabel.Text;
			}
			set
			{
				titleLabel.Text = value;
			}
		}

		/// <summary>
		/// Tooltip for the smartpart.
		/// </summary>
		public string Description
		{
			get
			{
				return toolTip.GetToolTip(titleLabel);
			}
			set
			{
				toolTip.SetToolTip(titleLabel, value);
			}
		}
	}
}
