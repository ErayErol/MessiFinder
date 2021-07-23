﻿namespace MessiFinder.Services.Countries
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Playground;

    public class CreateGameFirstStepServiceModel
    {
        public string Country { get; set; }

        public IEnumerable<string> Countries { get; set; }

        [Required]
        [StringLength(TownMaxLength, MinimumLength = TownMinLength, ErrorMessage = Range)]
        public string Town { get; set; }
    }
}