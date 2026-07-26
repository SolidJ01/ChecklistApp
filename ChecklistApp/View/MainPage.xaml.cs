using System.Windows.Input;
using ChecklistApp.Data;
using ChecklistApp.Services;
using ChecklistApp.View;
using ChecklistApp.ViewModel;

namespace ChecklistApp;

public partial class MainPage : DialoguePage
{
	public ICommand RegisterBackButtonCommand { get; set; }
	public ICommand OpenColourPickerCommand { get; set; }
	
	public MainPage(MainPageViewModel viewModel, ToastService toastService, ChecklistContext context) : base(toastService)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
		NavigatedTo += viewModel.ReloadList;
		OpenColourPickerCommand = new Command(ColorSelector.Open);
		OnPropertyChanged(nameof(OpenColourPickerCommand));
	}

	private void OnSettingsButtonClicked(object sender, EventArgs e)
	{
		SettingsPopup.Open();
	}

	private void OnExportChecklistsButtonClicked(object sender, EventArgs e)
	{
		ChecklistExportPopup.Open();
	}

	private void NewButtonClicked(object sender, EventArgs e)
	{
		CreateChecklistPopup.Open();
	}
}

