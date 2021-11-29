using Dal;
using System.Collections.Generic;
using System.Data;

namespace BusinessObjects
{
    public class otomobilvasitaViewModel
    {
        public List<ProductGroups> listpG { get; set; }
        public List<Categories> listCatG { get; set; }
        public List<Marks> listMarks { get; set; }
        public List<Products> products { get; set; }
        public List<Currencies> currencies { get; set; }
        public List<Cities> cities { get; set; }
        public List<FuelTypes> listFuelTypes { get; set; }
        public List<CaseTypes> listCaseTypes { get; set; }
        public List<GearTypes> listGearTypes { get; set; }
        public DataTable OrderBasket { get; set; }
        public List<UserMessages> UserMessages { get; set; }

        public string ViewType { get; set; }

        public string FormName { get; set; }
    }
}
