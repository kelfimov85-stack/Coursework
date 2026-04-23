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

namespace Coursework.ViewModels
{
    public partial class PageOne : ViewModelBase
    {
        private readonly Context db = new();

        //создание списков
        private List<Products> _allProducts = new();
        private ObservableCollection<Products> Products { get; set; } = new();
        private ObservableCollection<Categories> Categories { get; set; } = new();

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
            ApplyFilters();
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
            if (SearchCategories != null && SearchCategories.Id != -1)
            {
                query = query.Where(p => p.Category.Id == SearchCategories.Id);
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
