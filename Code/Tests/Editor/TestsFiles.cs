using System.IO;
using FileManagement.Code.Runtime;
using FileManagement.Code.Tests.Editor;
using NUnit.Framework;
using UnityEngine;

public class TestsFiles
{
    private FileManager _fileManager;
    private string _pathRoot;

    [Test]
    public void CanSave()
    {
        var data = new TestData {value = 1};
        var path = Path.Combine(_pathRoot, "test.json");

        _fileManager.Save(path, data);
        Assert.That(File.Exists(path));
    }

    [Test]
    public void CanSaveAddMakeBackup()
    {
        var dataOld = new TestData {value = 1};
        var path = Path.Combine(_pathRoot, "test.json");
        _fileManager.Save(path, dataOld);
        Assert.That(File.Exists(path));

        var dataNew = new TestData {value = 2};
        var pathBackup = Path.Combine(_pathRoot, "testOld.json");
        _fileManager.Save(path, dataNew, pathBackup);
        Assert.That(File.Exists(path));
        Assert.That(File.Exists(pathBackup));

        var dataOldValidate = _fileManager.Load<TestData>(pathBackup);
        var dataNewValidate = _fileManager.Load<TestData>(path);
        Assert.NotNull(dataOldValidate);
        Assert.NotNull(dataNewValidate);

        Assert.That(dataOldValidate, Is.EqualTo(dataOld));
        Assert.That(dataOldValidate, Is.Not.EqualTo(dataNewValidate));
    }

    [Test]
    public void CanSaveAndLoad()
    {
        var data = new TestData {value = 1};
        var path = Path.Combine(_pathRoot, "test.json");

        _fileManager.Save(path, data);
        var dataSaved = _fileManager.Load<TestData>(path);

        Assert.That(data, Is.EqualTo(dataSaved));
    }

    [Test]
    public void CanSaveAndValidate()
    {
        var data = new TestData {value = 1};
        var path = Path.Combine(_pathRoot, "test.json");

        _fileManager.Save(path, data, null, true);
    }

    [SetUp]
    public void Setup()
    {
        _pathRoot = Path.Combine(Application.streamingAssetsPath, "testsSave");
        _fileManager = new FileManager(new JsonSettings());

        if (Directory.Exists(_pathRoot))
        {
            Directory.Delete(_pathRoot, true);
        }
    }
}