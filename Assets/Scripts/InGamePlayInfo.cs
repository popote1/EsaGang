using UnityEngine;
using UnityEngine.UI;

public class InGamePlayInfo : MonoBehaviour
{

   public Image Panel;
   public Text TxtHP;
   public Slider SliderHP;
   public Image SpriteTete;

   public Sprite[] faces;



   public void SetHP(int currentHP)
   {
      TxtHP.text = currentHP + " / 10";
      SliderHP.value = (float)currentHP / 10;
   }

   public void SetPlayerColor(Color color)
   {
      Panel.color = color;
   }
   
}
