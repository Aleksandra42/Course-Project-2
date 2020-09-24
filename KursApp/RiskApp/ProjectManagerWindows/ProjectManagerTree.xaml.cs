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
    public partial class ProjectManagerTree : Window
    {
        bool flag = true;
        Risk risk = null;

        Project project;

        double Widht;
        new double Height;
        Vertex _first;

        List<double> listValues = new List<double>();
        List<Vertex> listVertex = new List<Vertex>();

        string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "back.jpg");
        string pathToPlus = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plus.png");

        User user = null;
        Point center;

        public ProjectManagerTree(Risk risk, Project project,User user,Point center)
        {
            this.center = center;
            this.user = user;
            this.risk = risk;
            this.project = project;
            InitializeComponent();
        }

        private async void AddNewVertexButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((Button)sender).DataContext == null) 
                    throw new Exception("You need to choose vertex!");

                if (DescriptionTextbox.Text == "") 
                        throw new Exception("You have to put a description!");
                
                if (!double.TryParse(CostTextbox.Text, out double d) || d <= 0) 
                     throw new Exception("The value of cost must be more than 0!");
                
                if (!double.TryParse(ProbabilityTextbox.Text, out double d1) || d1 <= 0 || d1 >= 1) 
                    throw new Exception("The value of probability must be in the interval (0;1)!");
                
                Vertex parentVertex = ((Vertex)((Button)sender).DataContext);

                int row = 1;
                GetCurrenRow(parentVertex, ref row);
                
                if (parentVertex.Probability != 0) 
                    row++;
                
                if (row >= 4) 
                    throw new ArgumentException("The tree branch cannot be more than 4!");
                
                Vertex vertex;
                string line = $"{(parentVertex.X - Widht / (2 * Math.Pow(4, row))):f3}";
                
                if (CheckIfAlreadyExists(double.Parse(line)))
                    vertex = new Vertex(parentVertex.X - Widht / (2 * Math.Pow(4, row)), parentVertex.Y + 50, double.Parse(CostTextbox.Text), double.Parse(ProbabilityTextbox.Text), parentVertex.ID, DescriptionTextbox.Text);
                else
                {
                    line = $"{(parentVertex.X + Widht / (2 * Math.Pow(4, row))):f3}";
                    
                    if (CheckIfAlreadyExists(double.Parse(line)))
                        vertex = new Vertex( parentVertex.X + Widht / (2 * Math.Pow(4, row)), parentVertex.Y + 50, double.Parse(CostTextbox.Text), double.Parse(ProbabilityTextbox.Text), parentVertex.ID, DescriptionTextbox.Text);
                    else
                    {
                        line = $"{(parentVertex.X - 3 * Widht / (2 * Math.Pow(4, row))):f3}";
                        if (CheckIfAlreadyExists(double.Parse(line)))
                        {
                            vertex = new Vertex(parentVertex.X - 3 * Widht / (2 * Math.Pow(4, row)), parentVertex.Y + 50, double.Parse(CostTextbox.Text), double.Parse(ProbabilityTextbox.Text), parentVertex.ID, DescriptionTextbox.Text);
                        }
                        else
                        {
                            line = $"{(parentVertex.X + 3 * Widht / (2 * Math.Pow(4, row))):f3}";
                            
                            if (CheckIfAlreadyExists(double.Parse(line)))
                                vertex = new Vertex(parentVertex.X + 3 * Widht / (2 * Math.Pow(4, row)), parentVertex.Y + 50, double.Parse(CostTextbox.Text), double.Parse(ProbabilityTextbox.Text), parentVertex.ID, DescriptionTextbox.Text);
                            else
                                throw new Exception("The amount of children cannot be more than 4!");
                        }
                    }
                }
                
                Tree tree = new Tree();
                
                await tree.AddVertex(parentVertex.ID, vertex);
                vertex = await tree.ShowVertex(vertex);
                listVertex.Add(vertex);

                RefreshTree(_first);
                Clear();
                CurrentBranchCost(_first, 0);
                DrawDangerousMaximum();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// метод рисует линии соединяющие точки графа
        /// </summary>
        /// <param name="newver"></param>
        /// <param name="parent"></param>
        private void DrawNewLine(Vertex vertex, Vertex parentVertex)
        {
            Line line = new Line();
            line.X1 = parentVertex.X;
            line.Y1 = parentVertex.Y + 20;
            line.X2 = vertex.X;
            line.Y2 = vertex.Y;
            line.Stroke = Brushes.Black;
            canvas.Children.Add(line);
        }

        /// <summary>
        /// метод рисует вершины
        /// </summary>
        /// <param name="current"></param>
        private void DrawNewVertex(Vertex current)
        {
            Button button = new Button();
            button.DataContext = current;
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.VerticalAlignment = VerticalAlignment.Top;
            button.Margin = new Thickness(current.X - 10, current.Y, Widht - current.X - 10, Height - current.Y - 20);
            button.Height = 20;
            button.Width = 20;
            button.Background = new ImageBrush(new BitmapImage(new Uri(pathToPlus)));
            button.Click += But_Click;
            canvas.Children.Add(button);
        }

        /// <summary>
        /// метод для очистки значений вершин
        /// </summary>
        private void Clear()
        {
            for (int i = 0; i < listVertex.Count; i++)
                listVertex[i].Value = default;
        }

        private void RefreshTree(Vertex cur)
        {
            canvas.Children.Clear();
            Button but = new Button();
            but.HorizontalAlignment = HorizontalAlignment.Left;
            but.VerticalAlignment = VerticalAlignment.Top;
            but.Margin = new Thickness(Widht / 2 - 10, 50, Widht / 2 - 10, Height - 70);
            but.Background = new ImageBrush(new BitmapImage(new Uri(pathToPlus)));
            Back.Background = new ImageBrush(new BitmapImage(new Uri(path)));
            but.DataContext = _first;
            but.Height = 20;
            but.Width = 20;
            but.Click += But_Click;
            canvas.Children.Add(but);
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (listVertex[i].IDParent == cur.ID && listVertex[i].Probability != default(double))
                {
                    DrawNewVertex(listVertex[i]);
                    DrawNewLine(listVertex[i], cur);
                    DrawVertexesByRoot(listVertex[i]);
                }
            }
        }

        /// <summary>
        /// метод для проверки на наличие вершины
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private bool CheckIfAlreadyExists(double x)
        {
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (x == listVertex[i].X)
                    return false;
            }
            
            return true;
        }

        /// <summary>
        /// метод для расчёта стоимости ветки
        /// </summary>
        /// <param name="currentVertex"></param>
        /// <param name="c"></param>
        private void CurrentBranchCost(Vertex currentVertex, double c)
        {
            double cost = c;
            
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (currentVertex.ID == listVertex[i].IDParent && listVertex[i].Probability != default)
                {
                    cost += listVertex[i].Probability * listVertex[i].Cost;
                    CurrentBranchCost(listVertex[i], cost);
                    cost -= listVertex[i].Probability * listVertex[i].Cost;
                }
            }
            
            if (CheckIfChildExists(currentVertex))
                currentVertex.Value = cost;
        }

        /// <summary>
        /// метод для проверки наличия ребенка у вершины
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private bool CheckIfChildExists(Vertex current)
        {
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (current.ID == listVertex[i].IDParent && listVertex[i].Probability != default)
                    return false;
            }
            
            return true;
        }


        /// <summary>
        /// метод выдает ряд дерева
        /// </summary>
        /// <param name="parentVertex"></param>
        /// <param name="k"></param>
        private void GetCurrenRow(Vertex parentVertex, ref int k)
        {
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (parentVertex.IDParent == listVertex[i].ID && listVertex[i].Probability == default)
                    return;
                    
                if (parentVertex.IDParent == listVertex[i].ID && listVertex[i].Probability != default)
                {
                    k++;
                    GetCurrenRow(listVertex[i], ref k);
                }
            }
        }


        private async void Window_Activated(object sender, EventArgs e)
        {
            if (flag)
            {
                try
                {
                    Widht = canvas.ActualWidth;
                    Height = canvas.ActualHeight;
                    
                    Label label = new Label();
                    label.HorizontalAlignment = HorizontalAlignment.Center;
                    label.VerticalAlignment = VerticalAlignment.Top;
                    label.Content = risk.RiskName;
                    
                    grid.Children.Add(label);
                    
                    Tree tree = new Tree();
                    Button button = new Button();
                    
                    button.HorizontalAlignment = HorizontalAlignment.Left;
                    button.VerticalAlignment = VerticalAlignment.Top;
                    button.Margin = new Thickness(Widht / 2 - 10, 50, Widht / 2 - 10, Height - 70);
                    button.Background = new ImageBrush(new BitmapImage(new Uri(pathToPlus)));
                    
                    Back.Background = new ImageBrush(new BitmapImage(new Uri(path)));
                    Back.Foreground = new ImageBrush(new BitmapImage(new Uri(path)));
                    
                    if (!await tree.CheckIfExistInDataBase(risk.ID))
                    {
                        _first = new Vertex(Widht / 2, 50, risk.ID, risk.RiskName);
                        await tree.AddVertex(risk.ID, _first);
                        
                        _first = await tree.ShowVertex(risk.ID);
                        button.DataContext = _first;
                    }
                    else
                    {
                        _first = await tree.ShowVertex(risk.ID);
                        button.DataContext = _first;
                    }
                    
                    button.Height = 20;
                    button.Width = 20;
                    button.Click += But_Click;
                    
                    canvas.Children.Add(button);
                    listVertex = await tree.ShowListVertexes();
                    
                    DrawVertexesByRoot(_first);
                    CurrentBranchCost(_first, 0);
                    DrawDangerousMaximum();
                    
                    flag = false;
                }
                catch (NullReferenceException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// метод выделяет самый опасный риск
        /// </summary>
        private void DrawDangerousMaximum()
        {
            Vertex Maximum = listVertex[0];
            Vertex Minimum = null;
            
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (listVertex[i].Value != 0)
                {
                    Minimum = listVertex[i];
                    
                    for (int j = 0; j < listVertex.Count; j++)
                        if (listVertex[j].Value < Minimum.Value && listVertex[j].Value != 0) 
                            Minimum = listVertex[j];
                    
                    break;
                }

            }
            
            for (int i = 1; i < listVertex.Count; i++)
            {
                if (listVertex[i].Value > Maximum.Value) 
                    Maximum = listVertex[i];
            }
            
            if (Maximum != null)
            {
                Label label = new Label();
                label.Content = Maximum.Value;
                label.Margin = new Thickness(Maximum.X, Maximum.Y + 20, 0, 0);
                label.VerticalAlignment = VerticalAlignment.Top;
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.Height = 40;
                label.Foreground = Brushes.Red;
                
                canvas.Children.Add(label);
                DrawLineRed(Maximum);
            }

            if (Minimum != null)
            {
                Label l1 = new Label();
                l1.Content = Minimum.Value;
                l1.Margin = new Thickness(Minimum.X, Minimum.Y + 20, 0, 0);
                l1.VerticalAlignment = VerticalAlignment.Top;
                l1.HorizontalAlignment = HorizontalAlignment.Left;
                l1.Height = 40;
                l1.Foreground = Brushes.Green;
                canvas.Children.Add(l1);
                DrawLineGreen(Minimum);
            }
        }

        private void DrawLineGreen(Vertex min)
        {
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (min.IDParent == listVertex[i].ID)
                {
                    Line line = new Line();
                    
                    line.X1 = listVertex[i].X;
                    line.Y1 = listVertex[i].Y + 20;
                    line.X2 = min.X;
                    line.Y2 = min.Y;
                    line.Stroke = Brushes.Green;
                    canvas.Children.Add(line);

                    if (listVertex[i].Probability == default) 
                        break;
                    else 
                        DrawLineGreen(listVertex[i]);

                }
            }
        }

        private void DrawLineRed(Vertex max)
        {
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (max.IDParent == listVertex[i].ID)
                {
                    Line line = new Line();
                    
                    line.X1 = listVertex[i].X;
                    line.Y1 = listVertex[i].Y + 20;
                    line.X2 = max.X;
                    line.Y2 = max.Y;

                    line.Stroke = Brushes.Red;
                    canvas.Children.Add(line);

                    if (listVertex[i].Probability == default) 
                        break;
                    else 
                        DrawLineRed(listVertex[i]);
                }
            }
        }

        /// <summary>
        /// рисует дерево при начальной загрузке
        /// </summary>
        /// <param name="current"></param>
        private void DrawVertexesByRoot(Vertex current)
        {
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (listVertex[i].IDParent == current.ID && listVertex[i].Probability != default)
                {
                    DrawNewVertex(listVertex[i]);
                    DrawNewLine(listVertex[i], current);
                    DrawVertexesByRoot(listVertex[i]);
                }
            }
        }

        /// <summary>
        /// нажатие на вершину графа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void But_Click(object sender, RoutedEventArgs e)
        {
            AddButton.DataContext = (Vertex)((Button)sender).DataContext;
            Vertex vertex = (Vertex)AddButton.DataContext;
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectManagerGraphic graphic = new ProjectManagerGraphic(project,user);
            Close();
            graphic.Show();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            Tree tree = new Tree();

            if (((Vertex)AddButton.DataContext).Probability != 0)
            {
                List<Vertex> list = DeleteVertexes((Vertex)AddButton.DataContext);
                list.Add((Vertex)AddButton.DataContext);
                
                await tree.DeleteVertex(list);
                listVertex = await tree.ShowListVertexes();
            }

            Button buttonAdd = new Button();

            buttonAdd.HorizontalAlignment = HorizontalAlignment.Left;
            buttonAdd.VerticalAlignment = VerticalAlignment.Top;
            buttonAdd.Margin = new Thickness(Widht / 2 - 10, 50, Widht / 2 - 10, Height - 70);
            buttonAdd.Background = new ImageBrush(new BitmapImage(new Uri(pathToPlus)));
            Back.Background = new ImageBrush(new BitmapImage(new Uri(path)));
            buttonAdd.DataContext = _first;
            buttonAdd.Height = 20;
            buttonAdd.Width = 20;
            buttonAdd.Click += But_Click;

            canvas.Children.Add(buttonAdd);
            DrawVertexesByRoot(_first);
            CurrentBranchCost(_first, 0);
            DrawDangerousMaximum();
        }

        /// <summary>
        /// метод для удаления вершины из списка
        /// </summary>
        /// <param name="currentVertex"></param>
        /// <returns></returns>
        private List<Vertex> DeleteVertexes(Vertex currentVertex)
        {
            List<Vertex> listDelete = new List<Vertex>();

            for (int i = 0; i < listVertex.Count; i++)
            {
                if (listVertex[i].Probability != 0 && listVertex[i].IDParent == currentVertex.ID)
                    listDelete.Add(listVertex[i]);
            }
            for (int i = 0; i < listVertex.Count; i++)
            {
                for (int j = 0; j < listDelete.Count; j++)
                {
                    if (listVertex[i].Probability != 0 && listVertex[i].IDParent == listDelete[j].ID)
                        listDelete.Add(listVertex[i]);
                }
            }

            return listDelete;
        }

        private void GoReportButton_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow window = new ReportWindow(risk, project, center);
            Close();
            window.Show();
        }
    }
}
