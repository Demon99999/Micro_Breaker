namespace Assets.Scripts.UI.AdditionalImprovement
{
    public class AdditionalImprovementValue
    {
        private const int MaxSelectValue = 1;
        private const int MinSelectValue = 0;

        public string AdditionalImprovementName { get; private set; }

        public int Value { get; private set; }

        public bool IsSelect { get; private set; }

        public AdditionalImprovementValue(string additionalImprovementName, int value, int selectValue)
        {
            AdditionalImprovementName = additionalImprovementName;
            Value = value;
            IsSelect = true ? selectValue == MaxSelectValue : selectValue == MinSelectValue;
        }

        public void SetSelect(bool isSelect)
        {
            IsSelect = isSelect;
        }

        public int GetSelectValue()
        {
            return IsSelect ? MaxSelectValue : MinSelectValue;
        }
    }
}