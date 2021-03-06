﻿using Newtonsoft.Json;

namespace StrongGrid.Model
{
    public class Template
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("versions")]
        public TemplateVersion[] Versions { get; set; }
    }
}
