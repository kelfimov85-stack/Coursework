using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Coursework.Data;
using Coursework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework.ViewModels
{
    public partial class PageForAddProducts : ViewModelBase
    {
        private readonly Context db = new();

        [ObservableProperty]
        private string _nameNewProduct;
        [ObservableProperty]
        private decimal _priceNewProduct;
        [ObservableProperty]
        private int _quantityNewProduct;
        //[ObservableProperty]
        //private string _categoryNewProduct;

        [RelayCommand]
        public void SaveNewProduct()
        {
            var newProduct = new Products { Name = NameNewProduct, Quantity = QuantityNewProduct, Price = PriceNewProduct};
            db.Products.Add(newProduct);   // перевожу сущность в Added
            db.SaveChanges();     // вставляю данные в БД
        }

    }
}
