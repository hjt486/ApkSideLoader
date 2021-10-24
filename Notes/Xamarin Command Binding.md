# Xamarin Command Binding

Read **Xamarin XAML Binding** first.

Add command binding in UI control:

```xaml
<Button Text="Connect" Command="{Binding ConnectCommand}"/>
```

In ModelView, add a Command Getter

```c#
public Command ConnectCommand { get; }
```

In constructor, add the method to the command:

(Able to add CanExecute parameter to check the condition.)

```c#
public MainPageViewModel()
{
    ConnectCommand = new Command(OnConnect);
}
```

Implement the method

```c#
private void OnConnect(object obj)
{
    //Implement here
}
```

Note: May search & learn example of ICommand on Google.

##### Async/Await (No UI Frozen)

Make change in the constructor:

```c#
ConnectCommand = new Command(async () => await OnConnect());
```

Change the method async Task, make sure there's a await Task Run inside to fulfill async

```c#
async Task OnConnect()
{
    await Task.Run(() => ExecuteConnect());
}
```

