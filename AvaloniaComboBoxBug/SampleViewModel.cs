using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AvaloniaComboBoxBug
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool logChanges = false;
        private int id;

        public int Id { get => this.id; protected set => this.RaiseAndSetIfChanged(ref this.id, value); }

        protected void RaiseAndSetIfChanged<TRet>(ref TRet backingField, TRet newValue, [CallerMemberName] string propertyName = null)
        {
            LogChanges(backingField, newValue, propertyName);

            backingField = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void LogChanges<TRet>(TRet oldValue, TRet newValue, string propertyName)
        {
            // To avoid logging those debug messages during ctors
            if (logChanges)
            {
                var oldValueString = oldValue == null ? "null" : oldValue.ToString();
                var newValueString = newValue == null ? "null" : newValue.ToString();
                System.Diagnostics.Debug.WriteLine($"Changed Object (Id: {this.Id}) Property: {propertyName} | OldValue: {oldValueString} | NewValue:{newValueString}");
            }
        }
    }

    public class SampleViewModel : ViewModel
    {
        private ObservableCollection<SampleItemViewModel> items;
        private SampleItemViewModel selectedItem;

        public SampleViewModel()
        {
            this.Id = -1;
            var data = new List<SampleItemViewModel>
            {
                new SampleItemViewModel(1, 10, SampleEnum.Value3),
                new SampleItemViewModel(2, 20, SampleEnum.Value2),
                new SampleItemViewModel(3, 30, SampleEnum.Value1)
            };

            this.Items = new ObservableCollection<SampleItemViewModel>(data);
            this.SelectedItem = this.Items.FirstOrDefault();
            this.logChanges = true;
        }

        public ObservableCollection<SampleItemViewModel> Items { get => this.items; protected set => this.RaiseAndSetIfChanged(ref this.items, value); }

        public SampleItemViewModel SelectedItem { get => this.selectedItem; set => this.RaiseAndSetIfChanged(ref this.selectedItem, value); }
    }

    public class SampleItemViewModel : ViewModel
    {
        private int value;
        private SampleEnum selectedEnumValue;
        private ObservableCollection<SampleEnum> availableTypes;

        public SampleItemViewModel(int id, int value, SampleEnum someEnum)
        {
            this.availableTypes = new ObservableCollection<SampleEnum>(Enum.GetValues<SampleEnum>().Cast<SampleEnum>().ToList());
            this.Id = id;
            this.Value = value;
            this.SelectedEnumValue = someEnum;
            this.logChanges = true;
        }

        public int Value { get => this.value; set => this.RaiseAndSetIfChanged(ref this.value, value); }

        public SampleEnum SelectedEnumValue { get => this.selectedEnumValue; set => this.RaiseAndSetIfChanged(ref this.selectedEnumValue, value); }

        public ObservableCollection<SampleEnum> AvailableTypes
        {
            get => this.availableTypes;
            //private set
            //{
            //    // If we uncomment this setter, Avalonia will set this to null, removing all items from the combobox
            //    this.RaiseAndSetIfChanged(ref this.availableTypes, value);
            //}
        }
    }

    public enum SampleEnum
    {
        Value0,
        Value1,
        Value2,
        Value3,
    }
}
