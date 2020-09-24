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
using Word = Microsoft.Office.Interop.Word;

namespace RiskApp
{
    public partial class ReportWindow : Window
    {
        const int K = 100;
        const double radius = 250;
        
        bool flag = true;
        double Widht;
        new double Height;
        
        Risk _risk = null;
        Project _project = null;
        List<Risk> listSelected = null;
        List<Vertex> listVertex = null;
        Vertex firstVertex;
        
        private Point center;

        object offMissing = System.Reflection.Missing.Value;
        object oEndOfDoc = "\\endofdoc";

        Word.Application wordApplication = new Word.Application();
        Word.Document oDoc;

        public ReportWindow(Risk risk,Project project, System.Windows.Point center)
        {
            this.center = center;
            _risk = risk;
            _project = project;
            
            InitializeComponent();
        }

        /// <summary>
        /// метод выполняеся при открытии окна отчёта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_Activated(object sender, EventArgs e)
        {
            if(flag)
            {
                Widht = canvas.ActualWidth;
                Height = canvas.ActualHeight;

                DatabaseActions databaseActions = new DatabaseActions();
                listSelected = await databaseActions.ShowRisks(_project);
                    
                if(listSelected == null)
                    listSelected = new List<Risk>();
                
                DrawLine();
                
                Tree tree = new Tree();
                
                listVertex = await tree.ShowListVertexes();
                firstVertex = await tree.ShowVertex(_risk.ID);
                
                Label label = new Label();
                
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.VerticalAlignment = VerticalAlignment.Top;
                label.Margin = new Thickness(Widht/2-100, 0, 0, 0);
                label.FontSize = 17;
                label.Content = $"Report for {_project.Name}";
                
                canvas.Children.Add(label);
                
                DrawVertex();
                CurrentBranchCost(firstVertex, 0);
                DrawDangerousMaximum();
                WriteDangerousItems();
                
                flag = false;

                //wordApplication.Visible = true;
                CreateDocument();
            }
        }

        /// <summary>
        /// метод для добавления в документ таблицы опасных рисков
        /// </summary>
        private void CreateDocument()
        {
            oDoc = wordApplication.Documents.Add(ref offMissing, ref offMissing,
                    ref offMissing, ref offMissing);

            Word.Paragraph para1;
            para1 = oDoc.Content.Paragraphs.Add(ref offMissing);
            para1.Range.Text = $"Report for {_risk} in {_project}";
            para1.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            para1.Range.Font.Bold = 1;
            para1.Format.SpaceAfter = 12;
            para1.Range.InsertParagraphAfter();

            Word.Table oTable;
            Word.Range wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            oTable = oDoc.Tables.Add(wrdRng, listDangerous.Items.Count + 1, 5, ref offMissing, ref offMissing);
            oTable.Range.ParagraphFormat.SpaceAfter = 2;

            string strText;
            oTable.Range.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            oTable.Range.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            //   oTable.Borders.InsideLineWidth = Word.WdLineWidth.wdLineWidth050pt;
            // oTable.Range.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleDashDot;
            oTable.Cell(1, 1).Range.Text = "Name of Risk";
            oTable.Cell(1, 2).Range.Text = "Source";
            oTable.Cell(1, 3).Range.Text = "Possible Solution";
            oTable.Cell(1, 4).Range.Text = "Effects";
            oTable.Cell(1, 5).Range.Text = "Rank";

            for (int r = 2, i = 0; r <= listDangerous.Items.Count + 1; r++, i++)
            {
                strText = System.Convert.ToString(((Risk)listDangerous.Items[i]).RiskName);
                oTable.Cell(r, 1).Range.Text = strText;

                strText = System.Convert.ToString(((Risk)listDangerous.Items[i]).Source);
                oTable.Cell(r, 2).Range.Text = strText;

                strText = System.Convert.ToString(((Risk)listDangerous.Items[i]).Solution);
                oTable.Cell(r, 3).Range.Text = strText;

                strText = System.Convert.ToString(((Risk)listDangerous.Items[i]).Effects);
                oTable.Cell(r, 4).Range.Text = strText;

                strText = System.Convert.ToString(((Risk)listDangerous.Items[i]).Rank);
                oTable.Cell(r, 5).Range.Text = strText;
            }
        }

