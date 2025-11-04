using Loja.Models;
using EstoquePerfumes.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Loja
{
    public partial class TelaPagamentos : Window
    {
        private Pagamento _pagamentoSelecionado;

        public TelaPagamentos()
        {
            InitializeComponent();
            CarregarPagamentos();
        }

        private void CarregarPagamentos()
        {
            using (var context = new AppDbContext())
            {
                dgPagamentos.ItemsSource = context.Pagamentos.ToList();
            }
        }

        private void btnCadastrarPagamento_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtIdPedido.Text, out int idPedido) ||
                cbFormaPagamento.SelectedItem == null)
            {
                MessageBox.Show("Preencha todos os campos corretamente!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                var pedido = context.Pedidos.Find(idPedido);
                if (pedido == null)
                {
                    MessageBox.Show("Pedido não encontrado!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var formaPagamento = ((ComboBoxItem)cbFormaPagamento.SelectedItem).Content.ToString();

                var novoPagamento = new Pagamento
                {
                    IdPedido = idPedido,
                    FormaPagamento = formaPagamento,
                    DataPagamento = DateTime.Now,
                    Valor = pedido.Total
                };

                context.Pagamentos.Add(novoPagamento);

                pedido.Status = "Pago";
                context.SaveChanges();
            }

            MessageBox.Show("Pagamento registrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            LimparCampos();
            CarregarPagamentos();
        }

        private void btnBuscarPagamento_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtBuscaPedido.Text, out int idPedido))
            {
                MessageBox.Show("Digite um ID de pedido válido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                var pagamentos = context.Pagamentos
                    .Where(p => p.IdPedido == idPedido)
                    .ToList();

                dgPagamentos.ItemsSource = pagamentos;
            }
        }

        private void btnLimparBuscaPagamento_Click(object sender, RoutedEventArgs e)
        {
            txtBuscaPedido.Clear();
            CarregarPagamentos();
        }

        private void btnRemoverPagamento_Click(object sender, RoutedEventArgs e)
        {
            if (_pagamentoSelecionado == null)
            {
                MessageBox.Show("Selecione um pagamento na lista.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                var pagamento = context.Pagamentos.Find(_pagamentoSelecionado.Id);
                if (pagamento != null)
                {
                    var pedido = context.Pedidos.Find(pagamento.IdPedido);
                    if (pedido != null)
                    {
                        pedido.Status = "Pendente";
                    }

                    context.Pagamentos.Remove(pagamento);
                    context.SaveChanges();
                }
            }

            MessageBox.Show("Pagamento removido com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            _pagamentoSelecionado = null;
            CarregarPagamentos();
        }

        private void dgPagamentos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPagamentos.SelectedItem is Pagamento pagamento)
            {
                _pagamentoSelecionado = pagamento;
            }
            else
            {
                _pagamentoSelecionado = null;
            }
        }

        private void LimparCampos()
        {
            txtIdPedido.Clear();
            cbFormaPagamento.SelectedIndex = -1;
        }

        private void btnVoltarPagamento_Click(object sender, RoutedEventArgs e)
        {
            var janelaPedido = new Pedido();
            janelaPedido.Show();
            this.Close();
        }
    }
}
