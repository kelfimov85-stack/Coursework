using Avalonia.Animation;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace Coursework.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        //создание перменной где будет храниться действующая страница
        [ObservableProperty]
        private ViewModelBase _currentPage;

        public MainWindowViewModel()
        {
            CurrentPage = new PageZero();
        }
        

        //Функции для кнопок, которые будут переносить на страницы
        [RelayCommand]
        private void GoPageZero() 
        {
            CurrentPage = new PageZero();
        }
        [RelayCommand]
        private void GoPageOne()
        {
            CurrentPage = new PageOne();
        }

    }
}
