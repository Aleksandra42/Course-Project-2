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
    /// Логика взаимодействия для GraphicForProjectManager.xaml
    /// </summary>
    public partial class ProjectManagerGraphic : Window
    {
        const int K = 100;
        const double radius = 250;

        User _user = null;

        string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "back.jpg");

        List<Risk> listAllRisks = null;
        List<string> listSource = null;
        List<Risk> listRisksSelected = null;

        bool flag = true;
        bool flag1 = true;

        Project _project = null;

        Point center = new Point(500, 50);
        Point mousePosition;
        Point currentCenter;
        public ProjectManagerGraphic(Project project, User user)
        {
            _project = project;
            _user=user;

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
                Label label = new Label();
                label.VerticalAlignment = VerticalAlignment.Top;
                label.HorizontalAlignment = HorizontalAlignment.Center;
                label.FontSize = 15;
                label.Margin = new Thickness(0, 25, 0, 0);
                label.Content = $"Project:  { _project.Name}";
                
                grid.Children.Add(label);
                BackButton.Background = new ImageBrush(new BitmapImage(new Uri(path)));

                DatabaseActions databaseActions = new DatabaseActions();

                listRisksSelected = await databaseActions.ShowRisks(_project);

                if (listRisksSelected == null)
                    listRisksSelected = new List<Risk>();

                RiskActions riskActions = new RiskActions();
                             
                listAllRisks = await riskActions.ShowRisks();
                ComboBoxTypes.Items.Add("Общие риски");
                ComboBoxTypes.Text = "Common Risks";
                ComboBoxTypes.Items.Add(_project.Type);
                listSource = await riskActions.ShowSources();

                AddInSelected();

                for (int i = 0; i < listSource.Count; i++)
                    ComboBoxTypes.Items.Add(listSource[i]);

                DrawHyperbola();
                FindDangerousRisks();
                await SetOwners();
                flag = false;
            }
        }

        
        private async Task SetOwners()
        {
            List<User> listUsers = await new UserActions().ShowUsers();
            
            for (int i = 0; i < listUsers.Count; i++)
            {
                OwnerCombobox.Items.Add(listUsers[i]);
                OwnerNewCombobox.Items.Add(listUsers[i]);
            }
        }

        /// <summary>
        /// метод добавляет во вкладку выбранные риски
        /// и убирает их из выбора
        /// </summary>
        private void AddInSelected()
        {
            if (listRisksSelected != null)
            {
                for (int i = 0; i < listRisksSelected.Count; i++)
                {
                    if (listRisksSelected[i].Status == 1)
                        listSelected.Items.Add(listRisksSelected[i]);
                    else
                    {
                        if (listRisksSelected[i].Status == 0)
                            listNewRisks.Items.Add(listRisksSelected[i]);
                        else
                            listUnSelected.Items.Add(listRisksSelected[i]);
                    }
                }
            }
        }

        /// <summary>
        /// метод проверяет, не нахоядтся ли данные элементы уже в проекте
        /// </summary>
        private void CheckIfInProject()
        {
            if (listRisksSelected != null)
            {
                for (int i = 0; i < listAllRisks.Count; i++)
                {
                    for (int j = 0; j < listRisksSelected.Count; j++)
                    {
                        if (listAllRisks[i].RiskName == listRisksSelected[j].RiskName)
                            listAllRisks.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// метод отрисовывает гиперболу и точки
        /// </summary>
        private void DrawHyperbola()
        {
            canvas.Children.Clear();
            
            for (int i = 0; i < K - 1; i++)
            {
                double oldX = center.X - radius + radius / K * i;
                double currentX = center.X - radius + radius / K * (i + 1);

                Line line = new Line();

                line.X1 = oldX;
                line.X2 = currentX;
                line.Y1 = FindYCoordinate(oldX, radius, center);
                line.Y2 = FindYCoordinate(currentX, radius, center);
                line.Stroke = Brushes.Black;
                
                canvas.Children.Add(line);
            }

            for (int i = 0; i < listSelected.Items.Count; i++)
            {
                if (((Risk)listSelected.Items[i]).Probability != default && 
                    ((Risk)listSelected.Items[i]).Influence != default&& ((Risk)listSelected.Items[i]).Status == 1)
                {
                    ((Risk)listSelected.Items[i]).point.X = 425 * ((Risk)listSelected.Items[i]).Probability + 75;
                    ((Risk)listSelected.Items[i]).point.Y = -350 * ((Risk)listSelected.Items[i]).Influence + 400;
                    
                    Ellipse ellipse = new Ellipse();
                    ellipse.Height = 10;
                    ellipse.Width = 10;
                    ellipse.StrokeThickness = 2;
                    
                    if (Math.Sqrt((((Risk)listSelected.Items[i]).point.X - center.X) * (((Risk)listSelected.Items[i]).point.X - center.X) +
                        (((Risk)listSelected.Items[i]).point.Y - center.Y) * (((Risk)listSelected.Items[i]).point.Y - center.Y)) < radius)
                    {
                        ellipse.Stroke = Brushes.Red;
                        ellipse.Fill = Brushes.Red;
                    }
                    else
                    {
                        ellipse.Stroke = Brushes.Green;
                        ellipse.Fill = Brushes.Green;
                    }
                    
                    ellipse.VerticalAlignment = VerticalAlignment.Top;
                    ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                    ellipse.Margin = new Thickness(((Risk)listSelected.Items[i]).point.X, ((Risk)listSelected.Items[i]).point.Y, 0, 0);
                    
                    canvas.Children.Add(ellipse);
                }
            }
        }

        static public double FindYCoordinate(double x, double radius, Point center)
        {
            double a = 1;
            double b = -2 * center.Y;
            double c = -radius * radius + (x - center.X) * (x - center.X) + center.Y * center.Y;
            double d = (b * b - 4 * a * c);
            
            if (d < 0)
                throw new Exception("There's no such point!");
                
            return (-b + Math.Sqrt(d)) / 2 * a;
        }

        /// <summary>
        /// метод срабатывает при нажатии кнопки мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.GetPosition(null).X < 540 && e.GetPosition(null).Y < 450)
            {
                mousePosition = e.GetPosition(null);
                currentCenter = center;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listRisks.Items.Clear();
            CheckIfInProject();
            if (flag)
            {
            }
            if (ComboBoxTypes.SelectedItem.ToString() == "Общие риски")
            {
                for (int i = 0; i < listAllRisks.Count; i++)
                {
                    if (listAllRisks[i].Type == "default")
                        listRisks.Items.Add(listAllRisks[i]);
                }
            }
            else
            {
                if (ComboBoxTypes.SelectedItem.ToString() == _project.Type)
                {
                    for (int i = 0; i < listAllRisks.Count; i++)
                    {
                        if (listAllRisks[i].Type == _project.Type)
                            listRisks.Items.Add(listAllRisks[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < listSource.Count; i++)
                    {
                        if (ComboBoxTypes.SelectedItem.ToString() == listSource[i])
                        {
                            for (int j = 0; j < listAllRisks.Count; j++)
                            {
                                if ((listAllRisks[j].Type == _project.Type || listAllRisks[j].Type == "default") && listAllRisks[j].Source == listSource[i])
                                    listRisks.Items.Add(listAllRisks[j]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// метод для комбобокса 
        /// </summary>
        private void ChangeSelected()
        {
            if (ComboBoxTypes.SelectedItem != null) 
                ComboBoxTypes.SelectedItem = "Общие риски";
            
            listRisks.Items.Clear();
            CheckIfInProject();
            if (flag) { }
            
            if (ComboBoxTypes.SelectedItem.ToString() == "Общие риски")
            {
                for (int i = 0; i < listAllRisks.Count; i++)
                {
                    if (listAllRisks[i].Type == "default")
                        listRisks.Items.Add(listAllRisks[i]);
                }
            }
            else
            {
                if (ComboBoxTypes.SelectedItem.ToString() == _project.Type)
                {
                    for (int i = 0; i < listAllRisks.Count; i++)
                    {
                        if (listAllRisks[i].Type == _project.Type)
                            listRisks.Items.Add(listAllRisks[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < listSource.Count; i++)
                    {
                        if (ComboBoxTypes.SelectedItem.ToString() == listSource[i])
                        {
                            for (int j = 0; j < listAllRisks.Count; j++)
                            {
                                if ((listAllRisks[j].Type == _project.Type || listAllRisks[j].Type == "default") 
                                    && listAllRisks[j].Source == listSource[i])
                                    listRisks.Items.Add(listAllRisks[j]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// метод для проверки существования риска в списке
        /// </summary>
        /// <param name="checkrisk"></param>
        /// <returns></returns>
        private bool CheckIfSelected(Risk risk)
        {
            if (listSelected.Items.Count != 0)
            {
                for (int i = 0; i < listSelected.Items.Count; i++)
                {
                    if (risk.ID == ((Risk)listSelected.Items[i]).ID)
                        return false;
                }
                
                return true;
            }
            
            return true;
        }

        /// <summary>
        /// метод для установления вероятности и влияния риска при нажатии на кнопку
        /// и внесении данных в бд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SetUpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Double.Parse(ParseLine(InfluenceTextbox.Text)) == default || Double.Parse(ParseLine(ProbabilityTextbox.Text)) == default)
                    throw new ArgumentException("The values of probability and influence must lay in the interval (0,1)!");

                if (OwnerCombobox.SelectedItem == null) 
                    throw new NullReferenceException("You have to choose the user you want to assign the risk to!");

                ((Risk)listSelected.SelectedItem).Influence = double.Parse(ParseLine(InfluenceTextbox.Text));
                ((Risk)listSelected.SelectedItem).Probability = double.Parse(ParseLine(ProbabilityTextbox.Text));
                
                DatabaseActions databaseActions = new DatabaseActions();
                await databaseActions.ChangeRisk((Risk)listSelected.SelectedItem, (User)OwnerCombobox.SelectedItem);
                
                listSelected.Items.Clear();
                listRisksSelected = await databaseActions.ShowRisks(_project);
                
                for (int i = 0; i < listRisksSelected.Count; i++)
                {
                    if (listRisksSelected[i].Status == 1)
                        listSelected.Items.Add(listRisksSelected[i]);
                }
                
                DrawHyperbola();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "Exception");

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Exception");

            }
            catch (Exception)
            {
                MessageBox.Show("Something's not right!");
            }
        }
        
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

        /// <summary>
        /// метод, считывающий введенные в текстбоксы значения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedRisks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listSelected.SelectedItem != null)
            {
                InfluenceTextbox.Text = ((Risk)listSelected.SelectedItem).Influence.ToString();
                ProbabilityTextbox.Text = ((Risk)listSelected.SelectedItem).Probability.ToString();
            }
        }

        /// <summary>
        /// перемещие гиперболы во время движения мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.GetPosition(null).X < 540 && e.GetPosition(null).Y < 450 && e.LeftButton == MouseButtonState.Pressed)
            {
                center.X = currentCenter.X - mousePosition.X + e.GetPosition(null).X;
                center.Y = currentCenter.Y + (mousePosition.X - e.GetPosition(null).X) / 1.2;
                
                if (center.X > 650 || center.Y < -100)
                {
                    center.Y = -100;
                    center.X = 650;
                }
                
                if (center.Y > 230 || center.X < 250)
                {
                    center.Y = 230;
                    center.X = 250;
                }
                
                DrawHyperbola();
                FindDangerousRisks();
            }
        }

        /// <summary>
        /// метод ищет опасные риски среди представленных на координатной плоскости
        /// </summary>
        private void FindDangerousRisks()
        {
            listDangerous.Items.Clear();
            
            if (listSelected.Items.Count != 0)
            {
                for (int i = 0; i < listSelected.Items.Count; i++)
                {
                    if (Math.Sqrt((((Risk)listSelected.Items[i]).point.X - center.X) * (((Risk)listSelected.Items[i]).point.X - center.X) +
                        (((Risk)listSelected.Items[i]).point.Y - center.Y) * (((Risk)listSelected.Items[i]).point.Y - center.Y)) < radius)
                    {
                        listDangerous.Items.Add((Risk)listSelected.Items[i]);
                    }
                }
            }
        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (listSelected.Items.Count != 0)
            {
                List<Risk> click = new List<Risk>();
                for (int i = 0; i < listSelected.Items.Count; i++)
                {
                    if (Math.Abs(((Risk)listSelected.Items[i]).point.X - e.GetPosition(null).X) <= 10
                        && Math.Abs(((Risk)listSelected.Items[i]).point.Y - e.GetPosition(null).Y) <= 10)
                    {
                        click.Add((Risk)listSelected.Items[i]);
                    }
                }
                if (click.Count != 0)
                {
                    MessageBox.Show(CreateLine(click), "Information about Selected Risks");
                }
            }
        }

        /// <summary>
        /// метод, возвращающий строку для вывода информации при нажатии праой кнопки мыши
        /// </summary>
        /// <param name="click"></param>
        /// <returns></returns>
        private string CreateLine(List<Risk> listClicks)
        {
            string line = "";
            
            for (int i = 0; i < listClicks.Count; i++)
                line += $"RiskName: {listClicks[i].RiskName}\nSource: {listClicks[i].Source}\nType: {listClicks[i].Type}\nDescription: {listClicks[i].Solution}";
    
            return line;
        }
        /// <summary>
        /// метод для удаления риска при нажатии кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseActions databaseActions = new DatabaseActions();
            Risk risk = (Risk)((Button)sender).DataContext;
            
            listRisksSelected.Remove(risk);
            
            if (listRisksSelected == null) 
                listRisksSelected = new List<Risk>();
            
            listSelected.Items.Remove(risk);
            risk.Status = 2;
            SearchForCurrentRisk(risk);
            
            await databaseActions.ChangeRisk(risk);
            listUnSelected.Items.Add(risk);

            DrawHyperbola();
        }

        /// <summary>
        /// метод для поиска риска в списке выбранных
        /// </summary>
        /// <param name="risk"></param>
        private void SearchForCurrentRisk(Risk risk)
        {
            for (int i = 0; i < listRisksSelected.Count; i++)
            {
                if (risk.ID == listRisksSelected[i].ID)
                {
                    listRisksSelected[i].Status = risk.Status;
                    listRisksSelected[i].IdUser = risk.IdUser;
                    listRisksSelected[i].OwnerLogin = risk.OwnerLogin;
                }
            }
        }
        
        /// <summary>
        /// метод, который добавляет риск в список выбранных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfSelected(((Risk)((Button)sender).DataContext)))
            {
                RiskSettingsWindow window = new RiskSettingsWindow();
                Risk risk = (Risk)(((Button)sender).DataContext);

                if (window.ShowDialog() == true)
                {
                    try
                    {
                        risk.Influence = window.Influence;
                        risk.Probability = window.Probability;
                        
                        if (window.Influence == default(double))
                            ((Risk)((Button)sender).DataContext).Status = 0;
                        else
                            ((Risk)((Button)sender).DataContext).Status = 1;

                        DatabaseActions databaseActions = new DatabaseActions();
                        
                        if (window.Owner == null)
                            await databaseActions.AddRisk(_project.Name, risk);
                        else
                        {
                            risk.OwnerLogin = window.Owner.Login;
                            risk.IdUser = window.Owner.ID;

                            await databaseActions.AddRisk(_project.Name, risk, window.Owner);
                            SearchForCurrentRisk(risk);
                        }
                        
                        listRisksSelected.Add(risk);
                        ChangeSelected();
                    }
                    catch
                    {
                        MessageBox.Show("Something went wrong");
                    }
                }

                listSelected.Items.Clear();
                listNewRisks.Items.Clear();
                for (int i = 0; i < listRisksSelected.Count; i++)
                {
                    if (listRisksSelected[i].Status == 1)
                        listSelected.Items.Add(listRisksSelected[i]);

                    if (listRisksSelected[i].Status == 0)
                        listNewRisks.Items.Add(listRisksSelected[i]);

                }
                
                DrawHyperbola();
                listAllRisks.Remove(risk);
                ComboBoxTypes.SelectedItem = ComboBoxTypes.SelectedItem;
            }
            else
            {
                MessageBox.Show("This element has already been selected!");
            }
        }
        private void DangerousRisks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (flag1 && listDangerous.SelectedItem != null)
            {
                ProjectManagerTree tree = new ProjectManagerTree((Risk)listDangerous.SelectedItem, _project, _user, center);
                Close();
                tree.Show();
                flag1 = false;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ChoiceWindow choice = new ChoiceWindow(_user);
            Close();
            choice.Show();
        }

        private async void AddToActive_Click(object sender, RoutedEventArgs e)
        {
            Risk risk = (Risk)((Button)sender).DataContext;
            
            listUnSelected.Items.Remove(risk);
            listRisksSelected.Add(risk);
            listSelected.Items.Add(risk);
            risk.Status = 1;
            SearchForCurrentRisk(risk);
            
            DatabaseActions databaseActions = new DatabaseActions();
            
            await databaseActions.ChangeRisk(risk);
            DrawHyperbola();
        }

        private async void SetUpNewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listNewRisks.SelectedItems != null &&
                    Double.Parse(ParseLine(NewInfluenceTextbox.Text)) != default &&
                    Double.Parse(ParseLine(NewProbabilityTextbox.Text)) != default
                    && OwnerNewCombobox.SelectedItem != null)
                {
                    ((Risk)listNewRisks.SelectedItem).Influence = double.Parse(ParseLine(NewInfluenceTextbox.Text));
                    ((Risk)listNewRisks.SelectedItem).Probability = double.Parse(ParseLine(NewProbabilityTextbox.Text));
                    ((Risk)listNewRisks.SelectedItem).Status = 1;
                    SearchForCurrentRisk(((Risk)listNewRisks.SelectedItem));

                    DatabaseActions databaseActions = new DatabaseActions();

                    await databaseActions.ChangeRisk((Risk)listNewRisks.SelectedItem, (User)OwnerNewCombobox.SelectedItem);
                    listNewRisks.Items.Clear();
                    listSelected.Items.Clear();
                    listRisksSelected = await databaseActions.ShowRisks(_project);
                    
                    for (int i = 0; i < listRisksSelected.Count; i++)
                    {
                        if (listRisksSelected[i].Status == 0)
                            listNewRisks.Items.Add(listRisksSelected[i]);

                        if (listRisksSelected[i].Status == 1)
                            listSelected.Items.Add(listRisksSelected[i]);
                    }
                    
                    DrawHyperbola();
                }
                else
                {
                    MessageBox.Show("Something went wrong!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// метод устанавливает нулевые значения для нового риска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRisksSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listNewRisks.SelectedItem != null)
            {
                NewInfluenceTextbox.Text = "0";
                NewProbabilityTextbox.Text = "0";
            }
        }
    }
}
