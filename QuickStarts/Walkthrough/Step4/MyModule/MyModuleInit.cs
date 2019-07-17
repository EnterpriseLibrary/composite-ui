using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Services;

namespace MyModule
{
	public class MyModuleInit: ModuleInit
	{
		private IWorkItemTypeCatalogService myCatalogService;
		private WorkItem parentWorkItem;

		[ServiceDependency]
		public IWorkItemTypeCatalogService myWorkItemCatalog
		{
			set { myCatalogService = value; }
		}

		[ServiceDependency]
		public WorkItem ParentWorkItem
		{
			set { parentWorkItem = value; }
		}

		public override void Load()
		{
			base.Load();
			MyWorkItem myWorkItem = parentWorkItem.WorkItems.AddNew<MyWorkItem>();
			myWorkItem.Run(parentWorkItem.Workspaces["tabWorkspace1"]);
		}
	}
}
