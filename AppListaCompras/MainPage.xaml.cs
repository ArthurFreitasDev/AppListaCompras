
using System.Collections.ObjectModel;
using AppListaCompras.Models;

namespace AppListaCompras
{
    public partial class MainPage : ContentPage
    {

        ObservableCollection<Produto> Lista_Produtos = new ObservableCollection<Produto>();

        public MainPage()
        {
            InitializeComponent();
            list_produtos.ItemsSource = Lista_Produtos;
        }

        private void ToolbarItem_Clicked_Somar(object sender, EventArgs e)
        {
            double Soma = Lista_Produtos.Sum(i => (i.Preco * i.Quantidade));
            string msg = $"O total é {Soma.ToString("C")}";
            DisplayAlert("Somatória", msg, "Fechar");
        }

        private void ToolbarItem_Clicked_Adicionar(object sender, EventArgs e)
        {

        }

        protected override void OnAppearing()
        {
            if(Lista_Produtos.Count == 0)
            {
                Task.Run(async () =>
                {
                    List<Produto> tmp = await App.Db.GetAll();
                    foreach(Produto p in tmp)
                    {
                        Lista_Produtos.Add(p);
                    }
                });
            }
        }
    }

}