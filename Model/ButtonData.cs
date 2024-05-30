using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using ProjectDirectory.PluginCore;

namespace ProjectDirectory.Model
{
    public class ButtonData : ObservableObject
    {
		private string buttonName;

		public string ButtonName
		{
			get { return buttonName; }
			set { SetProperty(ref buttonName, value); }
		}

		private string buttonTag;

		public string ButtonTag
		{
			get { return buttonTag; }
			set { SetProperty(ref buttonTag, value); }
		}
		private string buttonContent;

		public string ButtonContent
		{
			get { return buttonContent; }
			set { SetProperty(ref buttonContent, value); }
		}
		private ICommand buttonCommand;

		public ICommand ButtonCommand
		{
			get { return buttonCommand; }
            set { SetProperty(ref buttonCommand, value); }
        }

		private ObservableCollection<object> observableCollection;

		public ObservableCollection<object> ObservableCollection
		{
			get { return observableCollection; }
			set { SetProperty(ref observableCollection, value); }
		}


        public ButtonData(IPluginBase plugin)
		{
			ButtonName = plugin.ButtonName;
			ButtonTag = plugin.ButtonTag;
			ButtonContent = plugin.ButtonContent;
			ButtonCommand = plugin.ButtonCommand;
		}
	}
}
