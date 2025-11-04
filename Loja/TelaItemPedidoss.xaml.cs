using Loja.Models;
using EstoquePerfumes.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Loja
{
    public partial class TelaItemPedidoss : Window
    {
        private ItemPedido _itemSelecionado;

        public TelaItemPedidoss()
        {
            InitializeComponent();
            CarregarItens();
        }

        private void CarregarItens()
        {
            using (var context = new AppDbContext())
            {
                dgItemPedido.ItemsSource = context.ItemPedidos.ToList();
            }
        }

        private void btnCadastrarItem_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtIdPedido.Text, out int idPedido) ||
                !int.TryParse(txtIdProduto.Text, out int idProduto) ||
                !int.TryParse(txtQuantidade.Text, out int quantidade))
            {
                MessageBox.Show("Preencha todos os campos corretamente!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                var produto = context.Produtos.Find(idProduto);
                if (produto == null)
                {
                    MessageBox.Show("Produto não encontrado!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var preco = produto.Valor;

                var novoItem = new ItemPedido
                {
                    IdPedido = idPedido,
                    IdProduto = idProduto,
                    Quantidade = quantidade,
                    Preco = preco
                };

                context.ItemPedidos.Add(novoItem);
                context.SaveChanges();

                var pedido = context.Pedidos.Find(idPedido);
                if (pedido != null)
                {
                    var novoTotal = context.ItemPedidos
                        .Where(i => i.IdPedido == idPedido)
                        .Sum(i => i.Preco * i.Quantidade);

                    pedido.Total = novoTotal;
                    context.SaveChanges();
                }
            }

            MessageBox.Show("Item cadastrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            LimparCampos();
            CarregarItens();
        }

        private void btnBuscarItem_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtBuscaPedido.Text, out int idPedido))
            {
                MessageBox.Show("Digite um ID de pedido válido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                var itens = context.ItemPedidos
                    .Where(i => i.IdPedido == idPedido)
                    .ToList();

                dgItemPedido.ItemsSource = itens;
            }
        }

        private void btnLimparBuscaItem_Click(object sender, RoutedEventArgs e)
        {
            txtBuscaPedido.Clear();
            CarregarItens();
        }

        private void btnRemoverItem_Click(object sender, RoutedEventArgs e)
        {
            if (_itemSelecionado == null)
            {
                MessageBox.Show("Selecione um item na lista.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                var item = context.ItemPedidos.Find(_itemSelecionado.Id);
                if (item != null)
                {
                    int idPedido = item.IdPedido;

                    context.ItemPedidos.Remove(item);
                    context.SaveChanges();

                    var pedido = context.Pedidos.Find(idPedido);
                    if (pedido != null)
                    {
                        var novoTotal = context.ItemPedidos
                            .Where(i => i.IdPedido == idPedido)
                            .Sum(i => i.Preco * i.Quantidade);

                        pedido.Total = novoTotal;
                        context.SaveChanges();
                    }
                }
            }

            MessageBox.Show("Item removido com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            _itemSelecionado = null;
            CarregarItens();
        }

        private void dgItemPedido_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgItemPedido.SelectedItem is ItemPedido item)
            {
                _itemSelecionado = item;
            }
            else
            {
                _itemSelecionado = null;
            }
        }

        private void LimparCampos()
        {
            txtIdPedido.Clear();
            txtIdProduto.Clear();
            txtQuantidade.Clear();
        }

        private void btnVoltarItemPedido_Click(object sender, RoutedEventArgs e)
        {
            var janelaPedido = new Pedido();
            janelaPedido.Show();
            this.Close();
        }
    }
}
