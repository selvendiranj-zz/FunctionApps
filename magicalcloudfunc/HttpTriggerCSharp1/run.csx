using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    // parse query parameter
    string name = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
        .Value;

    if (name == null)
    {
        // Get request body
        dynamic data = await req.Content.ReadAsAsync<object>();
        name = data?.name;
    }
    
    WebHookAsync();

    return name == null
        ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
        : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
}

static void WebHookAsync()
{
    var task = MakeRequest();
    task.Wait();

    var response = task.Result;
    var body = response.Content.ReadAsStringAsync().Result;
    Console.WriteLine(body);
}

private static async Task<HttpResponseMessage> MakeRequest()
{
    var httpClient = new HttpClient();
    return await httpClient.GetAsync(new Uri("https://requestb.in/19g8oh11"));
}