using Loja.Models;
using EstoquePerfumes.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Loja
{
    public partial class Estoque : Window
    {
        private object _estoqueSelecionado;

        public Estoque()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarEstoque();
        }

        private void CarregarEstoque()
        {
            using (var context = new AppDbContext())
            {
                var lista = (from estoque in context.Estoques
                             join produto in context.Produtos
                             on estoque.IdProduto equals produto.Id
                             select new
                             {
                                 estoque.IdProduto,
                                 NomeProduto = produto.Nome,
                                 estoque.Quantidade
                             }).ToList();

                dgEstoque.ItemsSource = lista;
            }
        }

        private void dgEstoque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _estoqueSelecionado = dgEstoque.SelectedItem;
        }

        private void btnAdicionarEstoque_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtIdProduto.Text, out int idProduto) ||
                !int.TryParse(txtQuantidade.Text, out int quantidade) ||
                quantidade <= 0)
            {
                MessageBox.Show("Informe valores válidos!", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                var produtoExiste = context.Produtos.Any(p => p.Id == idProduto);
                if (!produtoExiste)
                {
                    MessageBox.Show("Produto não encontrado!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var estoque = context.Estoques.FirstOrDefault(e => e.IdProduto == idProduto);

                if (estoque != null)
                {
                    estoque.Quantidade += quantidade;
                }
                else
                {
                    var novoEstoque = new Loja.Models.Estoque
                    {
                        IdProduto = idProduto,
                        Quantidade = quantidade
                    };
                    context.Estoques.Add(novoEstoque);
                }

                context.SaveChanges();
            }

            MessageBox.Show("Estoque atualizado!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            txtIdProduto.Clear();
            txtQuantidade.Clear();
            CarregarEstoque();
        }

        private void btnExcluirEstoque_Click(object sender, RoutedEventArgs e)
        {
            if (_estoqueSelecionado == null)
            {
                MessageBox.Show("Selecione um item de estoque para excluir.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            var tipo = _estoqueSelecionado.GetType();
            var propIdProduto = tipo.GetProperty("IdProduto");
            if (propIdProduto == null)
            {
                MessageBox.Show("Não foi possível identificar o produto selecionado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int idProdutoSelecionado = (int)propIdProduto.GetValue(_estoqueSelecionado);

            var resultado = MessageBox.Show($"Deseja realmente excluir o estoque do produto ID {idProdutoSelecionado}?",
                                            "Confirmação",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                using (var context = new AppDbContext())
                {
                    var estoque = context.Estoques.FirstOrDefault(e => e.IdProduto == idProdutoSelecionado);
                    if (estoque != null)
                    {
                        context.Estoques.Remove(estoque);
                        context.SaveChanges();
                    }
                }

                MessageBox.Show("Estoque excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                _estoqueSelecionado = null;
                CarregarEstoque();
            }
        }

        private void btnVoltarEstoque_Click(object sender, RoutedEventArgs e)
        {
            TelaPrincipal telaPrincipal = new TelaPrincipal();
            Application.Current.MainWindow = telaPrincipal;
            telaPrincipal.Show();
            this.Close();
        }
    }
}
