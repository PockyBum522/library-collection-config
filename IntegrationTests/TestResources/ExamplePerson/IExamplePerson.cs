﻿namespace CollectionConfig.net.IntegrationTests.TestResources.ExamplePerson;

/// <summary>
/// Example person model, used for testing
/// </summary>
public interface IExamplePerson
{
    string Name { get; set; }
    int Age { get; set; }
    double MilesRun { get; set; }
    string PetsName { get; set; }
}