
using System.Collections.Generic;
using cManagement.Models.Dtos;

namespace cManagement.Models.ViewModels
{
    public class TruckList
    {
        public IEnumerable<TruckDto> Trucks { get; set; } = new List<TruckDto>();
        public bool IsAdmin { get; set; }
        public int Page { get; set; }      
        public int MaxPage { get; set; }     
    }
}
