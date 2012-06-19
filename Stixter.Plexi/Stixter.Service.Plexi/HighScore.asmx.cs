using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Stixter.Service.Plexi
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class HighScore : WebService
    {
        [WebMethod]
        public List<PlayerScore> GetHighScore()
        {
            var playerScores = new List<PlayerScore>();
            playerScores.Add(new PlayerScore{Name="Jonas", Score=1, Time = DateTime.Now});
            return playerScores;
        }

        [WebMethod]
        public void CommitPlayerScore(PlayerScore playerScore)
        {
            
        }

        public class PlayerScore
        {
            public string Name { get; set; }
            public int Score { get; set; }
            public DateTime Time { get; set; }
        }
    }
}