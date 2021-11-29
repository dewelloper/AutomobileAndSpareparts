using Dal;
using System.Collections.Generic;

namespace BusinessObjects
{
    public class otomobilyedekparcaservisilangirisiViewModel
    {
        public List<ProductGroups> listpG { get; set; }
        public List<Categories> listCatG { get; set; }
        public List<Marks> listMarks { get; set; }
        public List<Denominations> denominationList { get; set; }
        public List<ProductStates> prStateList { get; set; }
        public Products product { get; set; }
        public List<Currencies> listCurrencies { get; set; }
        public int? productType { get; set; }
        public List<Cities> listCities { get; set; }
        public List<ModelYears> listModelYears { get; set; }
        public List<FuelTypes> listFuelTypes { get; set; }
        public List<GearTypes> listGearTypes { get; set; }
        public List<CaseTypes> listCaseTypes { get; set; }
        public List<Colors> listColors { get; set; }
        public List<VehicleTypes> listVehicleTypes { get; set; }
        public List<PlateNationalities> listPlateNationalities { get; set; }
        public List<DamageStates> listDamageStates { get; set; }
        public List<TractionTypes> listTractionTypes { get; set; }
        public List<GuarantySituations> listGuarantySituations { get; set; }
        public List<PublishDurations> listPublishDurations { get; set; }
        public List<EnginePowers> listEnginePowers { get; set; }
        public List<EngineVolumes> listEngineVolumes { get; set; }
        public List<ProductSeller> listProductSellers { get; set; }
        public int[] PrdcCats { get; set; }
        public int[] ProductGroupIds { get; set; }
    }
}
