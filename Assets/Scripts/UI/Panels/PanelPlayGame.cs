using Assets.Scripts.LocationLogic;
using Assets.Scripts.SaveLoadServices;
using Assets.Scripts.UI.Buttons;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Panels
{
    public class PanelPlayGame : Panel
    {
        private const int MaxValue = 1;
        private const int MinValue = 0;
        private const string Access = "IS NOT ACCESS";
        private const string Level = "Level: ";

        [SerializeField] private ButtonPlayGame _buttonPlayGame;
        [SerializeField] private ButtonPanelInteraction _buttonClose;
        [SerializeField] private ButtonAdditionalImprovement _firstButtonUpgrade;
        [SerializeField] private ButtonAdditionalImprovement _secondButtonUpgrade;
        [SerializeField] private LocationChooseInput _locationChooseInput;
        [SerializeField] private LocationCreateView _locationCreateView;
        [SerializeField] private SaveService _saveService;
        [SerializeField] private TMP_Text _textAccess;
        [SerializeField] private TMP_Text _textName;

        public bool IsInit { get; private set; }

        public bool IsOpen { get; private set; }

        public LocationObject LocationObject { get; private set; }

        public ButtonPanelInteraction ButtonClose => _buttonClose;

        public ButtonPlayGame ButtonPlayGame => _buttonPlayGame;

        private void OnEnable()
        {
            _buttonPlayGame.Clicked += OnClick;
        }

        private void OnDisable()
        {
            _buttonPlayGame.Clicked -= OnClick;
        }

        public override async void OnMove(bool isActive)
        {
            if (IsOpen == isActive)
                return;

            IsOpen = isActive;
            base.OnMove(isActive);
            await MovePanel(isActive);
            _locationCreateView.gameObject.SetActive(isActive);
        }

        public void Init(LocationObject locationObject)
        {
            LocationObject = locationObject;
            _textName.text = $"{Level} {locationObject.Index}";

            if (LocationObject.AdditionaValue != string.Empty)
                _textName.text = $"{Level} {locationObject.Index}{locationObject.AdditionaValue}";

            if (LocationObject.IsActive == false)
            {
                SetAccess(false);
                return;
            }

            SetAccess(true);
            IsInit = true;
        }

        private void SetAccess(bool isAccess)
        {
            _firstButtonUpgrade.gameObject.SetActive(isAccess);
            _secondButtonUpgrade.gameObject.SetActive(isAccess);
            _buttonPlayGame.gameObject.SetActive(isAccess);
            _textAccess.gameObject.SetActive(!isAccess);
            _textAccess.color = Color.red;
            _textAccess.text = Access;
        }

        private void OnClick()
        {
            LevelData locationObjectData = new LevelData()
            {
                LocationName = LocationObject.Name.ToString(),
                AdditionaValue = LocationObject.AdditionaValue.ToString(),
                Passed = LocationObject.IsPassed ? MaxValue : MinValue,
            };

            _saveService.SaveCurrentLevelData(locationObjectData);
        }
    }
}