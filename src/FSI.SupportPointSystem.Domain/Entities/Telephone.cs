using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FSI.SupportPointSystem.Domain.Entities
{
    public class Telephone
    {
        [JsonPropertyName("ddd")]
        public string Ddd { get; set; }

        [JsonPropertyName("numero")]
        public string Numero { get; set; }

        [JsonPropertyName("is_fax")]
        public bool IsFax { get; set; }
    }
}
