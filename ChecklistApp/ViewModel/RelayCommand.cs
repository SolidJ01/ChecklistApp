using System.Windows.Input;

namespace ChecklistApp.ViewModel;

public class RelayCommand(Action action, Func<object?, bool> canExecute) : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        action?.Invoke();
    }

    public event EventHandler? CanExecuteChanged;

    public void UpdateCanExecute()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}

public class RelayCommand<T>(Action<T> action, Func<object?, bool> canExecute) : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        action?.Invoke((T)parameter);
    }

    public event EventHandler? CanExecuteChanged;

    public void UpdateCanExecute()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}