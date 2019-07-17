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

using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.UIElements;
using Microsoft.Practices.CompositeUI.WinForms;
using Microsoft.Practices.ObjectBuilder;

namespace CommandsQuickStart
{
	public class MainWorkItem : WorkItem
	{
		protected override void OnRunStarted()
		{
			base.OnRunStarted();
			Items.AddNew<MainController>();
			ProcessCommandMap();
			Activate();
		}

		/// Loads the comand configuration file, and processes
		/// each element in the file. Each element will become a menu
		/// item registered with the UIElement service, attached to the
		/// extension site defined in the XML file. (Note: For this example
		/// they are all added to the extension site named "File" that was added
		/// in CommandsApplication.AfterShellCreated.
		private void ProcessCommandMap()
		{
			CommandMap map = LoadCommandMap();

			foreach (Mapping mapping in map.Mapping)
			{
				ToolStripMenuItem item = CreateMenuItemForMapping(mapping);
				UIExtensionSites[mapping.Site].Add(item);
				Commands[mapping.CommandName].AddInvoker(item, "Click");
			}
		}

		/// <summary>
		/// Creates a new ToolStripMenuItem for the given Mapping from the XML file.
		/// </summary>
		/// <param name="mapping"></param>
		/// <returns></returns>
		private ToolStripMenuItem CreateMenuItemForMapping(Mapping mapping)
		{
			ToolStripMenuItem item = new ToolStripMenuItem();
			item.Text = mapping.Label;
			return item;
		}

		/// <summary>
		/// Loads the command map from the configuration file.
		/// </summary>
		/// <returns></returns>
		private CommandMap LoadCommandMap()
		{
			CommandMap map;
			using (Stream stream = new FileStream("SampleCommandMap.xml", FileMode.Open, FileAccess.Read))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(CommandMap));
				map = (CommandMap)serializer.Deserialize(stream);
			}
			return map;
		}
	}
}
