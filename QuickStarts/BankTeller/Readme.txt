* What Is It Intended to Show?

This QuickStart is a single example of all the primary CAB features used
together in one simple application.

* How Do I Use It?

Build and run the BankTeller WinForm application. The application starts
by showing a main form containing menu items.

* What Should I Look At?

Since this QuickStart is filled with examples of all the CAB features it is
advantageous to look at everything.

* General Design Philosophy

The purpose of this QuickStart is to emulate building a large application
with multiple teams.

The application is a general banking application. The shell provides the
area where specific modules of the application are presented. In
this release, we have provided a single "bank teller" module, but you should
imagine that in a full featured version of this application, there would
be many role-specific modules.

Architecturally, the development teams would be responsible for a single
module (for example, a bank teller team, a bank manager team) and share the
common shell. The shell would choose the module based on the current users
primary role (and in some cases, may offer empowered users the option to
switch between multiple views so they can perform many tasks).

* MVC and WorkItems

CAB helps you write MVC-style applications by providing some framework
classes. The files listed in the BusinessEntities folder in the BankTellerModule
represent the data models for the application. In the WorkItems folder
you will find views and view/controller pairs. Some views are simple
enough that they don't need a controller (this will happen less
often in the real world, obviously).

The WorkItem classes represent use cases. Often, a single work item will
encompass many view/controller pairs and some data models. Ideally, WorkItems
should be relatively self contained with as few upward dependencies
as achievable; this lets you re-use WorkItems across the application when
appropriate.

* SmartParts

Views which are "container aware" are called SmartParts. A view tells CAB
that it is a SmartPart by using the [SmartPart] attribute. This allows a view
to express its service and component dependencies and have those dependencies
automatically wired up for it when it is placed in the container.

* Services

The QuickStart simulates the availability of data model services using
simple static data. In a real system, these services would talk to a real
back end data store, such as a database or 3rd party data provider.

* Persistence

To illustrate persistence, we allow the user to save the changes they've
made to a customer. These settings are saved into isolated storage.

* Current Limitations

The system currently stores the entire menu structure in app.config. In the
future, we would like to evolve a more powerful menuing system that allows
the shell and modules to cooperate on how the application menu looks.

For more information about the BankTeller QuickStart, see the QuickStarts
section in the application block documentation.
