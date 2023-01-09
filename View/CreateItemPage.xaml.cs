using ChecklistApp.ViewModel;

namespace ChecklistApp.View;

public partial class CreateItemPage : ContentPage
{
	public CreateItemPage(CreateItemPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		NavigatedTo += viewModel.RetrieveChecklist;
	}
}