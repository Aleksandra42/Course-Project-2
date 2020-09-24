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
    public partial class RiskManagerGraphic : Window
    {
        const int C = 100;
        const double radius = 250;

        bool flag = true;
        bool flag1 = true;

        Project project = null;
        User user = null;

        Point mousePosition;
        Point currentCenter;
        Point center = new Point(500, 50);

        List<Risk> listSelected = null;

        string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "back.jpg");

        public RiskManagerGraphic(Project project, User user)
        {
            this.user=user;
            this.project = project;

            InitializeComponent();
        }
        /// <summary>
        /// метод при открытие окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_Activated(object sender, EventArgs e)
        {
            if (flag)
            {
                Label label = new Label();

                DatabaseActions databaseActions = new DatabaseActions();

                label.VerticalAlignment = VerticalAlignment.Top;
                label.HorizontalAlignment = HorizontalAlignment.Center;
                label.FontSize = 15;
                label.Margin = new Thickness(0, 25, 0, 0);
                label.Content = $"Project: { project.Name}";
                grid.Children.Add(label);
                BackButton.Background = new ImageBrush(new BitmapImage(new Uri(path)));

                listSelected = await databaseActions.ShowRisks(project);

                if (listSelected == null)
                    listSelected = new List<Risk>();

                AddToSelected();
                DrawGraphic();
                SearchForDangerousRisks();
                await SetUsers();

                flag = false;
            }
        }

        /// <summary>
        /// метод для добавления риска в список активных
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
            DrawGraphic();
        }

        private async void SetUpNewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listNewRisks.SelectedItems != null &&
                    Double.Parse(ParseLine(TextboxInfluenceNew.Text)) != default &&
                    Double.Parse(ParseLine(TextboxProbabilityNew.Text)) != default 
                    )
                {
                    ((Risk)listNewRisks.SelectedItem).Influence = double.Parse(ParseLine(InfluenceTextbox.Text));
                    ((Risk)listNewRisks.SelectedItem).Probability = double.Parse(ParseLine(ProbabilityTextbox.Text));
                    ((Risk)listNewRisks.SelectedItem).Status = 1;

                    SearchForCurrentRisk(((Risk)listNewRisks.SelectedItem));

                    DatabaseActions databaseActions = new DatabaseActions();

                    await databaseActions.ChangeRisk((Risk)listNewRisks.SelectedItem, (User)UserNewCombobox.SelectedItem);

                    listNewRisks.Items.Clear();
                    listSelected = await databaseActions.ShowRisks(project);

                    for (int i = 0; i < listSelected.Count; i++)
                    {
                        if (listSelected[i].Status == 0)
                            listNewRisks.Items.Add(listSelected[i]);

                        if (listSelected[i].Status == 1)
                            listRisksSelected.Items.Add(listSelected[i]);
                    }

                    DrawGraphic();
                }
                else
                {
                    MessageBox.Show("Wrong in enpty1");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("It's possible that user has not been set!");

            }
        }

        private void ListNewRisks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listNewRisks.SelectedItem != null)
            {
                TextboxInfluenceNew.Text = "0";
                TextboxProbabilityNew.Text = "0";
            }
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

        /// <summary>
        /// метод для кнопки Set up, который устанавливает новые значения характеристик для выбранного риска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SetUpRiskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listRisksSelected.SelectedItems == null) 
                    throw new NullReferenceException("You need to choose a risk you want to update!");

                if (Double.Parse(ParseLine(InfluenceTextbox.Text)) == default || Double.Parse(ParseLine(ProbabilityTextbox.Text)) == default)
                    throw new ArgumentException("The 'Probability' and 'Infuence' values must lay in the interval [0;1]!");

                ((Risk)listRisksSelected.SelectedItem).Influence = double.Parse(ParseLine(InfluenceTextbox.Text));
                ((Risk)listRisksSelected.SelectedItem).Probability = double.Parse(ParseLine(ProbabilityTextbox.Text));

                DatabaseActions databaseActions = new DatabaseActions();

                await databaseActions.ChangeRisk((Risk)listRisksSelected.SelectedItem);

                listRisksSelected.Items.Clear();
                listSelected = await databaseActions.ShowRisks(project);

                for (int i = 0; i < listSelected.Count; i++)
                    if (listSelected[i].Status == 1)
                        listRisksSelected.Items.Add(listSelected[i]);

                DrawGraphic();
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
                MessageBox.Show("Что-то пошло не так");

            }
        }

        /// <summary>
        /// вводим значение рисков в текстбоксы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RiskSelected_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listRisksSelected.Items.Count != 0)
            {
                InfluenceTextbox.Text = ((Risk)listRisksSelected.SelectedItem).Influence.ToString();
                ProbabilityTextbox.Text = ((Risk)listRisksSelected.SelectedItem).Probability.ToString();
            }
        }

        /// <summary>
        /// метод, который позволяет перемещать гиперболу по графику
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

                DrawGraphic();
                SearchForDangerousRisks();
            }
        }

        private async Task SetUsers()
        {
            List<User> listUsers = await new UserActions().ShowUsers();

            for (int i = 0; i < listUsers.Count; i++)
            {
                UserNewCombobox.Items.Add(listUsers[i]);
            }
        }

        /// <summary>
        /// метод, который выводит сообщение с информацией о риске при нажатии на вершину на графике
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (listRisksSelected.Items.Count != 0)
            {
                List<Risk> listRisks = new List<Risk>();

                for (int i = 0; i < listRisksSelected.Items.Count; i++)
                {
                    double _x = Math.Abs(((Risk)listRisksSelected.Items[i]).point.X - e.GetPosition(null).X);
                    double _y = Math.Abs(((Risk)listRisksSelected.Items[i]).point.Y - e.GetPosition(null).Y);

                    if (_x <= 10 && _y <= 10)
                        listRisks.Add((Risk)listRisksSelected.Items[i]);
                }

                if (listRisks.Count != 0)
                    MessageBox.Show(CreateLine(listRisks), "Information");
            }
        }

        /// <summary>
        /// метод для удаления рискапри нажатии кнопки Delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseActions databaseActions = new DatabaseActions();
            Risk risk = (Risk)((Button)sender).DataContext;

            listSelected.Remove(risk);

            if (listSelected == null) 
                listSelected = new List<Risk>();

            listRisksSelected.Items.Remove(risk);
            risk.Status = 2;

            SearchForCurrentRisk(risk);

            await databaseActions.ChangeRisk(risk);

            listRisksNonselected.Items.Add(risk);
            DrawGraphic();
        }

        /// <summary>
        /// метод, который закрывает текущее окно и открывает окно
        /// выбора проектов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            SelectionWindow selectWindow = new SelectionWindow(user);

            Close();
            selectWindow.Show();
        }

        /// <summary>
        /// метод, который при двойном нажатии мыши на риск в разделе опаных
        /// рисков закрывает текущее окно и открывает окно дерева рисков
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DangerousListRisks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(listDangerous.SelectedItem!=null&& flag1)
            {
                RiskManagerTree riskTree = new RiskManagerTree((Risk)listDangerous.SelectedItem, project, user, center);

                Close();
                riskTree.Show();
                flag1 = false;
            }
        }

        /// <summary>
        /// метод, который создаёт строку с информацией о риске 
        /// </summary>
        /// <param name="click"></param>
        /// <returns></returns>
        private string CreateLine(List<Risk> listRisks)
        {
            string line = "";

            for (int i = 0; i < listRisks.Count; i++)
                line += $"Name of Risk: {listRisks[i].RiskName}\nSource: {listRisks[i].Source}\nType: {listRisks[i].Type}\nEffects: {listRisks[i].Effects}\nDescription: {listRisks[i].Solution}";

            return line;
        }

        /// <summary>
        /// метод ищет риск по его номеру в списке выбранных рисков
        /// </summary>
        /// <param name="risk"></param>
        private void SearchForCurrentRisk(Risk risk)
        {
            for (int i = 0; i < listSelected.Count; i++)
            {
                if (listSelected[i].ID == risk.ID)
                {
                    listSelected[i].Status = risk.Status;
                    listSelected[i].IdUser = risk.IdUser;
                    listSelected[i].OwnerLogin = risk.OwnerLogin;
                }
            }
        }

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
                            listRisksSelected.Items.Add(listSelected[i]);
                    }
                }
            }
        }

        /// <summary>
        /// метод, который отрисовывает гиперболу и вершины
        /// </summary>
        private void DrawGraphic()
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
                if (((Risk)listRisksSelected.Items[i]).Probability != default(Double) &&
                    ((Risk)listRisksSelected.Items[i]).Influence != default(Double) && ((Risk)listRisksSelected.Items[i]).Status == 1)
                {
                    ((Risk)listRisksSelected.Items[i]).point.X = 425 * ((Risk)listRisksSelected.Items[i]).Probability + 75;
                    ((Risk)listRisksSelected.Items[i]).point.Y = -350 * ((Risk)listRisksSelected.Items[i]).Influence + 400;

                    Ellipse ellipse = new Ellipse();

                    ellipse.Height = 10;
                    ellipse.Width = 10;
                    ellipse.StrokeThickness = 2;

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

                    ellipse.VerticalAlignment = VerticalAlignment.Top;
                    ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                    ellipse.Margin = new Thickness(((Risk)listRisksSelected.Items[i]).point.X, ((Risk)listRisksSelected.Items[i]).point.Y, 0, 0);

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
                throw new Exception("There's no such point");

            return (-b + Math.Sqrt(d)) / 2 * a;
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
                    double c = Math.Sqrt((((Risk)listRisksSelected.Items[i]).point.X - center.X) *
                                         (((Risk)listRisksSelected.Items[i]).point.X - center.X) +
                                         (((Risk)listRisksSelected.Items[i]).point.Y - center.Y) *
                                         (((Risk)listRisksSelected.Items[i]).point.Y - center.Y));
                    if (c < radius)
                        listDangerous.Items.Add((Risk)listRisksSelected.Items[i]);
                }
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
