
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

        private async void ToolbarItem_Clicked_Adicionar(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.NovoProduto());
        }

        protected async override void OnAppearing()
        {
            if (Lista_Produtos.Count == 0)
            {

                List<Produto> tmp = await App.Db.GetAll();
                foreach (Produto p in tmp)
                {
                    Lista_Produtos.Add(p);
                }

            }
        }

        private void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string q = e.NewTextValue;
            Lista_Produtos.Clear();
            Task.Run(async () =>
            {
                List<Produto> tmp = await App.Db.Search(q);
                foreach (Produto p in tmp)
                {
                    Lista_Produtos.Add(p);
                }
            });
        }

        private void ref_carregando_Refreshing(object sender, EventArgs e)
        {
            Lista_Produtos.Clear();
            Task.Run(async () =>
            {
                List<Produto> tmp = await App.Db.GetAll();
                foreach (Produto p in tmp)
                {
                    Lista_Produtos.Add(p);
                }
            });
            ref_carregando.IsRefreshing = false;
        }

        private void list_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Produto? p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p
            });
        }

        private async void MenuItem_Clicked_Remover(object sender, EventArgs e)
        {
            try
            {
                MenuItem selecionado = (MenuItem)sender;

                Produto p = selecionado.BindingContext as Produto;

                bool confirm = await DisplayAlert("Tem certeza?", "Remover produto?", "OK", "Cancelar");

                if (confirm)
                {
                    await App.Db.Delete(p.Id);
                    await DisplayAlert("Sucesso!", "Produto Removido", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }

}