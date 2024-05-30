using CommunityToolkit.Mvvm.ComponentModel;
using ProjectDirectory.View;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ProjectDirectory.Models
{
    public class ButtonSafe : ObservableObject
    {
        private string projectName;

        public string ProjectName
        {
            get { return projectName; }
            set { SetProperty(ref projectName, value); }
        }
        private string filePath;

        public string FilePath
        {
            get { return filePath; }
            set { SetProperty(ref filePath, value); }
        }

        private int row;

        public int Row
        {
            get { return row; }
            set { SetProperty(ref row, value); }
        }
        private int column;

        public int Column
        {
            get { return column; }
            set { SetProperty(ref column, value); }
        }

        private ObservableCollection<object> observableCollection;

        public ObservableCollection<object> ObservableCollection
        {
            get { return observableCollection; }
            set { SetProperty(ref observableCollection, value); }
        }
    }
}
