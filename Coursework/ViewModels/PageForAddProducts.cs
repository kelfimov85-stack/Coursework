using Azure.Core;
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

        Products newProduct = new Products();

        [ObservableProperty]
        private string? _nameNewProduct;
        partial void OnNameNewProductChanged(string? value)
        {
            if (value != null)
            {
                newProduct.Name = value;
            }
            else
            {
                newProduct.Name = null;
            }
        }

        [ObservableProperty]
        private string? _priceNewProduct;
        partial void OnPriceNewProductChanged(string? value) // Функция для добавления в свойство переменной пользователя
        {
            if (decimal.TryParse(value, out decimal result)) // Программа пробует преобразовать string
            {
                newProduct.Price = result; // Если все успешно, то вставляет значение пользователя в модель
            }
            else
            {
                newProduct.Price = null; // Если не успещно, то ничего не вставляет
            }
        }

        [ObservableProperty]
        private string? _quantityNewProduct;
        partial void OnQuantityNewProductChanged(string? value)
        {
            if (int.TryParse(value, out int result))
            {
                newProduct.Quantity = result;
            }
            else
            {
                newProduct.Quantity = null;
            }
        }

        [ObservableProperty]
        private string? _categoryNewProduct;
        partial void OnCategoryNewProductChanged(string? value)
        {
            if(int.TryParse(value, out int result))
            {
                Categories categories = db.Categories.Find(result);
                newProduct.Category = categories;
            }
            else
            {
                newProduct.Category= null;
            }
        }

        [RelayCommand]
        public void SaveNewProduct()
        {
            try
            {
                db.Products.Add(newProduct);   // Перевожу сущность в Added
                db.SaveChanges();     // Вставляю данные в БД
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
