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
    public partial class RiskSettingsWindow : Window
    {
        bool flag = true;
        
        public double Influence
        {
            get => Double.Parse(ParseLine(InfluenceTextbox.Text));
        }
        public double Probability
        {
            get => Double.Parse(ParseLine(ProbabilityTextbox.Text));
        }
        public User Owner
        {
            get => (User)UsersCombobox.SelectedItem;
        }
        
        public RiskSettingsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// метод добавляет в combobox всех пользователей
        /// </summary>
        /// <returns></returns>
        private async Task AddUsersToCombobox()
        {
            List<User> listUser = await new UserActions().ShowUsers();

            for (int i = 0; i < listUser.Count; i++)
                UsersCombobox.Items.Add(listUser[i]);
        }

        /// <summary>
        /// метод срабатывает при нажатии кнопки "Add clean risk"
        /// создаётся риск без заданных характеристик
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CleanRisk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            InfluenceTextbox.Text = default(double).ToString();
            ProbabilityTextbox.Text = default(double).ToString();
        }

        /// <summary>
        /// метод запускается при нажатии кнопки Create Estimated Risk,
        /// риск задаётся с установленными значениями характеристик
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        private void SetEstimatedRisk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Double.Parse(ParseLine(InfluenceTextbox.Text)) >= 1 || Double.Parse(ParseLine(InfluenceTextbox.Text)) <= 0)
                    throw new ArgumentException("The value of the field 'Influence' must lay in the interval (0,1)");
                
                if (Double.Parse(ParseLine(ProbabilityTextbox.Text)) >= 1 || Double.Parse(ParseLine(ProbabilityTextbox.Text)) <= 0)
                    throw new ArgumentException("The value of the field 'Probability' must lay in the interval (0,1)");
                
                if (UsersCombobox.SelectedItem == null) 
                    throw new NullReferenceException("You must choose project's owner in the combobox!");
                
                this.DialogResult = true;
            }
            catch(ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Exception");
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "Exception");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong!" + ex.Message);
            }
        }

        /// <summary>
        /// метод запускается при открытии окна настроек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_Activated(object sender, EventArgs e)
        {
            if (flag)
            {
                UsersCombobox.Text = "Choose the User ";
                await AddUsersToCombobox();
                flag = false;
            }
        }
        
        /// <summary>
        /// метод для парсинга строки
        /// . заменяется на ,
        /// </summary>
        /// <param name="line"></param>
        /// <returns>возвращается строка</returns>
        private string ParseLine(string line)
        {
            string result = "";
            
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '.') 
                    result += ',';
                else
                    result += line[i];
            }
            
            return result;
        }
    }
}
