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

// ListarUsuariosWindow.xaml.cs
using EstoquePerfumes.Data;
using System.Linq;

namespace EstoquePerfumes
{
    public partial class ListarUsuariosWindow : Window
    {
        public ListarUsuariosWindow()
        {
            InitializeComponent();
            CarregarUsuarios(); // Chama o método para buscar os dados assim que a janela é criada
        }
        private void CarregarUsuarios()
        {
            using (var dbContext = new AppDbContext())
            {
                // Busca todos os usuários do banco e converte para uma lista
                var usuarios = dbContext.Usuarios.ToList();

                // Define a lista de usuários como a fonte de dados do nosso DataGrid
                dataGridUsuarios.ItemsSource = usuarios;
            }
        }
        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            // Simplesmente fecha a janela atual, voltando para a MainWindow que está atrás.
            this.Close();
        }
    }
}
