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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RiskApp
{
    public partial class AdministratorGraphic : Window
    {
        const double radius = 250;
        const int C = 100;

        double z = 1.0;
        bool flag = true;
        bool flag1 = true;

        string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "back.jpg");
        
        Project _project = null;
        
        List<Risk> listRisks = null;
        List<Risk> listSelected = null;
        List<string> listSource = null;
        
        Point mousePosition;
        Point currentCenter;
        Point center = new Point(500, 50);

        public AdministratorGraphic(Project project)
        {
            _project = project;
            InitializeComponent();
            MakeZoom();
        }        

        /// <summary>
        /// метод выполянется, когда открывается окно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                if (flag)
                {
                    Label label = new Label();

                    label.VerticalAlignment = VerticalAlignment.Top;
                    label.HorizontalAlignment = HorizontalAlignment.Center;
                    label.FontSize = 17;
                    label.Margin = new Thickness(0, 25, 0, 0);
                    label.Content = $"Project: { _project.Name}";
                    grid.Children.Add(label);

                    BackButton.Background = new ImageBrush(new BitmapImage(new Uri(path)));

                    DatabaseActions databaseActions = new DatabaseActions();

                    listSelected = await databaseActions.ShowRisks(_project);

                    if (listSelected == null)
                        listSelected = new List<Risk>();

                    RiskActions riskActions = new RiskActions();

                    listRisks = await riskActions.ShowRisks();
                    SeletionCombobox.Items.Add("Общие риски");
                    SeletionCombobox.Text = "Common Risks";
                    SeletionCombobox.Items.Add(_project.Type);
                    listSource = await riskActions.ShowSources();

                    AddToSelected();

                    for (int i = 0; i < listSource.Count; i++)
                        SeletionCombobox.Items.Add(listSource[i]);

                    Drawing();
                    SearchForDangerousRisks();

                    await SetUsers();
                    flag = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.TargetSite);
            }
        }


        private async Task SetUsers()
        {
            List<User> listUsers = await new UserActions().ShowUsers();

            for (int i = 0; i < listUsers.Count; i++)
            {
                Owner.Items.Add(listUsers[i]);
                NewOwnerCombobox.Items.Add(listUsers[i]);
            }
        }

        private void MakeZoom()
        {
            ZoomINButton.Click += new RoutedEventHandler(ZoomIn);
            ZoomOUTButton.Click += new RoutedEventHandler(ZoomOut);
        }

        private void ZoomIn(object sender, RoutedEventArgs e)
        {
            var scaler = grid.LayoutTransform as ScaleTransform;

            if (scaler == null)
            {
                scaler = new ScaleTransform(z, z);
                grid.LayoutTransform = scaler;
            }

            DoubleAnimation animator = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(600)),
            };
            if (scaler.ScaleX == z)
            {
                z = z + 0.5;
                animator.To = z;
            }
            else if (scaler.ScaleX > 1.0)
            {
                z = z + 0.5;
                animator.To = z;
            }
            scaler.BeginAnimation(ScaleTransform.ScaleXProperty, animator);
            scaler.BeginAnimation(ScaleTransform.ScaleYProperty, animator);
        }

        private void ZoomOut(object sender, RoutedEventArgs e)
        {
            var scaler = grid.LayoutTransform as ScaleTransform;
            if (scaler == null)
            {
                scaler = new ScaleTransform(1.0, 1.0);
                grid.LayoutTransform = scaler;
            }
            DoubleAnimation animator = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(600)),
            };
            if (scaler.ScaleX > 1.0)
            {
                z = scaler.ScaleX - 0.5;
                animator.To = z;
            }

            scaler.BeginAnimation(ScaleTransform.ScaleXProperty, animator);
            scaler.BeginAnimation(ScaleTransform.ScaleYProperty, animator);
        }
        /// <summary>
        /// метод добавляет выбранный риск во вкладку
        /// также убирает их из выбора
        /// </summary>
        private void AddToSelected()
        {
            if (listSelected != null)
            {
                for (int i = 0; i < listSelected.Count; i++)
                {
                    if (listSelected[i].Status == 1)
                        listRisksSelected.Items.Add(listSelected[i]);
                    else
                    {
                        if (listSelected[i].Status == 0)
                            listNewRisks.Items.Add(listSelected[i]);
                        else
                            listRisksNonselected.Items.Add(listSelected[i]);
                    }
                }
            }
        }

        /// <summary>
        /// метод прооверяет, нет ли такого элемента в списке 
        /// </summary>
        private void CheckIfAlreadyInProject()
        {
            if (listSelected != null)
            {
                for (int i = 0; i < listRisks.Count; i++)
                {
                    for (int j = 0; j < listSelected.Count; j++)
                    {
                        if (listRisks[i].RiskName == listSelected[j].RiskName)
                            listRisks.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// метод, который рисует вершины и гиперболу
        /// </summary>
        private void Drawing()
        {
            canvas.Children.Clear();
            
            for (int i = 0; i < C - 1; i++)
            {
                double oldX = center.X - radius + radius / C * i;
                double currentX = center.X - radius + radius / C * (i + 1);

                Line line = new Line();
                
                line.X1 = oldX;
                line.X2 = currentX;
                line.Y1 = FindYCoordinate(oldX, radius, center);
                line.Y2 = FindYCoordinate(currentX, radius, center);
                line.Stroke = Brushes.Black;
                
                canvas.Children.Add(line);
            }

            for (int i = 0; i < listRisksSelected.Items.Count; i++)
            {
                if(((Risk)listRisksSelected.Items[i]).Probability!=default && ((Risk)listRisksSelected.Items[i]).Influence!=default &&
                    ((Risk)listRisksSelected.Items[i]).Status==1)
                {
                    ((Risk)listRisksSelected.Items[i]).point.X = 425 * ((Risk)listRisksSelected.Items[i]).Probability + 75;
                    ((Risk)listRisksSelected.Items[i]).point.Y = -350 * ((Risk)listRisksSelected.Items[i]).Influence + 400;
                    
                    Ellipse ellipse = new Ellipse();
                    ellipse.Height = 12;
                    ellipse.Width = 12;
                    ellipse.StrokeThickness = 3;
                    if (Math.Sqrt((((Risk)listRisksSelected.Items[i]).point.X - center.X) * (((Risk)listRisksSelected.Items[i]).point.X - center.X) +
                        (((Risk)listRisksSelected.Items[i]).point.Y - center.Y) * (((Risk)listRisksSelected.Items[i]).point.Y - center.Y)) < radius)
                    {
                        ellipse.Stroke = Brushes.Red;
                        ellipse.Fill = Brushes.Red;
                    }
                    else
                    {
                        ellipse.Stroke = Brushes.Green;
                        ellipse.Fill = Brushes.Green;
                    }
                    
                    ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                    ellipse.Margin = new Thickness(((Risk)listRisksSelected.Items[i]).point.X, 
                        ((Risk)listRisksSelected.Items[i]).point.Y, 0, 0);
                    ellipse.VerticalAlignment = VerticalAlignment.Top;
                    
                    canvas.Children.Add(ellipse);
                }               
            }
        }

        /// <summary>
        /// метод, который вычисляет координату y с помощью квадратного уравнения
        /// </summary>
        /// <param name="x"></param>
        /// <param name="radius"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        static public double FindYCoordinate(double x, double radius, Point center)
        {
            double a = 1;
            double b = -2 * center.Y;
            double c = -(radius * radius) + (x - center.X) * (x - center.X) + center.Y * center.Y;
            double d = (b * b - 4 * a * c);
            
            if (d < 0)
                throw new Exception("There's no such point!");
                
            return (-b + Math.Sqrt(d)) / 2 * a;
        }

        /// <summary>
        /// метод срабатывает при нажатии на кнопку мыши
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

        private void SelectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listOfRisks.Items.Clear();
            CheckIfAlreadyInProject();

            if (flag){ }

            if (SeletionCombobox.SelectedItem.ToString() == "Общие риски")
            {
                for (int i = 0; i < listRisks.Count; i++)
                {
                    if (listRisks[i].Type == "default")
                        listOfRisks.Items.Add(listRisks[i]);
                }
            }
            else
            {
                if (SeletionCombobox.SelectedItem.ToString() == _project.Type)
                {
                    for (int i = 0; i < listRisks.Count; i++)
                        if (listRisks[i].Type == _project.Type)
                            listOfRisks.Items.Add(listRisks[i]);
                }
                else
                {
                    for (int i = 0; i < listSource.Count; i++)
                    {
                        if (SeletionCombobox.SelectedItem.ToString() == listSource[i])
                        {
                            for (int j = 0; j < listRisks.Count; j++)
                            {
                                if ((listRisks[j].Type == _project.Type || listRisks[j].Type == "default") && 
                                    listRisks[j].Source == listSource[i])
                                {
                                    listOfRisks.Items.Add(listRisks[j]);
                                }
                            }

                        }
                    }
                }
            }
        }


        private void ChangeSelected()
        {
            if (SeletionCombobox.SelectedItem != null) 
                SeletionCombobox.SelectedItem = "Common Risks";
            
            listOfRisks.Items.Clear();
            CheckIfAlreadyInProject();
            
            if (flag) { }
            
            if (SeletionCombobox.SelectedItem.ToString() == "Common Risks")
            {
                for (int i = 0; i < listRisks.Count; i++)
                {
                    if (listRisks[i].Type == "default")
                        listOfRisks.Items.Add(listRisks[i]);
                }
            }
            else
            {
                if (SeletionCombobox.SelectedItem.ToString() == _project.Type)
                {
                    for (int i = 0; i < listRisks.Count; i++)
                    {
                        if (listRisks[i].Type == _project.Type)
                            listOfRisks.Items.Add(listRisks[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < listSource.Count; i++)
                    {
                        if (SeletionCombobox.SelectedItem.ToString() == listSource[i])
                        {
                            for (int j = 0; j < listRisks.Count; j++)
                            {
                                if ((listRisks[j].Type == _project.Type || listRisks[j].Type == "default") && listRisks[j].Source == listSource[i])
                                    listOfRisks.Items.Add(listRisks[j]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// проверяет нет ли в выбранных уже такого элемента
        /// </summary>
        /// <param name="checkrisk"></param>
        /// <returns></returns>
        private bool CheckIfAlreadySelected(Risk risk)
        {
            if (listRisksSelected.Items.Count != 0)
            {
                for (int i = 0; i < listRisksSelected.Items.Count; i++)
                {
                    if (risk.ID == ((Risk)listRisksSelected.Items[i]).ID)
                        return false;
                }
                
                return true;
            }
            
            return true;
        }       

        /// <summary>
        /// метод, который срабатывает при нажатии кнопки и
        /// устанавливает характерстики для риска и добавляет в бд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SetUpRisk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Double.Parse(ParseLine(InfluenceTextbox.Text)) == default || Double.Parse(ParseLine(ProbabilityTextbox.Text)) == default)
                    throw new ArgumentException("Values of Probability and Influence fields must stay (0,1)");
                
                if(Owner.SelectedItem == null) 
                    throw new NullReferenceException("You need to choose the user you want to assign risk to!");
                
                ((Risk)listRisksSelected.SelectedItem).Influence = double.Parse(ParseLine(InfluenceTextbox.Text));
                ((Risk)listRisksSelected.SelectedItem).Probability = double.Parse(ParseLine(ProbabilityTextbox.Text));
                
                DatabaseActions databaseActions = new DatabaseActions();
                
                await databaseActions.ChangeRisk((Risk)listRisksSelected.SelectedItem, (User)Owner.SelectedItem);
                
                listRisksSelected.Items.Clear();
                listSelected = await databaseActions.ShowRisks(_project);
                    
                for (int i = 0; i < listSelected.Count; i++)
                {
                    if(listSelected[i].Status==1)
                        listRisksSelected.Items.Add(listSelected[i]);
                }
                
                Drawing();               
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message,"Exception");

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Exception");

            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong!");

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

        /// <summary>
        /// вводим значение рисков в текстбоксы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selected_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listRisksSelected.SelectedItem!=null)
            {
                InfluenceTextbox.Text = ((Risk)listRisksSelected.SelectedItem).Influence.ToString();
                ProbabilityTextbox.Text = ((Risk)listRisksSelected.SelectedItem).Probability.ToString();
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
                Drawing();
                SearchForDangerousRisks();
            }
        }

        /// <summary>
        /// метод, который ищет среди рисков проекта опасные и добавляет в раздел Dangerous Risks
        /// </summary>
        private void SearchForDangerousRisks()
        {
            listDangerous.Items.Clear();
            
            if (listRisksSelected.Items.Count != 0)
            {
                for (int i = 0; i < listRisksSelected.Items.Count; i++)
                {
                    double c = Math.Sqrt((((Risk) listRisksSelected.Items[i]).point.X - center.X) *
                                         (((Risk) listRisksSelected.Items[i]).point.X - center.X) +
                                         (((Risk) listRisksSelected.Items[i]).point.Y - center.Y) *
                                         (((Risk) listRisksSelected.Items[i]).point.Y - center.Y)); 
                    if (c < radius)
                        listDangerous.Items.Add((Risk)listRisksSelected.Items[i]);
                }
            }
        }

        /// <summary>
        /// метод, который выводит в MessageBox информацию о выбранном на графике риске
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (listRisksSelected.Items.Count != 0)
            {
                List<Risk> list = new List<Risk>();
                
                for (int i = 0; i < listRisksSelected.Items.Count; i++)
                {
                    double _x = Math.Abs(((Risk) listRisksSelected.Items[i]).point.X - e.GetPosition(null).X);
                    double _y = Math.Abs(((Risk) listRisksSelected.Items[i]).point.Y - e.GetPosition(null).Y);
                    
                    if ( _x <= 10 && _y <= 10)
                        list.Add((Risk)listRisksSelected.Items[i]);
                }
                
                if(list.Count!=0)
                    MessageBox.Show(CreateLine(list), "Information about Selected Risks");
            }
        }

        /// <summary>
        /// метод, который возвращает строку с необходимой информацией
        /// </summary>
        /// <param name="click"></param>
        /// <returns></returns>
        private string CreateLine(List<Risk> list)
        {
            string line = "";
            
            for (int i = 0; i < list.Count; i++)
                line += $"Name of Risk: {listRisks[i].RiskName}\nSource: {listRisks[i].Source}\nType: {listRisks[i].Type}\nEffects: {listRisks[i].Effects}\nPossible Solution: {listRisks[i].Solution}";
                
            return line;
        }

        /// <summary>
        /// метод для удаления  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            DatabaseActions databaseActions = new DatabaseActions();
            Risk risk = (Risk)((Button)sender).DataContext;
            User user = new User();
            
            if (listSelected == null) 
                listSelected = new List<Risk>();
            
            listRisksSelected.Items.Remove(risk);
            risk.Status = 2;
            SearchForCurrentRisk(risk);
            
            user.ID = risk.IdUser;
            user.Login = risk.OwnerLogin;
            
            await databaseActions.ChangeRisk(risk);
            listRisksNonselected.Items.Add(risk);
            Drawing();
        }

        /// <summary>
        /// метод ищет риск по его номеру в списке выбранных рисков
        /// </summary>
        /// <param name="risk"></param>
        private void SearchForCurrentRisk(Risk risk)
        {
            for (int i = 0; i < listSelected.Count; i++)
            {
                if(listSelected[i].ID == risk.ID)
                {
                    listSelected[i].Status = risk.Status;
                    listSelected[i].IdUser = risk.IdUser;
                    listSelected[i].OwnerLogin = risk.OwnerLogin;
                }
            }
        }

        /// <summary>
        /// метод добавляет риск в проект
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddToProject_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfAlreadySelected(((Risk)((Button)sender).DataContext)))
            {
                RiskSettingsWindow settingsWindow = new RiskSettingsWindow();
                Risk risk = (Risk)(((Button)sender).DataContext);

                if (settingsWindow.ShowDialog() == true)
                {
                    try
                    {
                        risk.Probability = settingsWindow.Probability;
                        risk.Influence = settingsWindow.Influence;

                        if (settingsWindow.Influence != default)
                            ((Risk)((Button)sender).DataContext).Status = 1;
                        else
                            ((Risk)((Button)sender).DataContext).Status = 0;

                        DatabaseActions databaseActions = new DatabaseActions();

                        if (settingsWindow.Owner == null)
                            await databaseActions.AddRisk(_project.Name, risk);
                        else
                        {
                            risk.OwnerLogin = settingsWindow.Owner.Login;
                            risk.IdUser = settingsWindow.Owner.ID;
                            await databaseActions.AddRisk(_project.Name, risk, settingsWindow.Owner);
                        }

                        listSelected.Add(risk);

                        SearchForCurrentRisk(risk);
                        ChangeSelected();                    
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Wrong in enpty" + ex.Message);
                    }
                }

                listRisksSelected.Items.Clear();
                listNewRisks.Items.Clear();
                
                for (int i = 0; i < listSelected.Count; i++)
                {
                    if (listSelected[i].Status == 1)
                        listRisksSelected.Items.Add(listSelected[i]);

                    if (listSelected[i].Status == 0)
                        listNewRisks.Items.Add(listSelected[i]);
                }

                Drawing();
                listRisks.Remove(risk);
                SeletionCombobox.SelectedItem = SeletionCombobox.SelectedItem;
            }
            else
            {
                MessageBox.Show("This element has already been selected");
            }
        }

        /// <summary>
        /// метод выполняется при двойном нажатии на риск из раздела опасных
        /// и открывает окно дерева рисков 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DangerousRisks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(listDangerous.SelectedItem != null && flag1)
            {
                AdminTree adminTree = new AdminTree((Risk)listDangerous.SelectedItem, _project, center);

                Close();
                adminTree.Show();

                flag1 = false;
            }
        }

        /// <summary>
        /// метод выполняется при нажатии кнопки Back, возвращает к окну выбора проектов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetBack_Click(object sender, RoutedEventArgs e)
        {
            AdminProjects project = new AdminProjects();
            Close();
            
            project.Show();
        }

        /// <summary>
        /// метод , который добавляет риск в раздел активных рисков таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddToActiveRisks_Click(object sender, RoutedEventArgs e)
        {
            Risk risk = (Risk)((Button)sender).DataContext;

            listRisksNonselected.Items.Remove(risk);
            listSelected.Add(risk);
            listRisksSelected.Items.Add(risk);

            risk.Status = 1;
            SearchForCurrentRisk(risk);

            DatabaseActions databaseActions = new DatabaseActions();

            await databaseActions.ChangeRisk(risk);
            Drawing();
        }

        /// <summary>
        /// метод выполняется при нажатии кнопки Set Up в разделе таблицы New Risks 
        /// устанавливает значения характеристик для нового риска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SetUpNewRisk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listNewRisks.SelectedItems != null  &&
                    Double.Parse(ParseLine(NewInfluenceTextbox.Text)) != default &&
                    Double.Parse(ParseLine(NewProbabilityTextbox.Text)) != default)
                {
                    ((Risk)listNewRisks.SelectedItem).Status = 1;
                    ((Risk)listNewRisks.SelectedItem).IdUser = ((User)NewOwnerCombobox.SelectedItem).ID;
                    ((Risk)listNewRisks.SelectedItem).OwnerLogin = ((User)NewOwnerCombobox.SelectedItem).Login;

                    ((Risk)listNewRisks.SelectedItem).Influence = double.Parse(ParseLine(NewInfluenceTextbox.Text));
                    ((Risk)listNewRisks.SelectedItem).Probability = double.Parse(ParseLine(NewProbabilityTextbox.Text));

                    SearchForCurrentRisk(((Risk)listNewRisks.SelectedItem));

                    DatabaseActions databaseActions = new DatabaseActions();

                    await databaseActions.ChangeRisk((Risk)listNewRisks.SelectedItem, (User)NewOwnerCombobox.SelectedItem);

                    listNewRisks.Items.Clear();
                    listRisksSelected.Items.Clear();
                    
                    for (int i = 0; i < listSelected.Count; i++)
                    {
                        if(listSelected[i].Status==0)
                            listNewRisks.Items.Add(listSelected[i]);
                        
                        if (listSelected[i].Status == 1)
                            listRisksSelected.Items.Add(listSelected[i]);
                    }

                    Drawing();
                }
                else
                {
                    MessageBox.Show("Wrong in entry");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong in entry");
            }
        }

        /// <summary>
        /// метод для элемента combobox 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRisksBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listNewRisks.SelectedItem != null)
            {
                NewInfluenceTextbox.Text = "0";
                NewProbabilityTextbox.Text = "0";
            }
        }
    }
}
