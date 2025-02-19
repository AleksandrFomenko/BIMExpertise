using System.Windows.Input;

namespace KapibaraCore.RelayCommand;

public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Func<object, bool> _canExecute;
     
    public event EventHandler CanExecuteChanged;
     
    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }
     
    public bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute(parameter);
    }
     
    public void Execute(object parameter)
    {
        _execute(parameter);
    }
    
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
public class AsyncRelayCommand1 : ICommand
{
    private readonly Func<object, Task> _execute;
    private readonly Predicate<object> _canExecute;
    private bool _isExecuting;

    public event EventHandler CanExecuteChanged;

    public AsyncRelayCommand1(Func<object, Task> execute, Predicate<object> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return !_isExecuting && (_canExecute == null || _canExecute(parameter));
    }

    public async void Execute(object parameter)
    {
        _isExecuting = true;
        RaiseCanExecuteChanged();

        try
        {
            await _execute(parameter);
        }
        finally
        {
            _isExecuting = false;
            RaiseCanExecuteChanged();
        }
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
