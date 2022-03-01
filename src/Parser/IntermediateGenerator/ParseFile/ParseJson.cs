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
            Stack<ListAttribute> currentListAttr = new Stack<ListAttribute>();
            string? propName = null;
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    propName = HandleValueToken(reader, intermediate, currentListAttr, propName);
                }
                else if (reader.TokenType.Equals(JsonToken.StartArray) || reader.TokenType.Equals(JsonToken.StartObject))
                {
                    if (intermediate == null)
                    {
                        intermediate = new IntermediateObject();
                        datasetObj.Objects.Add(intermediate);
                    }
                    else
                    {
                        ListAttribute newListAttr;
                        if (propName != null)
                        {
                            newListAttr = new ListAttribute(propName);
                            propName = null;
                        }
                        else
                        {
                            newListAttr = new ListAttribute(reader.TokenType.ToString());
                        }
                        if (currentListAttr.Count == 0)
                        {
                            intermediate.Attributes.Add(newListAttr);
                            currentListAttr.Push(newListAttr);
                        }
                        else
                        {
                            ((List<ObjectAttribute>)currentListAttr.Peek().Value).Add(newListAttr);
                        }
                        currentListAttr.Push(newListAttr);
                    }
                }
                else if (reader.TokenType.Equals(JsonToken.EndArray) || reader.TokenType.Equals(JsonToken.EndObject))
                {
                    if (currentListAttr.Count > 0)
                    {
                        currentListAttr.Pop();
                    }
                }
                else if (reader.TokenType.Equals(JsonToken.Null))
                {
                    if (currentListAttr.Count == 0)
                    {
                        intermediate.Attributes.Add(FindAndCreateType(propName, reader));
                        propName = null;
                    }
                    else
                    {
                        ((List<ObjectAttribute>)currentListAttr.Peek().Value).Add(FindAndCreateType(propName, reader));
                        propName = null;
                    }
                }
                _logger.LogInformation("Token: " + reader.TokenType);
            }
            //_logger.LogInformation();
            return Task.FromResult(datasetObj);
        }

        public string HandleValueToken(JsonTextReader reader, IntermediateObject? intermediate, Stack<ListAttribute> currentListAttr, string? propName)
        {
            if (reader.TokenType.Equals(JsonToken.PropertyName))
            {
                propName = reader.Value.ToString();
            }
            else
            {
                if (currentListAttr.Count == 0)
                {
                    intermediate.Attributes.Add(FindAndCreateType(propName, reader));
                    propName = null;
                }
                else
                {
                    ((List<ObjectAttribute>)currentListAttr.Peek().Value).Add(FindAndCreateType(propName, reader));
                    propName = null;
                }
            }
            _logger.LogInformation("Token: " + reader.TokenType + " Value: " + reader.Value);
            return propName;
        }

        private ObjectAttribute FindAndCreateType(string propName, JsonTextReader reader)
        {
            if (propName == null)
            {
                propName = reader.TokenType.ToString().Replace(" ", "");
            }
            switch (reader.TokenType)
            {             
                case JsonToken.Integer:
                    return new LongAttribute(propName, (long)reader.Value);
                case JsonToken.Float:
                    return new DoubleAttribute(propName, (double)reader.Value);
                case JsonToken.String:
                    return new TextAttribute(propName, (string)reader.Value);
                case JsonToken.Null:
                    return new NullAttribute(propName);
                case JsonToken.Date:
                    return new DateAttribute(propName, (DateTime)reader.Value);
                default:
                    throw new Exception("Json token did not match any supported type: the type was " + reader.TokenType);
            }
        }
    }
}
