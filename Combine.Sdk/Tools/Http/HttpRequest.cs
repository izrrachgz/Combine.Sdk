using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Tools.Http.Models;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Data.Representation.Response;

namespace Combine.Sdk.Tools.Http
{
  /// <summary>  
  /// Provides the mechanism to make http web requests
  /// </summary>
  public class HttpRequest
  {
    /// <summary>    
    /// Http headers that are going to be included
    /// in the requests
    /// </summary>
    private List<HttpHeader> Headers { get; }

    /// <summary>
    /// Creates a new http request instance
    /// </summary>
    /// <param name="headers"></param>
    public HttpRequest(List<HttpHeader> headers = null)
    {
      Headers = new List<HttpHeader>();
    }

    #region Private Methods

    /// <summary>    
    /// Adds all the supplied headers into the current
    /// request
    /// </summary>
    /// <param name="client">HttpClient reference</param>
    private void AddHeaders(HttpClient client)
    {
      if (!Headers.IsNotValid())
      {
        client.DefaultRequestHeaders.Clear();
        Headers.ForEach(h => client.DefaultRequestHeaders.Add(h.Name, h.Value));
      }
    }

    #endregion

    #region Post Type Request

    /// <summary>
    /// Sends a POST type request including the parameters values from 
    /// a json serialized object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="parameters">Parameters to include in request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> Post(string urlBase, string method, string parameters)
    {
      //Verify http request integrity
      if (!urlBase.IsWebDirectory(out Uri uri) || method.IsNotValid() || parameters.IsNotValid())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage response;
      try
      {
        using (HttpClient client = new HttpClient())
        {
          client.BaseAddress = new Uri(urlBase);
          AddHeaders(client);
          using (HttpContent content = new StringContent(parameters, Encoding.UTF8, @"application/json"))
          {
            response = await client.PostAsync(method, content);
            content.Dispose();
          }
          client.Dispose();
        }
      }
      catch (Exception ex)
      {
        response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return response;
    }

    /// <summary>
    /// Sends a POST type request including the parameters values from 
    /// a byte array object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="parameters">Parameters to include in request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> Post(string urlBase, string method, byte[] parameters)
    {
      //Verify http request integrity
      if (!urlBase.IsWebDirectory(out Uri uri) || method.IsNotValid() || parameters.IsNotValid())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage response;
      try
      {
        using (HttpClient client = new HttpClient())
        {
          client.BaseAddress = new Uri(urlBase);
          AddHeaders(client);
          using (HttpContent content = new ByteArrayContent(parameters))
          {
            response = await client.PostAsync(method, content);
            content.Dispose();
          }
          client.Dispose();
        }
      }
      catch (Exception ex)
      {
        response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return response;
    }

    /// <summary>
    /// Sends a POST type request including the parameters values from 
    /// a key dictionary object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="parameters">Parameters to include in request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> Post(string urlBase, string method, Dictionary<string, string> parameters)
    {
      //Verify http request integrity
      if (!urlBase.IsWebDirectory(out Uri uri) || method.IsNotValid() || parameters.IsNotValid())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage response;
      try
      {
        using (HttpClient client = new HttpClient())
        {
          client.BaseAddress = new Uri(urlBase);
          AddHeaders(client);
          using (HttpContent content = new FormUrlEncodedContent(parameters))
          {
            response = await client.PostAsync(method, content);
            content.Dispose();
          }
          client.Dispose();
        }
      }
      catch (Exception ex)
      {
        response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return response;
    }

    /// <summary>
    /// Sends a POST type request including the parameters values from 
    /// a data stream object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="parameters">Parameters to include in request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> Post(string urlBase, string method, Stream parameters)
    {
      //Verify http request integrity
      if (!urlBase.IsWebDirectory(out Uri uri) || method.IsNotValid() || parameters.IsNotValid())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage response;
      try
      {
        using (HttpClient client = new HttpClient())
        {
          client.BaseAddress = new Uri(urlBase);
          if (Headers != null)
          {
            client.DefaultRequestHeaders.Clear();
            Headers.ForEach(h => client.DefaultRequestHeaders.Add(h.Name, h.Value));
          }
          using (HttpContent content = new StreamContent(parameters))
          {
            response = await client.PostAsync(method, content);
            content.Dispose();
          }
          client.Dispose();
        }
      }
      catch (Exception ex)
      {
        response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return response;
    }

    #endregion

    #region Get Type Request

    /// <summary>
    /// Sends a GET type request including the parameters values from 
    /// a alphanumeric string object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="parameters">Parameters to include in request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> Get(string urlBase, string method, string parameters = null)
    {
      //Verify http request integrity
      if (!urlBase.IsWebDirectory(out Uri uri) || method.IsNotValid())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      if (parameters != null && parameters.IsNotValid())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage response;
      try
      {
        using (HttpClient client = new HttpClient())
        {
          client.BaseAddress = new Uri(urlBase);
          AddHeaders(client);
          parameters = parameters == null ? @"" : parameters.StartsWith(@"?") ? parameters : @"?" + parameters;
          response = await client.GetAsync(method + parameters);
          client.Dispose();
        }
      }
      catch (Exception ex)
      {
        response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return response;
    }

    /// <summary>
    /// Sends a GET type request including the parameters values from 
    /// a key value pair object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="parameters">Parameters to include in request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> Get(string urlBase, string method, KeyValuePair<string, string> parameters)
      => await Get(urlBase, method, $@"{parameters.Key}={parameters.Value}");

    /// <summary>
    /// Sends a GET type request including the parameters values from 
    /// a key value dictionary object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="parameters">Parameters to include in request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> Get(string urlBase, string method, Dictionary<string, dynamic> parameters)
      => await Get(urlBase, method, string.Join(@"&", parameters.Select(p => $@"{p.Key}={p.Value}")));

    /// <summary>
    /// Sends a GET type request including the parameters values from 
    /// a key value pair list object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="parameters">Parameters to include in request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> Get(string urlBase, string method, List<KeyValuePair<string, string>> parameters)
      => await Get(urlBase, method, string.Join(@"&", parameters.Select(p => $@"{p.Key}={p.Value}")));

    #endregion

    #region Put Type Request

    /// <summary>
    /// Sends a PUT type request including the parameters values from 
    /// a json serialized object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="parameters">Parameters to include in request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> Put(string urlBase, string method, string parameters)
    {
      //Verify http request integrity
      if (!urlBase.IsWebDirectory(out Uri uri) || method.IsNotValid() || parameters.IsNotValid())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage response;
      try
      {
        using (HttpClient client = new HttpClient())
        {
          client.BaseAddress = new Uri(urlBase);
          AddHeaders(client);
          using (HttpContent content = new StringContent(parameters, Encoding.UTF8, @"application/json"))
          {
            response = await client.PutAsync(method, content);
            content.Dispose();
          }
          client.Dispose();
        }
      }
      catch (Exception ex)
      {
        response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return response;
    }

    /// <summary>
    /// Sends a PUT type request including the parameters values from 
    /// a byte array object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="parameters">Parameters to include in request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> Put(string urlBase, string method, byte[] parameters)
    {
      //Verify http request integrity
      if (!urlBase.IsWebDirectory(out Uri uri) || method.IsNotValid() || parameters.IsNotValid())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage response;
      try
      {
        using (HttpClient client = new HttpClient())
        {
          client.BaseAddress = new Uri(urlBase);
          AddHeaders(client);
          using (HttpContent content = new ByteArrayContent(parameters))
          {
            response = await client.PutAsync(method, content);
            content.Dispose();
          }
          client.Dispose();
        }
      }
      catch (Exception ex)
      {
        response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return response;
    }

    /// <summary>
    /// Sends a PUT type request including the parameters values from 
    /// a key value dictionary object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="parameters">Parameters to include in request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> Put(string urlBase, string method, Dictionary<string, string> parameters)
    {
      //Verify http request integrity
      if (!urlBase.IsWebDirectory(out Uri uri) || method.IsNotValid() || parameters.IsNotValid())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage response;
      try
      {
        using (HttpClient client = new HttpClient())
        {
          client.BaseAddress = new Uri(urlBase);
          AddHeaders(client);
          using (HttpContent content = new FormUrlEncodedContent(parameters))
          {
            response = await client.PutAsync(method, content);
            content.Dispose();
          }
          client.Dispose();
        }
      }
      catch (Exception ex)
      {
        response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return response;
    }

    /// <summary>
    /// Sends a PUT type request including the parameters values from 
    /// a data stream object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="parameters">Parameters to include in request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> Put(string urlBase, string method, Stream parameters)
    {
      //Verify http request integrity
      if (!urlBase.IsWebDirectory(out Uri uri) || method.IsNotValid() || parameters.IsNotValid())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage response;
      try
      {
        using (HttpClient client = new HttpClient())
        {
          client.BaseAddress = new Uri(urlBase);
          AddHeaders(client);
          using (HttpContent content = new StreamContent(parameters))
          {
            response = await client.PutAsync(method, content);
            content.Dispose();
          }
          client.Dispose();
        }
      }
      catch (Exception ex)
      {
        response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return response;
    }

    #endregion

    #region Delete Type Request

    /// <summary>
    /// Sends a DELETE type request including the parameters values from 
    /// a alphanumeric string object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="parameters">Parameters to include in request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> Delete(string urlBase, string method, string parameters = null)
    {
      //Verify http request integrity
      if (!urlBase.IsWebDirectory(out Uri uri) || method.IsNotValid())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      if (parameters != null && parameters.IsNotValid())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage response;
      try
      {
        using (HttpClient client = new HttpClient())
        {
          client.BaseAddress = new Uri(urlBase);
          AddHeaders(client);
          parameters = parameters == null ? @"" : parameters.StartsWith(@"?") ? parameters : @"?" + parameters;
          response = await client.DeleteAsync(method + parameters);
          client.Dispose();
        }
      }
      catch (Exception ex)
      {
        response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return response;
    }

    /// <summary>
    /// Sends a DELETE type request including the parameters values from 
    /// a key value pair object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="parameters">Parameters to include in request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> Delete(string urlBase, string method, KeyValuePair<string, string> parameters)
      => await Delete(urlBase, method, $@"{parameters.Key}={parameters.Value}");

    /// <summary>
    /// Sends a DELETE type request including the parameters values from 
    /// a key value dictionary object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="parameters">Parameters to include in request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> Delete(string urlBase, string method, Dictionary<string, dynamic> parameters)
      => await Delete(urlBase, method, string.Join(@"&", parameters.Select(p => $@"{p.Key}={p.Value}")));

    /// <summary>
    /// Sends a DELETE type request including the parameters values from 
    /// a key value pair list object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="parameters">Parameters to include in request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> Delete(string urlBase, string method, List<KeyValuePair<string, string>> parameters)
      => await Delete(urlBase, method, string.Join(@"&", parameters.Select(p => $@"{p.Key}={p.Value}")));

    #endregion

    #region Download Type Request

    /// <summary>    
    /// Downloads the specified resource as a byte array
    /// object
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <returns>DownloadFile</returns>
    public async Task<ComplexResponse<DownloadFile>> Download(string urlBase, string method)
    {
      //Verify http request integrity
      if (!urlBase.IsWebDirectory(out Uri uri) || method.IsNotValid())
        return new ComplexResponse<DownloadFile>(false, @"The supplied address is not valid for request operation.");
      ComplexResponse<DownloadFile> response;
      try
      {
        using (WebClient client = new WebClient())
        {
          Uri url = new Uri(urlBase + method);
          response = new ComplexResponse<DownloadFile>(new DownloadFile(await client.DownloadDataTaskAsync(url)));
          client.Dispose();
        }
      }
      catch (Exception ex)
      {
        response = new ComplexResponse<DownloadFile>(ex);
      }
      return response;
    }

    /// <summary>    
    /// Downloads the specified resource inside an app
    /// directory
    /// </summary>
    /// <param name="urlBase">Main resource address</param>
    /// <param name="method">Method resource address part</param>
    /// <param name="directory">Direccion destino del archivo</param>
    /// <param name="name">Nombre destino del archivo</param>
    /// <param name="extension">File extension</param>
    /// <returns>Task State</returns>
    public async Task<BasicResponse> DownloadInPath(string urlBase, string method, string directory = null, string name = null, string extension = null)
    {
      //Verify http request integrity
      if (!urlBase.IsWebDirectory(out Uri uri) || method.IsNotValid())
        return new BasicResponse(false, @"The supplied address is not valid for request operation.");
      BasicResponse response;
      try
      {
        using (WebClient client = new WebClient())
        {
          Uri url = new Uri(urlBase + method);
          directory = directory ?? AppDomain.CurrentDomain.BaseDirectory;
          name = name ?? Guid.NewGuid().ToString(@"N");
          extension = extension ?? @"dat";
          string route = $"{directory}{name}.{extension}";
          await client.DownloadFileTaskAsync(url, route);
          response = new BasicResponse(new FileInfo(route).Exists);
          client.Dispose();
        }
      }
      catch (Exception ex)
      {
        response = new BasicResponse(ex);
      }
      return response;
    }

    #endregion
  }
}
