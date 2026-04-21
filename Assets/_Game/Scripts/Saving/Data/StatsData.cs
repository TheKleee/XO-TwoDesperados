namespace DataXO
{
    [System.Serializable]
    public class StatsData
    {
        public int totalGames;
        public int[] playerWins = new int[6];   // index 0 = player 1, etc.
        public int[] playerDraws = new int[6];  // same logic xD
        public int draws;
        public float totalDuration; // divide by totalGames for average
    }
}