using ChecklistApp.ViewModel;

namespace ChecklistApp.View;

public partial class ChecklistPage : DialoguePage
{
	public ChecklistPage(ChecklistPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		NavigatedTo += viewModel.RetrieveChecklist;
	}

	private void OptionsButtonClicked(object sender, EventArgs e)
	{
		OptionsPopup.Open();
	}

	private void CreateItemsButtonClicked(object sender, EventArgs e)
	{
		CreateItemPopup.Open();
	}
}