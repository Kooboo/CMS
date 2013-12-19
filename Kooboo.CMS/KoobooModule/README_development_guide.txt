Copyright 2013, Kooboo team
URL: http://www.kooboo.com

This is a brief guideline for developers of Kooboo Module. 

1. Set up environment
When you see this document, it means that you have installed the Kooboo module VS.NET template and created a VS project based on it.
This project is a standard Kooboo CMS application, your module is a standard ASP.NET MVC area within this project, your module is automatically installed within this CMS


2. Development
Developing a Kooboo module is almost the same as developing a Regular ASP.NET MVC area. Except the followings

Routes: You should define your MVC routes in routes.config
Views: Use partial view or remove the generated code from VS.NET. Because the reference to layout is defined within Kooboo CMS. 
Controller/action: Use @Url.ModuleUrl().Action("") and  @Html.ModuleHtml().ActionLink("") to generate controller action links.

 
3. Debug
You can debug a Kooboo module the same as any VS.NET projects. 

Kooboo module run within a Kooboo CMS page. In order for the code to hit your debug break points, you need to do the followings.

- Debug the module project, it is a standard kooboo CMS site, login to the Kooboo CMS. 
- Include this module into one of the websites.
- Create a page in the website and insert this module into that page, and then preview that page to execute the code.
- Start debugging in the VS.NET as you get used to.

4. Deployment
When you finish the development, you can pack your development as a standard Kooboo module to be installed in other Kooboo CMS instances. 

- Publish
Use VS.NET publish function to publish the project to disk file location, and open it with windows explorer.

- Pack
In the module folder, find a file named "PackModule.bat", click to pack your module. The package process rely on 7z application to zip the files. 7z is included in the project template.


5. Use the module
To use this module in other Kooboo CMS instances. 

- Login to the system as an administrator (default is admin). Navigate to module page and click ¡°install¡± to install the module.
- In website that needs this module, under the extension/module menu, select and click "include" to include this module into that website.You can now insert this module into any page positions. 

6. More configuration

- module.config and cmsmenu.config contains some more information to configure your module. cmsmenu.config contains menus that will be run within the Kooboo CMS backend interface.
- The ModuleEvents.cs contains events that you can use during installation or uninstallation. For example, to manually upgrade your database schema. 

Any questions, please post to: http://forum.kooboo.com or write an email to: guoqi AT kooboo.com