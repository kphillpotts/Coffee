using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coffee.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        private CoffeeItem selectedItem;

        public CoffeeItem SelectedItem 
        { 
            get => selectedItem;
            set => SetProperty(ref selectedItem, value); 
        }

        public ObservableRangeCollection<ShoppingCartItem> Items { get; set; }
            

        public decimal TotalPrice
        {
            get
            {
                decimal total = 0;
                foreach (var item in Items)
                {
                    total += item.Price;
                }
                return total;
            }
        }


        public OrderViewModel()
        {
            SelectedItem = new CoffeeItem()
            {
                Name = "Mocha Frappuccino",
                Description = "Buttery caramel syrup meets coffee, milk and ice for a rendezvous in the blender.",
                SmallPrice = 3.15m,
                MediumPrice = 3.45m,
                LargePrice = 4.20m,
                ImageUrl = "MochaFrappuccino.png"
            };

            // create a fake shopping cart item
            Items = new ObservableRangeCollection<ShoppingCartItem>();
            Items.CollectionChanged += Items_CollectionChanged;
            //Items.Add(new ShoppingCartItem
            //{
            //    Name = "Caramel Frappuccino",
            //    Size = "M",
            //    Price = 4.85m,
            //    Quantity = 1,
            //    ImageUrl = "MochaFrappuccino.png"
            //});
            //Items.Add(new ShoppingCartItem
            //{
            //    Name = "Mocha Frappuccino",
            //    Size = "L",
            //    Price = 5.20m,
            //    Quantity = 1,
            //    ImageUrl = "MochaFrappuccino.png"
            //});

        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TotalPrice));
        }
    }

    public class ShoppingCartItem : BaseViewModel
    {
        private CoffeeItem selectedItem;
        private decimal price;

        public ShoppingCartItem() { }

        public ShoppingCartItem(CoffeeItem selectedItem)
        {
            // make a shopping cart item from the current item
            Name = selectedItem.Name;
            Quantity = 1;
            Size = selectedItem.Size;
            ImageUrl = selectedItem.ImageUrl;
            Price = selectedItem.SelectedSizePrice;
        }

        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public decimal Price { 
            get => price; 
            set => SetProperty(ref price,value); }
        public string ImageUrl { get; internal set; }
    }

    public class CoffeeItem : BaseViewModel
    {
        private string name;

        public string Name { get => name; set => SetProperty(ref name, value); }
        public string Description { get; set; }
        public decimal SmallPrice { get; set; }
        public decimal MediumPrice { get; set; }
        public decimal LargePrice { get; set; }
        public string ImageUrl { get; set; }
        public string Size { get; internal set; }
        public decimal SelectedSizePrice 
        { 
            get
            {
                if (Size == "S")
                    return SmallPrice;
                if (Size == "M")
                    return MediumPrice;
                
                return LargePrice;
            }
        }
    }
}
