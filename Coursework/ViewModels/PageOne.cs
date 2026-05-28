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
using Microsoft.Identity.Client;

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
            LoadData();
            if (Products.Count() <= 0)
            {
                int productIdFaker = 0;
                string[] productNameFaker = ["NVIDIA GeForce RTX 4070 Super", "AMD Radeon RX 7800 XT", "NVIDIA GeForce RTX 4060 Ti", "AMD Ryzen 7 7800X3D", "Intel Core i5-14600K", "AMD Ryzen 5 5600X", "ASUS ROG STRIX B650-A GAMING WIFI", "MSI MAG B760 TOMAHAWK WIFI", "Gigabyte B550 AORUS ELITE V2", "Thermalright Peerless Assassin 120 SE", "Noctua NH-D15 chromax.black", "Deepcool AK620", "Kingston FURY Beast DDR5 32GB", "G.Skill Trident Z5 RGB DDR5 32GB", "Corsair Vengeance LPX DDR4 16GB", "Samsung 990 PRO NVMe M.2 SSD 1TB", "Kingston KC3000 NVMe M.2 SSD 2TB", "Crucial BX500 SATA SSD 1TB", "Corsair RM750x 750W", "Deepcool DQ750 750W", "be quiet! Straight Power 11 850W"];
                Random rnd = new Random();

                var productFaker = new Faker<Products>("ru")
                    .RuleFor(p => p.Name,  f => productNameFaker[rnd.Next(0, 20)])
                    .RuleFor(p => p.Price, f => Math.Round(f.Random.Decimal(100, 50000), 2))
                    .RuleFor(p => p.Quantity, f => f.Random.Number(1, 439))
                    .RuleFor(p => p.Category, (f, p) => p.Name switch 
                    {
                        "NVIDIA GeForce RTX 4070 Super" or
                        "AMD Radeon RX 7800 XT" or
                        "NVIDIA GeForce RTX 4060 Ti" => Categories[0], 

                        "AMD Ryzen 7 7800X3D" or
                        "Intel Core i5-14600K" or
                        "AMD Ryzen 5 5600X" => Categories[1], 

                        "ASUS ROG STRIX B650-A GAMING WIFI" or
                        "MSI MAG B760 TOMAHAWK WIFI" or
                        "Gigabyte B550 AORUS ELITE V2" => Categories[2], 

                        "Thermalright Peerless Assassin 120 SE" or
                        "Noctua NH-D15 chromax.black" or
                        "Deepcool AK620" => Categories[3], 

                        "Kingston FURY Beast DDR5 32GB" or
                        "G.Skill Trident Z5 RGB DDR5 32GB" or
                        "Corsair Vengeance LPX DDR4 16GB" => Categories[4], 

                        "Samsung 990 PRO NVMe M.2 SSD 1TB" or
                        "Kingston KC3000 NVMe M.2 SSD 2TB" or
                        "Crucial BX500 SATA SSD 1TB" => Categories[5], 

                        "Corsair RM750x 750W" or
                        "Deepcool DQ750 750W" or
                        "be quiet! Straight Power 11 850W" => Categories[6],

                        _ => Categories[0] 
                    });

                List<Products> syntheticProducts = productFaker.Generate(10);

                foreach (var product in syntheticProducts)
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                }

                syntheticProducts.Clear();
                LoadData();
            }
        }

        //Функция для заполнения листов
        public async void LoadData()
        {
            //очищаем списки
            Products.Clear();
            Categories.Clear();

            //заполнение листов
            foreach (var product in db.Products.ToList()) 
            {

                Products.Add(product);
                _allProducts = await db.Products.ToListAsync();
            }
            foreach (var categorya in db.Categories.ToList())
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
