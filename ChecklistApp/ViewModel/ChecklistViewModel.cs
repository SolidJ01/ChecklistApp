using ChecklistApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core.Extensions;

namespace ChecklistApp.ViewModel
{
    public class ChecklistViewModel : ViewModel
    {
        private Checklist _checklist;

        #region properties
        public Checklist Checklist { get => _checklist; }

        public int Id { get { return _checklist.Id; } }

        public string Name { get { return _checklist.Name; } }

        public string CompletionStatus
        {
            get
            {
                return _checklist.Items != null && _checklist.Items.Any()
                    ? _checklist.Items.Where(x => x.IsChecked).ToList().Any() 
                        ? $"{_checklist.Items.Where(x => x.IsChecked).ToList().Count}/{_checklist.Items.Count} Items" 
                        : $"{_checklist.Items.Count} Items" 
                    : "No Items Yet";
            }
        }

        public float CompletionPercentage
        {
            get
            {
                return _checklist.Items.Count > 0
                    ? (float)_checklist.Items.Count(x => x.IsChecked) / (float)_checklist.Items.Count
                    : 0;
            }
        }

        public string DeadlineStatus
        {
            get
            {
                return _checklist.UseDeadline //  Does it use a deadline? 
                    ? (_checklist.Deadline - DateTime.Now).TotalSeconds < 0 //  Is it overdue? 
                        ? (DateTime.Now.Date - _checklist.Deadline.Date).TotalDays >= 1 // Is it overdue by more than a day? 
                            ? $"Overdue by {(int)(DateTime.Now.Date - _checklist.Deadline.Date).TotalDays}d"
                            : $"Overdue by {(int)(DateTime.Now - _checklist.Deadline).TotalHours}h"
                        : (_checklist.Deadline.Date - DateTime.Now.Date).TotalDays >= 1 // Is there more than a day left? 
                            ? $"{(int)(_checklist.Deadline - DateTime.Now).TotalDays}d left"
                            : $"{(int)(_checklist.Deadline - DateTime.Now).TotalHours}h left"
                    : string.Empty;
            }
        }

        public bool IsOverdue
        {
            get
            {
                return _checklist.UseDeadline && (_checklist.Deadline - DateTime.Now).TotalSeconds < 0;
            }
        }

        public Checklist.ChecklistColor Color
        {
            get
            {
                return _checklist.Color;
            }
        }

        public ObservableCollection<ItemViewModel> Items { get; set; } = [];

        #endregion
        public ChecklistViewModel(Checklist checklist)
        {
            _checklist = checklist;
            Items = checklist.Items.Select(x => new ItemViewModel(x)).ToObservableCollection();
            Items.CollectionChanged += ItemsOnCollectionChanged;
            foreach (ItemViewModel item in Items)
            {
                item.PropertyChanged += ItemOnPropertyChanged;
            }
        }

        private void ItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems is null)
                        return;
                    
                    foreach (ItemViewModel item in e.NewItems)
                    {
                        item.PropertyChanged += ItemOnPropertyChanged;
                    }
                    
                    Update();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems is null)
                        return;
                    
                    foreach (ItemViewModel item in e.OldItems)
                    {
                        item.PropertyChanged -= ItemOnPropertyChanged;
                    }

                    Update();
                    break;
            }
        }

        private void ItemOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is not nameof(ItemViewModel.IsChecked))
                return;
            OnPropertyChanged(nameof(CompletionStatus));
            OnPropertyChanged(nameof(CompletionPercentage));
            _checklist.Items = _checklist.Items.OrderBy(x => x.IsChecked).ThenBy(x => x.Name).ToList();
            Items.Move(Items.IndexOf((ItemViewModel)sender), _checklist.Items.IndexOf(((ItemViewModel)sender).Item));
        }

        public async Task Update(Checklist checklist)
        {
            _checklist = checklist;
            
            List<ItemViewModel> newItems = _checklist.Items.Where(x => !Items.Any(y => y.Item == x)).Select(x => new ItemViewModel(x)).ToList();
            List<ItemViewModel> oldItems = Items.Where(x => !_checklist.Items.Contains(x.Item)).ToList();

            await Task.Run(() =>
            {
                foreach (var item in newItems)
                {
                    Items.Insert(_checklist.Items.IndexOf(item.Item), item);
                    item.PropertyChanged += ItemOnPropertyChanged;
                }

                foreach (var item in oldItems)
                {
                    item.PropertyChanged -= ItemOnPropertyChanged;
                    Items.Remove(item);
                }
            });
            
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(CompletionStatus));
            OnPropertyChanged(nameof(CompletionPercentage));
            OnPropertyChanged(nameof(DeadlineStatus));
            OnPropertyChanged(nameof(IsOverdue));
            OnPropertyChanged(nameof(Color));
        }

        public void Update()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(CompletionStatus));
            OnPropertyChanged(nameof(CompletionPercentage));
            OnPropertyChanged(nameof(DeadlineStatus));
            OnPropertyChanged(nameof(IsOverdue));
            OnPropertyChanged(nameof(Color));
        }

        public bool ModelEquals(Checklist checklist)
        {
            return _checklist.Equals(checklist);
        }
    }
}
