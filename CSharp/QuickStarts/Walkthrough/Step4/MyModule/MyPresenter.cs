using System;
using System.Collections.Generic;
using System.Text;

namespace MyModule
{
	public class MyPresenter
	{
		IMyView view;

		public MyPresenter(IMyView view)
		{
			this.view = view;
			view.Load += new EventHandler(view_Load);
		}

		void view_Load(object sender, EventArgs e)
		{
			view.Message = "Hello World from a Module";
		}
	}
}
