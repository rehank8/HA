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
    public partial class CalenderView : ContentPage
    {
        public CalenderView(List<string> Vendorsdatetime,UserIndex Vendor,UserQueryDTO userQuery)
        {
            InitializeComponent();
            BindingContext = new CalenderViewModel(Vendorsdatetime,Vendor,userQuery);
        }
    }
}