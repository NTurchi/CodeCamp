using System;
using System.ComponentModel.DataAnnotations;

namespace MyCodeCamp.Models
{
    public class CampModel
    {
        #region Camp Properties

        public string Url { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
		public string Moniker { get; set; }

		[Required]
		[MinLength(5)]
		[MaxLength(100)]
		public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

		[Required]
        [MinLength(25)]
		[MaxLength(4096)]
		public string Description { get; set; }
			
        #endregion


        #region Location Properties

        public string LocationAddress1 { get; set; }
        public string LocationAddress2 { get; set; }
        public string LocationAddress3 { get; set; }
        public string LocationCityTown   { get; set; }
		public string LocationStateProvince { get; set; }
        public string LocationPostalCode { get; set; }
        public string LocationCountry { get; set; }

        #endregion
    }
}
