using MiPlugin.UI.Commands;

namespace MiPlugin.Tests.UI;

public class RelayCommandTests
{
    [Fact]
    public void Execute_InvokesAction()
    {
        var executed = false;
        var command = new RelayCommand(() => executed = true);

        command.Execute(null);

        Assert.True(executed);
    }

    [Fact]
    public void CanExecute_WithoutPredicate_ReturnsTrue()
    {
        var command = new RelayCommand(() => { });

        Assert.True(command.CanExecute(null));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanExecute_ReturnsPredicateResult(bool expected)
    {
        var command = new RelayCommand(() => { }, () => expected);

        Assert.Equal(expected, command.CanExecute(null));
    }
}