        /// <summary>
        /// метод, который выводит в MessageBox информацию о выбранном на графике риске
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (listDangerous.Items.Count != 0)
            {
                List<Risk> list = new List<Risk>();

                for (int i = 0; i < listDangerous.Items.Count; i++)
                {
                    double _x = Math.Abs(((Risk)listDangerous.Items[i]).point.X - e.GetPosition(null).X);
                    double _y = Math.Abs(((Risk)listDangerous.Items[i]).point.Y - e.GetPosition(null).Y);

                    if (_x <= 10 && _y <= 10)
                        list.Add((Risk)listDangerous.Items[i]);
                }

                if (list.Count != 0)
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
                line += $"Name of Risk: {listSelected[i].RiskName}\nSource: {listSelected[i].Source}\nType: {listSelected[i].Type}\nEffects: {listSelected[i].Effects}\nPossible Solution: {listSelected[i].Solution}";

            return line;
        }

        /// <summary>
        /// метод составляет список опасных объектов
        /// </summary>
        private void WriteDangerousItems()
        {
            for (int i = 0; i < listSelected.Count; i++)
            {
                double m = Math.Sqrt((listSelected[i].point.X - center.X) * (listSelected[i].point.X - center.X) +
                                     (listSelected[i].point.Y - center.Y) * (listSelected[i].point.Y - center.Y));
                
                if ((listSelected[i].Status == 1) && (m < radius))
                    listDangerous.Items.Add(listSelected[i]);
            }
        }

        /// <summary>
        /// метод создаёт первую вершину графика
        /// </summary>
        private void DrawVertex()
        {
            Point point = new Point(firstVertex.X * 3 / 2, firstVertex.Y);
            Label label = new Label();
            
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.FontSize = 17;
            label.Margin = new Thickness(point.X - 200, point.Y + 200, 0, 0);
            label.Content = _risk.RiskName;
            
            canvas.Children.Add(label);

            Ellipse ellipse = new Ellipse();

            ellipse.Width = 4.5;
            ellipse.Height = 4.5;
            ellipse.HorizontalAlignment = HorizontalAlignment.Left;
            ellipse.VerticalAlignment = VerticalAlignment.Top;
            ellipse.StrokeThickness = 3;
            ellipse.Stroke = Brushes.Black;
            ellipse.Margin = new Thickness(point.X - 2, point.Y - 2, 0, 0);
            
            canvas.Children.Add(ellipse);
            
            DrawByRoot(firstVertex);
        }
        
        /// <summary>
        /// метод добавляет вершины по корню
        /// </summary>
        /// <param name="currentVertex"></param>
        private void DrawByRoot(Vertex currentVertex)
        {
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (listVertex[i].Probability != default && listVertex[i].IDParent == currentVertex.ID)
                {
                    DrawNewVertex(listVertex[i]);
                    AddLine(listVertex[i], currentVertex);
                    DrawByRoot(listVertex[i]);
                }
            }
        }

        /// <summary>
        /// метод для добавления новой вершины
        /// </summary>
        /// <param name="vertex"></param>
        private void DrawNewVertex(Vertex vertex)
        {
            Point point = new Point(vertex.X, vertex.Y);
            Ellipse ellipse = new Ellipse();

            ellipse.Width = 6.5;
            ellipse.Height = 6.5;
            ellipse.HorizontalAlignment = HorizontalAlignment.Left;
            ellipse.VerticalAlignment = VerticalAlignment.Top;
            ellipse.StrokeThickness = 3;
            ellipse.Stroke = Brushes.Black;
            ellipse.Margin = new Thickness(vertex.X / 2 + Widht / 2 - 3, vertex.Y -3, 0, 0);
            
            canvas.Children.Add(ellipse);
        }

