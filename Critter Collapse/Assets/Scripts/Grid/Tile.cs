using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private GameObject highlight;

    // if this is an offset tile, change its color on init
    public void Init(bool isOffset)
    {
        renderer.color = isOffset ? offsetColor : baseColor; 
    }

    // when we hover over this tile, set the highlight color active
    private void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    // when we are not hovering over this tile, set the highlight color inactive
    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }
}
