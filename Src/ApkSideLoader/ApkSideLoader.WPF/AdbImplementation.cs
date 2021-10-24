using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApkSideLoader.Services;
using Microsoft.Win32;

[assembly: Xamarin.Forms.Dependency(typeof(ApkSideLoader.WPF.AdbImplementation))]
namespace ApkSideLoader.WPF
{
  public class AdbImplementation:IAdbAccess
  {
    public string CallAdb(string arg)
    {
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
    }

    public string LoadFile()
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      openFileDialog.Filter =
        "APK File (*.apk)|" +
        "*.apk|" +
        "All  (*.*)|" +
        "*.*";
      if (openFileDialog.ShowDialog() == true)
      {
        return openFileDialog.FileName;
      }
      return "";
    }

  }
}
