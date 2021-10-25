using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using ApkSideLoader.Services;
using System.Threading.Tasks;
using Java.Lang;
using System.IO;
using Android.Content.PM;

[assembly: Xamarin.Forms.Dependency(typeof(ApkSideLoader.Droid.AdbImplementation))]
namespace ApkSideLoader.Droid
{
	public class AdbImplementation : IAdbAccess
  {
    public string CallAdb(string arg)
    {
      return GetAdbStatus(arg);
    }

    public string LoadFile()
    {
      return "";
    }

    private string GetAdbStatus(string param)
    {
      try
      {
        //var applicationInfo = Platform.AppContext.ApplicationInfo;
        //var packageManager = Platform.AppContext.PackageManager;

        //Java.IO.File workdir = new Java.IO.File(Platform.AppContext.ApplicationInfo.NativeLibraryDir);
        //var builder = new ProcessBuilder(Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so ", "version", "start-server", param).Directory(workdir);
        //var process = builder.Start();
        //var process = Runtime.GetRuntime().Exec("start - server", Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so " + param);
        var process = Runtime.GetRuntime().Exec(Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so " + "connect 192.168.1.114:58526");
        var exitCode = process.WaitFor();
        process = Runtime.GetRuntime().Exec(Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so " + param);
        exitCode = process.WaitFor();

        if (exitCode == 0)
        {
          var outputStreamReader = new StreamReader(process.InputStream);
          var result = outputStreamReader.ReadToEnd();
          return result.ToString();
        }

        if (process.ErrorStream != null)
        {
          var errorStreamReader = new StreamReader(process.ErrorStream);
          var result = errorStreamReader.ReadToEnd();
          return result.ToString();
        }
      }
      catch (System.Exception ee)
      {
        int a = 0;
      }

      return "";
    }

  }
}