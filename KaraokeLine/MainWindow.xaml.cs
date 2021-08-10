using System;
using KaraokeLine.ViewModels;

namespace KaraokeLine
{
    public partial class MainWindow
    {
        public MainWindowVm ViewModel { get; }

        public MainWindow(MainWindowVm viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            await ViewModel.Initialize();
        }
    }
}