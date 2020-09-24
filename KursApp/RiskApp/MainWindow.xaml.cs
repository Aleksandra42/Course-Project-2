/*
 * Курсовая работа
 * Программа иддентификации и анализа рисков ИТ-проектов
 * Ф.И.О. исполнителя: Карпова Александра Евгеньевна, группа БПИ 181
 * Ф.И.О. руководителя: Песоцкая Елена Юрьевна
 * Год создания: 2020
 */ 

using System.ComponentModel;
using System.Windows;

namespace RiskApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            UserActions userActions = new UserActions();

            if (await userActions.CheckLogin(loginBox.Text.Trim(), passwordBox.Password.Trim()) == 3)
            {
                AdminProjects adminProject = new AdminProjects();
                Close();
                adminProject.Show();
            }
            else
            {
                if (await userActions.CheckLogin(loginBox.Text.Trim(), passwordBox.Password.Trim()) == 2)
                {
                    User user = await userActions.SearchForUser(loginBox.Text.Trim(), passwordBox.Password.Trim());
                    ChoiceWindow choice = new ChoiceWindow(user);
                    Close();
                    choice.Show();
                }
                else
                {
                    if (await userActions.CheckLogin(loginBox.Text.Trim(), passwordBox.Password.Trim()) == 1)
                    {
                        User user = await userActions.SearchForUser(loginBox.Text.Trim(), passwordBox.Password.Trim());
                        SelectionWindow selectWindow = new SelectionWindow(user);
                        selectWindow.Show();
                        Close();
                    }
                    else
                        MessageBox.Show("Error occured after entering login and password!");
                }
            }
        }
        
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            
        }
    }
}