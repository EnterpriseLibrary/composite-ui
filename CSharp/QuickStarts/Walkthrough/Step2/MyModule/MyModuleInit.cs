using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Services;
using System.Windows.Forms;

namespace MyModule
{
	public class MyModuleInit: ModuleInit
	{
		private IWorkItemTypeCatalogService myCatalogService;

		[ServiceDependency]
		public IWorkItemTypeCatalogService myWorkItemCatalog
		{
			set { myCatalogService = value; }
		}

		public override void Load()
		{
			base.Load();
			myCatalogService.RegisterWorkItem<MyWorkItem>();
			MessageBox.Show("Hello there!");
		}
	}
}
