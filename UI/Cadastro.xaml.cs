using System.Windows;
using UI.Model;

namespace UI
{
    public partial class Cadastro : Window
    {
        public Cadastro()
        {
            InitializeComponent();

        }

        private async void btnCadastro(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;
            UsuarioModel uModel = new UsuarioModel();

            if (boxPass.Password == boxReptPass.Password && boxEmail.Text != string.Empty && boxNome.Text != string.Empty && boxPass.Password != string.Empty)
            {
                if (!uModel.ValidarEmail(boxEmail.Text))
                {
                    MessageBox.Show("E-mail inválido.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                    IsEnabled = true;
                    return;
                }

                bool emailValido = await uModel.CriarUsuario(boxNome.Text, boxEmail.Text, boxPass.Password);

                if (emailValido)
                {
                    MessageBox.Show("Cadastrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    Login login = new Login();
                    login.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Email já cadastrado!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("As senhas são diferentes ou existem campos vazios", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            IsEnabled = true;
        }
    }
}
