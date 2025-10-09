using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.BoosterLogic
{
    public class BoostersContainer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        private List<AbstractBooster> _boosters;
        private Transform _transform;
        private bool _isInitialized = false;
        private bool _isResetting = false;

        public List<AbstractBooster> AbstractBoosters => _boosters ?? new List<AbstractBooster>();

        private void Awake()
        {
            if (_boosters == null)
            {
                Fill();
            }

            _isInitialized = true;
        }

        private void OnDisable()
        {
            foreach (var booster in _boosters)
                booster.TimeRunning -= OnPlaySound;
        }

        public void Fill()
        {
            _transform = transform;
            _boosters = new List<AbstractBooster>();

            for (int i = 0; i < _transform.childCount; i++)
            {
                _transform.GetChild(i).TryGetComponent(out AbstractBooster booster);
                _boosters.Add(booster);
                _boosters[i].TimeRunning += OnPlaySound;
            }
        }

        public BoosterEffect GetRandomBoosters(BoosterNames boxName)
        {
            int minLength = 0;
            var booster = _boosters.Where(b => b.BoosterName == boxName && b.BoosterEffect.IsCreated == false)
                                   .Select(b => b).ToList();

            if (booster.Count == 0) return null;

            int index = _boosters.IndexOf(booster[Random.Range(minLength, booster.Count)]);
            _boosters[index].BoosterEffect.HaveCreated();
            return _boosters[index].BoosterEffect;
        }

        public void Reset()
        {
            if (_isResetting)
            {
                Debug.LogWarning("BoostersContainer is already resetting");
                return;
            }

            _isResetting = true;

            try
            {
                Debug.Log($"BoostersContainer.Reset called. Boosters count: {_boosters?.Count}");

                if (_boosters == null || _boosters.Count == 0)
                {
                    Debug.LogWarning("Boosters list is null or empty");
                    return;
                }

                // —оздаем копию списка дл€ безопасной итерации
                var boostersCopy = new List<AbstractBooster>(_boosters);

                foreach (var booster in boostersCopy)
                {
                    if (booster == null)
                    {
                        Debug.LogWarning("Null booster in list, skipping");
                        continue;
                    }

                    if (!booster.gameObject.activeInHierarchy)
                    {
                        Debug.LogWarning($"Booster {booster.name} is inactive, skipping");
                        continue;
                    }

                    if (booster.BoosterEffect == null)
                    {
                        Debug.LogError($"BoosterEffect is null for booster: {booster.name}");
                        continue;
                    }

                    try
                    {
                        Debug.Log($"Stopping booster: {booster.name}");
                        booster.StopAction(booster.BoosterEffect);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Error stopping booster {booster.name}: {e.Message}");
                    }
                }
            }
            finally
            {
                _isResetting = false;
            }
        }

        private void OnPlaySound()
        {
            if (_audioSource.isPlaying) _audioSource.Stop();

            _audioSource.Play();
        }
    }
}