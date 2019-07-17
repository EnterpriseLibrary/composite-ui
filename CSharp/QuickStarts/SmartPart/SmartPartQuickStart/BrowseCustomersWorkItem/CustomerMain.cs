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
using Microsoft.Practices.CompositeUI.SmartParts;
using System.Collections.ObjectModel;

namespace SmartPartQuickStart.BrowseCustomersWorkItem
{
	/// <summary>
	/// The SmartPart attribute tells the SmartPartMonitor to add this view 
	/// to the WorkItem.
	/// </summary>
	[SmartPart]
	public partial class CustomerMain : UserControl, IWorkspace
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public CustomerMain()
		{
			InitializeComponent();
		}

		// You can make anything a workspace by implementing the
		// IWorkspace interface.
		#region IWorkspace Members

		/// <summary>
		/// Fired when a smartpart is closing.
		/// </summary>
		public event EventHandler<WorkspaceCancelEventArgs> SmartPartClosing;

		/// <summary>
		/// Fires when the smartpart is activated.
		/// </summary>
		public event EventHandler<WorkspaceEventArgs> SmartPartActivated
		{
			add { throw new Exception("The method or operation is not implemented."); }
			remove { throw new Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// Shows the given smartpart in a workspace with the 
		/// given smartpartinfo which provides additional information.
		/// </summary>
		/// <param name="smartPart"></param>
		/// <param name="smartPartInfo"></param>
		public void Show(object smartPart, ISmartPartInfo smartPartInfo)
		{
			this.customersDeckedWorkspace.Show(smartPart, smartPartInfo);
		}

		/// <summary>
		/// Shows the given smartpart in a workspace.
		/// </summary>
		/// <param name="smartPart"></param>
		public void Show(object smartPart)
		{
			this.customersDeckedWorkspace.Show(smartPart);
		}

		/// <summary>
		/// Hides the given smartpart.
		/// </summary>
		/// <param name="smartPart"></param>
		public void Hide(object smartPart)
		{
			this.customersDeckedWorkspace.Hide(smartPart);
		}

		/// <summary>
		/// Closes the given smartpart.
		/// </summary>
		/// <param name="smartPart"></param>
		public void Close(object smartPart)
		{
			SmartPartClosing(this, new WorkspaceCancelEventArgs(smartPart));

			this.customersDeckedWorkspace.Close(smartPart);
		}

		#endregion


		#region IWorkspace Members


		public ReadOnlyCollection<object> SmartParts
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public object ActiveSmartPart
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public void Activate(object smartPart)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void ApplySmartPartInfo(object smartPart, ISmartPartInfo smartPartInfo)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public bool Contains(object smartPart)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public bool IsActive(object smartPart)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

	}
}
