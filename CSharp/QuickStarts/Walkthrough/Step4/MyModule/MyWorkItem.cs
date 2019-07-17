using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace MyModule
{
	public class MyWorkItem: WorkItem
	{
		public void Run(IWorkspace tabWorkspace)
		{
			IMyView view = this.Items.AddNew<MyView>();
			MyPresenter presenter = new MyPresenter(view);
			this.Items.Add(presenter);
			tabWorkspace.Show(view);
		}
	}
}
