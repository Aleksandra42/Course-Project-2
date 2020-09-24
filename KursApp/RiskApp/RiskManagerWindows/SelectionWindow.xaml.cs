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
    /// Логика взаимодействия для SelectProject.xaml
    /// </summary>
    public partial class SelectionWindow : Window
    {
        bool flag = true;
        User user = null;
        string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "back.jpg");

        public SelectionWindow(User user)
        {
            this.user = user;

            InitializeComponent();
        }

        /// <summary>
        /// метод, который срабатывает при нажатии кнопки BackButton,
        /// закрывает окно выбора и открывает окно ввода логина и пароля
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
        /// метод, который при выборе объекта в списке открывает окно проекта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                Project project = (Project)listBox.SelectedItem;
                RiskManagerGraphic graphic = new RiskManagerGraphic(project, user);

                Close();
                graphic.Show();
            }
            else
            {
                MessageBox.Show("You need to select project!");
            }

        }

        /// <summary>
        /// метод, который запускается при открытии окна
        /// открывается список проектов менеджера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async  void Window_Activated(object sender, EventArgs e)
        {
            if (flag)
            {
                ProjectActions projectActions = new ProjectActions();
                DatabaseActions databaseActions = new DatabaseActions();

                List<string> listName = await databaseActions.ShowUserProjects(user);
                List<Project> listProjects = await projectActions.ShowProjects();

                BackButton.Background = new ImageBrush(new BitmapImage(new Uri(path)));
                BackButton.Foreground = new ImageBrush(new BitmapImage(new Uri(path)));

                try
                {
                    for (int i = 0; i < listProjects.Count; i++)
                    {
                        for (int j = 0; j < listName.Count; j++)
                        {
                            if (listName[j] == listProjects[i].Name)
                                listBox.Items.Add(listProjects[i]);
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("You havent projects and risks");
                }

                flag = false;
            }
        }
    }
}
