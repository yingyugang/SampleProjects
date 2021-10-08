using UnityEngine;
using System.Collections;
namespace BaPK
{
    public class Label : MonoBehaviour
    {
        private BitmapFont bitmapFont;
        private string text;
        private float kerning;
        private float space;

        private int layer = -1;
        private string sortingLayer = "";
        private int sortingOrderInLayer = 0;
        private Color color = new Color(1, 1, 1, 1);
        private TextAlignment alignment = TextAlignment.Left;
        private float scaleX = 1;
        private float scaleY = 1;

        public void createLabel(BitmapFont font)
        {
            this.text = "";
            this.kerning = 0;
            this.space = 15;
            this.bitmapFont = new BitmapFont(font, gameObject);
            this.build();
        }

        public void createLabel(BitmapFont font, string text, float kerning, float space)
        {
            this.text = text;
            this.kerning = kerning;
            this.space = space;
            this.bitmapFont = new BitmapFont(font, gameObject);
            this.build();
        }

        public void createLabel(BitmapFont font, string text)
        {
            this.text = text;
            this.kerning = 0;
            this.space = 15;
            this.bitmapFont = new BitmapFont(font, gameObject);
            this.build();
        }

        private void build()
        {
            this.bitmapFont.setParams(text, kerning, space);
            this.bitmapFont.setColor(color);
            this.bitmapFont.setScale(scaleX, scaleY);
            this.bitmapFont.setAlignment(alignment);
            this.bitmapFont.setLayer(layer);
            this.bitmapFont.setSortingLayer(sortingLayer);
            this.bitmapFont.setSortingOrderInlayer(sortingOrderInLayer);
            this.bitmapFont.build();
        }

        public void refresh()
        {
            if (bitmapFont != null)
            {
                build();
            }
        }

        public void setText(string text)
        {
            this.text = text;
        }

        public void setLayer(int layer)
        {
            this.layer = layer;
        }

        public void setSortingLayer(string sortingLayer)
        {
            this.sortingLayer = sortingLayer;
        }

        public void setSortingOrderInLayer(int orderInLayer)
        {
            this.sortingOrderInLayer = orderInLayer;
        }

        public void setScale(float scaleX, float scaleY)
        {
            this.scaleX = scaleX;
            this.scaleY = scaleY;
        }

        public void setColor(Color color)
        {
            this.color = color;
        }

        public void setAlignment(TextAlignment alignment)
        {
            this.alignment = alignment;
        }
    }
}