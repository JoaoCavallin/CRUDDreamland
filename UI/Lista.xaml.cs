
using Dominio;
using Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UI.Model;

namespace UI
{
    /// <summary>
    /// Lógica interna para Lista.xaml
    /// </summary>
    public partial class Lista : Window
    {
        public Lista()
        {
            InitializeComponent();
            CarregarUsuarios();
        }

        private void CarregarUsuarios()
        {
            using (var context = new Context())
            {
          
                dataGrid.ItemsSource = context.Usuarios.ToList();
            }
        }
        private void btnVoltar(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            var usuario = dataGrid.SelectedItem as Usuarios;

            if (usuario == null)
            {
                MessageBox.Show("Selecione um usuário");
                return;
            }

            string senha = PromptSenha();

            if (string.IsNullOrEmpty(senha))
                return;

            var model = new UsuarioModel();
            var repo = new UsuarioRepositorio();

            bool senhaValida = await model.ValidarSenha(usuario.IdUsuario, senha);

            if (!senhaValida)
            {
                MessageBox.Show("Senha incorreta!");
                return;
            }

            var confirm = MessageBox.Show("Tem certeza que deseja excluir?",
                "Confirmação", MessageBoxButton.YesNo);

            if (confirm != MessageBoxResult.Yes)
                return;

            repo.Delete(usuario);
            await repo.SaveChangesAsync();

            MessageBox.Show("Usuário excluído com sucesso!");

            dataGrid.ItemsSource = await repo.GetAllAsync();
        }

        private string PromptSenha()
        {
            Window janela = new Window
            {
                Title = "Confirmação",
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            var stack = new StackPanel { Margin = new Thickness(10) };

            var txt = new PasswordBox();
            var btn = new Button
            {
                Content = "Confirmar",
                Margin = new Thickness(0, 10, 0, 0)
            };

            string senha = null;

            btn.Click += (s, e) =>
            {
                senha = txt.Password;
                janela.Close();
            };

            stack.Children.Add(new TextBlock { Text = "Digite sua senha:" });
            stack.Children.Add(txt);
            stack.Children.Add(btn);

            janela.Content = stack;

            janela.ShowDialog();

            return senha;
        }

    }
}
