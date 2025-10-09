using System;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    [Serializable]
    public class EqualizerMovementSettings : MovementStrategySettings
    {
        [Header("Equalizer Settings")]
        public float equalizerDepth = 5f;
        public float equalizerUpdateRate = 0.1f;
        public float equalizerSmoothTime = 0.3f;
        public AnimationCurve equalizerCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public EqualizerDirection equalizerDirection = EqualizerDirection.ForwardBack;
        public WaveType waveType = WaveType.Sine;
        public float waveSpeed = 1f;
        public float waveFrequency = 1f;
        public float waveAmplitude = 1f;
    }
}
