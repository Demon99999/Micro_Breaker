using System;
using UnityEngine;

namespace Assets.Scripts.SaveLoadServices
{
    public class SaveGameProgress
    {
        private GameProgress2 _gameProgress2;

        public event Action<GameProgress2> Loaded;

        public void Save(GameProgress2 gameProgress2)
        {
            string json = JsonUtility.ToJson(gameProgress2);

            PlayerPrefs.SetString(nameof(_gameProgress2), json);
            PlayerPrefs.Save();
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(nameof(_gameProgress2)))
            {
                Load(PlayerPrefs.GetString(nameof(_gameProgress2)));
                return;
            }

            _gameProgress2 = new GameProgress2();
            string json = JsonUtility.ToJson(_gameProgress2);
            Load(json);
        }

        private void Load(string value)
        {
            _gameProgress2 = JsonUtility.FromJson<GameProgress2>(value);
            Loaded?.Invoke(_gameProgress2);
        }
    }
}