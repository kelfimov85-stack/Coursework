using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Bogus.Bson;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Coursework.Data;
using Coursework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Coursework.ViewModels
{
    public partial class UserReg : ViewModelBase
    {
        private readonly Context db = new();

        User newUser = new User();
        List<User> users = new List<User>();
        [ObservableProperty]
        private ViewModelBase _currentPage;

        [ObservableProperty]
        private string _firstName;
        partial void OnFirstNameChanging(string value)
        {
            if (value != null)
            {
                newUser.FirstName = value;
            }
            else
            {
                newUser.FirstName = null;
            }
        }

        [ObservableProperty]
        private string _lastName;
        partial void OnLastNameChanging(string value)
        {
            if (value != null)
            {
                newUser.LastName = value;
            }
            else
            {
                newUser.LastName = null;
            }
        }

        [ObservableProperty]
        private string? _wallet;
        partial void OnWalletChanging(string? value)
        {
            newUser.Wallet = 0; 
        }

        [RelayCommand]
        public async void SaveNewUser()
        {
            try
            {
                db.User.Add(newUser);
                db.SaveChanges();

                // 2. Теперь у newUser.Id появился реальный номер (например, 5). Привязываем корзину:
                var newBasket = new Basket { UserId = newUser.Id };
                db.Basket.Add(newBasket);
                db.SaveChanges();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                // Здесь будет подробный ответ от вашей СУБД (например, SQLite, PostgreSQL, MS SQL)
                var innerMessage = ex.InnerException?.Message;
                System.Diagnostics.Debug.WriteLine($"ОШИБКА БАЗЫ ДАННЫХ: {innerMessage}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            GoMain();
        }

        public void GoMain()
        {
            CurrentPage = new MainWindowViewModel();
        }
    }
}
