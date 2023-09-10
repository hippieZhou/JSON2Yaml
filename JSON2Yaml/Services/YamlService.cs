using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using JSON2Yaml.Models;
using YamlDotNet.Serialization.EventEmitters;

namespace JSON2Yaml.Services
{
    public class YamlService : IYamlService
    {
        public string ConvertFromJson(string input, Indentation indentationMode = Indentation.TwoSpaces)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            try
            {
                object jsonObject = null;
                var token = JToken.Parse(input!);
                if (token is null)
                {
                    return string.Empty;
                }

                JsonSerializerSettings defaultJsonSerializerSettings = new()
                {
                    FloatParseHandling = FloatParseHandling.Decimal
                };
                                                         
                if (token is JArray)
                {
                    jsonObject = JsonConvert.DeserializeObject<ExpandoObject[]>(input!, defaultJsonSerializerSettings);
                }
                else
                {
                    jsonObject = JsonConvert.DeserializeObject<ExpandoObject>(input!, defaultJsonSerializerSettings);
                }

                if (jsonObject is not null and not string)
                {
                    var graph = new
                    {
                        env = ((IDictionary<string, object>)jsonObject).Select(x => new { Name = x.Key, Value = x.Value })
                    };
                    
                    int indent = 0;
                    indent = indentationMode switch
                    {
                        Indentation.TwoSpaces => 2,
                        Indentation.FourSpaces => 4,
                        _ => throw new NotSupportedException(),
                    };

                    var serializer = Serializer.FromValueSerializer(
                            new SerializerBuilder()
                            .WithDefaultScalarStyle(ScalarStyle.DoubleQuoted)
                            //.WithEventEmitter(nextEmitter => new QuoteSurroundingEventEmitter(nextEmitter))
                            .BuildValueSerializer(),
                            EmitterSettings.Default.WithBestIndent(indent).WithIndentedSequences());

                    string yaml = serializer.Serialize(graph);
                    if (string.IsNullOrWhiteSpace(yaml))
                    {
                        return string.Empty;
                    }

                    return yaml;
                }
                return string.Empty;
            }
            catch (JsonReaderException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        internal class QuoteSurroundingEventEmitter : ChainedEventEmitter
        {
            public QuoteSurroundingEventEmitter(IEventEmitter nextEmitter) : base(nextEmitter)
            { }

            public override void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
            {
                if (eventInfo.Source.StaticType == typeof(object))
                {
                    eventInfo.Style = ScalarStyle.DoubleQuoted;
                }
                base.Emit(eventInfo, emitter);
            }
        }
    }
}
