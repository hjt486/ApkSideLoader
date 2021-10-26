using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ApkSideLoader.Services;
using Xamarin.Essentials;

namespace ApkSideLoader.ViewModels
{
  public class MainPageViewModel : BaseViewModel
  {
    string _ipAddress, _port, _filePath, _connectionStatus, _debugStatus;

    public Command SendFileCommand { get; }
    public Command PushInstallCommand { get; }
    public Command LoadCommand { get; }

    // Debug Parameters Begin
    int _counter = 0;
    // Debug Parameters End

    /*
     *  Constructor
     */
    public MainPageViewModel()
    {
      //Init Properties
      IpAddress = "192.168.1.114";
      Port = "58526";
      //IpAddress = "169.254.100.102";
      //Port = "5555";
      FilePath = "*.*";
      ConnectionStatus = "Standby;";

      // Init Commands
      PushInstallCommand = new Command(async () => await OnPushInstall());
      SendFileCommand = new Command(async () => await OnSend());
      LoadCommand = new Command(OnLoad);
    }

    /*
     * Getters/Setters Begin
     */

    public string IpAddress
    {
      set
      {
        _ipAddress = value;
        OnPropertyChanged("IpAddress");
      }
      get { return _ipAddress; }
    }

    public string Port
    {
      set
      {
        _port = value;
        OnPropertyChanged("Port");
      }
      get { return _port.ToString(); }
    }

    public string FilePath
    {
      set
      {
        _filePath = value;
        OnPropertyChanged("FilePath");
      }
      get { return _filePath; }
    }

    public string FileName
    {
      get
      {
        string[] splitPath = { "" };
        if (Device.RuntimePlatform == Device.WPF || Device.RuntimePlatform == Device.UWP)
        {
          splitPath = _filePath.Split('\\');
        }
        else
        {
          splitPath = _filePath.Split('/');
        }
        return splitPath[splitPath.Length-1]; 
      }
    }

    public string ConnectionStatus
    {
      set
      {
        _connectionStatus = value ;
        OnPropertyChanged("ConnectionStatus");
        OnPropertyChanged("FileName");
      }
      get { return _connectionStatus; }
    }
    public string DebugStatus
    {
      set
      {
        _debugStatus = value;
        OnPropertyChanged("DebugStatus");
      }
      get { return _debugStatus; }
    }

    // Getters/Setters End

    /*
     * Command Methods Begin
     */
    private async Task OnSend()
    {
      await Task.Run(() => ExecuteSend());
    }
    private async Task OnPushInstall()
    {
      await Task.Run(() => ExecutePushInstall());
    }
    private void OnLoad()
    {
      if (Device.RuntimePlatform == Device.WPF)
      {
        FilePath = DependencyService.Get<IAdbAccess>().LoadFile();
        if (FilePath.Length > 4 && FilePath.Substring(FilePath.Length - 4) == ".apk")
        {
          ConnectionStatus += "APK File loaded;";
        }
        else
        {
          ConnectionStatus += "File loaded;";
        }
      }
      else
      {
        OnLoadXamarin();
      }

    }
    // Command Method End

    /*
     * Processing Methods Begin
     */

    private void OnLoadXamarin()
    {
      PickAndShow();
    }

    private void ExecuteSend()
    {
      if (FilePath.Length == 0)
      {
        ConnectionStatus += "Please load a file first;";
        return;
      }
      bool isConnected = false;
      string response = "";
      ConnectionStatus += DependencyService.Get<IAdbAccess>().CallAdb("disconnect");
      ConnectionStatus += DependencyService.Get<IAdbAccess>().CallAdb("kill-server");
      ConnectionStatus += "Connecting...";
      ConnectionStatus += DependencyService.Get<IAdbAccess>().CallAdb("connect " + IpAddress + ":" + Port);
      if (ConnectionStatus.Contains("connected to")) isConnected = true;
      if (isConnected)
      {
        ConnectionStatus += "Sending the file... (be patient...)";
        response = DependencyService.Get<IAdbAccess>().CallAdb("push " + "\"" + FilePath + "\" " + "/sdcard/Download/");
        ConnectionStatus += response;
      }
    }

    private void ExecutePushInstall()
    {
      if (!(FilePath.Length > 4 && FilePath.Substring(FilePath.Length - 4) == ".apk"))
      {
        ConnectionStatus += "APK file is not loaded, please load first;";
        return;
      }
      bool isConnected = false;
      string response = "";
      ConnectionStatus += DependencyService.Get<IAdbAccess>().CallAdb("disconnect");
      ConnectionStatus += DependencyService.Get<IAdbAccess>().CallAdb("kill-server");
      ConnectionStatus += "Connecting...";
      ConnectionStatus += DependencyService.Get<IAdbAccess>().CallAdb("connect " + IpAddress + ":" + Port);
      if (ConnectionStatus.Contains("connected to")) isConnected = true;
      if (isConnected)
      {
        ConnectionStatus += "Pushing and installing APK... (be patient...)";
        ConnectionStatus += DependencyService.Get<IAdbAccess>().CallAdb("install " + "\"" + FilePath + "\"");
      }
    }

    async Task<FileResult> PickAndShow()
    {
      try
      {
        var result = await FilePicker.PickAsync(new PickOptions()
        {
          PickerTitle = "Please select a comic file",
          FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
          {
              { DevicePlatform.iOS, new[] { "public.my.apk.extension" } },
              { DevicePlatform.UWP, new[] { ".apk" } },
              { DevicePlatform.Tizen, new[] { "*/*" } },
              { DevicePlatform.macOS, new[] { "apk" } },
              { DevicePlatform.Android, new[] { "application/*" } },
          }),
        }
        );
        if (result != null && result.FullPath.Length > 0)
        {
          FilePath = result.FullPath;
          if (FilePath.Length > 4 && FilePath.Substring(FilePath.Length - 4) == ".apk")
          {
            ConnectionStatus += "APK File loaded;";
          }
          else
          {
            ConnectionStatus += "File loaded;";
          }
        }
        return result;
      }
      catch (Exception ex)
      {
        // The user canceled or something went wrong
        int a = 0;
      }
      return null;
    }

    //Processing Methods End


  }
}
