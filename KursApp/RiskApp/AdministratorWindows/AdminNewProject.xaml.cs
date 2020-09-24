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
    /// Логика взаимодействия для NewProject.xaml
    /// </summary>
    public partial class AdminNewProject : Window
    {
        string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "back.jpg");
        bool flag =true;
        public AdminNewProject()
        {
            InitializeComponent();
        }

        /// <summary>
        /// метод срабатывает при нажатии кнопки Back-кнопки возврата в предыдущее окно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminProjects project = new AdminProjects();
            Close();

            project.Show();
        }

        /// <summary>
        /// метод, который при нажатии кнопки Create Project создаёт новый проект с заданными пользователем параметрами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProjectActions projectActions = new ProjectActions();

                if (TypeCombobox.Text == "Choose Type of the Project")
                    MessageBox.Show("You have to choose the type of a project!");
                else if (Name.Text == null)
                    MessageBox.Show("You have to enter the name of the project!");
                else if (listOwners.SelectedItem == null)
                    MessageBox.Show("You have to choose project's owner!");
                else
                {
                    await projectActions.AddProject(Name.Text, TypeCombobox.Text, ((User)(listOwners.SelectedItem)).Login);
                    AdministratorGraphic graphic = new AdministratorGraphic(new Project(Name.Text, ((User)(listOwners.SelectedItem)).Login, TypeCombobox.Text));
                    Close();
                    graphic.Show();
                }
            }
            catch(ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// метод, который срабатывает при активации окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_Activated(object sender, EventArgs e)
        {
            if (flag)
            {
                BackButton.Foreground = new ImageBrush(new BitmapImage(new Uri(path)));
                BackButton.Background = new ImageBrush(new BitmapImage(new Uri(path)));

                TypeCombobox.Text = "Choose Type of the Project";

                List<User> listUsers = await new UserActions().ShowUsers();

                for (int i = 0; i < listUsers.Count; i++)
                {
                    if (listUsers[i].Position != "RiskManager")
                        listOwners.Items.Add(listUsers[i]);
                }
                flag = false;
            }
        }
    }
}
