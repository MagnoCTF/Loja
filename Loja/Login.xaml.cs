using Loja.Models;
using EstoquePerfumes.Data;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows;

namespace Loja
{
    public partial class Login : Window
    {
        private Loja.Models.Login _loginSelecionado;

        public Login()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarLogins();
        }

        private void CarregarLogins()
        {
            using (var context = new AppDbContext())
            {
                dgLogin.ItemsSource = context.Logins.ToList();
            }
        }

        private void dgLogin_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _loginSelecionado = (Loja.Models.Login)dgLogin.SelectedItem;
        }

        private void btnCadastrarLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtIdFuncionario.Text, out int idFuncionario) ||
                string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtSenha.Password))
            {
                MessageBox.Show("Preencha todos os campos corretamente!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var sha = SHA256.Create();
            var asByteArray = Encoding.UTF8.GetBytes(txtSenha.Password);
            var hashedPassword = sha.ComputeHash(asByteArray);
            var senhaCriptografada = Convert.ToBase64String(hashedPassword);

            var novoLogin = new Loja.Models.Login
            {
                IdFuncionario = idFuncionario,
                Usuario = txtUsuario.Text,
                Senha = senhaCriptografada
            };

            using (var context = new AppDbContext())
            {
                context.Logins.Add(novoLogin);
                context.SaveChanges();
            }

            MessageBox.Show("Login cadastrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            txtIdFuncionario.Clear();
            txtUsuario.Clear();
            txtSenha.Clear();
            CarregarLogins();
        }

        private void btnBuscarLogin_Click(object sender, RoutedEventArgs e)
        {
            string usuarioBusca = txtBuscaLogin.Text.Trim();

            if (string.IsNullOrWhiteSpace(usuarioBusca))
            {
                MessageBox.Show("Digite um nome de usuário para buscar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                var resultado = context.Logins
                    .Where(l => l.Usuario.Contains(usuarioBusca))
                    .ToList();

                dgLogin.ItemsSource = resultado;
            }
        }

        private void btnLimparBuscaLogin_Click(object sender, RoutedEventArgs e)
        {
            txtBuscaLogin.Clear();
            CarregarLogins();
        }

        private void btnExcluirLogin_Click(object sender, RoutedEventArgs e)
        {
            if (_loginSelecionado == null)
            {
                MessageBox.Show("Selecione um login para excluir.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var resultado = MessageBox.Show($"Deseja realmente excluir o login do usuário {_loginSelecionado.Usuario}?",
                                            "Confirmação",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                using (var context = new AppDbContext())
                {
                    var login = context.Logins.Find(_loginSelecionado.Id);
                    if (login != null)
                    {
                        context.Logins.Remove(login);
                        context.SaveChanges();
                    }
                }

                MessageBox.Show("Login excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                _loginSelecionado = null;
                CarregarLogins();
            }
        }

        private void btnVoltarLogin_Click(object sender, RoutedEventArgs e)
        {
            Funcionario funcionarioWindow = new Funcionario();
            Application.Current.MainWindow = funcionarioWindow;
            funcionarioWindow.Show();
            this.Close();
        }
    }
}
