using EstoquePerfumes.Data;
using System.Security.Cryptography; 
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

namespace Loja
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnentrar_Click(object sender, RoutedEventArgs e)
        {
            string login = txtlogin.Text;
            string senha = txtsenha.Password;
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Preencha os campos!!!");
                return;
            }
            var sha = SHA256.Create();
            var asByteArray = Encoding.UTF8.GetBytes(senha);
            var hashedPassword = sha.ComputeHash(asByteArray);
            var senhaHashBase64 = Convert.ToBase64String(hashedPassword);

            using (var dbContext = new AppDbContext())
            {             
                var usuarioEncontrado = dbContext.Logins.FirstOrDefault(u =>
                    u.Usuario == login && u.Senha == senhaHashBase64);
   
                if (usuarioEncontrado != null)
                {

                    TelaPrincipal telaPrincipal = new TelaPrincipal();
                    Application.Current.MainWindow = telaPrincipal;
                    telaPrincipal.Show();
                    this.Close();

                }
                else
                {       
                    MessageBox.Show("E-mail ou senha inválidos.");
                }
            }

        }
    }
}