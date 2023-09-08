using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace JSON2Yaml
{
    public class MainViewModel : ObservableObject
    {
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
                    Filter = "(*.json)|*.json"
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
            var obj = JsonConvert.DeserializeObject<JObject>(json);
            var a = ConvertJTokenToObject(obj);
            Yaml = json;
        }, args => !string.IsNullOrWhiteSpace(Json));

        static object ConvertJTokenToObject(JToken token)
        {
            if (token is JValue)
                return ((JValue)token).Value;
            if (token is JArray)
                return token.AsEnumerable().Select(ConvertJTokenToObject).ToList();
            if (token is JObject)
                return token.AsEnumerable().Cast<JProperty>().ToDictionary(x => x.Name, x => ConvertJTokenToObject(x.Value));
            throw new InvalidOperationException("Unexpected token: " + token);
        }
    }
}
