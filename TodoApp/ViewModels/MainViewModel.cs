using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace TodoApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    /// <summary>
    /// Gets a collection of <see cref="Models.ToDoItem"/> which allows adding and removing items
    /// </summary>
    public ObservableCollection<ToDoItemViewModel> ToDoItems { get; } = [];

    /// <summary>
    /// Gets or sets the content for new items to add. If this string is not empty, the AddItemCommand will be enabled automatically
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
    public partial string? NewItemContent { get; set; }
    
    /// <summary>
    /// Returns if a new item can be added. We require to have the NewItem some text.
    /// </summary>
    private bool CanAddItem() => !string.IsNullOrWhiteSpace(NewItemContent);

    /// <summary>
    /// This command is used to add a new Item to the list
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanAddItem))]
    private void AddItem()
    {
        // Add a new item to the list
        ToDoItems.Add(new ToDoItemViewModel() { Content = NewItemContent });
        
        // Reset the NewItemContent
        NewItemContent = null;
    }

    /// <summary>
    /// Removes the given item from the list
    /// </summary>
    /// <param name="item">The item to remove</param>
    [RelayCommand]
    private void RemoveItem(ToDoItemViewModel item)
    {
        // Remove the given item from the list
        ToDoItems.Remove(item);
    }
}
