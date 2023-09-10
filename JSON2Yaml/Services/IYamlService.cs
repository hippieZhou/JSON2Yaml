using JSON2Yaml.Models;

namespace JSON2Yaml.Services
{
    public interface IYamlService
    {
        public string ConvertFromJson(string input, Indentation indentationMode = Indentation.TwoSpaces);
    }
}
