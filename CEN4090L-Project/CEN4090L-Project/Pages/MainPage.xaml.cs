using CEN4090L_Project.Models;
using CEN4090L_Project.PageModels;

namespace CEN4090L_Project.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}