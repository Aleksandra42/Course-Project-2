﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace RiskApp
{
    public class ProjectActions
    {
        SqlConnection sqlConnection;
        public ProjectActions()
        {
            string key = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\Database.mdf';Integrated Security=False;Trusted_Connection=True";
            sqlConnection = new SqlConnection(key);
        }

        /// <summary>
        /// метод добавляет новый проект 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <param name="type"></param>
        public async Task AddProject(string name, string type, string owner)
        {
            if (name == "")
                throw new FormatException("Error! Inappropriate Name!");

            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("INSERT INTO [Projects] (Name,Owner,Type) VALUES(@Name,@OWner,@Type)", sqlConnection);

            sqlCommand.Parameters.AddWithValue("Name", name);
            sqlCommand.Parameters.AddWithValue("Owner", owner);
            sqlCommand.Parameters.AddWithValue("Type", type);

            await sqlCommand.ExecuteNonQueryAsync();

            sqlConnection.Close();
        }

        /// <summary>
        /// метод, который возвращает список всех проектов в базе 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Project>> ShowProjects()
        {
            List<Project> listProjects = new List<Project>();
            SqlDataReader sqlDataReader = null;

            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM[Projects]", sqlConnection);

            try
            {
                sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    listProjects.Add(new Project(Convert.ToInt32(sqlDataReader["Id"]),
                        Convert.ToString(sqlDataReader["Name"]),
                        Convert.ToString(sqlDataReader["Owner"]),
                        Convert.ToString(sqlDataReader["Type"])));
                }

                return listProjects;
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
        /// метод , который возвращает список проектов владельца
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public async Task<List<Project>> ShowUsersProjects(string user)
        {
            List<Project> listProjects = new List<Project>();
            SqlDataReader sqlDataReader = null;

            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM[Projects]", sqlConnection);

            try
            {
                sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    if (user == Convert.ToString(sqlDataReader["Owner"]))
                        listProjects.Add(new Project(Convert.ToInt32(sqlDataReader["Id"]),
                            Convert.ToString(sqlDataReader["Name"]),
                            Convert.ToString(sqlDataReader["Owner"]), Convert.ToString(sqlDataReader["Type"])));
                }

                return listProjects;
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

    }
}
