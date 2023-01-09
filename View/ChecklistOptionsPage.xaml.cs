using ChecklistApp.ViewModel;

namespace ChecklistApp.View;

public partial class ChecklistOptionsPage : ContentPage
{
	public ChecklistOptionsPage(ChecklistOptionsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		NavigatedTo += viewModel.RetrieveChecklist;
	}
}