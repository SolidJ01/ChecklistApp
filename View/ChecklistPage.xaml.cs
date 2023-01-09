using ChecklistApp.ViewModel;

namespace ChecklistApp.View;

public partial class ChecklistPage : ContentPage
{
	public ChecklistPage(ChecklistPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		NavigatedTo += viewModel.RetrieveChecklist;
	}
}