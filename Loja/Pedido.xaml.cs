using Loja.Models;
using EstoquePerfumes.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Loja
{
    public partial class Pedido : Window
    {
        private Loja.Models.Pedido _pedidoSelecionado;

        public Pedido()
        {
            InitializeComponent();
            CarregarPedidos();
        }

        private void CarregarPedidos()
        {
            using (var context = new AppDbContext())
            {
                dgPedido.ItemsSource = context.Pedidos.ToList();
            }
        }

        private void btnCadastrarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtIdCliente.Text, out int idCliente))
            {
                MessageBox.Show("Digite um ID de cliente válido!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var novoPedido = new Loja.Models.Pedido
            {
                IdCliente = idCliente,
                DataPedido = DateTime.Now,
                Status = "Pendente",
                Total = 0
            };

            using (var context = new AppDbContext())
            {
                context.Pedidos.Add(novoPedido);
                context.SaveChanges();
            }

            MessageBox.Show("Pedido cadastrado!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            txtIdCliente.Clear();
            CarregarPedidos();
        }

        private void btnBuscarPedido_Click(object sender, RoutedEventArgs e)
        {
            string tipoBusca = ((ComboBoxItem)cbTipoBusca.SelectedItem).Content.ToString();
            string valorBusca = txtBusca.Text.Trim();

            if (string.IsNullOrWhiteSpace(valorBusca))
            {
                MessageBox.Show("Digite um valor para buscar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                if (tipoBusca == "ID do Pedido" && int.TryParse(valorBusca, out int idPedido))
                {
                    var resultado = context.Pedidos.Where(p => p.Id == idPedido).ToList();
                    dgPedido.ItemsSource = resultado;
                }
                else if (tipoBusca == "ID do Cliente" && int.TryParse(valorBusca, out int idCliente))
                {
                    var resultado = context.Pedidos.Where(p => p.IdCliente == idCliente).ToList();
                    dgPedido.ItemsSource = resultado;
                }
                else
                {
                    MessageBox.Show("Valor inválido para busca.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnLimparBuscaPedido_Click(object sender, RoutedEventArgs e)
        {
            txtBusca.Clear();
            CarregarPedidos();
        }

        private void btnExcluirPedido_Click(object sender, RoutedEventArgs e)
        {
            if (_pedidoSelecionado == null)
            {
                MessageBox.Show("Selecione um pedido na lista.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                var pedido = context.Pedidos.Find(_pedidoSelecionado.Id);
                if (pedido != null)
                {
                    context.Pedidos.Remove(pedido);
                    context.SaveChanges();
                }
            }

            MessageBox.Show("Pedido excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            _pedidoSelecionado = null;
            CarregarPedidos();
        }

        private void dgPedido_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _pedidoSelecionado = (Loja.Models.Pedido)dgPedido.SelectedItem;
        }

        private void btnVoltarPedido_Click(object sender, RoutedEventArgs e)
        {
            TelaPrincipal telaPrincipal = new TelaPrincipal();
            Application.Current.MainWindow = telaPrincipal;
            telaPrincipal.Show();
            this.Close();
        }

        private void btnGerenciarItens_Click(object sender, RoutedEventArgs e)
        {
            TelaItemPedidoss itempedido = new TelaItemPedidoss();
            Application.Current.MainWindow = itempedido;
            itempedido.Show();
            this.Close();
        }

        private void btnGerenciarPagamentos_Click(object sender, RoutedEventArgs e)
        {
            TelaPagamentos pagamento = new TelaPagamentos();
            Application.Current.MainWindow = pagamento;
            pagamento.Show();
            this.Close();
        }
    }
}
