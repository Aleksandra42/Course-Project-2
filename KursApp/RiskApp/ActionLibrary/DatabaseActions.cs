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
    public class DatabaseActions
    {
        SqlConnection sqlConnection;

        public DatabaseActions()
        {
            string key = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\Database.mdf';Integrated Security=False;Trusted_Connection=True";
            sqlConnection = new SqlConnection(key);
        }

        /// <summary>
        /// метод, который доавляет новый риск в проект
        /// </summary>
        /// <param name="risk"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public async Task AddRisk(string project, Risk risk)
        {
            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand =
                new SqlCommand("INSERT INTO [RiskData] (RiskName,Probability,Influence,Project,Source,Effects,PossibleSolution,Type,Status) " +
                "VALUES(@riskName,@probability,@influence,@project,@source,@effects,@possibleSolution,@type,@Status)", sqlConnection);

            sqlCommand.Parameters.AddWithValue("RiskName", risk.RiskName);
            sqlCommand.Parameters.AddWithValue("Probability", risk.Probability);
            sqlCommand.Parameters.AddWithValue("Influence", risk.Influence);
            sqlCommand.Parameters.AddWithValue("Project", project);
            sqlCommand.Parameters.AddWithValue("Source", risk.Source);
            sqlCommand.Parameters.AddWithValue("Effects", risk.Effects);
            sqlCommand.Parameters.AddWithValue("PossibleSolution", risk.Solution);
            sqlCommand.Parameters.AddWithValue("Type", risk.Type);
            sqlCommand.Parameters.AddWithValue("Status", risk.Status);

            await sqlCommand.ExecuteNonQueryAsync();

            sqlConnection.Close();
        }

        /// <summary>
        /// метод, который добавляет новые риски в проект
        /// </summary>
        /// <param name="project"></param>
        /// <param name="risk"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddRisk(string project, Risk risk, User user)
        {
            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand =
                new SqlCommand("INSERT INTO [RiskData] (RiskName,Probability,Influence,Project,Source,Effects,PossibleSolution,Type,OwnerLogin,OwnerId,Status) " +
                "VALUES(@riskName,@probability,@influence,@project,@source,@effects,@possibleSolution,@type,@OwnerLogin,@OwnerId,@Status)", sqlConnection);

            sqlCommand.Parameters.AddWithValue("RiskName", risk.RiskName);
            sqlCommand.Parameters.AddWithValue("Probability", risk.Probability);
            sqlCommand.Parameters.AddWithValue("Influence", risk.Influence);
            sqlCommand.Parameters.AddWithValue("Project", project);
            sqlCommand.Parameters.AddWithValue("Source", risk.Source);
            sqlCommand.Parameters.AddWithValue("Effects", risk.Effects);
            sqlCommand.Parameters.AddWithValue("PossibleSolution", risk.Solution);
            sqlCommand.Parameters.AddWithValue("Type", risk.Type);
            sqlCommand.Parameters.AddWithValue("OwnerLogin", user.Login);
            sqlCommand.Parameters.AddWithValue("OwnerId", user.ID);
            sqlCommand.Parameters.AddWithValue("Status", risk.Status);

            await sqlCommand.ExecuteNonQueryAsync();

            sqlConnection.Close();
        }

        /// <summary>
        /// метод, который меняет характеристики риска
        /// </summary>
        /// <param name="risk"></param>
        /// <returns></returns>
        public async Task ChangeRisk(Risk risk)
        {
            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand =
                new SqlCommand("UPDATE [RiskData] SET [Probability]= @Probability, [Influence]= @Influence, [OwnerLogin]= @OwnerLogin, [Status]= @Status, [OwnerId]= @OwnerId WHERE id=@id", sqlConnection);

            sqlCommand.Parameters.AddWithValue("Id", risk.ID);
            sqlCommand.Parameters.AddWithValue("Status", risk.Status);
            sqlCommand.Parameters.AddWithValue("Probability", risk.Probability);
            sqlCommand.Parameters.AddWithValue("Influence", risk.Influence);
            sqlCommand.Parameters.AddWithValue("OwnerLogin", risk.OwnerLogin);
            sqlCommand.Parameters.AddWithValue("OwnerId", risk.IdUser);

            await sqlCommand.ExecuteNonQueryAsync();

            sqlConnection.Close();
        }

        /// <summary>
        /// метод, который меняет характеристики риска для пользователя
        /// </summary>
        /// <param name="risk"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task ChangeRisk(Risk risk, User user)
        {
            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand =
                new SqlCommand("UPDATE [RiskData] SET [Probability]= @Probability, [Influence]= @Influence, [OwnerLogin]= @OwnerLogin, [Status]= @Status, [OwnerId]= @OwnerId WHERE id=@id", sqlConnection);

            sqlCommand.Parameters.AddWithValue("Id", risk.ID);
            sqlCommand.Parameters.AddWithValue("Probability", risk.Probability);
            sqlCommand.Parameters.AddWithValue("Influence", risk.Influence);
            sqlCommand.Parameters.AddWithValue("OwnerLogin", user.Login);
            sqlCommand.Parameters.AddWithValue("OwnerId", user.ID);
            sqlCommand.Parameters.AddWithValue("Status", risk.Status);

            await sqlCommand.ExecuteNonQueryAsync();
            sqlConnection.Close();
        }

        /// <summary>
        /// метод, который возвращает список рисков проекта
        /// </summary>
        /// <param name="ProjectName"></param>
        /// <returns></returns>
        public async Task<List<Risk>> ShowRisks(Project project)
        {
            List<Risk> listRisks = new List<Risk>();
            SqlDataReader sqlDataReader = null;

            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM[RiskData]", sqlConnection);

            try
            {
                sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    if (project.Name == Convert.ToString(sqlDataReader["Project"]))
                    {
                        if (Convert.ToInt32(sqlDataReader["Status"]) == 0)
                        {
                            listRisks.Add(new Risk(Convert.ToInt32(sqlDataReader["Id"]),
                                    Convert.ToInt32(sqlDataReader["Status"]),
                                    Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["Probability"]))),
                                    Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["Influence"]))),
                                Convert.ToString(sqlDataReader["RiskName"]), Convert.ToString(sqlDataReader["Source"]),
                            Convert.ToString(sqlDataReader["Effects"]),
                            Convert.ToString(sqlDataReader["Type"]), Convert.ToString(sqlDataReader["PossibleSolution"])));
                        }
                        else
                        {
                            listRisks.Add(new Risk(Convert.ToInt32(sqlDataReader["Id"]),
                                Convert.ToInt32(sqlDataReader["Status"]),
                                Convert.ToInt32(sqlDataReader["OwnerId"]),
                                Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["Probability"]))),
                                Convert.ToDouble(ParseLine(Convert.ToString(sqlDataReader["Influence"]))),
                                Convert.ToString(sqlDataReader["RiskName"]),
                                Convert.ToString(sqlDataReader["Source"]),
                                Convert.ToString(sqlDataReader["Effects"]),
                                Convert.ToString(sqlDataReader["Type"]),
                                Convert.ToString(sqlDataReader["OwnerLogin"]), 
                                Convert.ToString(sqlDataReader["PossibleSolution"])));
                        }
                    }
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
        /// метод,  который возвращает список проетков полльзователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<string>> ShowUserProjects(User user)
        {
            List<string> listProjects = new List<string>();
            SqlDataReader sqlDataReader = null;

            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM[RiskData]", sqlConnection);

            try
            {
                sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    if (user.Login == Convert.ToString(sqlDataReader["OwnerLogin"]))
                    {
                        if (CheckIfInList(Convert.ToString(sqlDataReader["Project"]), listProjects))
                            listProjects.Add(Convert.ToString(sqlDataReader["Project"]));
                    }
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
        /// метод проверяет, находится ли проект в списке
        /// </summary>
        /// <param name="project"></param>
        /// <param name="lst"></param>
        /// <returns></returns>
        private bool CheckIfInList(string project, List<string> listProject)
        {
            for (int i = 0; i < listProject.Count; i++)
                if (project == listProject[i])
                    return false;

            return true;
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
