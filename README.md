SimpleInjector AssemblyScanner
===================

What is this repository for?
-------------
I was using simpleinjector for one of my projects and the default auto-registration mentioned in the [documentation](https://simpleinjector.readthedocs.org/en/latest/advanced.html#batch-registration).  did not fill all my needs. For example, what if your class has multiple interfaces (like IDisposable), then Single() isn't going to cut it.

Build status| Coverage Status| NuGet downloads
----------- | -------------- | ---------------
[![](https://ci.appveyor.com/api/projects/status/080nvsbtt264y6bf?svg=true)](https://ci.appveyor.com/project/jeroenpot/simpleinjector-assemblyscanner)|[![](https://coveralls.io/repos/jeroenpot/SimpleInjector-AssemblyScanner/badge.svg?branch=master&service=github)](https://coveralls.io/github/jeroenpot/SimpleInjector-AssemblyScanner?branch=master)|[![](https://img.shields.io/nuget/dt/Simpleinjector.AssemblyScanner.svg)](https://www.nuget.org/packages/SimpleInjector.AssemblyScanner/)


How do i use it?
-------------
This will scan the assembly you gave me and try to register all classes that have an interface, but it will igore the ones which implement IIWantYouNotToLookForThisOne or are of type NorThisOne. Because ISayToTheWorld is already registered, this will also be ignored.

```C#
var container = new Container();
// Register class with string argument, which cannot be resolved.
container.Register<ISayToTheWorld>(() => new SayToTheWorld("hello"));

DependencyRegistration.Register(container, typeof(ATypeInYourAssembly).Assembly, typeof(IIWantYouNotToLookForThisOne), typeof(NorThisOne));
```

How do i get it?
-------------
Clone this repository or copy the class DependencyRegistration.cs, this contains most of the logic.
Or get the nuget package
```sh
Install-Package JeroenPot.SimpleInjector.AssemblyScanner
```

How can i find what types are registered?
-------------

There is a logwriter class that outputs all the registerd classes with Debug.WriteLine. If you want other output, implement your own ILogWriter and add this code before calling the Register Method:

```C#
DependencyRegistration.LogWriter = new MyOwnLogWriterImplementation();
```

I found a bug!
-------------
Make a pull request or register and issue, i'll fix it asap :)

Contribution guidelines
-------------
* Pull request should be made to develop branch.
* Comments, methods and variables in english.
* Create unittests where possible.
* Try to stick to the existing coding style.
* Give a short description in the pull request what you're doing and why.

