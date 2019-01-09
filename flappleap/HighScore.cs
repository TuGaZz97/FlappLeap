/* 
 * Author : Pin Guillaume
 * Class  : TIS-E1B
 * Date   : 15.01.2018
 * Projet : FlappLeap
 */
namespace FlappLeap
{
    public class HighScore
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public string Created { get; set; }
        public int Difficulty { get; set; }
        /// <summary>
        /// Main constructor
        /// </summary>
        public HighScore(string name, int score, string created, int difficulty)
        {
            this.Name = name;
            this.Score = score;
            this.Created = created;
            this.Difficulty = difficulty;
        }
    }
}