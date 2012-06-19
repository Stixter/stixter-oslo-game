using System;
using System.Collections.Generic;
using Stixter.Plexi.Service.HighScoreService;

namespace Stixter.Plexi.Service
{
    public class HighScore
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
        public string Rank { get; set; }
    }

    public class PlexiIntegrationService
    {
        private readonly HighScoreService.HighScore _client;

        public PlexiIntegrationService()
        {
           // _client = new HighScoreService.HighScore {Url = "http://localhost:51100/HighScore.asmx"};
            _client = new HighScoreService.HighScore {Url = "http://service.stixter.se/HighScore.asmx"};
        }

        public List<HighScore> GetHighScores()
        {
            var playerScores = _client.GetHighScore();
            var scores = new List<HighScore>();
            foreach (var playerScore in playerScores)
            {
                scores.Add(new HighScore { Date = playerScore.Time, Name = playerScore.Name, Score = playerScore.Score, Rank = "rank" });
            }

            return scores;
        }

        public void CommitHighScore(HighScore highScore)
        {
            _client.CommitPlayerScore(new PlayerScore{Name = highScore.Name, Score = highScore.Score, Time = highScore.Date});
        }
    }
}
