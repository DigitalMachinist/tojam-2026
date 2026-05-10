using UnityEngine;
using UnityEngine.UI;

public class UISelectable : MonoBehaviour
{
    [SerializeField] Image backing;
    [SerializeField] Image shadow;
    [SerializeField] GameObject glyph;
    [SerializeField] Color unselectedColor = new Color(0,0,0,0);
    [SerializeField] Color unselectedShadowColor = new Color( 0, 0, 0, 0 );
    [SerializeField] Color selectedColor = new Color( 0, 0, 0, 0 );
    [SerializeField] Color selectedShadowColor = new Color( 0, 0, 0, 0 );
    public void SetThemeColor( Color color1, Color color2) 
    {
        backing.color = color1;
        shadow.color = color2;
    }

    public void GainFocus() 
    {
        SetThemeColor( selectedColor, selectedShadowColor );
        glyph.SetActive( true );
    }

    public void LoseFocus() 
    {
        SetThemeColor( unselectedColor, unselectedShadowColor );
        glyph.SetActive( false );
    }
}