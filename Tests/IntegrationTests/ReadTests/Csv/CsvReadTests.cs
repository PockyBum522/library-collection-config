﻿using System;
using System.Collections.Generic;
using System.IO;
using CollectionConfig.net.Main;
using FluentAssertions;
using IntegrationTests.TestResources.ExamplePerson;
using NUnit.Framework;

namespace IntegrationTests.ReadTests.CSV;

public class CsvReadTests
{
    private const string ExamplePersonListCsvPath = @"TestResources\ExamplePerson\ExamplePersonList.csv";
    
    [Test]
    public void ThisTest_WhenStarted_ResourcesCsvShouldBePresent()
    {
        var result = File.Exists(ExamplePersonListCsvPath);
    
        result.Should().Be(true);
    }
    
    [Test]
    public void CollectionConfigurationBuilder_WhenBuiltWithCsvFile_ShouldNotBeNull()
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExamplePersonListCsvPath).Build();

        myList.Should().NotBeNull();
    }
    
    [Test]
    [TestCase(0, "David")]
    [TestCase(1, "Alyssa")]
    [TestCase(2, "Robin")]
    [TestCase(3, "Dyamond")]
    public void CollectionConfiguration_StringsWhenAccessed_NamesShouldBeCorrect(int index, string expected)
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExamplePersonListCsvPath)
                .Build();

        var result = myList[index].Name;
        
        result.Should().Be(expected);
    }    
    
    [Test]
    [TestCase(0, 32)]
    [TestCase(1, 26)]
    [TestCase(2, 28)]
    [TestCase(3, 31)]
    public void CollectionConfiguration_IntsWhenAccessed_AgesShouldBeCorrect(int index, int expected)
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExamplePersonListCsvPath).Build();
    
        var result = myList[index].Age;
        
        result.Should().Be(expected);
    }
    
    [Test]
    [TestCase(0, 3.15)]
    [TestCase(1, 2.12)]
    [TestCase(2, 1.23)]
    [TestCase(3, 2.23)]
    public void CollectionConfiguration_DoublesWhenAccessed_MilesRunShouldBeCorrect(int index, double expected)
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExamplePersonListCsvPath).Build();
    
        var result = myList[index].MilesRun;
        
        result.Should().Be(expected);
    }
    
    [Test]
    public void CollectionConfiguration_WhenBuiltWithoutUseXFile_ShouldThrowException()
    {
        var buildAttempt = () => { new CollectionConfigurationBuilder<IList<IExamplePerson>>().Build(); };

        buildAttempt.Should().Throw<ArgumentException>();
    }
}
