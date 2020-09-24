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

namespace RiskApp
{
    /// <summary>
    /// Логика взаимодействия для ProjectChoise.xaml
    /// </summary>
    public partial class ChoiceWindow : Window
    {
        User user = null;
        bool flag = true;
        string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "back.jpg");

        public ChoiceWindow(User user)
        {
            this.user = user;
            InitializeComponent();
        }

        /// <summary>
        /// метод запускается при открытии окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_Activated(object sender, EventArgs e)
        {
            if (flag)
            {
                BackButton.Background = new ImageBrush(new BitmapImage(new Uri(path)));
                BackButton.Foreground = new ImageBrush(new BitmapImage(new Uri(path)));

                ProjectActions projectActions = new ProjectActions();
                List<Project> listProjects = await projectActions.ShowUsersProjects(user.Login);

                for (int i = 0; i < listProjects.Count; i++)
                    listBoxProjects.Items.Add(listProjects[i]);
            }
        }

        /// <summary>
        /// метод для элемента ListBox, отображающего проекты пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxProjects_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listBoxProjects.SelectedItem != null)
            {
                Project project = (Project)listBoxProjects.SelectedItem;
                ProjectManagerGraphic graphic = new ProjectManagerGraphic(project,user);

                Close();
                graphic.Show();
            }
            else
                MessageBox.Show("You need to select project!");
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();

            Close();
            mainWindow.Show();
        }
    }
}
