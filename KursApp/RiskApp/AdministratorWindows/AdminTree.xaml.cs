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
    public partial class AdminTree : Window
    {
        bool flag = true;

        double Width;
        new double Height;

        string pathToBack = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "back.jpg");
        string pathToPlus = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plus.png");

        Point center;

        Risk risk = null;
        Project project;
        Vertex _vertexFirst;

        //List<double> listValues = new List<double>();
        List<Vertex> listVertex = new List<Vertex>();
        public AdminTree(Risk risk, Project project,Point center)
        {
            this.center = center;
            this.risk = risk;
            this.project = project;

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
                try
                {
                    Label label = new Label();
                    Tree tree = new Tree();
                    Button button = new Button();

                    Width = canvas.ActualWidth;
                    Height = canvas.ActualHeight;

                    label.HorizontalAlignment = HorizontalAlignment.Center;
                    label.VerticalAlignment = VerticalAlignment.Top;
                    label.Content = risk.RiskName;
                    grid.Children.Add(label);

                    button.HorizontalAlignment = HorizontalAlignment.Left;
                    button.VerticalAlignment = VerticalAlignment.Top;
                    button.Margin = new Thickness(Width / 2 - 10, 50, Width / 2 - 10, Height - 70);
                    button.Background = new ImageBrush(new BitmapImage(new Uri(pathToPlus)));

                    BackButton.Background = new ImageBrush(new BitmapImage(new Uri(pathToBack)));
                    BackButton.Foreground = new ImageBrush(new BitmapImage(new Uri(pathToBack)));

                    if (!await tree.CheckIfExistInDataBase(risk.ID))
                    {
                        _vertexFirst = new Vertex(Width / 2, 50, risk.ID, risk.RiskName);
                        await tree.AddVertex(risk.ID, _vertexFirst);
                        _vertexFirst = await tree.ShowVertex(risk.ID);
                        button.DataContext = _vertexFirst;
                    }
                    else
                    {
                        _vertexFirst = await tree.ShowVertex(risk.ID);
                        button.DataContext = _vertexFirst;
                    }

                    button.Height = 20;
                    button.Width = 20;
                    button.Click += Button_Click;

                    canvas.Children.Add(button);
                    listVertex = await tree.ShowListVertexes();

                    DrawRoot(_vertexFirst);
                    CurrentBranchCost(_vertexFirst, 0);
                    DrawDangerousMaximum();

                    flag = false;
                }
                catch (NullReferenceException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// метод, который добавляет новую вершину с установленными характеристиками
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((Button)sender).DataContext == null)
                    throw new Exception("You need to choose the top!");

                if (TextboxDescription.Text == "") 
                    throw new Exception("You must fill the field 'Description'!");

                if (!double.TryParse(TextboxCost.Text, out double d) || d <= 0) 
                    throw new Exception("The value of 'Cost' can only be of Double type and it must be more than 0!");

                if (!double.TryParse(TextboxProbability.Text, out double d1) || d1 > 1 || d1 < 0) 
                    throw new Exception("The value of 'Probability' can only be of Double type and it must be in the interval [0, 1]!");

                Vertex parentVertex = ((Vertex)((Button)sender).DataContext);

                int row = 1;

                SearchForRow(parentVertex, ref row);

                if (parentVertex.Probability != 0) 
                    row++;

                if (row >= 4) 
                    throw new ArgumentException("Branches cannot be more than 4!");

                Vertex currentVertex;
                string line = $"{(parentVertex.X - Width / (2 * Math.Pow(4, row))):f3}";

                if (CheckIfInList(double.Parse(line)))
                    currentVertex = new Vertex(parentVertex.X - Width / (2 * Math.Pow(4, row)), parentVertex.Y + 50, double.Parse(TextboxCost.Text), 
                        double.Parse(TextboxProbability.Text), parentVertex.ID, TextboxDescription.Text);
                else
                {
                    line = $"{(parentVertex.X + Width / (2 * Math.Pow(4, row))):f3}";

                    if (CheckIfInList(double.Parse(line)))
                        currentVertex = new Vertex(parentVertex.X + Width / (2 * Math.Pow(4, row)), parentVertex.Y + 50, double.Parse(TextboxCost.Text), double.Parse(TextboxProbability.Text), 
                            parentVertex.ID, TextboxDescription.Text);
                    else
                    {
                        line = $"{(parentVertex.X - 3 * Width / (2 * Math.Pow(4, row))):f3}";

                        if (CheckIfInList(double.Parse(line)))
                            currentVertex = new Vertex(parentVertex.X - 3 * Width / (2 * Math.Pow(4, row)), parentVertex.Y + 50, 
                                double.Parse(TextboxCost.Text), double.Parse(TextboxProbability.Text), parentVertex.ID, TextboxDescription.Text);
                        else
                        {
                            line = $"{(parentVertex.X + 3 * Width / (2 * Math.Pow(4, row))):f3}";

                            if (CheckIfInList(double.Parse(line)))
                                currentVertex = new Vertex(parentVertex.X + 3 * Width / (2 * Math.Pow(4, row)), parentVertex.Y + 50, double.Parse(TextboxCost.Text), double.Parse(TextboxProbability.Text), parentVertex.ID, TextboxDescription.Text);
                            else
                                throw new Exception("The amount of children cannot be more than 4!");
                        }
                    }
                }

                Tree tree = new Tree();

                await tree.AddVertex(parentVertex.ID, currentVertex);
                currentVertex = await tree.ShowVertex(currentVertex);
                listVertex.Add(currentVertex);
                
                RefreshTree(_vertexFirst);

                for (int i = 0; i < listVertex.Count; i++)
                    listVertex[i].Value = default;

                CurrentBranchCost(_vertexFirst, 0);
                DrawDangerousMaximum();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Empty Exception");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Empty Exception");
            }
        }

        /// <summary>
        /// метод для 'обновления' дерева
        /// добавляются, либо удаляются элементы
        /// </summary>
        /// <param name="currentVertex"></param>
        private void RefreshTree(Vertex currentVertex)
        {
            canvas.Children.Clear();
            Button button = new Button();

            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.VerticalAlignment = VerticalAlignment.Top;
            button.Margin = new Thickness(Width / 2 - 10, 50, Width / 2 - 10, Height - 70);

            button.Background = new ImageBrush(new BitmapImage(new Uri(pathToPlus)));
            BackButton.Background = new ImageBrush(new BitmapImage(new Uri(pathToBack)));

            button.DataContext = _vertexFirst;
            button.Height = 20;
            button.Width = 20;
            button.Click += Button_Click;

            canvas.Children.Add(button);

            for (int i = 0; i < listVertex.Count; i++)
            {
                if (listVertex[i].IDParent == currentVertex.ID && listVertex[i].Probability != default)
                {
                    AddNewVertexToTree(listVertex[i]);
                    DrawNewLine(listVertex[i], currentVertex);
                    DrawRoot(listVertex[i]);
                }
            }
        }

        /// <summary>
        /// метод проверяет по значению вершины x, находится ли такой же элемент в списке
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private bool CheckIfInList(double x)
        {
            for (int i = 0; i < listVertex.Count; i++)
                if (x == listVertex[i].X)
                    return false;

            return true;
        }

        /// <summary>
        /// метод для установления стоимости ветки
        /// </summary>
        /// <param name="curver"></param>
        /// <param name="k"></param>
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
        /// метод, котоый проверяет, есть ли у вершины ребенок
        /// </summary>
        /// <param name="currentVertex"></param>
        /// <returns></returns>
        private bool CheckIfChildExists(Vertex currentVertex)
        {
            for (int i = 0; i < listVertex.Count; i++)
                if (currentVertex.ID == listVertex[i].IDParent && listVertex[i].Probability != default)
                    return false;
            
            return true;
        }


        /// <summary>
        /// метод, который возвращает ряд дерева
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="k"></param>
        private void SearchForRow(Vertex parentVertex, ref int k)
        {
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (parentVertex.IDParent == listVertex[i].ID && listVertex[i].Probability == default)
                    return;

                if (parentVertex.IDParent == listVertex[i].ID && listVertex[i].Probability != default)
                {
                    k++;
                    SearchForRow(listVertex[i], ref k);
                }
            }
        }

        /// <summary>
        /// метод, который рисует линии между вершинами
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
        /// метод, который добавляет новые вершины
        /// </summary>
        /// <param name="newver"></param>
        private void AddNewVertexToTree(Vertex vertex)
        {
            Button plusButton = new Button();

            plusButton.DataContext = vertex;
            plusButton.HorizontalAlignment = HorizontalAlignment.Left;
            plusButton.VerticalAlignment = VerticalAlignment.Top;
            plusButton.Margin = new Thickness(vertex.X - 10, vertex.Y, Width - vertex.X - 10, Height - vertex.Y - 20);
            plusButton.Height = 20;
            plusButton.Width = 20;
            plusButton.Background = new ImageBrush(new BitmapImage(new Uri(pathToPlus)));
            plusButton.Click += Button_Click;

            canvas.Children.Add(plusButton);
        }

        /// <summary>
        /// метод, который находит наиболее опасный рис и рисует его
        /// </summary>
        private void DrawDangerousMaximum()
        {
            Vertex vertexMaximum = listVertex[0];
            Vertex vertexMinimum = null;

            for (int i = 0; i < listVertex.Count; i++)
            {
                if (listVertex[i].Value != 0)
                {
                    vertexMinimum = listVertex[i];

                    for (int j = 0; j < listVertex.Count; j++)
                    {
                        if (listVertex[j].Value < vertexMinimum.Value && listVertex[j].Value != 0)
                            vertexMinimum = listVertex[j];
                    }

                    break;
                }
            }

            for (int i = 1; i < listVertex.Count; i++)
            {
                if (listVertex[i].Value > vertexMaximum.Value) 
                    vertexMaximum = listVertex[i];
            }

            if (vertexMaximum != null)
            {
                Label label = new Label();

                label.Content = vertexMaximum.Value;
                label.Margin = new Thickness(vertexMaximum.X, vertexMaximum.Y + 20, 0, 0);
                label.VerticalAlignment = VerticalAlignment.Top;
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.Height = 40;
                label.Foreground = Brushes.Red;

                canvas.Children.Add(label);
                DrawLineRed(vertexMaximum);
            }

            if (vertexMinimum != null)
            {
                Label label1 = new Label();

                label1.Content = vertexMinimum.Value;
                label1.Margin = new Thickness(vertexMinimum.X, vertexMinimum.Y + 20, 0, 0);
                label1.VerticalAlignment = VerticalAlignment.Top;
                label1.HorizontalAlignment = HorizontalAlignment.Left;
                label1.Height = 40;
                label1.Foreground = Brushes.Green;

                canvas.Children.Add(label1);
                DrawLineGreen(vertexMinimum);
            }
        }

        private void DrawLineGreen(Vertex minimumVertex)
        {
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (minimumVertex.IDParent == listVertex[i].ID)
                {
                    Line line = new Line();
                    line.X1 = listVertex[i].X;
                    line.Y1 = listVertex[i].Y + 20;
                    line.X2 = minimumVertex.X;
                    line.Y2 = minimumVertex.Y;
                    line.Stroke = Brushes.Green;
                    canvas.Children.Add(line);

                    if (listVertex[i].Probability == default) 
                        break;
                    else 
                        DrawLineGreen(listVertex[i]);

                }
            }
        }

        private void DrawLineRed(Vertex maximumVertexes)
        {
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (maximumVertexes.IDParent == listVertex[i].ID)
                {
                    Line l = new Line();
                    l.X1 = listVertex[i].X;
                    l.Y1 = listVertex[i].Y + 20;
                    l.X2 = maximumVertexes.X;
                    l.Y2 = maximumVertexes.Y;

                    l.Stroke = Brushes.Red;
                    canvas.Children.Add(l);

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
        /// <param name="currentVertexes"></param>
        private void DrawRoot(Vertex currentVertexes)
        {
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (listVertex[i].IDParent == currentVertexes.ID && listVertex[i].Probability != default)
                {
                    AddNewVertexToTree(listVertex[i]);
                    DrawNewLine(listVertex[i], currentVertexes);
                    DrawRoot(listVertex[i]);
                }
            }
        }

        /// <summary>
        /// метод срабатывает при нажатии на вершину графа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddButton.DataContext = (Vertex)((Button)sender).DataContext;
            Vertex vertex = (Vertex)AddButton.DataContext;

           
        }

        /// <summary>
        /// метод для удаления вершины из дерева 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            Tree tree = new Tree();

            if (((Vertex)AddButton.DataContext).Probability != 0)
            {
                List<Vertex> listDelete = DeleteVertexes((Vertex)AddButton.DataContext);
                listDelete.Add((Vertex)AddButton.DataContext);
                await tree.DeleteVertex(listDelete);
                listVertex = await tree.ShowListVertexes();
            }

            Button button = new Button();

            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.VerticalAlignment = VerticalAlignment.Top;
            button.Margin = new Thickness(Width / 2 - 10, 50, Width / 2 - 10, Height - 70);
            button.Background = new ImageBrush(new BitmapImage(new Uri(pathToPlus)));

            BackButton.Background = new ImageBrush(new BitmapImage(new Uri(pathToBack)));

            button.DataContext = _vertexFirst;
            button.Height = 20;
            button.Width = 20;
            button.Click += Button_Click;

            canvas.Children.Add(button);

            DrawRoot(_vertexFirst);
            CurrentBranchCost(_vertexFirst, 0);
            DrawDangerousMaximum();
        }

        /// <summary>
        /// метод для удаления вершин
        /// </summary>
        /// <param name="currentvertex"></param>
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

        /// <summary>
        /// метод, который срабатывает при нажатии на кнопку Create Report,
        /// закрывает окно дерева, открывает окно отчёта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoToReport_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow(risk, project, center);
            Close();
            reportWindow.Show();
        }

        /// <summary>
        /// метод срабатывает при нажатии кнопки Back
        /// закрывается окно с деревом, снова открывается окно с графиком
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdministratorGraphic graphicWindow = new AdministratorGraphic(project);
            Close();
            graphicWindow.Show();
        }
    }
}
