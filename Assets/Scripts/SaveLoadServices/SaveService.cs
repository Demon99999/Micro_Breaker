using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enums;
using Assets.Scripts.UI.AdditionalImprovement;
using UnityEngine;

namespace Assets.Scripts.SaveLoadServices
{
    public class SaveService : MonoBehaviour
    {
        private const int MinValue = 0;
        private const int MaxValue = 1;

        private readonly SaveGameProgress _saveGameProgress = new SaveGameProgress();
        private GameProgress _gameProgress = new GameProgress();

        public event Action Loaded;

        public int Coins => _gameProgress.Coins;

        public string CurrentBall => _gameProgress.CurrentBall;

        public string CurrentPlatform => _gameProgress.CurrentPlatform;

        public string[] Balls => _gameProgress.Balls;

        public string[] Platforms => _gameProgress.Platforms;

        public LevelData[] LevelDatas => _gameProgress.LevelDatas;

        public int LevelCount => _gameProgress.LevelCount;

        public LevelData LevelData => _gameProgress.CurrentLevelData;

        private void Start()
        {
            _saveGameProgress.Load();
        }

        private void OnEnable()
        {
            _saveGameProgress.Loaded += OnFill;
        }

        private void OnDisable()
        {
            _saveGameProgress.Loaded -= OnFill;
        }

        public int GetCoins()
        {
            return _gameProgress.Coins;
        }

        public string GetCurrentProduct(ObjectsName objectsName)
        {
            if (objectsName == ObjectsName.Ball) return CurrentBall;

            if (objectsName == ObjectsName.Platform) return CurrentPlatform;

            return string.Empty;
        }

        public string[] GetArrayProducts(ObjectsName objectsName)
        {
            if (objectsName == ObjectsName.Ball) return Balls;

            if (objectsName == ObjectsName.Platform) return Platforms;

            return new string[MinValue];
        }

        public int GetScale(ObjectsName objectsName)
        {
            int scale = 0;

            if (objectsName == ObjectsName.Ball) scale = _gameProgress.ScaleBall;

            if (objectsName == ObjectsName.Platform) scale = _gameProgress.ScalePlatform;

            return scale;
        }

        public List<AdditionalImprovementValue> GetAdditionalImprovementValues()
        {
            List<AdditionalImprovementValue> additionalImprovementValue = new List<AdditionalImprovementValue>();

            for (int i = 0; i < _gameProgress.AdditionalImprovementDatas.Length; i++)
            {
                additionalImprovementValue.Add(new AdditionalImprovementValue(_gameProgress.AdditionalImprovementDatas[i].UpgradeName,
                                      _gameProgress.AdditionalImprovementDatas[i].Value,
                                      _gameProgress.AdditionalImprovementDatas[i].ValueSelect));
            }

            return additionalImprovementValue;
        }

        public List<LevelData> GetLevelDatas()
        {
            return _gameProgress.LevelDatas.ToList();
        }

        public void SaveLevelDatas(List<LevelData> locationObjectDatas)
        {
            _gameProgress.LevelDatas = locationObjectDatas.ToArray();
            Save();
        }

        public void SaveAdditionalImprovementValues(List<AdditionalImprovementValue> additionalImprovementValue)
        {
            List<AdditionalImprovementData> additionalImprovementData = new List<AdditionalImprovementData>();

            for (int i = 0; i < additionalImprovementValue.Count; i++)
            {
                additionalImprovementData.Add(new AdditionalImprovementData()
                {
                    UpgradeName = additionalImprovementValue[i].AdditionalImprovementName,
                    Value = additionalImprovementValue[i].Value,
                    ValueSelect = additionalImprovementValue[i].GetSelectValue()
                });
            }

            _gameProgress.AdditionalImprovementDatas = additionalImprovementData.ToArray();
            Save();
        }

        public void SaveScale(bool isCanScale, ObjectsName objectsName)
        {
            if (objectsName == ObjectsName.Ball)
                _gameProgress.ScaleBall = isCanScale ? MaxValue : MinValue;

            if (objectsName == ObjectsName.Platform)
                _gameProgress.ScalePlatform = isCanScale ? MaxValue : MinValue;

            Save();
        }

        public void SaveCurrentProduct(ObjectsName objectsName, string currentName)
        {
            if (objectsName == ObjectsName.Ball)
                _gameProgress.CurrentBall = currentName;

            if (objectsName == ObjectsName.Platform)
                _gameProgress.CurrentPlatform = currentName;

            Save();
        }

        public void SaveArrayProducts(ObjectsName objectsName, string[] Names)
        {
            if (objectsName == ObjectsName.Ball)
                _gameProgress.Balls = Names;

            if (objectsName == ObjectsName.Platform)
                _gameProgress.Platforms = Names;

            Save();
        }

        public void SaveCoins(int amount)
        {
            _gameProgress.Coins = amount;
            Save();
        }

        public void SaveLevel(int level)
        {
            _gameProgress.LevelCount = level;
            Save();
        }

        public void SaveCurrentLevelData(LevelData currentLevelData)
        {
            _gameProgress.CurrentLevelData = currentLevelData;
            Save();
        }

        private void Save()
        {
            _saveGameProgress.Save(_gameProgress);
        }

        private void OnFill(GameProgress gameData)
        {
            _gameProgress = gameData;
            Loaded?.Invoke();
        }
    }
}