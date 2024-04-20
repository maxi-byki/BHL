using System.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BHL_url_server.Controllers;

public class HttpController
{
    // [Route("api/[controller]")]
    // [ApiController]
    private readonly HttpClient _httpClient;

    public HttpController(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }
    
}