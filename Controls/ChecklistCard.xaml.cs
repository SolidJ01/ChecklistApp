using ChecklistApp.Model;
using System.Windows.Input;
using ChecklistApp.ViewModel;

namespace ChecklistApp.Controls;

public partial class ChecklistCard : ContentView
{
    public static readonly BindableProperty ChecklistProperty = BindableProperty.Create(nameof(Checklist), typeof(ChecklistCardViewModel), typeof(ChecklistCard), propertyChanged:ChecklistPropertyChanged);

    private static void ChecklistPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
	    ChecklistCard checklistCard = (ChecklistCard)bindable;
	    checklistCard.OnChecklistPropertyChanged();
    }

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ChecklistCard));
    
	public ChecklistCardViewModel Checklist
	{
		get => (ChecklistCardViewModel)GetValue(ChecklistProperty);
		set => SetValue(ChecklistProperty, value);
	}

	public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	/*public string CompletionStatus
	{
		get
		{
			return Checklist is not null ? 
						Checklist.Items != null && Checklist.Items.Any()
							? Checklist.Items.Where(x => x.IsChecked).ToList().Any() 
								? $"{Checklist.Items.Where(x => x.IsChecked).ToList().Count}/{Checklist.Items.Count} Items" 
								: $"{Checklist.Items.Count} Items" 
							: "No Items Yet"
						:  string.Empty;
		}
	}

	public float CompletionPercentage
	{
		get
		{
			return Checklist is not null ? (float)Checklist.Items.Count(x => x.IsChecked) / (float)Checklist.Items.Count
										 : 0.0f;
		}
	}

	public string DeadlineStatus
	{
		get
		{
			return Checklist is not null 
						? Checklist.UseDeadline //  Does it use a deadline? 
							? (Checklist.Deadline - DateTime.Now).TotalSeconds < 0 //  Is it overdue? 
								? (DateTime.Now.Date - Checklist.Deadline.Date).TotalDays >= 1 // Is it overdue by more than a day? 
									? $"Overdue by {(int)(DateTime.Now.Date - Checklist.Deadline.Date).TotalDays}d"
									: $"Overdue by {(int)(DateTime.Now - Checklist.Deadline).TotalHours}h"
								: (Checklist.Deadline.Date - DateTime.Now.Date).TotalDays >= 1 // Is there more than a day left? 
									? $"{(int)(Checklist.Deadline - DateTime.Now).TotalDays}d left"
									: $"{(int)(Checklist.Deadline - DateTime.Now).TotalHours}h left"
							: string.Empty
						: string.Empty;
		}
	}
	
	public bool IsOverdue => Checklist is not null && Checklist.UseDeadline && (Checklist.Deadline - DateTime.Now).TotalSeconds < 0;*/


	public ChecklistCard()
	{
		InitializeComponent();
	}

	public void OnChecklistPropertyChanged()
	{
		// OnPropertyChanged(nameof(CompletionStatus));
		// OnPropertyChanged(nameof(CompletionPercentage));
		// OnPropertyChanged(nameof(DeadlineStatus));
		// OnPropertyChanged(nameof(IsOverdue));
	}
}