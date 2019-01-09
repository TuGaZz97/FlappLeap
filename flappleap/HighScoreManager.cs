/* 
 * Author : Pin Guillaume
 * Class  : TIS-E1B
 * Date   : 15.01.2018
 * Projet : FlappLeap
 */
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace FlappLeap
{
    public class HighScoreManagement
    {
        private SQLiteConnection DbConnection { get; set; }

        /// <summary>
        /// Main constructor
        /// </summary>
        public HighScoreManagement()
        {
            this.DbConnection = new SQLiteConnection("Data Source=highscore.db");
            if (!Checksdatabase())
            {
                FileStream fs = File.Create(Directory.GetCurrentDirectory() + "\\highscore.db");
                fs.Close();               
                CreateDatabase();
            }
           
        }

        /// <summary>
        /// Method that adds a new score
        /// </summary>
        /// <param name="name">Player's nickname</param>
        /// <param name="score">Player's score</param>
        public void AddHighScore(string name, int score, int difficulty)
        {
            /* Opens the connection to the database */
            this.DbConnection.Open();
            /* Prepare the query */
            SQLiteCommand dbCommand = new SQLiteCommand("Insert Into HIGHSCORE(name, score, created, difficulty)" +
                "values(@name, @score,'" + DateTime.Now + "', @difficulty)", this.DbConnection);
            dbCommand.Prepare();
            dbCommand.Parameters.AddWithValue("@name", name);
            dbCommand.Parameters.AddWithValue("@score", score);
            dbCommand.Parameters.AddWithValue("@difficulty", difficulty);
            /* Execute the query */
            dbCommand.ExecuteNonQuery();
            /* Close the connection to the database */
            this.DbConnection.Close();
        }

        /// <summary>
        /// method that retrieves data in a list
        /// </summary>
        /// <returns>the list of scores</returns>
        public List<HighScore> ReadHighScores()
        {
            /* Opens the connection to the database */
            this.DbConnection.Open();
            List<HighScore> listHighScore = new List<HighScore>();
            /* Prepare the query */
            SQLiteCommand dbCommand = new SQLiteCommand("Select name, score, created, difficulty from HIGHSCORE order by score desc", this.DbConnection);
            /* Execute the query */
            SQLiteDataReader dbReader = dbCommand.ExecuteReader();
            /* Recovers data from the listHighScore */
            while (dbReader.Read())
            {
                listHighScore.Add(new HighScore(dbReader["name"].ToString(), Convert.ToInt32(dbReader["score"]), dbReader["created"].ToString(), Convert.ToInt32(dbReader["difficulty"])));
            }
            /* Close the connection to the database */
            this.DbConnection.Close();
            /* Returns the score list */
            return listHighScore;
        }

        /// <summary>
        /// Checks if the database is already created
        /// </summary>
        /// <returns></returns>
        private bool Checksdatabase()
        {
            FileInfo file = new FileInfo(Directory.GetCurrentDirectory() + "\\highscore.db");
            if (file.Exists)
                return true;
            else
                return false;                
        }

        /// <summary>
        /// Creating the database
        /// </summary>
        private void CreateDatabase()
        {
            /* Opens the connection to the database */
            this.DbConnection.Open();
            /* Creating the database */
            SQLiteCommand command = new SQLiteCommand("create table HIGHSCORE(name varchar(100), score int, created varchar(100), difficulty int)", this.DbConnection);
            command.ExecuteNonQuery();
            /* Close the connection to the database */
            this.DbConnection.Close();
        }
    }
}
