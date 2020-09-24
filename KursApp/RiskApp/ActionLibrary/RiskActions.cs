﻿using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System;
using System.IO;

namespace RiskApp
{
    public class RiskActions
    {
        SqlConnection sqlConnection;

        public RiskActions()
        {
            string key = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\Database.mdf';Integrated Security=False;Trusted_Connection=True";
            sqlConnection = new SqlConnection(key);
        }

        /// <summary>
        /// метод, который проверяет, находится ли элемент в списке
        /// </summary>
        /// <param name="line"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool CheckIfInList(string line, List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
                if (line == list[i])
                    return false;

            return true;
        }

        /// <summary>
        /// метод, который возвращает список всех рисков проекта
        /// </summary>
        /// <returns></returns>
        public async Task<List<Risk>> ShowRisks()
        {
            List<Risk> listRisks = new List<Risk>();
            SqlDataReader sqlDataReader = null;

            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM[Risks]", sqlConnection);

            try
            {
                sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    listRisks.Add(new Risk(Convert.ToString(sqlDataReader["RiskName"]), Convert.ToString(sqlDataReader["Source"]),
                        Convert.ToString(sqlDataReader["Effects"]),
                        Convert.ToString(sqlDataReader["Type"]),
                        Convert.ToString(sqlDataReader["PossibleSolution"]), Convert.ToInt32(sqlDataReader["Id"])));
                }

                return listRisks;
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
        /// метод, который возвращает список всех источников
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> ShowSources()
        {
            List<string> listSources = new List<string>();
            SqlDataReader sqlDataReader = null;

            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM[Risks]", sqlConnection);

            try
            {
                sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    if (CheckIfInList(Convert.ToString(sqlDataReader["Source"]), listSources))
                        listSources.Add(Convert.ToString(sqlDataReader["Source"]));
                }

                return listSources;
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
    }
}
