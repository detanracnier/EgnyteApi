using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EgnyteApi.Models;
using Newtonsoft.Json;

namespace EgnyteApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private const string _url = "https://prettztestapp.egnyte.com";
        
        public FileController()
        {
            _httpClient = new HttpClient();
        }

        // http://localhost:5001/file/pdfs/centerbottom
        [HttpGet]
        [Route("pdfs/{fileName}")]
        public string Get(string fileName)
        {
            var token = Authorize();
            var searchResponse = SearchForFile(token, fileName);

            ValidateResponse(searchResponse);

            var file = searchResponse.Results.First();

            CopyFile(token, file);

            return "File_Copied";
        }

        private void CopyFile(AuthorizeResponse token, FileSearchResponseResult fileSearchResponseResult)
        {
            var requestBody = new FileCopyRequest();
            requestBody.Action = "copy";
            requestBody.Destination = 
                $"/Shared/Documents/{DateTime.Now.ToString("yyyy-MM-dd_hh-mm")}/{fileSearchResponseResult.Name}";

            var requestBodyJson = JsonConvert.SerializeObject(requestBody);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_url}/pubapi/v1/fs/{fileSearchResponseResult.Path}");
            request.Headers.Add("Authorization", $"Bearer {token.AccessToken}");
            request.Content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

            var response = _httpClient.SendAsync(request).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
        }

        private void ValidateResponse(FileSearchResponse searchResponse)
        {
            if (searchResponse.Results.Count > 1)
            {
                var msg = "More than one file found for that name";
                msg += $" - {string.Join(",", searchResponse.Results.Select(x => x.Path))}";
                throw new Exception(msg);
            }

            if (searchResponse.Results.Count == 0)
            {
                var msg = "No file found for that name";
                throw new Exception(msg);
            }

        }

        private FileSearchResponse SearchForFile(AuthorizeResponse token, string fileName)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_url}/pubapi/v1/search?query={fileName}");
            request.Headers.Add("Authorization", $"Bearer {token.AccessToken}");

            var response = _httpClient.SendAsync(request).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;

            var searchResponse = JsonConvert.DeserializeObject<FileSearchResponse>(responseBody);

            return searchResponse;
        }

        private AuthorizeResponse Authorize()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_url}/puboauth/token");

            var key = "3e9du2wpcqdw2ae4ehrjs7sq";
            var username = "prettz009";
            var password = "ukDjRL4qLEvcQkQX4hpb";
            var body = $"client_id={key}&username={username}&password={password}&grant_type=password";

            request.Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = _httpClient.SendAsync(request).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;

            var authorizeResponse = JsonConvert.DeserializeObject<AuthorizeResponse>(responseBody);

            return authorizeResponse;
        }

    }
}
