using KaraokeLine.ViewModels;

namespace KaraokeLine.Configuration
{
    public class ViewModelLocator
    {
        public static MainWindowVm MainWindowVm => ContainerManager.GetInstance<MainWindowVm>();
    }
}