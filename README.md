# TWISTER
## A tool for data wrangling geographical data from open data portals such as Opendata.dk
The accompanying paper describing the tool is avaliable [here](https://github.com/theodor349/P6-Making-Sense-of-OpenData.dk-/blob/main/Paper.pdf).

### Requirements/ Installation
1. Make sure you have .NET 6.0 installed.
2. Clone the [repository](https://github.com/theodor349/P6-Making-Sense-of-OpenData.dk-)
3. Good to go.

### Setup
To setup TWISTER you will have to insert an input path and an output path. These paths should point at a folder (preferebly empty)
Both of these paths should be set in ```src/Parser/OpenDataParser/appsettings.Production.json```. Below is an example of the input and output paths.
```json
"Output": {
        "JsonText": "/Users/<username>/Desktop/output/"
    },
    "Input": {
        "FolderPath": "/Users/<username>/Desktop/input"
    },
```
The ```src/Parser/OpenDataParser/appsettings.json``` file is a template for what can be put into ```src/Parser/OpenDataParser/appsettings.Production.json```.

### Acceptable Input
TWISTER currently only supports input files of .csv or .geojson. Furthermore TWISTER is currently only intended to work with datasets that contain data about parking spots, or routes (bike routes, running trails etc.)

### Changing Labels
This will cover how to change the labesl in both the labeler and the decider.

#### Labeler
To change labels in the labeler the file ```src/Parser/OpenDataParser/LabelerLookup.json``` can be modified. A new label should have the form as seen below.
``` json
    {
      "Target": "<LabelName>",
      "Languages": [
        {
          "Language": "ENG",
          "Values": [
            "<English value 1>",
            ...
            "<English value N>"
          ]
        },
        {
          "Language": "DK",
          "Values": [
            "<Danish value 1>",
            ...
            "<Danish value N>"
          ]
        }
      ]
    }
```
This should then be inserted into the ```LookupTargets``` list.
When a new label has been inserted TWISTER will add the new label to properties which have a resemblance with the values given in each language list.

#### Decider
To change labels in the decider the file ```src/Parser/OpenDataParser/DatasetDeciderLookup.json``` can be modified. There are two different methods that help classify a dataset. The first is the [title specification](https://github.com/theodor349/P6-Making-Sense-of-OpenData.dk-/new/main?readme=1#title-specification) and the second is the [content specification](https://github.com/theodor349/P6-Making-Sense-of-OpenData.dk-/new/main?readme=1#content-specification).

##### Title Specification
The Title specification will help classify a dataset by the title name. This means that the ```Requirements``` are treated as strings that should be included in the name of a dataset file to classify a dataset.
The format of a new title specification is as seen below, and should be put in the ```TitleSpecification``` list.
```json
    {
        "DatasetClassification": "<Classification>",
        "Score": 1,
        "Requirements": [
            "<Requirement 1>",
            ...
            "<Requirement N>"
        ]
    }
```

If the title includes any of the requirements the dataset will be classified with the ```DatasetClassification```. 

##### Content Specification
The content specification will help classify a dataset by the labels that the labeler has set. This means that the ```Requirements``` are labels instead of a string that should be contained in the content.
The format of a new content specification is as seen below, and should be put in the ```ContentSpecification``` list.
```json
    {
      "DatasetClassification": "<Classification>",
      "Score": 0.6,
      "Requirements": [
          "<Requirement 1>",
          ...
          "<Requirement N>"
      ]
    }
```
If the content of a dataset has the required labels then the dataset will be classified with the given ```DatasetClassification```.

### Output Specializations 
Updating or adding output specializations can be done in the DatasetParser.cs file located in the DatsetParser project.
Adding a new specialization can be done by creating a new switch case and making sure that the Classifer can create that classification.
When adding/updateing a specialization, the Geofeature must be specefied together with a list of properties.
An example of a Route specilization can be seen below
```c#
description = new SpecializationDescription()
{
        GeoFeatureType = GeoFeatureType.LineString,
        Properties = new List<SpecializationPropertyDescription>()
        {
                new SpecializationPropertyDescription("Name", new List<string>(){ "Name" }),
                new SpecializationPropertyDescription("Length", new List<string>(){ "Length" }),
        }
};
```
