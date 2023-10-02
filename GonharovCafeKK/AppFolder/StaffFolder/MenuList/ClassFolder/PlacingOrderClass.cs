using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GonharovCafeKK.AppFolder.StaffFolder.MenuList
{
    public class PlacingOrderClass
    {



        public string DishOrder { get; set; }

        public int AmountOrder { get; set; }

        public int PriceOrderOne { get; set; }
        public int PriceOrder
        {
            get
            {
                return PriceOrderOne * AmountOrder;

            }
        }

    }
}
