
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
using Repositorio;

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
    }
}
