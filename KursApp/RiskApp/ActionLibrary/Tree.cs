﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
 using System.Windows;

 namespace RiskApp
{
    class Tree
    {
        SqlConnection sqlConnection;

        public Tree()
        {
            string key = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\Database.mdf';Integrated Security=False;Trusted_Connection=True";
            sqlConnection = new SqlConnection(key);
        }

        /// <summary>
        /// метод, который добавляет новую вершину в базу
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task AddVertex(int id, Vertex vertex)
        {
            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand =
                new SqlCommand("INSERT INTO [RiskTree] (ParentId,Description,Cost,Probability,X,Y) VALUES(@ParentId,@Description,@Cost,@Probability,@X,@Y)", sqlConnection);

            sqlCommand.Parameters.AddWithValue("ParentId", id);
            sqlCommand.Parameters.AddWithValue("Description", vertex.Description);
            sqlCommand.Parameters.AddWithValue("Cost", vertex.Cost);
            sqlCommand.Parameters.AddWithValue("Probability", vertex.Probability);
            sqlCommand.Parameters.AddWithValue("X", vertex.X);
            sqlCommand.Parameters.AddWithValue("Y", vertex.Y);

            await sqlCommand.ExecuteNonQueryAsync();

            sqlConnection.Close();
        }

        /// <summary>
        /// метод, который проверяет, есть ли заданная вершина в базе данных
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<bool> CheckIfExistInDataBase(int idParent)
        {
            SqlDataReader sqlDataReader = null;

            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM[RiskTree]", sqlConnection);

            try
            {
                sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    if (idParent == Convert.ToInt32(sqlDataReader["ParentId"]) && Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["Probability"]))) == default)
                        return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();

                sqlConnection.Close();
            }
        }

        /// <summary>
        /// метод для удаления вершин 
        /// </summary>
        /// <param name="listVertex"></param>
        /// <returns></returns>
        public async Task DeleteVertex(List<Vertex> listVertex)
        {
            try
            {
                await sqlConnection.OpenAsync();

                for (int i = 0; i < listVertex.Count; i++)
                {
                    SqlCommand sqlCommand = new SqlCommand("DELETE FROM [RiskTree] WHERE [Id]=@Id", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("Id", listVertex[i].ID);
                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// метод, который показывает первую вершину графа
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<Vertex> ShowVertex(int idParent)
        {
            SqlDataReader sqlDataReader = null;

            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM[RiskTree]", sqlConnection);

            try
            {
                sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
                {
                    if (idParent == Convert.ToInt32(sqlDataReader["ParentId"]) && Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["Probability"]))) == default)
                        return new Vertex(Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["X"]))),
                            Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["Y"]))),
                            Convert.ToInt32(sqlDataReader["Id"]), Convert.ToInt32(sqlDataReader["ParentId"]),
                            Convert.ToString(sqlDataReader["Description"]));
                }

                return null;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();

                sqlConnection.Close();
            }
        }

        /// <summary>
        /// метод, который возвращает вершину 
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public async Task<Vertex> ShowVertex(Vertex vertex)
        {
            SqlDataReader sqlDataReader = null;

            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM[RiskTree]", sqlConnection);

            try
            {
                sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    if (vertex.X == Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["X"]))) &&
                        vertex.Y == Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["Y"]))) &&
                        vertex.IDParent == Convert.ToInt32(sqlDataReader["ParentId"]) &&
                        default != Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["Probability"]))))
                    {
                        return new Vertex(
                            Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["X"]))),
                            Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["Y"]))),
                            Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["Cost"]))),
                            Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["Probability"]))),
                            Convert.ToInt32(sqlDataReader["Id"]), Convert.ToInt32(sqlDataReader["ParentId"]),
                            Convert.ToString(sqlDataReader["Description"]));
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();

                sqlConnection.Close();
            }
        }


        /// <summary>
        /// метод, который возвращает все вершины
        /// </summary>
        /// <returns></returns>
        public async Task<List<Vertex>> ShowListVertexes()
        {
            SqlDataReader sqlDataReader = null;
            List<Vertex> listVertex = new List<Vertex>();

            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM[RiskTree]", sqlConnection);

            try
            {
                sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    listVertex.Add(new Vertex(Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["X"]))),
                        Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["Y"]))),
                        Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["Cost"]))),
                        Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["Probability"]))),
                        Convert.ToInt32(sqlDataReader["Id"]), Convert.ToInt32(sqlDataReader["ParentId"]),
                        Convert.ToString(sqlDataReader["Description"])));
                }

                return listVertex;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();

                sqlConnection.Close();
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
