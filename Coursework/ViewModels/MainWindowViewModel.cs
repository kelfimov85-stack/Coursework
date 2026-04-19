using Avalonia.Animation;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace Coursework.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase _currentPage;

        public MainWindowViewModel()
        {
            CurrentPage = new PageZero();
        }

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
