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
    /// Логика взаимодействия для Projects.xaml
    /// </summary>
    public partial class AdminProjects : Window
    {
        bool flag = true;
        string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "back.jpg");

        public AdminProjects()
        {
            InitializeComponent();
        }

        /// <summary>
        /// метод, запускающийся при активации окна
        /// отображается список существующих проектов пользователя
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
                List<Project> listProjects = await projectActions.ShowProjects();

                for (int i = 0; i < listProjects.Count; i++)
                {
                    listBoxProjects.Items.Add(listProjects[i]);
                    listBoxProjects.Items.ToString();
                }

                flag = false;
            }
        }


        /// <summary>
        /// метод, котрый запускается при нажатии кнопки Back 
        /// возвращает окно ввода логина и пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Close();
            mainWindow.Show();
        }


        /// <summary>
        /// метод, который срабатывает при нажатии кнопки Create New Project
        /// запускает окно для создания нового проекта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            AdminNewProject newProject = new AdminNewProject();
            Close();
            newProject.Show();
        }

        /// <summary>
        /// метод, который запускается при двойном нажатии кнопкой мыши на один из элементов
        /// списка в ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listBoxProjects.SelectedItem != null)
            {
                Project project = (Project)listBoxProjects.SelectedItem;
                AdministratorGraphic graphic = new AdministratorGraphic(project);

                Close();
                graphic.Show();
            }
            else
            {
                MessageBox.Show("You need to select project or click the button to create a new one!");
            }
        }
    }
}
