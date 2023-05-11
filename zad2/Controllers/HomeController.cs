using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Gus.Models;

using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Gus.Controllers;

public class DataObject
{
    /*
    "id": 1,
    "nazwa": "Ceny",
    "id-nadrzedny-element": 727,
    "id-poziom": 1,
    "nazwa-poziom": "Dziedzina",
    "czy-zmienne": false
    */

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("nazwa")]
    public string Name { get; set; }

    [JsonProperty("id-nadrzedny-element")]
    public int IdParent { get; set; }

    [JsonProperty("id-poziom")]
    public int IdLevel { get; set; }

    [JsonProperty("nazwa-poziom")]
    public string LevelName { get; set; }

    [JsonProperty("czy-zmienne")]
    public bool Variable { get; set; }

    public DataObject () {
        Name = "";
        LevelName = "";
    }
}

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        const string URL = "https://api-dbw.stat.gov.pl/api/area/area-area";

        HttpClient client = new HttpClient();

        client.BaseAddress = new Uri(URL);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = client.GetAsync("").Result;

        List<DataObject> DataObjects = new List<DataObject>();

        if (response.IsSuccessStatusCode) {
            string data = await response.Content.ReadAsStringAsync();

            DataObjects = JsonConvert.DeserializeObject<List<DataObject>>(data);

            //foreach (DataObject d in DataObjects) {
            //    Console.WriteLine("{0} : {1} : {2} : {3} : {4} : {5}", d.Id, d.Name, d.IdParent, d.IdLevel, d.LevelName, d.Variable);
            //}
        } else {
            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        }

        client.Dispose();

        //string data = System.IO.File.ReadAllText("data/data.txt");

        //List<DataObject> DataObjects = JsonConvert.DeserializeObject<List<DataObject>>(data);

        return View(DataObjects);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
