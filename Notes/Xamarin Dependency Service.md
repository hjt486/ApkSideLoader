# Xamarin Dependency Service

Original Tutorial is at:

https://www.c-sharpcorner.com/article/understanding-synchronization-context-task-configureawait-in-action/

To implement platform-specific code in each platform's project, then use them in the shared core project with UI and ViewModels, here's how to do.



Create a Services folder in shared project, to hold the Interface, then create an interface, for example, IAdbAccess, contains all methods to be implemented in platform project:

```c#
namespace ApkSideLoader.Services
{
  public interface IAdbAccess
  {
    string CallAdb(string arg);
    string LoadFile();
  }
}
```

Go to platform project, create a Class named AdbImplementation.cs, make it inherited from IAdbAccess, here we can implement platform-specific code, for example, using Microsoft.Win32 etc, be sure to include [assembly:] and using ApkSideLoader.Services:

```c#
using ApkSideLoader.Services;
using Microsoft.Win32;

[assembly: Xamarin.Forms.Dependency(typeof(ApkSideLoader.WPF.AdbImplementation))]
namespace ApkSideLoader.WPF
{
	public class AdbImplementation:IAdbAccess
  	{
        public string CallAdb(string arg)
        {
            // Implement WPF code here
        }

        public string LoadFile()
        {
            // Implement WPF code here
        }
        
}
```

