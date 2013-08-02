ThreeSixtySharp
===============

ThreeSixtySharp is a C# library for working with Autodesk's BIM 360 Field Rest API.  The goal of this library is to present client code with easy to work with objects without dealing with a lot of the webby bits.  It is possible this could be expanded to work with Glue as well in the future.


##Basic Usage

A "ticket" is used with all requests as the means of authentication.  The ticket is obtained through a user's username and password:

```C#
ThreeSixtySharp ts = new ThreeSixtySharp(username, password);
AuthTicket ticket = ts.GetTicket();
```

Once you have a ticket, you can get a list of project objects that the user has access to:

```C#
List<Project> projects = ts.GetProjects(ticket);
```

The Project object has a lot of properties, some of which are containers for other objects.  Review the source to see everything that is accessible through the Project.  

Currently this library mainly deals with the "library" realm of BIM 360.  To get a list of File objects:

```C#
//You need to pass in one Project object.  This example is using the first one from the example
//above.  If you are using System.IO you will need to use the full name of the File 
//object ThreeSixtySharp.Objects.File.
List<File> files = ts.GetAllFiles(ticket, projects[0]);
```

If you want to upload a file:

```C#
string my_file_path = "C:\\myfile.3dm";
//The Project object you want to upload to has a Document_Paths property that is a list 
//of Document_Path instances.  This returns a File object as well containing the metadata 
//of the file you just uploaded.
ThreeSixtySharp.Objects.File upload_file = ts.Publish(ticket, 
                                                      projects[0], 
                                                      projects[0].Document_Paths[0],
                                                      my_file_path);
```

There is also an asynchronous version of Publish, ThreeSixtySharpBase.PublishAsync if you are programming a UI that you don't want to block. There are other methods and properties that I will be documenting as well as adding support for the rest of the API in the future.

#### Disclaimers

Use with caution, make sure you test things on a test project and understand what will happen.

Also as Autodesk is inclined to "protect  their copyrights", this library is in no way affiliated with Autodesk or 360 BIM etc.

