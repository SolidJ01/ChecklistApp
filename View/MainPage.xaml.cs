using ChecklistApp.ViewModel;

namespace ChecklistApp;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
		NavigatedTo += viewModel.ReloadList;
	}
}