        /// <summary>
        /// метод для добавления новой линии
        /// </summary>
        /// <param name="newver"></param>
        /// <param name="parent"></param>
        private void AddLine(Vertex vertex, Vertex parentVertex)
        {
            Line line = new Line();
            
            line.X1 = parentVertex.X / 2 + Widht / 2;
            line.Y1 = parentVertex.Y;
            line.X2 = vertex.X / 2 + Widht / 2;
            line.Y2 = vertex.Y ;
            line.Stroke = Brushes.Black;
            
            canvas.Children.Add(line);
        }
        /// <summary>
        /// метод для подсчёта стоимости ветки
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="c"></param>
        private void CurrentBranchCost(Vertex vertex, double c)
        {
            double cost = c;
            
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (vertex.ID == listVertex[i].IDParent && listVertex[i].Probability != default)
                {
                    cost += listVertex[i].Probability * listVertex[i].Cost;
                    CurrentBranchCost(listVertex[i], cost);
                    cost -= listVertex[i].Probability * listVertex[i].Cost;
                }
            }
            
            if (CheckIfChildExists(vertex))
                vertex.Value = cost;
        }

        /// <summary>
        /// метод , который проверяет, есть ли заданаая вершина в списке вершин
        /// </summary>
        /// <param name="curver"></param>
        /// <returns>false, если в списке уже есть такая вершина, иначе true</returns>
        private bool CheckIfChildExists(Vertex vertex)
        {
            for (int i = 0; i < listVertex.Count; i++)
                if (listVertex[i].Probability != default && vertex.ID == listVertex[i].IDParent)
                    return false;
            
            return true;
        }
        
        /// <summary>
        /// метод, который сортирует вершины, а затем
        /// выделяет самую опасную
        /// </summary>
        private void DrawDangerousMaximum()
        {
            Vertex vertexMax = listVertex[0];
            Vertex vertexMin = null;
            
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (listVertex[i].Value != 0)
                {
                    vertexMin = listVertex[i];
                    
                    for (int j = 0; j < listVertex.Count; j++)
                        if (listVertex[j].Value < vertexMin.Value && listVertex[j].Value != 0) 
                            vertexMin = listVertex[j];
                        
                    break;
                }
            }
            
            for (int i = 1; i < listVertex.Count; i++)
                if (listVertex[i].Value > vertexMax.Value) 
                    vertexMax = listVertex[i];
            
            if (vertexMax != null)
            {
                Label label = new Label();
                
                label.Content = vertexMax.Value;
                label.Margin = new Thickness(vertexMax.X / 2 + Widht / 2, vertexMax.Y + 20, 0, 0);
                label.VerticalAlignment = VerticalAlignment.Top;
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.Height = 40;
                label.Foreground = Brushes.Red;
                
                canvas.Children.Add(label);
                DrawLineRed(vertexMax);
            }

            if (vertexMin != null)
            {
                Label label1 = new Label();
                
                label1.Content = vertexMin.Value;
                label1.Margin = new Thickness(vertexMin.X/2 + Widht / 2, vertexMin.Y + 20, 0, 0);
                label1.VerticalAlignment = VerticalAlignment.Top;
                label1.HorizontalAlignment = HorizontalAlignment.Left;
                label1.Height = 40;
                label1.Foreground = Brushes.Green;
                
                canvas.Children.Add(label1);
                
                DrawLineGreen(vertexMin);
            }
        }

        /// <summary>
        /// метод для отрисовки зелёной линии
        /// </summary>
        /// <param name="min"></param>
        private void DrawLineGreen(Vertex vertexMinimum)
        {
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (vertexMinimum.IDParent == listVertex[i].ID)
                {
                    Line line = new Line();
                    
                    line.X1 = listVertex[i].X/ 2 + Widht / 2;
                    line.Y1 = listVertex[i].Y;
                    line.X2 = vertexMinimum.X / 2 + Widht / 2;
                    line.Y2 = vertexMinimum.Y;
                    line.Width = 12;
                    line.Stroke = Brushes.Green;
                    
                    canvas.Children.Add(line);

                    if (listVertex[i].Probability == default(double)) 
                        break;
                    else 
                        DrawLineGreen(listVertex[i]);

                }
            }
        }

        /// <summary>
        /// метод для отрисовки красной линии
        /// </summary>
        /// <param name="vertexMaximum"></param>
        private void DrawLineRed(Vertex vertexMaximum)
        {
            for (int i = 0; i < listVertex.Count; i++)
            {
                if (vertexMaximum.IDParent == listVertex[i].ID)
                {
                    Line line = new Line();
                    
                    line.X1 = listVertex[i].X / 2 + Widht / 2;
                    line.Y1 = listVertex[i].Y;
                    line.X2 = vertexMaximum.X / 2 + Widht / 2;
                    line.Y2 = vertexMaximum.Y;

                    line.Stroke = Brushes.Red;
                    
                    canvas.Children.Add(line);

                    if (listVertex[i].Probability == default(double)) 
                        break;
                    else 
                        DrawLineRed(listVertex[i]);

                }
            }
        }

        /// <summary>
        /// метод, который вычисляет координату y для точки при помощи вадратного уравнения
        /// </summary>
        /// <param name="x"></param>
        /// <param name="radius"></param>
        /// <param name="center"></param>
        /// <returns>координата y</returns>
        /// <exception cref="Exception"></exception>
        public double FindYCoordinate(double x, double radius, Point center)
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
        /// метод рисует линию и вершины из списка выбранных вершин
        /// </summary>
        private void DrawLine()
        {
            canvas.Children.Clear();
            
            for (int i = 0; i < K - 1; i++)
            {
                Line line = new Line();
                
                double oldX = center.X - radius + radius / K * i;
                double currentX = center.X - radius + radius / K * (i + 1);
                
                line.X1 = oldX;
                line.X2 = currentX;
                line.Y1 = FindYCoordinate(oldX, radius, center);
                line.Y2 = FindYCoordinate(currentX, radius, center);
                line.Stroke = Brushes.Black;
                
                canvas.Children.Add(line);
            }

            for (int i = 0; i < listSelected.Count; i++)
            {
                if (listSelected[i].Influence != default && listSelected[i].Probability != default  && listSelected[i].Status==1)
                {
                    Ellipse ellipse = new Ellipse();

                    listSelected[i].point.X = 425 * listSelected[i].Probability + 75;
                    listSelected[i].point.Y = -350 * listSelected[i].Influence + 400;
                    
                    ellipse.Height = 12;
                    ellipse.Width = 12;
                    ellipse.StrokeThickness = 3;

                    double m = Math.Sqrt((listSelected[i].point.X - center.X) * (listSelected[i].point.X - center.X) +
                                         (listSelected[i].point.Y - center.Y) * (listSelected[i].point.Y - center.Y));
                    
                    if (m < radius)
                    {
                        ellipse.Fill = Brushes.Red;
                        ellipse.Stroke = Brushes.Red;
                    }
                    else
                    {
                        ellipse.Fill = Brushes.Green;
                        ellipse.Stroke = Brushes.Green;
                    }
                    
                    ellipse.VerticalAlignment = VerticalAlignment.Top;
                    ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                    ellipse.Margin = new Thickness(listSelected[i].point.X, listSelected[i].point.Y, 0, 0);
                    
                    canvas.Children.Add(ellipse);
                }
            }
        }

        /// <summary>
        /// метод закрывает окно при нажатии кнопки 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            AdministratorGraphic graphicWindow = new AdministratorGraphic(_project);
            Close();
            graphicWindow.Show();
        }

        private void OpenDocument_Click(object sender, RoutedEventArgs e)
        {
            wordApplication.Visible = true;
        }
    }
}
