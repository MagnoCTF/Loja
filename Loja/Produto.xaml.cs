using Loja.Models;
using EstoquePerfumes.Data; 
using System.Linq;
using System.Windows;

namespace Loja
{
    public partial class Produto : Window
    {
        private Loja.Models.Produto _produtoSelecionado;

        public Produto()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarProdutos();
        }

        private void CarregarProdutos()
        {
            using (var context = new AppDbContext())
            {
                dgProduto.ItemsSource = context.Produtos.ToList();
            }
        }

        private void dgProduto_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _produtoSelecionado = (Loja.Models.Produto)dgProduto.SelectedItem;
        }

        private void btnCadastrarProduto_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomeProduto.Text) ||
                string.IsNullOrWhiteSpace(txtValorProduto.Text) ||
                string.IsNullOrWhiteSpace(txtDescricaoProduto.Text) ||
                !decimal.TryParse(txtValorProduto.Text, out decimal valor))
            {
                MessageBox.Show("Preencha os campos corretamente!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var novoProduto = new Loja.Models.Produto
            {
                Nome = txtNomeProduto.Text,
                Valor = valor,
                Descricao = txtDescricaoProduto.Text
            };

            using (var context = new AppDbContext())
            {
                context.Produtos.Add(novoProduto);
                context.SaveChanges();
            }

            MessageBox.Show("Produto cadastrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            txtNomeProduto.Clear();
            txtValorProduto.Clear();
            txtDescricaoProduto.Clear();
            CarregarProdutos();
        }

        private void btnBuscarProduto_Click(object sender, RoutedEventArgs e)
        {
            string nomeBusca = txtBuscaProduto.Text.Trim();

            if (string.IsNullOrWhiteSpace(nomeBusca))
            {
                MessageBox.Show("Digite um nome para buscar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                var resultado = context.Produtos
                    .Where(p => p.Nome.Contains(nomeBusca))
                    .ToList();

                dgProduto.ItemsSource = resultado;
            }
        }

        private void btnLimparBuscaProduto_Click(object sender, RoutedEventArgs e)
        {
            txtBuscaProduto.Clear();
            CarregarProdutos();
        }

        private void btnExcluirProduto_Click(object sender, RoutedEventArgs e)
        {
            if (_produtoSelecionado == null)
            {
                MessageBox.Show("Selecione um produto para excluir.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var resultado = MessageBox.Show($"Deseja realmente excluir o produto {_produtoSelecionado.Nome}?",
                                            "Confirmação",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                using (var context = new AppDbContext())
                {
                    var produto = context.Produtos.Find(_produtoSelecionado.Id);
                    if (produto != null)
                    {
                        context.Produtos.Remove(produto);
                        context.SaveChanges();
                    }
                }

                MessageBox.Show("Produto excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                _produtoSelecionado = null;
                CarregarProdutos();
            }
        }

        private void btnVoltarProduto_Click(object sender, RoutedEventArgs e)
        {
            TelaPrincipal telaPrincipal = new TelaPrincipal();
            Application.Current.MainWindow = telaPrincipal;
            telaPrincipal.Show();
            this.Close();
        }
    }
}
