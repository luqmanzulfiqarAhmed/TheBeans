using System;
using Newtonsoft.Json;

public class CoffeeBean
{
    [JsonProperty("_id")]
    public string Id { get; set; }

    public int Index { get; set; }

    public bool IsBOTD { get; set; }

    public string Cost { get; set; }

    public string Image { get; set; }

    public string Colour { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Country { get; set; }
}