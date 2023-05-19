using Entity_JSON_Config_using_MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Entity_JSON_Config_using_MVVM.ViewModels
{
    class ViewModel : INotifyPropertyChanged
    {
        private string enteredData;

        public string EnteredData
        {
            get { return enteredData; }
            set { enteredData = value;
                OnPropertyChanged("EnteredData");
            }
        }

        private string dataToOutput;

        public string DataToOutput
        {
            get { return dataToOutput; }
            set { dataToOutput = value;
                OnPropertyChanged("DataToOutput");
            }
        }

        private RelayCommand addToDB;
        public RelayCommand AddToDB
        {
            get
            {
                return addToDB ??
                  (addToDB = new RelayCommand(obj =>
                  {
                      var EntityAdd = new EntityDB();
                      DataToOutput = EntityAdd.SaveData(EnteredData, DataToOutput);
                      OnPropertyChanged("EnteredData");
                  }));
            }
        }

        private RelayCommand clearDB;
        public RelayCommand ClearDB
        {
            get
            {
                return clearDB ??
                  (clearDB = new RelayCommand(obj =>
                  {
                      EntityDB.ClearDB();
                      DataToOutput = null; 
                      OnPropertyChanged("DataToOutput");
                      OnPropertyChanged("EnteredData");
                  }));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }



    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
