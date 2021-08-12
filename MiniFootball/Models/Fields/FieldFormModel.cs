﻿namespace MiniFootball.Models.Fields
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;

    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Field;

    public class FieldFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Empty)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = Range)]
        public string Name { get; set; }

        //[Required]
        [StringLength(CountryMaxLength, MinimumLength = CountryMinLength, ErrorMessage = Range)]
        [Display(Name = "Select Country:")]
        public string CountryName { get; set; }
        public Country Country { get; set; }

        public IEnumerable<string> Countries { get; set; }

        //[Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength, ErrorMessage = Range)]
        [Display(Name = "City")]
        public string CityName { get; set; }
        public City City { get; set; }

        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength, ErrorMessage = Range)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Image URL")]
        [Url]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public bool Parking { get; set; }

        public bool Shower { get; set; }

        public bool ChangingRoom { get; set; }

        public bool Cafe { get; set; }

        [Required(ErrorMessage = Empty)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = Range)]
        public string Description { get; set; }
    }
}