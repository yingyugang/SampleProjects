using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.IO;
namespace BaPK
{
    public class BitmapFont
    {

        private Dictionary<string, Sprite> sprites;
        private Dictionary<string, Vector2> yoffsets_xadvances;
        private Dictionary<string, Rect> rects;

        private GameObject labelObject;
        private GameObject fontObject;

        private Color color;
        private TextAlignment alignment;
        private float kerning;
        private float space;
        private float scaleX;
        private float scaleY;

        private int layer;
        private string sortingLayer;
        private int sortingOrderInLayer;

        private string text;

        public BitmapFont(BitmapFont bitmapFont, GameObject labelObject)
        {
            this.labelObject = labelObject;
            this.sprites = bitmapFont.getSprites();
            this.yoffsets_xadvances = bitmapFont.get_YOffset_XAdvance();
            this.rects = bitmapFont.getRects();
        }

        public BitmapFont(string pathPNG, string pathXML)
        {
            sprites = new Dictionary<string, Sprite>();
            yoffsets_xadvances = new Dictionary<string, Vector2>();
            rects = new Dictionary<string, Rect>();

            Texture2D texture = Resources.Load<Texture2D>(pathPNG);
            float heightTexture = texture.height;
            TextAsset xml = Resources.Load<TextAsset>(pathXML);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(xml.text));

            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                XmlAttributeCollection collection = node.Attributes;
                Rect rect = new Rect(
                    float.Parse(collection.Item(0).Value), //left
                    heightTexture - float.Parse(collection.Item(1).Value) - float.Parse(collection.Item(3).Value), //top
                    float.Parse(collection.Item(2).Value), //width
                    float.Parse(collection.Item(3).Value));//height

                rects.Add(collection.Item(6).Value, rect);//rects

                sprites.Add(collection.Item(6).Value, Sprite.Create(texture, rect, Vector2.zero));//sprites

