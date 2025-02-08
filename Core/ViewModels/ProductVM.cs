namespace Core.ViewModels
{
    public class ProductVM
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Sku { get; set; }

        public string Description { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; } = 0;

        public string ProductType { get; set; }  //  Diamond, Gold

        // If Gold

        public string GoldPurity { get; set; } // 18 k

        public string GoldWeight { get; set; } // 3.00 GMS

        //  If Diamond
        public string ShapeName { get; set; }  // ROUND, CUSHION , PRINCESS
        
        public string CaratName { get; set; } // Weight --->  0.25, 0.50,0.75,1.00, 1.25,1.50,2.00

        public string CaratSize { get; set; } // Display like :- Shape & Carat ==>  3.50mm, 4.4 mm 

        public string ClarityName { get; set; } // FL, IF, VVS1,VVS2,	VS1 ,	VS2 ,	SI1 ,	SI2 ,	I1 ,	I2,	I3
        
        public string ColorName { get; set; }  //  Metal Colour

        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }

        public bool IsActivated { get; set; } = false;
    }
}
