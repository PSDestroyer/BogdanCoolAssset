
using HalvaStudio.Save;
using TMPro;

namespace GenesisStudio
{
    public class MoneyManager
    {
        private TMP_Text _text;
        public int Money
        {
            get => SaveManager.Instance.saveData.money;
            set
            {
                if (value < 0) value = 0;

                int newValue = value - SaveManager.Instance.saveData.money;
                
                char sign = newValue > 0 ? '+' : '-';
                
                // GameManager.Instance.AddFloatingText(newValue, sign);
                
                SaveManager.Instance.saveData.money = value;
                if(_text != null)
                    _text.text = "$" + value;
            }
        }


        public MoneyManager(int money, TMP_Text UI_Text)
        {
            Money = money;
            _text = UI_Text;
            _text.text = "$" + Money;
        }
    }
    
    
}