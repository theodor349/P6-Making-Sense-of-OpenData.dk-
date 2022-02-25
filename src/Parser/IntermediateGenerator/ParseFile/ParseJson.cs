using Newtonsoft.Json.Linq;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.ComponentInterfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Models.ObjectAttributes;

namespace IntermediateGenerator.ParseFile
{
    public class ParseJson : IParseJson
    {
        
        private readonly ILogger<ParseJson> _logger;
        public ParseJson(ILogger<ParseJson> logger)
        {
            _logger = logger;
        }

        public Task<DatasetObject> Parse(string stringFile, string extensionName, string fileName)
        {
            DatasetObject datasetObj = new DatasetObject(extensionName.ToLower(), fileName.ToLower());
            JsonTextReader reader = new JsonTextReader(new StringReader(stringFile));

            IntermediateObject intermediate = null;
            ListAttribute? currentListAttr = null;
            int attrDepth = 0;
            string? propName = null;
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType.Equals(JsonToken.PropertyName))
                    {
                        propName = reader.Value.ToString();
                    }
                    else
                    {
                        if (attrDepth == 0)
                        {
                            intermediate.Attributes.Add(FindAndCreateType(propName, reader));
                        }
                        else
                        {
                            ((List<ObjectAttribute>)currentListAttr.Value).Add(FindAndCreateType(propName, reader));
                        }
                    }
                    _logger.LogInformation("Token: " + reader.TokenType + " Value: " + reader.Value);
                }
                else
                {
                    if (reader.TokenType.Equals(JsonToken.StartArray) || reader.TokenType.Equals(JsonToken.StartObject))
                    {
                        if (intermediate == null)
                        {
                            intermediate = new IntermediateObject();
                            datasetObj.Objects.Add(intermediate);
                        }
                        else
                        {
                            ListAttribute newListAttr = new ListAttribute(reader.TokenType.ToString());
                            if (attrDepth == 0)
                            {
                                intermediate.Attributes.Add(newListAttr);
                                currentListAttr = newListAttr;
                                attrDepth++;
                            }
                            else
                            {
                                ((List<ObjectAttribute>)currentListAttr.Value).Add(newListAttr);
                            }
                        }   
                    }
                    else if (reader.TokenType.Equals(JsonToken.EndArray) || reader.TokenType.Equals(JsonToken.EndObject))
                    {
                        attrDepth--;
                        if (attrDepth == 0)
                        {
                            currentListAttr = null;
                        }
                    }
                    _logger.LogInformation("Token: " + reader.TokenType);
                }

            }


            //_logger.LogInformation();

            return Task.FromResult(datasetObj);
        }

        private ObjectAttribute FindAndCreateType(string propName, JsonTextReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonToken.None:
                    break;
                case JsonToken.StartObject:
                    break;
                case JsonToken.StartArray:
                    break;
                case JsonToken.StartConstructor:
                    break;
                case JsonToken.PropertyName:
                    break;
                case JsonToken.Comment:
                    break;
                case JsonToken.Raw:
                    break;
                case JsonToken.Integer:
                    return new IntegerAttribute(propName, (int)reader.Value);
                    break;
                case JsonToken.Float:
                    return new DoubleAttribute(propName, (double)reader.Value);
                    break;
                case JsonToken.String:
                    return new TextAttribute(propName, (string)reader.Value);
                    break;
                case JsonToken.Boolean:
                    break;
                case JsonToken.Null:
                    break;
                case JsonToken.Undefined:
                    break;
                case JsonToken.EndObject:
                    break;
                case JsonToken.EndArray:
                    break;
                case JsonToken.EndConstructor:
                    break;
                case JsonToken.Date:
                    break;
                case JsonToken.Bytes:
                    break;
                default:
                    break;
            }
            throw new Exception("Json token didnt match any possible token");
        }
    }
}
