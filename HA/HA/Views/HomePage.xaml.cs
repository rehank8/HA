using HA.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        ObservableCollection<CollectionViewImages> collections = null;
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new RegisterViewModel();
            //collections = new ObservableCollection<CollectionViewImages>()
            //{
            //new CollectionViewImages(){ ImageUrl = "https://i.ya-webdesign.com/images/medical-clip-animated-3.png", Name = "Dentist"},
            //new CollectionViewImages(){ ImageUrl = "https://i.ya-webdesign.com/images/medical-clip-animated-3.png", Name = "Physician"},
            //new CollectionViewImages(){ ImageUrl = "https://i.ya-webdesign.com/images/medical-clip-animated-3.png", Name = "Spa"},
            //new CollectionViewImages(){ ImageUrl = "https://i.ya-webdesign.com/images/medical-clip-animated-3.png", Name = "Massage"},
            //new CollectionViewImages(){ ImageUrl = "https://i.ya-webdesign.com/images/medical-clip-animated-3.png", Name = "Chiropractor"},
            //new CollectionViewImages(){ ImageUrl = "https://i.ya-webdesign.com/images/medical-clip-animated-3.png", Name = "Nail"},
            //new CollectionViewImages(){ ImageUrl = "https://i.ya-webdesign.com/images/medical-clip-animated-3.png", Name = "Hair"},
            //new CollectionViewImages(){ ImageUrl = "https://i.ya-webdesign.com/images/medical-clip-animated-3.png", Name = "Photherpy"}
            //};
            //lstNew.ItemsSource = collections;
        }

        //private void lstNew_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var Categoryname = e.CurrentSelection.FirstOrDefault() as CollectionViewImages;
        //    Application.Current.MainPage.Navigation.PushAsync(new VendorListCount(Categoryname.Name));
        //}
    }
    public class CollectionViewImages
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
    }
}