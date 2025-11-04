using Loja.Models;
using EstoquePerfumes.Data;
using System.Linq;
using System.Windows;

namespace Loja
{
    public partial class Funcionario : Window
    {
        private Loja.Models.Funcionario _funcionarioSelecionado;

        public Funcionario()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarFuncionarios();
        }

        private void CarregarFuncionarios()
        {
            using (var context = new AppDbContext())
            {
                dgFuncionario.ItemsSource = context.Funcionarios.ToList();
            }
        }

        private void dgFuncionario_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _funcionarioSelecionado = (Loja.Models.Funcionario)dgFuncionario.SelectedItem;
        }

        private void btnCadastrarFuncionario_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomeFuncionario.Text) ||
                string.IsNullOrWhiteSpace(txtEmailFuncionario.Text) ||
                string.IsNullOrWhiteSpace(txtCargoFuncionario.Text) ||
                string.IsNullOrWhiteSpace(txtTelefoneFuncionario.Text) ||
                string.IsNullOrWhiteSpace(txtEnderecoFuncionario.Text))
            {
                MessageBox.Show("Preencha todos os campos!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var novoFuncionario = new Loja.Models.Funcionario
            {
                Nome = txtNomeFuncionario.Text,
                Email = txtEmailFuncionario.Text,
                Cargo = txtCargoFuncionario.Text,
                Telefone = txtTelefoneFuncionario.Text,
                Endereco = txtEnderecoFuncionario.Text
            };

            using (var context = new AppDbContext())
            {
                context.Funcionarios.Add(novoFuncionario);
                context.SaveChanges();
            }

            MessageBox.Show("Funcionário cadastrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            txtNomeFuncionario.Clear();
            txtEmailFuncionario.Clear();
            txtCargoFuncionario.Clear();
            txtTelefoneFuncionario.Clear();
            txtEnderecoFuncionario.Clear();
            CarregarFuncionarios();
        }

        private void btnBuscarFuncionario_Click(object sender, RoutedEventArgs e)
        {
            string nomeBusca = txtBuscaFuncionario.Text.Trim();

            if (string.IsNullOrWhiteSpace(nomeBusca))
            {
                MessageBox.Show("Digite um nome para buscar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                var resultado = context.Funcionarios
                    .Where(f => f.Nome.Contains(nomeBusca))
                    .ToList();

                dgFuncionario.ItemsSource = resultado;
            }
        }

        private void btnLimparBuscaFuncionario_Click(object sender, RoutedEventArgs e)
        {
            txtBuscaFuncionario.Clear();
            CarregarFuncionarios();
        }

        private void btnExcluirFuncionario_Click(object sender, RoutedEventArgs e)
        {
            if (_funcionarioSelecionado == null)
            {
                MessageBox.Show("Selecione um funcionário para excluir.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var resultado = MessageBox.Show($"Deseja realmente excluir o funcionário {_funcionarioSelecionado.Nome}?",
                                            "Confirmação",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                using (var context = new AppDbContext())
                {
                    var funcionario = context.Funcionarios.Find(_funcionarioSelecionado.Id);
                    if (funcionario != null)
                    {
                        context.Funcionarios.Remove(funcionario);
                        context.SaveChanges();
                    }
                }

                MessageBox.Show("Funcionário excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                _funcionarioSelecionado = null;
                CarregarFuncionarios();
            }
        }

        private void btnVoltarFuncionario_Click(object sender, RoutedEventArgs e)
        {
            TelaPrincipal telaPrincipal = new TelaPrincipal();
            Application.Current.MainWindow = telaPrincipal;
            telaPrincipal.Show();
            this.Close();
        }

        private void btnGerenciarLogin_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            Application.Current.MainWindow = login;
            login.Show();
            this.Close();
        }
    }
    }
