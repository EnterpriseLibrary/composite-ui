using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI;
using BankTellerModule;
using Microsoft.Practices.CompositeUI.WinForms;
using Microsoft.Practices.CompositeUI.SmartParts;
using System.Windows.Forms;
using BankTellerCommon;
using Microsoft.Practices.CompositeUI.UIElements;

namespace CustomerMapExtensionModule
{
	[WorkItemExtension(typeof(CustomerWorkItem))]
	public class CustomerWorkItemExtension : WorkItemExtension
	{
		private CustomerMap mapView;

		protected override void OnActivated()
		{
			if (mapView == null)
			{
				mapView = WorkItem.Items.AddNew<CustomerMap>();

				TabSmartPartInfo info = new TabSmartPartInfo();
				info.Title = "Customer Map";
				info.Description = "Map of the customer location";
				WorkItem.Workspaces[CustomerWorkItem.CUSTOMERDETAIL_TABWORKSPACE].Show(mapView, info);
			}
		}
	}
}
