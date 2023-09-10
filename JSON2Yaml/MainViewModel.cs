using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JSON2Yaml.Services;
using Microsoft.Win32;
using System.IO;

namespace JSON2Yaml
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IYamlService yamlService;

        public MainViewModel(IYamlService yamlService)
        {
            this.yamlService = yamlService;
        }

        private string json;
        public string Json
        {
            get { return json; }
            set
            {
                SetProperty(ref json, value);
                SwitchCommand.NotifyCanExecuteChanged();
            }
        }
        private string yaml;
        public string Yaml
        {
            get { return yaml; }
            set { SetProperty(ref yaml, value); }
        }
        

        private IRelayCommand loadedCommand;
        public IRelayCommand LoadedCommand => loadedCommand ??= new RelayCommand(
            () =>
            {
                SwitchCommand.NotifyCanExecuteChanged();
            });

        private IRelayCommand openFileCommand;
        public IRelayCommand OpenFileCommand => openFileCommand ??= new RelayCommand(
            () =>
            {
                var openFileDialog = new OpenFileDialog()
                {
                    Filter = "(*.json)|*.json",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    Json = File.ReadAllText(openFileDialog.FileName);
                }
            });

        private IRelayCommand switchCommand;
        public IRelayCommand SwitchCommand => switchCommand ??= new RelayCommand<object>(
            args =>
        {
            Yaml = yamlService.ConvertFromJson(json);
        }, args => !string.IsNullOrWhiteSpace(Json));
    }
}
