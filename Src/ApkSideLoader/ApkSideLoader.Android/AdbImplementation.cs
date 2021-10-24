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

[assembly: Xamarin.Forms.Dependency(typeof(ApkSideLoader.Droid.AdbImplementation))]
namespace ApkSideLoader.Droid
{
	public class AdbImplementation : IAdbAccess
  {
    public string CallAdb(string arg)
    {
      GetAdbStatus();
      //Debug.WriteLine(result);
      int a = 0;
      /*
      string filename = "adb.exe";

      var sb = new StringBuilder();
      var proc = new System.Diagnostics.Process();
      // Hookup the eventhandlers to capture the data that is received
      proc.OutputDataReceived += (sender, args) => sb.AppendLine(args.Data);
      proc.ErrorDataReceived += (sender, args) => sb.AppendLine(args.Data);
      // Set the StartInfo
      proc.StartInfo.RedirectStandardOutput = true;
      proc.StartInfo.RedirectStandardError = true;
      proc.StartInfo.UseShellExecute = false;
      proc.StartInfo.RedirectStandardOutput = true;
      proc.StartInfo.CreateNoWindow = true;
      proc.StartInfo.FileName = filename;
      proc.StartInfo.Arguments = arg;
      // Start
      proc.Start();
      // start our event pumps
      proc.BeginOutputReadLine();
      proc.BeginErrorReadLine();

      if (!arg.Contains("install "))
      {
        if (!proc.WaitForExit(5000))
        {
          proc.Kill();
        }
      }
      else
      {
        proc.WaitForExit(60000);
      }
      return sb.ToString();
      */
      return "";
    }

    public string LoadFile()
    {
      return "";
    }

    async Task<string> GetAdbStatus()
    {
      try
      {
        var process = Java.Lang.Runtime.GetRuntime().Exec(new string[] { "adb version" });
        var exitCode = await process.WaitForAsync();

        if (exitCode == 0)
        {
          using (var outputStreamReader = new StreamReader(process.InputStream))
            return await outputStreamReader.ReadToEndAsync();
        }

        if (process.ErrorStream != null)
        {
          using (var errorStreamReader = new StreamReader(process.ErrorStream))
            return await errorStreamReader.ReadToEndAsync();
        }
      }
      catch (System.Exception ee)
      {
        int a = 0;
      }

      return null;
    }

  }
}