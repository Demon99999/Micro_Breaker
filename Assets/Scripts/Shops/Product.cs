using System;

namespace Assets.Scripts.Shops
{
    public class Product
    {
        public event Action<Product> Selected;

        public Template Template { get; private set; }

        public string Name { get; private set; }

        public int Price { get; private set; }

        public bool IsBuy { get; private set; } = false;

        public bool IsSelected { get; private set; } = false;

        public Product(Template template)
        {
            Name = template.Name;
            Price = template.Price;
            Template = template;
        }

        public void Buy() => IsBuy = true;

        public void SetStatusOfTheSelected(bool isSelected) => IsSelected = isSelected;

        public void Select() => Selected?.Invoke(this);
    }
}