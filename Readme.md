# Composite UI Application Block December 2005 (For C#)

The Composite UI Application Block is a source code-based component built on the Microsoft .NET Framework 2.0 that provides proven practices to build complex smart client user interface. It is based on proven design patterns in which rich and complex user interface solutions can be built out of simpler user interface parts that can be independently developed, tested, versioned, and deployed.

It provides guidance on the architecture of your solution leveraging platform features of the .NET Framework including Windows Forms and ClickOnce.

The application block is designed to separate the different parts of software development, enabling each developer or team to concentrate on their area of expertise. For example, business logic, infrastructure components, or user interface components. The application block provides a framework that shell developers can use to link all of these distinct parts of the application together into a loosely-coupled, yet collaborating set of components to create a fully functioning application.

For more information about the Composite UI Application Block, see the main documentation installed with the application block.

## **Introduction to This Release**

This is the first released version of the Composite UI Application Block. There are currently no plans to develop further versions of it. Information about the application block, related content and future releases will be available on the
[workspace](http://practices.gotdotnet.com/projects/cab). This application block is licensed under
the terms described in the included EULA.rtf file.

## **How to Use the QuickStarts**

This version of the application block includes a number of QuickStarts. Each one has a Readme.txt file that explains its purpose and provides a simple guide on how to use it. 

To run any of the QuickStarts:

1.On the taskbar, click **Start**, point to **Programs**, point
to **Microsoft patterns & practices**, point to **Composite UI
Application Block December 2005 (C#)**, point to &lt;_name of QuickStart_&gt;,
and then click &lt;_name of QuickStart_&gt;.

2.Rebuild the solution.

3.Review the information in the Readme.txt file included in the
solution directory.

4.Review the execution of the QuickStart using the instructions in the
QuickStarts section of the Composite UI Application Block documentation.

_The example companies, organizations, products, domain names, e-mail addresses, logos, people, places, and events depicted herein are fictitious. No association with any real company, organization, product, domain name, email address, logo, person, places, or events is intended or should be inferred."_

  
## **How to Submit Feedback**

Feedback is very important to us. We believe that by using this first release of the Composite UI Application Block, you will have a very powerful instrument to experiment with the application block patterns and design; you can compare those to your own requirements and needs. We usually modify the design of the application block based on the feedback we receive.

You can send feedback to [devfdbk@microsoft.com]()
or post it on the workspace: [http://practices.gotdotnet.com/projects/cab](http://practices.gotdotnet.com/projects/cab).

If you decide to blog about this release, please send us the link to your post, and we will add references to it from our own.

## **FAQ**

We have compiled this list of frequently asked questions for you. If you have any other inquires; feel free to send an e-mail message to the address in the preceding section. We will update the FAQ based on the feedback received.

### **Can I blog about this?**

Yes. Send us your post!

### **Is this tested?**

The application block was developed using a test driven approach. You can see the tests in the appropriate UnitTests folder in the main solution, if you chose to install them. We have performed security reviews and performance testing. Extensive functional testing was also performed by our test teams.

### **Can I start building on this now?**

Yes you can.

### **I'm building on .NET 1.1 (Everett)—can I use this block?**

The Composite UI Application Block is a based on the .NET Framework 2.0. You can reuse the concepts and design patterns implemented in the application block and do a back port to the .NET Framework 1.1.

### **Does the block support NUnit tests?**
### **Does the block support Visual Studio Team System tests?**

The unit tests in the Composite UI Application Block were built using NUnit 2.2.0 and Visual Studio Team System unit tests. During the setup process you will prompted to choose whether you want NUnit tests, VSTS Tests or both.

### **What will happen with the UIP Application Block?**

The current plan is to split the User Interface Process (UIP) Application Block into two different assets. A new version of UIP for the Web is scheduled to ship some time in the coming year. Our current thinking for UIP WinForms is to implement it as a Workflow Foundation service in the Composite UI Application Block in a follow-on release.

## **Known Issues**

The following issues have been identified in this release.

### **Possible Data Loss When Loading State**

Under very specific circumstances it is possible to lose data that was placed into a WorkItem’s State when performing Load and Set operations from multiple threads. It is important, therefore, if you need to call Load, always ensure that Load has been called on the WorkItem before any attempt is made to change or set values in the State property.

### **Manually Added Command Invokers Need To Be Cleaned Up Manually**

If a command exist in a parent WorkItem, and an invoker is manually added to that command object from a child WorkItem, when the child WorkItem is terminated, the invoker is not removed because the command is in the parent WorkItem.

As a work-around, when manually adding invokers to a parent WorkItem’s commands, listen to the Terminating event on the child WorkItem and clean up the invoker manually.

### **Possible Missing Reference To Unit Testing Frameworks**

If you choose to install either the VSTS or NUnit unit tests during your installation, you must ensure that you have the requisite unit testing frameworks properly installed on your system or the unit test projects may have missing references when opened in Visual Studio.