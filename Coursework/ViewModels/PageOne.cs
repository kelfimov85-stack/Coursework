using CommunityToolkit.Mvvm.ComponentModel;
using Coursework.Data;
using Coursework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;

namespace Coursework.ViewModels
{
    public partial class PageOne : ViewModelBase
    {
        private readonly Context db = new();

        //создание списков
        private List<Products> _allProducts = new();
        public ObservableCollection<Products> Products { get; set; } = new();
        public ObservableCollection<Categories> Categories { get; set; } = new();

        [ObservableProperty]
        private string _searchText;
        [ObservableProperty]
        private Categories _searchCategories;
        [ObservableProperty]
        private string _sortCategory;

        
        public List<string> SortOptions { get; } = new()
        {
            "Reset",
            "Name ↑",
            "Name ↓",
            "Price ↑",
            "Price ↓"
        };

        //конструктор
        public PageOne() 
        {
            if (0 != Products.Count())
            {
                LoadData();
            }
            else
            {
                int productIdFaker = 0;

                var productFaker = new Faker<Products>("ru")
                    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Price, f => f.Random.Bool(0.9f)
                    ? Math.Round(f.Random.Decimal(100, 50000), 2)
                    : null)
                    .RuleFor(p => p.Quantity, f => f.Random.Bool(0.95f)
                    ? f.Random.Number(0, 150)
                    : null)
                    .RuleFor(p => p.Category, f => f.Random.Bool(0.95f) // вот здесь ошибка
                    ? f.PickRandom<Categories>()
                    : null);

                List<Products> syntheticProducts = productFaker.Generate(10);

                foreach (var product in syntheticProducts)
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                }
            }
        }

        //Функция для заполнения листов
        private async void LoadData()
        {
            //очищаем списки
            Categories.Clear();
            Products.Clear();

            //заполнение листов
            foreach (var product in await db.Products.ToListAsync()) 
            {

                Products.Add(product);
                _allProducts = await db.Products.ToListAsync();
            }
            foreach (var categorya in await db.Categories.ToListAsync())
            {
                Categories.Add(categorya);
            }
        }

        partial void OnSearchTextChanged(string value)
        {
            ApplyFilters();
        }
        partial void OnSortCategoryChanged(string value)
        {
            ApplyFilters();
        }
        partial void OnSearchCategoriesChanged(Categories value)
        {
            ApplyFilters();
        }

        public void ApplyFilters()
        {
            var query = _allProducts.AsEnumerable();

            //поиск
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(p => p.Name.ToLower().Contains(SearchText.ToLower()));
            }
            
            //сортировка по категориям 
            if (SearchCategories != null && SearchCategories.Id != 8)
            {
                query = query.Where(p => p.Category != null && p.Category.Id == SearchCategories.Id);
            }

            //сортиовка по именам и цене
            query = SortCategory switch
            {
                "Name ↑" => query.OrderBy(p => p.Name),
                "Name ↓" => query.OrderByDescending(p => p.Name),
                "Price ↑" => query.OrderBy(p => p.Price),
                "Price ↓" => query.OrderByDescending(p => p.Price),
                _ => query
            };

            Products.Clear();

            foreach (var item in query)
            {
                Products.Add(item);
            }
        }
    }
}
