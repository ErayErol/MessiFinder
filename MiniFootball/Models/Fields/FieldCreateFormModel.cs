﻿namespace MiniFootball.Models.Fields
{
    using System.Collections.Generic;
    using Services.Fields;

    public class FieldCreateFormModel : FieldDetailServiceModel
    {
        public int CountryId { get; set; }
        public IEnumerable<string> Countries { get; set; }

        public int CityId { get; set; }
    }
}
