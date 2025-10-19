using System.Collections.Generic;

namespace MobileAPI.Models
{
    public class CafeteriaMenu : EntityBase
    {
        public CafeteriaMenu() 
        {
            MenuItems = new List<CafeteriaItem>();
        }

        public List<CafeteriaItem> MenuItems { get; set; }
    }
}
