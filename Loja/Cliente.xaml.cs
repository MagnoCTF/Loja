using EstoquePerfumes.Data;
using Loja.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Loja
{
    public partial class Cliente : Window
    {
        private Loja.Models.Cliente _clienteSelecionado;

        public Cliente()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarClientes();
        }

        private void CarregarClientes()
        {
            using (var context = new AppDbContext())
            {
                dgCliente.ItemsSource = context.Clientes.ToList();
            }
        }

        private void dgCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCliente.SelectedItem is Loja.Models.Cliente cliente)
            {
                _clienteSelecionado = cliente;
            }
            else
            {
                _clienteSelecionado = null;
            }
        }

        private void btnCadastrarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomeCliente.Text) ||
                string.IsNullOrWhiteSpace(txtEmailCliente.Text) ||
                string.IsNullOrWhiteSpace(txtTelefoneCliente.Text) ||
                string.IsNullOrWhiteSpace(txtEnderecoCliente.Text))
            {
                MessageBox.Show("Preencha os campos!!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var novoCliente = new Loja.Models.Cliente
            {
                Nome = txtNomeCliente.Text,
                Email = txtEmailCliente.Text,
                Telefone = txtTelefoneCliente.Text,
                Endereco = txtEnderecoCliente.Text
            };

            using (var context = new AppDbContext())
            {
                context.Clientes.Add(novoCliente);
                context.SaveChanges();
            }

            MessageBox.Show("Cliente cadastrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            txtNomeCliente.Clear();
            txtTelefoneCliente.Clear();
            txtEnderecoCliente.Clear();
            txtEmailCliente.Clear();

            CarregarClientes();
        }

        private void btnEcluirCliente_Click(object sender, RoutedEventArgs e)
        {
            if (_clienteSelecionado == null)
            {
                MessageBox.Show("Selecione um cliente.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var resultado = MessageBox.Show($"Deseja realmente excluir o cliente {_clienteSelecionado.Nome}?",
                                            "Confirmação",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                using (var context = new AppDbContext())
                {
                   
                    bool possuiPedidos = context.Pedidos.Any(p => p.IdCliente == _clienteSelecionado.Id);

                    if (possuiPedidos)
                    {
                        MessageBox.Show("Este cliente possui pedidos e não pode ser excluído.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var cliente = context.Clientes.Find(_clienteSelecionado.Id);
                    if (cliente != null)
                    {
                        context.Clientes.Remove(cliente);
                        context.SaveChanges();
                    }
                }

                MessageBox.Show("Cliente excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                _clienteSelecionado = null;
                CarregarClientes();
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string nomeBusca = txtBuscaCliente.Text.Trim();

            if (string.IsNullOrWhiteSpace(nomeBusca))
            {
                MessageBox.Show("Digite um nome para buscar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                var resultado = context.Clientes
                    .Where(c => c.Nome.Contains(nomeBusca))
                    .ToList();

                dgCliente.ItemsSource = resultado;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            txtBuscaCliente.Clear();
            CarregarClientes();
        }

        private void btnVoltarCliente_Click(object sender, RoutedEventArgs e)
        {
            TelaPrincipal telaPrincipal = new TelaPrincipal();
            Application.Current.MainWindow = telaPrincipal;
            telaPrincipal.Show();
            this.Close();
        }
    }
}
