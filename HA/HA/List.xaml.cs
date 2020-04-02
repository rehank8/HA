using HA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class List : ContentPage
    {
        public List()
        {
            InitializeComponent();
            var name1 = new List<Contact>
            {
              new Contact{Name="meraj",Status="IT",ImageURl="http://lorempixel.com/100/100/people/1"},

               new Contact{Name="siraj",Status="HR",ImageURl="http://lorempixel.com/100/100/people/2"},

               new Contact{Name="raj",Status="qa",ImageURl="http://lorempixel.com/100/100/people/3"}
            };

            Listview.ItemsSource = name1;
            
        }
    }
}