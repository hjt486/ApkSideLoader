# Xamarin XAML Binding

Add a base view class:
```c#
public class BaseViewModel : INotifyPropertyChanged
{
	#region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
      	var changed = PropertyChanged;
      	if (changed == null) return;
  		changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	#endregion
}
```

Create a **XxxPageViewModel** class inherit from the base class, add **setter**, **getter**, and **OnPropertyChanged(string)**:

```c#
public class MainPageViewModel : BaseViewModel
{
    string _ipAddress;
	public string IpAddress
    {
      set
      {
        _ipAddress = value;
        OnPropertyChanged("IpAddress");
      }
      get { return _ipAddress; }
    }
}
```

Add viewmodels support to XAML:

```xaml
xmlns:viewmodels="clr-namespace:ApkSideLoader.ViewModels"
x:DataType="viewmodels:MainPageViewModel"
```

In View code (MainPage.xaml.cs), add BindingContext:

```c#
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new ViewModels.MainPageViewModel();
    }
}
```

Note: Pay attention to namespace.
