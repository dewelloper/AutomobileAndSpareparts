using Dal;
using System.Collections.Generic;

namespace BusinessObjects
{
    public class PrdcDetailViewModel
    {
        public Products selectedProduct { get; set; }
        public List<Denominations> denominationList { get; set; }

        public List<string> ProductGroups { get; set; }

        public string prdcGroup { get; set; }
        public string prdcCat { get; set; }
        public string prdcMark { get; set; }
        public string prdcCurrency { get; set; }
        public string prdcGroupIds { get; set; }
        public string prdcCatIds { get; set; }
        public string prdcEngineCap { get; set; }
        public string prdcEnginePow { get; set; }
        public string prdcVehicleTyp { get; set; }
        public string prdcCaseTyp { get; set; }
        public string prdcColor { get; set; }
        public string prdcGearTyp { get; set; }
        public string prdcFuelTyp { get; set; }
        public string prdcSeller { get; set; }
        public string prdcOwnerName { get; set; }
        public string prdcOwnerPhone { get; set; }
        public string prdcCity { get; set; }
        public string prdcTown { get; set; }
        public string prdcDistrict { get; set; }
    }
}
