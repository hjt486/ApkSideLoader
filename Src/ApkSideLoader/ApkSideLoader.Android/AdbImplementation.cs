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
      string result_str = "";
      try
      {
        //var applicationInfo = Platform.AppContext.ApplicationInfo;
        //var packageManager = Platform.AppContext.PackageManager;

        //Java.IO.File workdir = new Java.IO.File(Platform.AppContext.ApplicationInfo.NativeLibraryDir);
        //var builder = new ProcessBuilder(Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so ", "version", "start-server", param).Directory(workdir);
        //var process = builder.Start();
        //var process = Runtime.GetRuntime().Exec("start - server", Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so " + param);
        var process = Runtime.GetRuntime().Exec(Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so " + "disconnect");
        var exitCode = process.WaitFor();
        result_str += new StreamReader(process.InputStream).ReadToEnd().ToString();
        if (param.Contains("connect"))
        {
          process = Runtime.GetRuntime().Exec(Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so " + param);
          exitCode = process.WaitFor();
          result_str += new StreamReader(process.InputStream).ReadToEnd().ToString();
        }
        if (param.Contains("install"))
        {
          string adb = Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so";
          ProcessBuilder builder = new ProcessBuilder(adb, "devices");
          process = builder.Start();
          exitCode = process.WaitFor();
          result_str += new StreamReader(process.InputStream).ReadToEnd().ToString();
          builder.Command(adb, "disconnect");
          process = builder.Start();
          exitCode = process.WaitFor();
          result_str += new StreamReader(process.InputStream).ReadToEnd().ToString();
          //process = Runtime.GetRuntime().Exec(new string[] { adb + "devices", adb + "disconnect" });
          //exitCode = process.WaitFor();
          /*
          Java.IO.File workdir = new Java.IO.File(Platform.AppContext.ApplicationInfo.NativeLibraryDir);
          var builder = new ProcessBuilder(
            Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so version"
            //Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so " + "connect 192.168.1.114:58526"
            //Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so " + "install " + Platform.AppContext.GetExternalFilesDir("DIRECTORY_DOWNLOADS") + "/shafa_mobile.apk"
            ).Directory(workdir);
          process = builder.Start();
          exitCode = process.WaitFor();
          */
          //process = Runtime.GetRuntime().Exec(Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so " + "connect 169.254.100.102:5555");
          //process = Runtime.GetRuntime().Exec(Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so " + "connect 192.168.1.114:58526");
          //process = Runtime.GetRuntime().Exec(Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so "  + "devices");
          //exitCode = process.WaitFor();
          //process = Runtime.GetRuntime().Exec(Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so " + " -s 169.254.100.102:5555 " + param);
          //exitCode = process.WaitFor();
          //process = Runtime.GetRuntime().Exec(Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so " + "install " + Platform.AppContext.GetExternalFilesDir("DIRECTORY_DOWNLOADS") + "/shafa_mobile.apk");
          //exitCode = process.WaitFor();
          //process = Runtime.GetRuntime().Exec(Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so " + " -s 169.254.100.102:5555 " + param);

          //process = Runtime.GetRuntime().Exec(Platform.AppContext.ApplicationInfo.NativeLibraryDir + "/lib_adb_arm64.so " + " -s 169.254.100.102:5555 " + "install './storage/emulated/0/Download/HTTP File Server_v1.4.1_apkpure.com.apk'");
          //process = Runtime.GetRuntime().Exec("ls -l 'storage/emulated/0/Download/HTTP File Server_v1.4.1_apkpure.com.apk'");
          //process = Runtime.GetRuntime().Exec("cd " + Platform.AppContext.ApplicationInfo.SourceDir);
          //exitCode = process.WaitFor();
          //process = Runtime.GetRuntime().Exec("ls");
          //exitCode = process.WaitFor();
        }

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

      return result_str;
    }

  }
}