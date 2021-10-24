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
    string _ipAddress, _port, _filePath, _connectionStatus;

    public Command PushCommand { get; }
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
      IpAddress = "127.0.0.1";
      Port = "58526";
      FilePath = "*.APK";
      ConnectionStatus = "Standby;";

      // Init Commands
      PushCommand = new Command(async () => await OnPush());
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

    // Getters/Setters End

    /*
     * Command Methods Begin
     */
    private async Task OnPush()
    {
      await Task.Run(() => ExecutePush());
    }
    private void OnLoad()
    {
      if (Device.RuntimePlatform == Device.WPF)
      {
        FilePath = DependencyService.Get<IAdbAccess>().LoadFile();
        if (FilePath.Length > 0) ConnectionStatus = "APK file loaded;";
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

    private void ExecutePush()
    {
      bool isConnected = false;
      string response = "";
      DependencyService.Get<IAdbAccess>().CallAdb("disconnect");
      DependencyService.Get<IAdbAccess>().CallAdb("kill-server");
      ConnectionStatus = "Connecting...";
      response = DependencyService.Get<IAdbAccess>().CallAdb("connect " + IpAddress + ":" + Port);
      if (response.Contains("connected") && response.Contains(IpAddress))
      {
        ConnectionStatus = "Connected;";
        isConnected = true;
      }
      else
      {
        ConnectionStatus = "ERROR: Failed to connect!";
      }
      if (isConnected)
      {
        ConnectionStatus = "Pushing APK... (be patient if file is large)";
        response = DependencyService.Get<IAdbAccess>().CallAdb("install " + "\"" + FilePath + "\"");
        if (response.Contains("Success"))
        {
          ConnectionStatus = "Success!";
        }
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
        if (result.FullPath.Length > 0)
        {
          FilePath = result.FullPath;
          ConnectionStatus = "APK file loaded;";
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
