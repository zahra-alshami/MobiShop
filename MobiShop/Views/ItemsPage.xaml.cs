﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MobiShop.Models;
using MobiShop.Views;
using MobiShop.ViewModels;
using System.Collections.ObjectModel;

namespace MobiShop.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ObservableCollection<Item> phones { set; get; }
        public ItemsPage()
        {
            InitializeComponent();
            Device.SetFlags(new[] {
                "CarouselView_Experimental",
                "IndicatorView_Experimental"
            });
            phones = new ObservableCollection<Item>();
            phones.Add(new Item {ImageUrl="j4.jpg",Text="j4",Description="this is a description"});
            phones.Add(new Item { ImageUrl = "j7.jpg", Text = "j7", Description = "this is a description" });
            ItemsListView.ItemsSource = phones;
            BindingContext = viewModel = new ItemsViewModel();
      Device.StartTimer(TimeSpan.FromSeconds(5), (Func<bool>)(() =>
      {
          TheCarousel.Position = (TheCarousel.Position + 1) % 2;
          return true;
      }));  
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}