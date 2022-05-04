﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Castle.DynamicProxy;
using CollectionConfig.net.Common.Logic.Interfaces;
using CollectionConfig.net.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CollectionConfig.net.Common.Logic.Json;

/// <summary>
/// Handles taking a CSV and making a proxy list with all the values
/// </summary>
public class JsonCacheLoader : ICacheLoader
{
    private readonly ProxyGenerator _generator = new ();
    private readonly IFileReader _fileReader;
    
    /// <summary>
    /// Constructor to take injected dependencies
    /// </summary>
    /// <param name="jsonFileReader">Injected</param>
    public JsonCacheLoader(JsonFileReader jsonFileReader)
    {
        _fileReader = jsonFileReader;
    }

    /// <summary>
    /// Returns data from the JSON on disk in the form of List of ProxiedListElement, presumably to be cached 
    /// </summary>
    /// <param name="instanceData">Injected so that we have the FilePath</param>
    /// <returns>Data from the JSON on disk in the form of List of ProxiedListElement</returns>
    public List<FileElement> UpdateCachedDataFromFile(CollectionConfigurationInstanceData instanceData)
    {
        var rawJsonData = _fileReader.Read(instanceData.FullFilePath);
        
        var returnList = new List<FileElement>();
        
        var dynamicJsonObject = JsonConvert.DeserializeObject(rawJsonData);

        var jArrayObject = (JArray)(dynamicJsonObject ?? throw new ArgumentNullException(
                                        nameof(dynamicJsonObject), 
                                        "Could not deserialize JSON data into JArray"));

        var positionInList = 0;
        
        foreach (var jsonToken in jArrayObject.Children())
        {
            var fileElement = MakeFileElementFrom(jsonToken, positionInList);
            
            returnList.Add(fileElement);

            positionInList++;
        }
        
        return returnList;
    }

    private FileElement MakeFileElementFrom(JToken jsonToken, int positionInList)
    {
        var returnFileElement = new FileElement()
        {
            PositionInList = positionInList
        };

        foreach (JProperty jElementToken in jsonToken.Children())
        {
            var elementValue = (JValue)(jElementToken.First ?? throw new ArgumentNullException());
            
            returnFileElement.StoredValues.Add(
                new KeyValuePair<string, string>(
                    jElementToken.Name, elementValue.ToString(CultureInfo.InvariantCulture)
                ));
        }
        
        return returnFileElement;
    }
}