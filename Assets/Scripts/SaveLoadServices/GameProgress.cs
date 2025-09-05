namespace Assets.Scripts.SaveLoadServices
{
    public class GameProgress
    {
        public int Coins = 10000;
        public int LevelCount = 1;
        public int ScaleBall = 0;
        public int ScalePlatform = 0;
        public string CurrentBall = string.Empty;
        public string CurrentPlatform = string.Empty;
        public string[] Balls = null;
        public string[] Platforms = null;
        public LevelData CurrentLevelData = null;
        public LevelData[] LevelDatas = null;
        public AdditionalImprovementData[] AdditionalImprovementDatas = null;
    }
}