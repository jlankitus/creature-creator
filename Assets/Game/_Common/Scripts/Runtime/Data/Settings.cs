// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class Settings : Data
    {
        #region Fields
        [Header("Video")]
        [SerializeField] private Vector2Int displaySize;
        [SerializeField] private int refreshRate;
        [SerializeField] private bool fullscreen;
        [SerializeField] private bool vSync;
        [SerializeField] private int targetFrameRate = -1;
        [SerializeField] private float screenScale = 1f;
        [Space]
        [SerializeField] private CreatureMeshQualityType creatureMeshQuality;
        [SerializeField] private ShadowQualityType shadowQuality;
        [SerializeField] private TextureQualityType textureQuality;
        [SerializeField] private AmbientOcclusionType ambientOcclusion;
        [SerializeField] private AntialiasingType antialiasing;
        [SerializeField] private ScreenSpaceReflectionsType screenSpaceReflections;
        [SerializeField] private FoliageType foliage;
        [SerializeField] private bool ambientParticles = true;
        [SerializeField] private bool reflections;
        [SerializeField] private bool anisotropicFiltering;
        [SerializeField] private bool bloom;
        [SerializeField] private bool depthOfField;
        [SerializeField] private bool motionBlur;
        [SerializeField] private bool optimizeCreatures;
        [SerializeField] private float creatureRenderDistance = 50f;

        [Header("Audio")]
        [SerializeField, Range(0, 1)] private float masterVolume;
        [SerializeField, Range(0, 1)] private float musicVolume;
        [SerializeField, Range(0, 1)] private float soundEffectsVolume;
        [SerializeField] private InGameMusicType inGameMusic;

        [Header("Gameplay")]
        [SerializeField] private string onlineUsername;
        [SerializeField] private List<CreatureData> creaturePresets;
        [SerializeField] private int exportPrecision;
        [SerializeField] private List<string> hiddenBodyParts;
        [SerializeField] private List<string> hiddenPatterns;
        [SerializeField] private string locale;
        [SerializeField] private float touchOffset = 100f;
        [SerializeField] private bool cameraShake;
        [SerializeField] private bool vibrations = true;
        [SerializeField] private bool debugMode;
        [SerializeField] private bool previewFeatures;
        [SerializeField] private bool networkStats;
        [SerializeField] private bool tutorial;
        [SerializeField] private bool worldChat;
        [SerializeField] private bool footsteps = true;
        [SerializeField] private bool map = true;
        [SerializeField] private bool exportAll = false;

        [Header("Controls")]
        [SerializeField, Range(0, 3)] private float sensitivityHorizontal;
        [SerializeField, Range(0, 3)] private float sensitivityVertical;
        [SerializeField] private bool invertHorizontal;
        [SerializeField] private bool invertVertical;
        [SerializeField] private JoystickType joystick = JoystickType.Floating;
        [SerializeField] private float joystickPositionHorizontal = 0.2f;
        [SerializeField] private float joystickPositionVertical = 0.4f;
        [SerializeField] private float interfaceScale = 1f;
        [SerializeField] private bool flipButton = false;
        #endregion

        #region Properties
        public Resolution Resolution
        {
            get => new Resolution()
            {
                width = displaySize.x,
                height = displaySize.y,
                refreshRate = refreshRate
            };
            set
            {
                displaySize = new Vector2Int(value.width, value.height);
                refreshRate = value.refreshRate;
            }
        }
        public bool Fullscreen
        {
            get => fullscreen;
            set => fullscreen = value;
        }
        public bool VSync
        {
            get => vSync;
            set => vSync = value;
        }
        public int TargetFrameRate
        {
            get => targetFrameRate;
            set => targetFrameRate = value;
        }
        public float ScreenScale
        {
            get => screenScale;
            set => screenScale = value;
        }

        public CreatureMeshQualityType CreatureMeshQuality
        {
            get => creatureMeshQuality;
            set => creatureMeshQuality = value;
        }
        public ShadowQualityType ShadowQuality
        {
            get => shadowQuality;
            set => shadowQuality = value;
        }
        public TextureQualityType TextureQuality
        {
            get => textureQuality;
            set => textureQuality = value;
        }
        public AmbientOcclusionType AmbientOcclusion
        {
            get => ambientOcclusion;
            set => ambientOcclusion = value;
        }
        public AntialiasingType Antialiasing
        {
            get => antialiasing;
            set => antialiasing = value;
        }
        public ScreenSpaceReflectionsType ScreenSpaceReflections
        {
            get => screenSpaceReflections;
            set => screenSpaceReflections = value;
        }
        public FoliageType Foliage
        {
            get => foliage;
            set => foliage = value;
        }
        public bool AmbientParticles
        {
            get => ambientParticles;
            set => ambientParticles = value;
        }
        public bool Reflections
        {
            get => reflections;
            set => reflections = value;
        }
        public bool AnisotropicFiltering
        {
            get => anisotropicFiltering;
            set => anisotropicFiltering = value;
        }
        public bool Bloom
        {
            get => bloom;
            set => bloom = value;
        }
        public bool DepthOfField
        {
            get => depthOfField;
            set => depthOfField = value;
        }
        public bool MotionBlur
        {
            get => motionBlur;
            set => motionBlur = value;
        }
        public bool OptimizeCreatures
        {
            get => optimizeCreatures;
            set => optimizeCreatures = value;
        }
        public float CreatureRenderDistance
        {
            get => creatureRenderDistance;
            set => creatureRenderDistance = value;
        }

        public float MasterVolume
        {
            get => masterVolume;
            set => masterVolume = Mathf.Clamp01(value);
        }
        public float MusicVolume
        {
            get => musicVolume;
            set => musicVolume = Mathf.Clamp01(value);
        }
        public float SoundEffectsVolume
        {
            get => soundEffectsVolume;
            set => soundEffectsVolume = Mathf.Clamp01(value);
        }
        public InGameMusicType InGameMusic
        {
            get => inGameMusic;
            set => inGameMusic = value;
        }
        public string InGameMusicId
        {
            get
            {
                if (InGameMusic == InGameMusicType.None)
                {
                    return null;
                }
                else
                {
                    return $"in-game_{InGameMusic}".ToLower();
                }
            }
        }

        public string OnlineUsername
        {
            get => onlineUsername;
            set => onlineUsername = value;
        }
        public List<CreatureData> CreaturePresets
        {
            get => creaturePresets;
        }
        public int ExportPrecision
        {
            get => exportPrecision;
            set => exportPrecision = value;
        }
        public List<string> HiddenBodyParts
        {
            get => hiddenBodyParts;
        }
        public List<string> HiddenPatterns
        {
            get => hiddenPatterns;
        }
        public string Locale
        {
            get => locale;
            set => locale = value;
        }
        public float TouchOffset
        {
            get => touchOffset;
            set => touchOffset = value;
        }
        public bool CameraShake
        {
            get => cameraShake;
            set => cameraShake = value;
        }
        public bool Vibrations
        {
            get => vibrations;
            set => vibrations = value;
        }
        public bool DebugMode
        {
            get => debugMode;
            set => debugMode = value;
        }
        public bool PreviewFeatures
        {
            get => previewFeatures;
            set => previewFeatures = value;
        }
        public bool NetworkStats
        {
            get => networkStats;
            set => networkStats = value;
        }
        public bool Tutorial
        {
            get => tutorial;
            set => tutorial = value;
        }
        public bool WorldChat
        {
            get => worldChat;
            set => worldChat = value;
        }
        public bool Footsteps
        {
            get => footsteps;
            set => footsteps = value;
        }
        public bool Map
        {
            get => map;
            set => map = value;
        }
        public bool ExportAll
        {
            get => exportAll;
            set => exportAll = value;
        }

        public float SensitivityHorizontal
        {
            get => sensitivityHorizontal;
            set => sensitivityHorizontal = value;
        }
        public float SensitivityVertical
        {
            get => sensitivityVertical;
            set => sensitivityVertical = value;
        }
        public bool InvertHorizontal
        {
            get => invertHorizontal;
            set => invertHorizontal = value;
        }
        public bool InvertVertical
        {
            get => invertVertical;
            set => invertVertical = value;
        }

        public JoystickType Joystick
        {
            get => joystick;
            set => joystick = value;
        }
        public float JoystickPositionHorizontal
        {
            get => joystickPositionHorizontal;
            set => joystickPositionHorizontal = value;
        }
        public float JoystickPositionVertical
        {
            get => joystickPositionVertical;
            set => joystickPositionVertical = value;
        }
        public float InterfaceScale
        {
            get => interfaceScale;
            set => interfaceScale = value;
        }
        public bool FlipButton
        {
            get => flipButton;
            set => flipButton = value;
        }
        #endregion

        #region Methods
        public override void Revert()
        {
            // Defaults
            Resolution = Screen.currentResolution;
            Fullscreen = true;
            VSync = false;
            TargetFrameRate = -1;
            ScreenScale = 1f;

            CreatureMeshQuality = CreatureMeshQualityType.High;
            ShadowQuality = ShadowQualityType.Medium;
            AmbientOcclusion = AmbientOcclusionType.MSVO;
            TextureQuality = TextureQualityType.VeryHigh;
            Antialiasing = AntialiasingType.FXAA;
            ScreenSpaceReflections = ScreenSpaceReflectionsType.None;
            Foliage = FoliageType.Medium;
            AmbientParticles = true;
            Reflections = false;
            AnisotropicFiltering = true;
            Bloom = true;
            DepthOfField = false;
            MotionBlur = false;
            OptimizeCreatures = false;
            CreatureRenderDistance = 50f;

            MasterVolume = 1f;
            MusicVolume = 0.5f;
            SoundEffectsVolume = 0.75f;
            InGameMusic = InGameMusicType.WistfulHarp;

            OnlineUsername = "";
            CreaturePresets.Clear();
            ExportPrecision = 3;
            TouchOffset = 100;
            CameraShake = true;
            Vibrations = true;
            DebugMode = false;
            PreviewFeatures = false;
            NetworkStats = true;
            Tutorial = true;
            WorldChat = true;
            Map = true;
            Footsteps = true;
            ExportAll = false;

            SensitivityHorizontal = 1f;
            SensitivityVertical = 1f;
            InvertHorizontal = false;
            InvertVertical = false;
            FlipButton = false;

            Joystick = JoystickType.Floating;
            JoystickPositionHorizontal = 0.2f;
            JoystickPositionVertical = 0.4f;
            InterfaceScale = 1f;

            // Overrides
            if (SystemUtility.IsDevice(DeviceType.Handheld))
            {
                TargetFrameRate = 60;
                TextureQuality = TextureQualityType.High;
                ShadowQuality = ShadowQualityType.None;
                Foliage = FoliageType.VeryLow;
                AmbientOcclusion = AmbientOcclusionType.None;
                Bloom = false;
                AmbientParticles = false;
                ScreenScale = Mathf.Min(720f / Display.main.systemHeight, 0.75f); // Minimum: 720p or 75%
                VSync = true;
                CreatureRenderDistance = 25f;
                FlipButton = true;

                if (SystemUtility.IsLowEndDevice)
                {
                    TargetFrameRate = 30;
                    ScreenScale = Mathf.Min(0.5f, ScreenScale);
                    ShadowQuality = ShadowQualityType.None;
                    TextureQuality = TextureQualityType.Medium;
                }
                
                TouchOffset *= ScreenScale;
                SensitivityHorizontal = SensitivityVertical = 1f / ScreenScale;
            }
        }
        #endregion

        #region Enums
        public enum PresetType
        {
            Custom,
            VeryLow,
            Low,
            Medium,
            High,
            VeryHigh
        }
        public enum CreatureMeshQualityType
        {
            Low,
            Medium,
            High
        }
        public enum ShadowQualityType
        {
            None,
            Low,
            Medium,
            High,
            VeryHigh
        }
        public enum TextureQualityType
        {
            VeryLow,
            Low,
            Medium,
            High,
            VeryHigh
        }
        public enum AmbientOcclusionType
        {
            None,
            SAO,
            MSVO
        }
        public enum AntialiasingType
        {
            None,
            FXAA,
            LowSMAA,
            MediumSMAA,
            HighSMAA,
            Temporal
        }
        public enum ScreenSpaceReflectionsType
        {
            None,
            Low,
            Medium,
            High,
            VeryHigh
        }
        public enum FoliageType
        {
            VeryLow,
            Low,
            Medium,
            High,
            VeryHigh
        }
        public enum InGameMusicType
        {
            None,
            WistfulHarp,
        }
        public enum JoystickType
        {
            Fixed,
            Floating
        }
        #endregion
    }
}