using HA.Model;
using HA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyRecentApp : ContentPage
    {
        public MyRecentApp(string location,List<UserIndex> Vendors)
        {
            InitializeComponent();
            BindingContext = new MyRecentAppViewModel(location,Vendors);
        }
    }
}