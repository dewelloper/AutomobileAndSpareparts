namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.AutoServices",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ServiceName = c.String(),
            //            AuthorizedName = c.String(),
            //            AuthorizedSurname = c.String(),
            //            AuthorizedEmail = c.String(),
            //            AuthorizedPassword = c.String(),
            //            AuthorizedPhone = c.String(),
            //            Website = c.String(),
            //            AddressDefinition = c.String(),
            //            CountryId = c.Int(),
            //            CityId = c.Int(),
            //            RegionId = c.Int(),
            //            RegistererId = c.Long(),
            //            ModifierId = c.Long(),
            //            MapURL = c.String(),
            //            FeedCount = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.ProductPlaces",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //            AutoServiceId = c.Int(),
            //            IpAddress = c.String(),
            //            AutoService_Id = c.Int(),
            //            AutoService_Id1 = c.Int(),
            //            AutoServices_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.AutoServices", t => t.AutoServices_Id)
            //    .Index(t => t.AutoServices_Id);
            
            CreateTable(
                "dbo.BankBins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BIN = c.String(),
                        Location = c.String(),
                        Type = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsCompany = c.Boolean(),
                        TaxNumber = c.String(),
                        TaxOffice = c.String(),
                        IdentityNumber = c.String(),
                        Name = c.String(),
                        Surname = c.String(),
                        Email = c.String(),
                        CompanyName = c.String(),
                        CityId = c.Int(nullable: false),
                        TownId = c.Int(nullable: false),
                        SubDistrictId = c.Int(nullable: false),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        GsmNumber = c.String(),
                        FaxNumber = c.String(),
                        OrderNote = c.String(),
                        Campaign = c.Decimal(precision: 18, scale: 2),
                        OrderId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CaseTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Categories",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            RootLevel = c.Int(nullable: false),
            //            ParentId = c.Int(),
            //            Name = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChatLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderUserId = c.Int(nullable: false),
                        ReceiverUserId = c.Int(nullable: false),
                        Message = c.String(),
                        Guid = c.String(),
                        Ip = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityPlateCode = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CountryCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Credits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Long(),
                        CustomerId = c.Long(),
                        Price = c.Decimal(precision: 18, scale: 2),
                        CreditcardNumber = c.String(),
                        CreditcardOwner = c.String(),
                        SecureCode = c.String(),
                        Month = c.Int(),
                        Year = c.Int(),
                        Installments = c.Int(),
                        DateTime = c.DateTime(),
                        Ip = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DamageStates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Denominations",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //            ShortName = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        City_Id = c.Int(nullable: false),
                        Town_Id = c.Int(nullable: false),
                        Name = c.String(),
                        PostCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EnginePowers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EngineVolumes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FuelTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Galleries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                        WebAddress = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Fax = c.String(),
                        Address = c.String(),
                        MapURL = c.String(),
                        AuthorName = c.String(),
                        AuthorSurname = c.String(),
                        AuthorPhone = c.String(),
                        AuthorEmail = c.String(),
                        FeedCount = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GearTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GuarantySituations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Marks",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            MarkName = c.String(),
            //            MarkCode = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ModelYears",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Long(),
                        ProductId = c.Long(),
                        ProductPrice = c.Decimal(precision: 18, scale: 2),
                        Quantity = c.Int(),
                        ProductGrupId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Date = c.DateTime(),
                        Explanation = c.String(),
                        OrderStatusId = c.Int(),
                        PaymentTypeId = c.Int(),
                        TransportTypeId = c.Int(),
                        RequesterUserId = c.Int(),
                        RequesterAddressId = c.Int(),
                        ResponderUserId = c.Int(),
                        ResponderAddressId = c.Int(),
                        ResponderFirmTypeId = c.Int(),
                        ResponderFirmId = c.Int(),
                        Count = c.Int(),
                        IsActive = c.Boolean(),
                        IsBilling = c.Boolean(),
                        Discount = c.Int(),
                        IsTransportFree = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaymentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlateNationalities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PostComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostId = c.Long(nullable: false),
                        UserId = c.Int(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PostLikes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostId = c.Long(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PostShares",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostId = c.Long(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductCategoryProvider",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OtomotivistCategoryId = c.Int(),
                        OtherSystemStockCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductGroupProvider",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OtomotivistProductGroupId = c.Int(),
                        OtherSystemProductGroupId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.ProductGroups",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ParentId = c.Int(),
            //            CatId = c.Int(),
            //            ProductName = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Products",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            EqualId = c.Long(),
            //            SubProductId = c.Long(),
            //            CategoryId = c.Int(),
            //            GroupId = c.Int(),
            //            MarkId = c.Int(),
            //            StatusId = c.Int(),
            //            DenominationId = c.Int(),
            //            Quantity = c.Int(),
            //            PlaceId = c.Int(),
            //            Name = c.String(),
            //            Code = c.String(),
            //            Barcode = c.String(),
            //            Explanation = c.String(),
            //            ImagePath0 = c.String(),
            //            ImagePath1 = c.String(),
            //            ImagePath2 = c.String(),
            //            ImagePath3 = c.String(),
            //            ImagePath4 = c.String(),
            //            ImagePath5 = c.String(),
            //            ImagePath6 = c.String(),
            //            ImagePath7 = c.String(),
            //            ImagePath8 = c.String(),
            //            ImagePath9 = c.String(),
            //            ImagePath10 = c.String(),
            //            VideoPath = c.String(),
            //            OfferedPrice = c.Decimal(precision: 18, scale: 2),
            //            CurrentPrice = c.Decimal(precision: 18, scale: 2),
            //            PriceCurrencyId = c.Int(),
            //            ModelYear = c.Int(),
            //            RecordDate = c.DateTime(nullable: false),
            //            LastAccessDate = c.DateTime(),
            //            RegistererId = c.Int(),
            //            ModifierId = c.Int(),
            //            FeedCount = c.Int(),
            //            SpecialCode = c.String(),
            //            IsActive = c.Boolean(),
            //            Category_Id = c.Int(),
            //            ProductGroup_Id = c.Int(),
            //            ProductPlace_Id = c.Int(),
            //            ProductState_Id = c.Int(),
            //            ProductType = c.Int(),
            //            Km = c.String(),
            //            IsExchangable = c.Boolean(nullable: false),
            //            AAS = c.Boolean(nullable: false),
            //            ASR = c.Boolean(nullable: false),
            //            HavaYastigiPerdeler = c.Boolean(nullable: false),
            //            Immobilizer = c.Boolean(nullable: false),
            //            ParkMesafeKontrol = c.Boolean(nullable: false),
            //            EDB = c.Boolean(nullable: false),
            //            HavaYastigiSurucu = c.Boolean(nullable: false),
            //            ABS = c.Boolean(nullable: false),
            //            Isofix = c.Boolean(nullable: false),
            //            Alarm = c.Boolean(nullable: false),
            //            EDL = c.Boolean(nullable: false),
            //            HavaYastigiYan = c.Boolean(nullable: false),
            //            LastikArizaGostergesi = c.Boolean(nullable: false),
            //            ArkaGorusKamerasi = c.Boolean(nullable: false),
            //            ESP = c.Boolean(nullable: false),
            //            HavaYastigiYolcu = c.Boolean(nullable: false),
            //            MerkeziKilit = c.Boolean(nullable: false),
            //            SeritTakipSistemi = c.Boolean(nullable: false),
            //            GeceGorus = c.Boolean(nullable: false),
            //            UzaktanKumanda = c.Boolean(nullable: false),
            //            Airmatic = c.Boolean(nullable: false),
            //            TCS = c.Boolean(nullable: false),
            //            YorgunlukTespitSistemi = c.Boolean(nullable: false),
            //            Bas = c.Boolean(nullable: false),
            //            AhsapDireksiyon = c.Boolean(nullable: false),
            //            DeriDireksiyon = c.Boolean(nullable: false),
            //            HafizaliKoltuklar = c.Boolean(nullable: false),
            //            Koltukisitma = c.Boolean(nullable: false),
            //            Sunroof = c.Boolean(nullable: false),
            //            AnahtarsizCalistirabilme = c.Boolean(nullable: false),
            //            DeriKoltuk = c.Boolean(nullable: false),
            //            HizSabitleyici = c.Boolean(nullable: false),
            //            Klima = c.Boolean(nullable: false),
            //            BlueTooth = c.Boolean(nullable: false),
            //            Telefon = c.Boolean(nullable: false),
            //            AyarlanabilirDireksiyon = c.Boolean(nullable: false),
            //            HidrolikDireksiyon = c.Boolean(nullable: false),
            //            YolBilgisayari = c.Boolean(nullable: false),
            //            ElektirikliIsitmaliCamlar = c.Boolean(nullable: false),
            //            IsitmaliDireksiyon = c.Boolean(nullable: false),
            //            SogutmaliTorpido = c.Boolean(nullable: false),
            //            HeadUpDisplay = c.Boolean(nullable: false),
            //            IleriGorusKamerasi = c.Boolean(nullable: false),
            //            GeriGorusKamerasi = c.Boolean(nullable: false),
            //            KolDayama = c.Boolean(nullable: false),
            //            IleriIleriVitesler = c.Boolean(nullable: false),
            //            AlasimJantlar = c.Boolean(nullable: false),
            //            FarYikama = c.Boolean(nullable: false),
            //            YagmurSensoru = c.Boolean(nullable: false),
            //            CamTavan = c.Boolean(nullable: false),
            //            Modifiyeli = c.Boolean(nullable: false),
            //            SisFari = c.Boolean(nullable: false),
            //            ElektirikliYanAynalar = c.Boolean(nullable: false),
            //            FarSensoru = c.Boolean(nullable: false),
            //            XenonFarlar = c.Boolean(nullable: false),
            //            ParkSensoru = c.Boolean(nullable: false),
            //            KatlanirAynalar = c.Boolean(nullable: false),
            //            GeceSensoru = c.Boolean(nullable: false),
            //            ArkaCamBuzCozucu = c.Boolean(nullable: false),
            //            PanoramikOnCam = c.Boolean(nullable: false),
            //            PanaromikCamTavan = c.Boolean(nullable: false),
            //            HardTop = c.Boolean(nullable: false),
            //            RadyoType = c.Boolean(nullable: false),
            //            RadyoCD = c.Boolean(nullable: false),
            //            Mp3Calabilme = c.Boolean(nullable: false),
            //            NavigasyonTV = c.Boolean(nullable: false),
            //            USBAUX = c.Boolean(nullable: false),
            //            AUX = c.Boolean(nullable: false),
            //            IPod = c.Boolean(nullable: false),
            //            Hoparlor6 = c.Boolean(nullable: false),
            //            CDDVDDegistirici = c.Boolean(nullable: false),
            //            NotSmoked = c.Boolean(nullable: false),
            //            FromForeigner = c.Boolean(nullable: false),
            //            UrgentSale = c.Boolean(nullable: false),
            //            Maturity = c.Boolean(nullable: false),
            //            ProductSeller = c.Int(),
            //            IsUsed = c.Boolean(),
            //            VehicleType = c.Int(),
            //            DamageState = c.Int(),
            //            GuarantySituation = c.Int(),
            //            PublishDuration = c.Int(),
            //            FuelType = c.Int(),
            //            GearType = c.Int(),
            //            Color = c.Int(),
            //            CaseType = c.Int(),
            //            EngineCapacity = c.Int(),
            //            EnginePower = c.Int(),
            //            Traction = c.Int(),
            //            Warranty = c.Int(),
            //            PlateNationality = c.Int(),
            //            Country = c.Int(),
            //            City = c.Int(),
            //            Region = c.Int(),
            //            District = c.Int(),
            //            ServiceName = c.String(),
            //            ServiceAddress = c.String(),
            //            ServiceType = c.String(),
            //            ServiceTelephone = c.String(),
            //            ServiceAuthor = c.String(),
            //            ServiceProperties = c.String(),
            //            ServicePoint = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductSeller",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.ProductStates",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PublishDurations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PostCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        RoleName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SpareParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        WebAddress = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Fax = c.String(),
                        Address = c.String(),
                        MapURL = c.String(),
                        AuthorName = c.String(),
                        AuthorSurname = c.String(),
                        AuthorPhone = c.String(),
                        AuthorEmail = c.String(),
                        FeedCount = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubDistrict",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityId = c.Int(nullable: false),
                        TownId = c.Int(nullable: false),
                        DistrictId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.sysdiagrams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        principal_id = c.Int(nullable: false),
                        diagram_id = c.Int(nullable: false),
                        version = c.Int(),
                        definition = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        Token = c.String(),
                        Date = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Towns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        City_Id = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TractionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransportTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserAdresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        CountryId = c.Int(),
                        CityId = c.Int(),
                        RegionId = c.Int(),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserEducationLevel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EducationLevel = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserFeedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        AutoServiceId = c.Int(),
                        GalleryId = c.Int(),
                        PleasedCount = c.Int(),
                        UnpleasedCount = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserFriends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FriendUserId = c.Int(nullable: false),
                        AutherTypeId = c.Int(),
                        OtherSettings = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserGender",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Gender = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserJob",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Job = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderUserId = c.Int(nullable: false),
                        ReceiverUserId = c.Int(nullable: false),
                        Title = c.String(),
                        Content = c.String(),
                        SendingDate = c.DateTime(nullable: false),
                        ReceiveDate = c.DateTime(),
                        AttachedFilePath = c.String(),
                        IsRead = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        UploadPath = c.String(),
                        PostDate = c.DateTime(nullable: false),
                        ModifyDate = c.DateTime(),
                        LikeCount = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        UserName = c.String(),
                        UserTypeId = c.Int(),
                        eMail = c.String(),
                        Name = c.String(),
                        Surname = c.String(),
                        HomePhone = c.String(),
                        GSM = c.String(),
                        WorkPhone = c.String(),
                        Fax = c.String(),
                        DateOfBirth = c.String(),
                        Gender = c.Boolean(),
                        EducationLevel = c.Int(),
                        Job = c.Int(),
                        ProfilePhoto = c.String(),
                        TCid = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        RoleId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.UserTypes",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            UserId = c.Int(),
            //            AutoServiceId = c.Int(),
            //            GalleryId = c.Int(),
            //            SparePartsId = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VehicleTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.webpages_Membership",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        ConfirmationToken = c.String(),
                        IsConfirmed = c.Boolean(),
                        LastPasswordFailureDate = c.DateTime(),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        Password = c.String(),
                        PasswordChangedDate = c.DateTime(),
                        PasswordSalt = c.String(),
                        PasswordVerificationToken = c.String(),
                        PasswordVerificationTokenExpirationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.webpages_OAuthMembership",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Provider = c.String(),
                        ProviderUserId = c.String(),
                        UserId = c.Int(nullable: false),
                        webpages_Membership_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.webpages_Membership", t => t.webpages_Membership_Id)
                .Index(t => t.webpages_Membership_Id);
            
            CreateTable(
                "dbo.webpages_Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        RoleName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.webpages_Roleswebpages_Membership",
                c => new
                    {
                        webpages_Roles_Id = c.Int(nullable: false),
                        webpages_Membership_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.webpages_Roles_Id, t.webpages_Membership_Id })
                .ForeignKey("dbo.webpages_Roles", t => t.webpages_Roles_Id, cascadeDelete: true)
                .ForeignKey("dbo.webpages_Membership", t => t.webpages_Membership_Id, cascadeDelete: true)
                .Index(t => t.webpages_Roles_Id)
                .Index(t => t.webpages_Membership_Id);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.webpages_Roleswebpages_Membership", "webpages_Membership_Id", "dbo.webpages_Membership");
            //DropForeignKey("dbo.webpages_Roleswebpages_Membership", "webpages_Roles_Id", "dbo.webpages_Roles");
            //DropForeignKey("dbo.webpages_OAuthMembership", "webpages_Membership_Id", "dbo.webpages_Membership");
            //DropForeignKey("dbo.ProductPlaces", "AutoServices_Id", "dbo.AutoServices");
            //DropIndex("dbo.webpages_Roleswebpages_Membership", new[] { "webpages_Membership_Id" });
            //DropIndex("dbo.webpages_Roleswebpages_Membership", new[] { "webpages_Roles_Id" });
            //DropIndex("dbo.webpages_OAuthMembership", new[] { "webpages_Membership_Id" });
            //DropIndex("dbo.ProductPlaces", new[] { "AutoServices_Id" });
            //DropTable("dbo.webpages_Roleswebpages_Membership");
            //DropTable("dbo.webpages_Roles");
            //DropTable("dbo.webpages_OAuthMembership");
            //DropTable("dbo.webpages_Membership");
            //DropTable("dbo.VehicleTypes");
            //DropTable("dbo.UserTypes");
            //DropTable("dbo.UserRole");
            //DropTable("dbo.UserProfile");
            //DropTable("dbo.UserPosts");
            //DropTable("dbo.UserMessages");
            //DropTable("dbo.UserJob");
            //DropTable("dbo.UserGender");
            //DropTable("dbo.UserFriends");
            //DropTable("dbo.UserFeedbacks");
            //DropTable("dbo.UserEducationLevel");
            //DropTable("dbo.UserAdresses");
            //DropTable("dbo.TransportTypes");
            //DropTable("dbo.TractionTypes");
            //DropTable("dbo.Towns");
            //DropTable("dbo.Tokens");
            //DropTable("dbo.sysdiagrams");
            //DropTable("dbo.SubDistrict");
            //DropTable("dbo.SpareParts");
            //DropTable("dbo.Roles");
            //DropTable("dbo.Regions");
            //DropTable("dbo.PublishDurations");
            //DropTable("dbo.ProductTypes");
            //DropTable("dbo.ProductStates");
            //DropTable("dbo.ProductSeller");
            //DropTable("dbo.Products");
            //DropTable("dbo.ProductGroups");
            //DropTable("dbo.ProductGroupProvider");
            //DropTable("dbo.ProductCategoryProvider");
            //DropTable("dbo.PostShares");
            //DropTable("dbo.PostLikes");
            //DropTable("dbo.PostComments");
            //DropTable("dbo.PlateNationalities");
            //DropTable("dbo.PaymentTypes");
            //DropTable("dbo.Orders");
            //DropTable("dbo.OrderDetails");
            //DropTable("dbo.ModelYears");
            //DropTable("dbo.Marks");
            //DropTable("dbo.GuarantySituations");
            //DropTable("dbo.GearTypes");
            //DropTable("dbo.Galleries");
            //DropTable("dbo.FuelTypes");
            //DropTable("dbo.EngineVolumes");
            //DropTable("dbo.EnginePowers");
            //DropTable("dbo.Districts");
            //DropTable("dbo.Denominations");
            //DropTable("dbo.DamageStates");
            //DropTable("dbo.Currencies");
            //DropTable("dbo.Credits");
            //DropTable("dbo.Countries");
            //DropTable("dbo.Colors");
            //DropTable("dbo.Cities");
            //DropTable("dbo.ChatLogs");
            //DropTable("dbo.Categories");
            //DropTable("dbo.CaseTypes");
            //DropTable("dbo.Bills");
            //DropTable("dbo.BankBins");
            //DropTable("dbo.ProductPlaces");
            //DropTable("dbo.AutoServices");
            //DropTable("dbo.AuthTypes");
        }
    }
}
