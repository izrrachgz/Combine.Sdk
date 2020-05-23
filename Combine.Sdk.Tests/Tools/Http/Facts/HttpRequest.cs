using Xunit;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Combine.Sdk.ToolBox.Http;
using System.Collections.Generic;
using Combine.Sdk.Extensions.Json;
using Combine.Sdk.ToolBox.Http.Models;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Data.Definitions.Response;
using Combine.Sdk.Data.Definitions.Collection;

namespace Combine.Sdk.Tests.Tools.Http.Facts
{
  /// <summary>
  /// 
  /// </summary>
  public class FactHttpRequestTests
  {
    /// <summary>
    /// 
    /// </summary>
    private string BinUrlBase { get; }

    /// <summary>
    /// 
    /// </summary>
    private string BinMethod { get; }

    /// <summary>
    /// 
    /// </summary>
    public FactHttpRequestTests()
    {
      JsonLoader<Configuration> config = new JsonLoader<Configuration>($@"{AppDomain.CurrentDomain.BaseDirectory}Tools\Http\HttpConfig.json");
      config.Load()
        .Wait();
      BinUrlBase = config.Instance.Values.GetFirst<string>(@"UrlBin");
      BinMethod = config.Instance.Values.GetFirst<string>(@"MethodBin");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PostJson()
    {
      HttpRequest http = new HttpRequest();
      HttpResponseMessage message = await http.Post(BinUrlBase, BinMethod, @"{hola:'adios'}");
      Assert.True(message.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PostBuffer()
    {
      HttpRequest http = new HttpRequest();
      byte[] content = new byte[3] { 1, 2, 3 };
      HttpResponseMessage message = await http.Post(BinUrlBase, BinMethod, content);
      Assert.True(message.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PostForm()
    {
      HttpRequest http = new HttpRequest();
      Dictionary<string, string> content = new Dictionary<string, string>(1)
      {
        {@"hola",@"adios"}
      };
      HttpResponseMessage message = await http.Post(BinUrlBase, BinMethod, content);
      Assert.True(message.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PostStream()
    {
      HttpRequest http = new HttpRequest();
      Stream content = File.OpenRead($@"{AppDomain.CurrentDomain.BaseDirectory}Tools\Http\HttpConfig.json");
      HttpResponseMessage message = await http.Post(BinUrlBase, BinMethod, content);
      Assert.True(message.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PutJson()
    {
      HttpRequest http = new HttpRequest();
      HttpResponseMessage message = await http.Put(BinUrlBase, BinMethod, @"{hola:'adios'}");
      Assert.True(message.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PutBuffer()
    {
      HttpRequest http = new HttpRequest();
      byte[] content = new byte[3] { 1, 2, 3 };
      HttpResponseMessage message = await http.Put(BinUrlBase, BinMethod, content);
      Assert.True(message.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PutForm()
    {
      HttpRequest http = new HttpRequest();
      Dictionary<string, string> content = new Dictionary<string, string>(1)
      {
        {@"hola",@"adios"}
      };
      HttpResponseMessage message = await http.Put(BinUrlBase, BinMethod, content);
      Assert.True(message.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PutStream()
    {
      HttpRequest http = new HttpRequest();
      Stream content = File.OpenRead($@"{AppDomain.CurrentDomain.BaseDirectory}Tools\Http\HttpConfig.json");
      HttpResponseMessage message = await http.Put(BinUrlBase, BinMethod, content);
      Assert.True(message.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Get()
    {
      HttpRequest http = new HttpRequest();
      HttpResponseMessage content = await http.Get(BinUrlBase, BinMethod);
      Assert.True(content.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetQuery()
    {
      HttpRequest http = new HttpRequest();
      string parametros = @"hola=adios";
      HttpResponseMessage content = await http.Get(BinUrlBase, BinMethod, parametros);
      Assert.True(content.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetKeys()
    {
      HttpRequest http = new HttpRequest();
      KeyValuePair<string, string> parametros = new KeyValuePair<string, string>(@"hola", @"adios");
      HttpResponseMessage content = await http.Get(BinUrlBase, BinMethod, parametros);
      Assert.True(content.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetDictionary()
    {
      HttpRequest http = new HttpRequest();
      Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>(1)
      {
        {@"hola",@"adios"}
      };
      HttpResponseMessage content = await http.Get(BinUrlBase, BinMethod, parametros);
      Assert.True(content.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetKeyList()
    {
      HttpRequest http = new HttpRequest();
      List<KeyValuePair<string, string>> parametros = new List<KeyValuePair<string, string>>(1)
      {
        new KeyValuePair<string, string>(@"hola",@"adios")
      };
      HttpResponseMessage content = await http.Get(BinUrlBase, BinMethod, parametros);
      Assert.True(content.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Delete()
    {
      HttpRequest http = new HttpRequest();
      HttpResponseMessage content = await http.Delete(BinUrlBase, BinMethod);
      Assert.True(content.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteQuery()
    {
      HttpRequest http = new HttpRequest();
      string parametros = @"hola=adios";
      HttpResponseMessage content = await http.Delete(BinUrlBase, BinMethod, parametros);
      Assert.True(content.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteKeys()
    {
      HttpRequest http = new HttpRequest();
      KeyValuePair<string, string> parametros = new KeyValuePair<string, string>(@"hola", @"adios");
      HttpResponseMessage content = await http.Delete(BinUrlBase, BinMethod, parametros);
      Assert.True(content.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteDictionary()
    {
      HttpRequest http = new HttpRequest();
      Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>(1)
      {
        {@"hola",@"adios"}
      };
      HttpResponseMessage content = await http.Delete(BinUrlBase, BinMethod, parametros);
      Assert.True(content.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteKeyList()
    {
      HttpRequest http = new HttpRequest();
      List<KeyValuePair<string, string>> parametros = new List<KeyValuePair<string, string>>(1)
      {
        new KeyValuePair<string, string>(@"hola",@"adios")
      };
      HttpResponseMessage content = await http.Delete(BinUrlBase, BinMethod, parametros);
      Assert.True(content.IsSuccessStatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Download()
    {
      string url = @"https://www.google.com";
      string method = @"/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png";
      HttpRequest http = new HttpRequest();
      ComplexResponse<RawBytes> content = await http.Download(url, method);
      Assert.True(content.Correct);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DownloadInPath()
    {
      string url = @"https://www.google.com";
      string method = @"/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png";
      HttpRequest http = new HttpRequest();
      BasicResponse content = await http.DownloadInPath(url, method, $@"{AppDomain.CurrentDomain.BaseDirectory}TestResults\Tools\Http\", @"LogoGoogle", @"png");
      Assert.True(content.Correct);
    }
  }
}
