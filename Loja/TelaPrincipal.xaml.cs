using Loja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Loja
{
    /// <summary>
    /// </summary>
    public partial class TelaPrincipal : Window
    {
        public TelaPrincipal()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFuncionario_Click(object sender, RoutedEventArgs e)
        {
            Funcionario funcionarioWindow = new Funcionario();

            
            Application.Current.MainWindow = funcionarioWindow;

            funcionarioWindow.Show();

            
            this.Close();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Cliente cliente = new Cliente();
            Application.Current.MainWindow = cliente;
            cliente.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Produto produto = new Produto();
            Application.Current.MainWindow = produto;
            produto.Show();
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Estoque estoque = new Estoque();
            Application.Current.MainWindow = estoque;
            estoque.Show();
            this.Close();
        }

        private void btnPedidos_Click(object sender, RoutedEventArgs e)
        {
            Pedido pedido = new Pedido();
            Application.Current.MainWindow = pedido;
            pedido.Show();
            this.Close();
        }
    }
}
