using CEN4090L_Project.Models;
using CommunityToolkit.Mvvm.Input;

namespace CEN4090L_Project.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}