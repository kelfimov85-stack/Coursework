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

        //создание спмсков
        private List<Products> _allProducts = new();
        private ObservableCollection<Products> Products { get; set; } = new();
        private ObservableCollection<Categories> Categories { get; set; } = new();

        //[ObservableProperty]

        //конструктор
        public PageOne() 
        {
            LoadData();
        }

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

        

    }
}
