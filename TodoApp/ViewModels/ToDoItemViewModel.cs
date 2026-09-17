using CommunityToolkit.Mvvm.ComponentModel;
using TodoApp.Models;

namespace TodoApp.ViewModels;

/// <summary>
/// This is a ViewModel which represents a <see cref="Models.ToDoItem"/>
/// </summary>
public partial class ToDoItemViewModel : ViewModelBase
{
    /// <summary>
    /// Creates a new blank ToDoItemViewModel
    /// </summary>
    public ToDoItemViewModel()
    {
    }

    /// <summary>
    /// Creates a new ToDoItemViewModel for the given <see cref="Models.ToDoItem"/>
    /// </summary>
    /// <param name="item">The item to load</param>
    public ToDoItemViewModel(ToDoItem item)
    {
        // Init the properties with the given values
        IsChecked = item.IsChecked;
        Content = item.Content;
    }
    
    /// <summary>
    /// Gets or sets the checked status of each item
    /// </summary>
    [ObservableProperty]
    public partial bool IsChecked { get; set; }
    
    /// <summary>
    /// Gets or sets the content of the to-do item
    /// </summary>
    [ObservableProperty]
    public partial string? Content { get; set; }

    public ToDoItem GetToDoItem()
    {
        return new ToDoItem
        {
            IsChecked = IsChecked,
            Content = Content
        };
    }
}
