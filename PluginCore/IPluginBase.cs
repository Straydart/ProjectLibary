using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ProjectDirectory.PluginCore
{
    public interface IPluginBase
    {
        public string ButtonName { get; set; }
        public string ButtonTag { get; set; }
        public string ButtonContent { get; set; }

        public ICommand ButtonCommand { get; set; }

        public ObservableCollection<object> DynamicValues { get; set; }

        public PluginResponse Initialize(PluginParameters args);

        public PluginResponse Execute(PluginParameters args);
    }

}