                //yoffset and xadvance
                yoffsets_xadvances.Add(collection.Item(6).Value,
                    new Vector2(float.Parse(collection.Item(4).Value), float.Parse(collection.Item(5).Value)));
            }
        }

        public BitmapFont(string pathPNG, string[] lines)
        {
            sprites = new Dictionary<string, Sprite>();
            yoffsets_xadvances = new Dictionary<string, Vector2>();
            rects = new Dictionary<string, Rect>();

            Texture2D texture = Resources.Load<Texture2D>(pathPNG);
            float heightTexture = texture.height;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] items = line.Split(' ');

                Rect rect = new Rect(
                    float.Parse(items[0]), //left
                    heightTexture - float.Parse(items[1]) - float.Parse(items[3]), //top
                    float.Parse(items[2]), //width
                    float.Parse(items[3]));//height

                rects.Add(items[6], rect);//rects

                sprites.Add(items[6], Sprite.Create(texture, rect, Vector2.zero));//sprites

                //yoffset and xadvance
                yoffsets_xadvances.Add(items[6],
                    new Vector2(float.Parse(items[4]), float.Parse(items[5])));
            }
        }

        public BitmapFont(Texture2D texture, string pathXML)
        {
            sprites = new Dictionary<string, Sprite>();
            yoffsets_xadvances = new Dictionary<string, Vector2>();
            rects = new Dictionary<string, Rect>();

            float heightTexture = texture.height;
            TextAsset xml = Resources.Load<TextAsset>(pathXML);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(xml.text));

            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                XmlAttributeCollection collection = node.Attributes;
                Rect rect = new Rect(
                    float.Parse(collection.Item(0).Value), //left
                    heightTexture - float.Parse(collection.Item(1).Value) - float.Parse(collection.Item(3).Value), //top
                    float.Parse(collection.Item(2).Value), //width
                    float.Parse(collection.Item(3).Value));//height

                rects.Add(collection.Item(6).Value, rect);//rects

                sprites.Add(collection.Item(6).Value, Sprite.Create(texture, rect, Vector2.zero));//sprites

                //yoffset and xadvance
                yoffsets_xadvances.Add(collection.Item(6).Value,
                    new Vector2(float.Parse(collection.Item(4).Value), float.Parse(collection.Item(5).Value)));
            }
        }

        public BitmapFont(Texture2D texture, string[] lines)
        {
            sprites = new Dictionary<string, Sprite>();
            yoffsets_xadvances = new Dictionary<string, Vector2>();
            rects = new Dictionary<string, Rect>();

            float heightTexture = texture.height;
            
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] items = line.Split(' ');

                Rect rect = new Rect(
                    float.Parse(items[0]), //left
                    heightTexture - float.Parse(items[1]) - float.Parse(items[3]), //top
                    float.Parse(items[2]), //width
                    float.Parse(items[3]));//height
                rects.Add(items[6], rect);//rects
                sprites.Add(items[6], Sprite.Create(texture, rect, Vector2.zero));//sprites
              
                //yoffset and xadvance
                yoffsets_xadvances.Add(items[6],
                    new Vector2(float.Parse(items[4]), float.Parse(items[5])));
            }
        }

        public void build()
        {
            float k = kerning / 100;
            float sp = space / 100;

            removeChildren();

            if (text.Trim().Equals(""))
            {
                return;
            }

            this.fontObject = new GameObject("BitmapFont");
            this.fontObject.transform.parent = labelObject.transform;
            this.fontObject.transform.localPosition = new Vector3(0, 0, 0);
            if (layer != -1)
                this.fontObject.layer = layer;

            float dis;
            float width = 0;

            {
                string c = text[0].ToString();
                GameObject go = new GameObject(c);
                SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprites[c];
                spriteRenderer.color = color;
                go.transform.parent = fontObject.transform;
                if (layer != -1)
                    go.layer = layer;
                if (sortingLayer != "")
                    spriteRenderer.sortingLayerName = sortingLayer;
                spriteRenderer.sortingOrder = sortingOrderInLayer;
                Vector2 p = yoffsets_xadvances[c];
                //go.transform.localPosition = new Vector3(width, -p.x / 100 - rects[c].height / 100, 0);
                go.transform.localPosition = new Vector3(width, 0, 0);
                width += p.y / 100 + k;
                dis = (p.x + rects[c].height) / 100;
            }

            for (int i = 1; i < text.Length; i++)
            {
                string c = text[i].ToString();
                if (c == " ")
                {
                    width += sp;
                }
                else
                {
                    GameObject go = new GameObject(c);
                    SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = sprites[c];
                    spriteRenderer.color = color;
                    go.transform.parent = fontObject.transform;
                    if (layer != -1)
                        go.layer = layer;
                    if (sortingLayer != "")
                        spriteRenderer.sortingLayerName = sortingLayer;
                    spriteRenderer.sortingOrder = sortingOrderInLayer;
                    Rect rect = rects[c];
                    Vector2 p = yoffsets_xadvances[c];
                    go.transform.localPosition = new Vector3(width, -p.x / 100 - rects[c].height / 100 + dis, 0);
                    width += p.y / 100 + k;
                }
            }

            //Alignment
            if (alignment == TextAlignment.Center)
            {
                fontObject.transform.localPosition = new Vector3(-width * scaleX / 2, 0, 0);
            }
            else if (alignment == TextAlignment.Right)
            {
                fontObject.transform.localPosition = new Vector3(-width * scaleX, 0, 0);
            }
            //scale
            fontObject.transform.localScale = new Vector3(scaleX, scaleY, fontObject.transform.localScale.z);
        }

        private void removeChildren()
        {
            if (labelObject.transform.childCount > 0)
            {
                Transform[] children = fontObject.GetComponentsInChildren<Transform>(true);
                for (int i = 0; i < children.Length; i++)
                    Object.Destroy(children[i].gameObject);
            }
        }

        public void setLayer(int layer)
        {
            this.layer = layer;
        }

        public void setSortingLayer(string sortingLayer)
        {
            this.sortingLayer = sortingLayer;
        }

        public void setSortingOrderInlayer(int orderInLayer )
        {
            this.sortingOrderInLayer = orderInLayer;
        }

        public void setAlignment(TextAlignment alignment)
        {
            this.alignment = alignment;
        }

        public void setColor(Color color)
        {
            this.color = color;
        }

        public void setScale(float scaleX, float scaleY)
        {
            this.scaleX = scaleX;
            this.scaleY = scaleY;
        }

        public void setKerning(float kerning)
        {
            this.kerning = kerning;
        }

        public void setSpace(float space)
        {
            this.space = space;
        }

        public void setText(string text)
        {
            this.text = text;
        }

        public void setParams(string text, float kerning, float space)
        {
            this.text = text;
            this.kerning = kerning;
            this.space = space;
        }

        public Dictionary<string, Sprite> getSprites()
        {
            return this.sprites;
        }

        public Dictionary<string, Vector2> get_YOffset_XAdvance()
        {
            return this.yoffsets_xadvances;
        }

        public Dictionary<string, Rect> getRects()
        {
            return this.rects;
        }
    }
}