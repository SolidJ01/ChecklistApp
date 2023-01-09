using ChecklistApp.ViewModel;

namespace ChecklistApp.View;

public partial class CreateChecklistPage : ContentPage
{
	public CreateChecklistPage(CreateChecklistPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}