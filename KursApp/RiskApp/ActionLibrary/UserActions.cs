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
    public class UserActions
    {
        SqlConnection sqlConnection;

        public UserActions()
        {
            string key = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\Database.mdf';Integrated Security=False;Trusted_Connection=True";
            sqlConnection = new SqlConnection(key);
        }

        /// <summary>
        /// метод, который проводит идентификацию пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns> возвращает 2, если менеджер, 1 если тестировщик проекта, 0 если ввод неверный</returns>
        public async Task<int> CheckLogin(string login, string password)
        {
            SqlDataReader sqlDataReader = null;

            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM[Users]", sqlConnection);

            try
            {
                sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    if (login == Convert.ToString(sqlDataReader["Login"]) && password == Convert.ToString(sqlDataReader["Password"]))
                    {
                        if ("MainManager" == Convert.ToString(sqlDataReader["Position"]))
                            return 3;
                        else
                        {
                            if ("ProjectManager" == Convert.ToString(sqlDataReader["Position"]))
                                return 2;

                            return 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return 0;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();

                sqlConnection.Close();
            }

            return 0;
        }

        /// <summary>
        /// метод находит пользователя по логину и паролю
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns>возвращает польозователя</returns>
        public async Task<User> SearchForUser(string login, string password)
        {
            SqlDataReader sqlDataReader = null;

            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM[Users]", sqlConnection);

            try
            {
                sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    if (login == Convert.ToString(sqlDataReader["Login"]) && password == Convert.ToString(sqlDataReader["Password"]))
                        return (new User(Convert.ToInt32(sqlDataReader["id"]), Convert.ToString(sqlDataReader["Name"]),
                            Convert.ToString(sqlDataReader["Login"]), Convert.ToString(sqlDataReader["Password"]), Convert.ToString(sqlDataReader["Position"])));
                }
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

            return null;
        }

        /// <summary>
        /// метод, который возвращает список всех пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> ShowUsers()
        {
            SqlDataReader sqlDataReader = null;
            List<User> listUser = new List<User>();

            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM[Users]", sqlConnection);

            try
            {
                sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    listUser.Add(new User(Convert.ToInt32(sqlDataReader["id"]), Convert.ToString(sqlDataReader["Name"]),
                        Convert.ToString(sqlDataReader["Login"]), Convert.ToString(sqlDataReader["Password"]),
                        Convert.ToString(sqlDataReader["Position"])));
                }

                return listUser;
            }
            catch (SqlException ex)
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
