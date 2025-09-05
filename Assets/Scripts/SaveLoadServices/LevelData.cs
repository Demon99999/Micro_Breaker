using System;

namespace Assets.Scripts.SaveLoadServices
{
    [Serializable]
    public class LevelData
    {
        public string LocationName;
        public string AdditionaValue = string.Empty;
        public int Active;
        public int Passed;
    }
}