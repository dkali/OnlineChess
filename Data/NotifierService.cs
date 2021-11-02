using System;
using System.Threading.Tasks;

// https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0#invoke-component-methods-externally-to-update-state-1
public class NotifierService
{
    public async Task Update(string key, int value)
    {
        if (Notify != null)
        {
            await Notify.Invoke(key, value);
        }
    }

    public event Func<string, int, Task> Notify;
}