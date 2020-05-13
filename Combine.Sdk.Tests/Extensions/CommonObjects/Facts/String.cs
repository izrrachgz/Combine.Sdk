using Xunit;
using System;
using System.Net.Mail;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.CommonObjects.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the extension
  /// methods availables for String type objects
  /// </summary>
  public class FactStringTests
  {
    /// <summary>
    /// Creates a new instance of Fact String Tests
    /// </summary>
    public FactStringTests()
    {

    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with String object
    /// </summary>
    [Fact]
    public void IsNotValid()
    {
      string s = @"";
      Assert.True(s.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with String Array object
    /// </summary>
    [Fact]
    public void IsNotValidArray()
    {
      string[] array = new string[1] { @"" };
      Assert.True(array.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with String List object
    /// </summary>
    [Fact]
    public void IsNotValidList()
    {
      List<string> list = new List<string>(0);
      Assert.True(list.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with String Array List object
    /// </summary>
    [Fact]
    public void IsNotValidArrayList()
    {
      List<string[]> arrayList = new List<string[]>(0);
      Assert.True(arrayList.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNumber extension method
    /// evaluates correctly a number formated String object
    /// </summary>
    [Fact]
    public void IsNumber()
    {
      string number = @"123";
      Assert.True(number.IsNumber());
    }

    /// <summary>
    /// Proves that the IsEmail extension method
    /// evaluates correctly a e-mail formated String object
    /// </summary>
    [Fact]
    public void IsEmail()
    {
      string email = @"izrra.ch@icloud.com";
      email.IsEmail(out MailAddress mail);
      Assert.True(mail != null);
    }

    /// <summary>
    /// Proves that the IsDateTime extension method
    /// evaluates correctly a Date Time formated String object
    /// </summary>
    [Fact]
    public void IsDateTime()
    {
      string date = @"2020/04/10 22:30:00";
      date.IsDateTime(out DateTime dateValue);
      Assert.True(!dateValue.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsTimeSpan extension method
    /// evaluates correctly a Time Span formated String object
    /// </summary>
    [Fact]
    public void IsTimeSpan()
    {
      string time = @"22:30:00";
      time.IsTimeSpan(out TimeSpan timeValue);
      Assert.True(!timeValue.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsGuid extension method
    /// evaluates correctly a Guid formated String object
    /// </summary>
    [Fact]
    public void IsGuid()
    {
      string guid = @"0f8fad5b-d9cb-469f-a165-70867728950e";
      guid.IsGuid(out Guid value);
      Assert.True(!value.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsDirectoryPath extension method
    /// evaluates correctly a directory path formated String object
    /// </summary>
    [Fact]
    public void IsDirectoryPath()
    {
      string directory = AppDomain.CurrentDomain.BaseDirectory;
      Assert.True(directory.IsDirectoryPath());
    }

    /// <summary>
    /// Proves that the IsFilePath extension method
    /// evaluates correctly a file path formated String object
    /// </summary>
    [Fact]
    public void IsFilePath()
    {
      string dllFile = $@"{AppDomain.CurrentDomain.BaseDirectory}CommonObjects.Tests.dll";
      dllFile.IsFilePath(out Uri uri);
      Assert.True(!uri.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsFilePath extension method
    /// evaluates correctly a web resource file path formated String object
    /// </summary>
    [Fact]
    public void IsWebFilePath()
    {
      string dllFile = @"https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png";
      dllFile.IsFilePath(out Uri uri);
      Assert.True(!uri.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsWebDirectory extension method
    /// evaluates correctly a univeral resource locator formated String object
    /// </summary>
    [Fact]
    public void IsWebDirectory()
    {
      string url = @"https://www.google.com/";
      url.IsWebDirectory(out Uri uri);
      Assert.True(!uri.IsNotValid());
    }

    /// <summary>
    /// Proves that the SaveAsTextFile extension method
    /// saves correctly String content object into a file on disk
    /// </summary>
    [Fact]
    public async void SaveAsTextFile()
    {
      string content = @"Hello";
      string path = $@"{AppDomain.CurrentDomain.BaseDirectory}TestResults\Extensions\CommonObjects\";
      Assert.True(await content.SaveAsTextFile(path, @"Hello"));
    }
  }
}
