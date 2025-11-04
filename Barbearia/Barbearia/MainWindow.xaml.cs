using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Barbearia.Data;
using Barbearia.Models;
using System.Linq;

namespace Barbearia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Agendamento _agendamentoSelecionado;
        private void CarregarAgendamentos()
        {
            using (var context = new BarbeariaContext())
            {
                dgAgendamentos.ItemsSource = context.Agendamentos.ToList();
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            CarregarAgendamentos();
        }

        private void LimparFormulario()
        {
            txtNomeCliente.Clear();
            txtServico.Clear();
            dpData.SelectedDate = null;
            _agendamentoSelecionado = null;
            dgAgendamentos.SelectedItem = null;
            btnExcluir.IsEnabled = false;
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomeCliente.Text) || string.IsNullOrWhiteSpace(txtServico.Text) || !dpData.SelectedDate.HasValue)
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            using (var context = new BarbeariaContext())
            {
                if (_agendamentoSelecionado == null) // Criar
                {
                    var novoAgendamento = new Agendamento
                    {
                        NomeCliente = txtNomeCliente.Text,
                        Servico = txtServico.Text,
                        DataHora = dpData.SelectedDate.Value
                    };
                    context.Agendamentos.Add(novoAgendamento);
                    MessageBox.Show("Agendamento salvo com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else // Atualizar
                {
                    var agendamentoParaAtualizar = context.Agendamentos.Find(_agendamentoSelecionado.ID);
                    if (agendamentoParaAtualizar != null)
                    {
                        agendamentoParaAtualizar.NomeCliente = txtNomeCliente.Text;
                        agendamentoParaAtualizar.Servico = txtServico.Text;
                        agendamentoParaAtualizar.DataHora = dpData.SelectedDate.Value;
                    }
                    MessageBox.Show("Agendamento atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                context.SaveChanges();
            }
            CarregarAgendamentos();
            LimparFormulario();
        }

        private void dgAgendamentos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _agendamentoSelecionado = dgAgendamentos.SelectedItem as Agendamento;
            if (_agendamentoSelecionado != null)
            {
                txtNomeCliente.Text = _agendamentoSelecionado.NomeCliente;
                txtServico.Text = _agendamentoSelecionado.Servico;
                dpData.SelectedDate = _agendamentoSelecionado.DataHora;

                btnExcluir.IsEnabled = true;
            }
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (_agendamentoSelecionado == null)
            {
                MessageBox.Show("Selecione um agendamento para excluir.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var resultado = MessageBox.Show(
                $"Tem certeza que deseja excluir o agendamento de '{_agendamentoSelecionado.NomeCliente}'?",
                "Confirmação de Exclusão",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (resultado == MessageBoxResult.Yes)
            {
                using (var context = new BarbeariaContext())
                {
                    var agendamentoParaExcluir = context.Agendamentos.Find(_agendamentoSelecionado.ID);
                    if (agendamentoParaExcluir != null)
                    {
                        context.Agendamentos.Remove(agendamentoParaExcluir);
                        context.SaveChanges();
                    }
                }
                MessageBox.Show("Agendamento excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                CarregarAgendamentos();
                LimparFormulario();
            }
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            LimparFormulario();
        }

    }
}