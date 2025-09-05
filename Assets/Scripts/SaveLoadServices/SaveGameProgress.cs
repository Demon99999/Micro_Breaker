using System;
using UnityEngine;

namespace Assets.Scripts.SaveLoadServices
{
    public class SaveGameProgress
    {
        private GameProgress _gameProgress;

        public event Action<GameProgress> Loaded;

        public void Save(GameProgress gameProgress)
        {
            string json = JsonUtility.ToJson(gameProgress);

            PlayerPrefs.SetString(nameof(_gameProgress), json);
            PlayerPrefs.Save();
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(nameof(_gameProgress)))
            {
                Load(PlayerPrefs.GetString(nameof(_gameProgress)));
                return;
            }

            _gameProgress = new GameProgress();
            string json = JsonUtility.ToJson(_gameProgress);
            Load(json);
        }

        private void Load(string value)
        {
            _gameProgress = JsonUtility.FromJson<GameProgress>(value);
            Loaded?.Invoke(_gameProgress);
        }
    }
}