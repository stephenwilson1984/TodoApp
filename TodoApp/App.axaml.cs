using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TodoApp.ViewModels;
using TodoApp.Views;

namespace TodoApp;

using System.Linq;
using System.Threading.Tasks;
using Services;

public partial class App : Application
{
    // This is a reference to our MainViewModel which we use to save the list on shutdown. You can also
    // use Dependency Injection in your app.
    private readonly MainViewModel _mainViewModel = new();

    // We want to save our ToDoList before we actually shut down the App. As File I/O is async, we need to wait until file is closed
    // before we can actually close this window
    
    private bool _canClose; // This flag is used to check if the window is allowed to close

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = _mainViewModel
            };
            
            // Listen to the ShutdownRequested event
            desktop.ShutdownRequested += DesktopOnShutdownRequested;
        }
        
        // Init the MainViewModel
        await InitMainViewModelAsync();

        base.OnFrameworkInitializationCompleted();
    }

    private async void DesktopOnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
        e.Cancel = !_canClose;

        if (!_canClose)
        {
            // To save the items, we map them to the ToDoItem-Model which is better suited for I/O operations
            var itemsToSave = _mainViewModel.ToDoItems.Select(item => item.GetToDoItem());
            await ToDoListFileService.SaveToFileAsync(itemsToSave);
            
            // Set _canClose to true and close this Window again
            _canClose = true;
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
    }
    
    // Optional: Load data from disc
    private async Task InitMainViewModelAsync()
    {
        // get the items to load
        var itemsLoaded = await ToDoListFileService.LoadFromFileAsync();

        if (itemsLoaded is not null)
        {
            foreach (var item in itemsLoaded)
            {
                _mainViewModel.ToDoItems.Add(new ToDoItemViewModel(item));
            }
        }
    }
}
