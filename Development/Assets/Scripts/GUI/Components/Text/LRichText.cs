/****************************************************************************
Copyright (c) 2015 Lingjijian

Created by Lingjijian on 2015

342854406@qq.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using System;

namespace XGUI
{

    public enum RichType
    {
        TEXT,
        IMAGE,
        ANIM,
        NEWLINE,
    }

    public enum RichAlignType
    {
        DESIGN_CENTER,
        LEFT_TOP,
    }

    class LRichElement : UnityEngine.Object
    {
        public RichType type { get; protected set; }
        public Color color { get; protected set; }
        public string data { get; protected set; }
    }
    /// <summary>
    /// 文本元素
    /// </summary>
    class LRichElementText : LRichElement
    {
        public string txt { get; protected set; }
        public bool isUnderLine { get; protected set; }
        public bool isOutLine { get; protected set; }
        public bool isGradient { get; protected set; }
        public int fontSize { get; protected set; }
        public Color outLineColor { get; protected set; }
        public Color topColor { get; protected set; }
        public Color bottomColor { get; protected set; }

        public LRichElementText(Color color, string txt, int fontSize, bool isUnderLine, bool isOutLine,Color outLineColor,bool isGradient, Color topColor,Color bottomColor, string data)
        {
            this.type = RichType.TEXT;
            this.color = color;
            this.txt = txt;
            this.fontSize = fontSize;
            this.isUnderLine = isUnderLine;
            this.isOutLine = isOutLine;
            this.outLineColor = outLineColor;
            this.isGradient = isGradient;
            this.topColor = topColor;
            this.bottomColor = bottomColor;
            this.data = data;
        }
    }

    /// <summary>
    /// 图片元素
    /// </summary>
    class LRichElementImage : LRichElement
    {
        public string path { get; protected set; }
        public float customWidth;
        public float customHeight;

        public LRichElementImage(string path, string data,float customWidth,float customHeight)
        {
            this.type = RichType.IMAGE;
            this.path = path;
            this.data = data;
            this.customWidth = customWidth;
            this.customHeight = customHeight;
        }
    }

    /// <summary>
    /// 动画元素
    /// </summary>
    class LRichElementAnim : LRichElement
    {
        public string path { get; protected set; }
        public float fs { get; protected set; }
        public float customWidth;
        public float customHeight;

        public LRichElementAnim(string path, float fs, string data,float customWidth,float customHeight)
        {
            this.type = RichType.ANIM;
            this.path = path;
            this.data = data;
            this.fs = fs;
            this.customWidth = customWidth;
            this.customHeight = customHeight;
        }
    }

    /// <summary>
    /// 换行元素
    /// </summary>
    class LRichElementNewline : LRichElement
    {
        public LRichElementNewline()
        {
            this.type = RichType.NEWLINE;
        }
    }

    /// <summary>
    /// 缓存结构
    /// </summary>
    class LRichCacheElement : UnityEngine.Object
    {
        public bool isUse;
        public GameObject node;
        public LRichCacheElement(GameObject node)
        {
            this.node = node;
        }
    }

    /// <summary>
    /// 渲染结构
    /// </summary>
    struct LRenderElement
    {
        public RichType type;
        public string strChar;
        public int width;
        public int height;
        public bool isOutLine;
        public bool isUnderLine;
        public Color outLineColor;
        public Font font;
        public int fontSize;
        public Color color;
        public string data;
        public string path;
        public float fs;
        public bool isNewLine;
        public bool isGradient;
        public Vector2 pos;
        public Color topColor;
        public Color bottomColor;

        public LRenderElement Clone()
        {
            LRenderElement cloneObj;
            cloneObj.type = this.type;
            cloneObj.strChar = this.strChar;
            cloneObj.width = this.width;
            cloneObj.height = this.height;
            cloneObj.isOutLine = this.isOutLine;
            cloneObj.isUnderLine = this.isUnderLine;
            cloneObj.outLineColor = this.outLineColor;
            cloneObj.font = this.font;
            cloneObj.fontSize = this.fontSize;
            cloneObj.color = this.color;
            cloneObj.data = this.data;
            cloneObj.path = this.path;
            cloneObj.fs = this.fs;
            cloneObj.isNewLine = this.isNewLine;
            cloneObj.pos = this.pos;
            cloneObj.isGradient = this.isGradient;
			cloneObj.topColor = this.topColor;
			cloneObj.bottomColor = this.bottomColor;
            return cloneObj;
        }

		public bool isSameStyle(LRenderElement elem)
		{
			return (this.color 			== elem.color &&
				    this.isOutLine 		== elem.isOutLine &&
					this.isUnderLine 	== elem.isUnderLine &&
                    this.outLineColor   == elem.outLineColor &&
                    this.isGradient     == elem.isGradient &&
                    this.topColor       == elem.topColor && 
                    this.bottomColor    == elem.bottomColor &&
                    this.font 			== elem.font &&
					this.fontSize 		== elem.fontSize &&
					this.data 			== elem.data);
		}
    }

    /// <summary>
    /// 富文本
    /// </summary>
    public class LRichText : MonoBehaviour, IPointerClickHandler
    {
        public RichAlignType alignType;
        public int verticalSpace;
        public int maxLineWidth;
        public Font font;

        public UnityAction<string> onClickHandler;
        public int realLineHeight { get; protected set; }
        public int realLineWidth { get; protected set; }
		private bool _hasFontSetting;
        private int _lastFontSize;
        private TextGenerationSettings _fontSetting;
        private TextGenerator _fontGen;

        List<LRichElement> _richElements;
        List<LRenderElement> _elemRenderArr;
        List<LRichCacheElement> _cacheLabElements;
        List<LRichCacheElement> _cacheImgElements;
        List<LRichCacheElement> _cacheFramAnimElements;
        Dictionary<GameObject, string> _objectDataMap;
        //custom content parser setting
        public int defaultLabSize = 20;
        public string defaultLabColor = "#ff00ff";
        public bool raycastTarget = false;

        [TextArea(3, 10)][SerializeField] protected string m_Text = String.Empty;

        public string text
        {
            get { return m_Text; }
            set
            {
                m_Text = value;
                parseRichDefaultString(m_Text);
            }
        }

        private RectTransform m_RectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (m_RectTransform == null)
                {
                    m_RectTransform = GetComponent<RectTransform>();
                }
                return m_RectTransform;
            }
        }

        public void removeAllElements()
        {
            int len = _cacheLabElements.Count;
            for (int i=0;i< len;i++)
            {
                _cacheLabElements[i].isUse = false;
                _cacheLabElements[i].node.gameObject.SetActive(false);
            }
            len = _cacheImgElements.Count;
            for (int i = 0; i < len; i++)
            {
                _cacheImgElements[i].isUse = false;
                _cacheImgElements[i].node.gameObject.SetActive(false);
            }
            len = _cacheFramAnimElements.Count;
            for (int i = 0; i < len; i++)
            {
                _cacheFramAnimElements[i].isUse = false;
                _cacheFramAnimElements[i].node.gameObject.SetActive(false);
            }
            _elemRenderArr.Clear();
            _objectDataMap.Clear();
        }

        public void insertElement(string txt, Color color, int fontSize, bool isUnderLine, bool isOutLine, Color outLineColor,bool isGradient, Color topColor,Color bottomColor, string data)
        {
            _richElements.Add(new LRichElementText(color, txt, fontSize, isUnderLine, isOutLine, outLineColor, isGradient, topColor, bottomColor, data));
        }

        public void insertElement(string path, float fp, string data,float customWidth,float customHeight)
        {
            _richElements.Add(new LRichElementAnim(path, fp, data,customWidth,customHeight));
        }

        public void insertElement(string path, string data,float customWidth,float customHeight)
        {
            _richElements.Add(new LRichElementImage(path, data,customWidth,customHeight));
        }

        public void insertElement(int newline)
        {
            _richElements.Add(new LRichElementNewline());
        }

        public LRichText()
        {
            this.alignType = RichAlignType.LEFT_TOP;
            this.verticalSpace = 0;
            this.maxLineWidth = 300;

            _richElements = new List<LRichElement>();
            _elemRenderArr = new List<LRenderElement>();
            _cacheLabElements = new List<LRichCacheElement>();
            _cacheImgElements = new List<LRichCacheElement>();
            _cacheFramAnimElements = new List<LRichCacheElement>();
            _objectDataMap = new Dictionary<GameObject, string>();
        }

        public void reloadData()
        {
            this.removeAllElements();

            RectTransform rtran = this.GetComponent<RectTransform>();

            rtran.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);

            int len = _richElements.Count;
            for (int i =0;i< len;i++)
            {
                LRichElement elem = _richElements[i];
                if (elem.type == RichType.TEXT)
                {
                    LRichElementText elemText = elem as LRichElementText;
                    char[] _charArr = elemText.txt.ToCharArray();

                    if(_fontGen == null)
                        _fontGen = new TextGenerator();

                    foreach (char strChar in _charArr)
                    {
                        LRenderElement rendElem = new LRenderElement();
                        rendElem.type = RichType.TEXT;
                        rendElem.strChar = strChar.ToString();
                        rendElem.isOutLine = elemText.isOutLine;
                        rendElem.isUnderLine = elemText.isUnderLine;
                        rendElem.outLineColor = elemText.outLineColor;
                        rendElem.isGradient = elemText.isGradient;
                        rendElem.topColor = elemText.topColor;
                        rendElem.bottomColor = elemText.bottomColor;
                        rendElem.font = this.font;
                        rendElem.fontSize = elemText.fontSize;
                        rendElem.data = elemText.data;
                        rendElem.color = elemText.color;

                        if(!_hasFontSetting || _lastFontSize != elemText.fontSize ){
                            _fontSetting = new TextGenerationSettings();
                            _fontSetting.font = this.font;
                            _fontSetting.fontSize = elemText.fontSize;
                            _fontSetting.lineSpacing = 0;
                            _fontSetting.scaleFactor = 1;
                            _fontSetting.verticalOverflow = VerticalWrapMode.Overflow;
                            _fontSetting.horizontalOverflow = HorizontalWrapMode.Overflow;
							_hasFontSetting = true;
                        }

                        _lastFontSize = elemText.fontSize;

                        rendElem.width = (int)_fontGen.GetPreferredWidth(rendElem.strChar, _fontSetting);
                        rendElem.height = (int)_fontGen.GetPreferredHeight(rendElem.strChar, _fontSetting);
                        _elemRenderArr.Add(rendElem);
                    }
                }
                else if (elem.type == RichType.IMAGE)
                {
                    LRichElementImage elemImg = elem as LRichElementImage;
                    LRenderElement rendElem = new LRenderElement();
                    rendElem.type = RichType.IMAGE;
                    rendElem.path = elemImg.path;
                    rendElem.data = elemImg.data;

					//string atlas = System.IO.Path.GetDirectoryName(rendElem.path);
					//string spname = System.IO.Path.GetFileName(rendElem.path);

                    // Sprite sp = FXGame.Managers.ResourceManager.Instance.GetSpriteByName(atlas, spname);
                    // rendElem.width = (int)sp.rect.size.x;
                    // rendElem.height = (int)sp.rect.size.y;
                    rendElem.width = (int)elemImg.customWidth;
                    rendElem.height = (int)elemImg.customHeight;

                    _elemRenderArr.Add(rendElem);
                }
                else if (elem.type == RichType.ANIM)
                {
                    LRichElementAnim elemAnim = elem as LRichElementAnim;
                    LRenderElement rendElem = new LRenderElement();
                    rendElem.type = RichType.ANIM;
                    rendElem.path = elemAnim.path;
                    rendElem.data = elemAnim.data;
                    rendElem.fs = elemAnim.fs;

                    //string atlas = System.IO.Path.GetDirectoryName(rendElem.path);
                    //string spname = System.IO.Path.GetFileName(rendElem.path);

                    // Sprite sp = FXGame.Managers.ResourceManager.Instance.GetSpriteByName(atlas, spname+"01");
                    // rendElem.width = (int)sp.rect.size.x;
                    // rendElem.height = (int)sp.rect.size.y;
                    rendElem.width = (int)elemAnim.customWidth;
                    rendElem.height = (int)elemAnim.customHeight;

                    _elemRenderArr.Add(rendElem);
                }
                else if (elem.type == RichType.NEWLINE)
                {
                    LRenderElement rendElem = new LRenderElement();
                    rendElem.isNewLine = true;
                    rendElem.type = RichType.NEWLINE;
                    _elemRenderArr.Add(rendElem);
                }
            }

            _richElements.Clear();

            // UnityEngine.Profiling.Profiler.BeginSample("LRichElement:formatRenderers");
            formatRenderers();
            // UnityEngine.Profiling.Profiler.EndSample();

        }

        protected void formatRenderers()
        {
            int oneLine = 0;
            int lines = 1;
            bool isReplaceInSpace = false;
            int len = _elemRenderArr.Count;

            for (int i = 0; i < len; i++)
            {
                isReplaceInSpace = false;
                LRenderElement elem = _elemRenderArr[i];
                if (elem.isNewLine) // new line
                {
                    oneLine = 0;
                    lines++;
                    elem.width = 10;
                    elem.height = 27;
                    elem.pos = new Vector2(oneLine, -lines * 27);

                }
                else //other elements
                {
                    if (oneLine + elem.width > maxLineWidth)
                    {
                        if (elem.type == RichType.TEXT)
                        {
                            if (isChinese(elem.strChar) || elem.strChar == " ")
                            {
                                oneLine = 0;
                                lines++;

                                elem.pos = new Vector2(oneLine, -lines * 27);
                                oneLine = elem.width;
                            }
                            else // en
                            {
                                int spaceIdx = 0;
                                int idx = i;
                                while (idx > 0)
                                {
                                    idx--;
                                    if (_elemRenderArr[idx].strChar == " " &&
                                        _elemRenderArr[idx].pos.y == _elemRenderArr[i - 1].pos.y) // just for the same line
                                    {
                                        spaceIdx = idx;
                                        break;
                                    }
                                }
                                // can't find space , force new line
                                if (spaceIdx == 0)
                                {
                                    oneLine = 0;
                                    lines++;
                                    elem.pos = new Vector2(oneLine, -lines * 27);
                                    oneLine = elem.width;
                                }
                                else
                                {
                                    oneLine = 0;
                                    lines++;
                                    isReplaceInSpace = true; //reset cuting words position

                                    for (int _i = spaceIdx + 1; _i <= i; ++_i)
                                    {
                                        LRenderElement _elem = _elemRenderArr[_i];
                                        _elem.pos = new Vector2(oneLine, -lines * 27);
                                        oneLine += _elem.width;

                                        _elemRenderArr[_i] = _elem;
                                    }
                                }
                            }
                        }
                        else if (elem.type == RichType.ANIM || elem.type == RichType.IMAGE)
                        {
                            lines++;
                            elem.pos = new Vector2(0, -lines * 27);
                            oneLine = elem.width;
                        }
                    }
                    else
                    {
                        elem.pos = new Vector2(oneLine, -lines * 27);
                        oneLine += elem.width;
                    }
                }
                if (isReplaceInSpace == false)
                {
                    _elemRenderArr[i] = elem;
                }
            }
            //sort all lines
            Dictionary<int, List<LRenderElement>> rendElemLineMap = new Dictionary<int, List<LRenderElement>>();
            List<int> lineKeyList = new List<int>();
            len = _elemRenderArr.Count;
            for (int i = 0; i < len; i++)
            {
                LRenderElement elem = _elemRenderArr[i];
                List<LRenderElement> lineList;

                if (!rendElemLineMap.ContainsKey((int)elem.pos.y))
                {
                    lineList = new List<LRenderElement>();
                    rendElemLineMap[(int)elem.pos.y] = lineList;
                }
                rendElemLineMap[(int)elem.pos.y].Add(elem);
            }
            // all lines in arr
            List<List<LRenderElement>> rendLineArrs = new List<List<LRenderElement>>();
            var e = rendElemLineMap.GetEnumerator();
            while (e.MoveNext())
            {
                lineKeyList.Add(-1 * e.Current.Key);
            }
            lineKeyList.Sort();
            len = lineKeyList.Count;

            for (int i = 0; i < len; i++)
            {
                int posY = -1 * lineKeyList[i];
                string lineString = "";
				int margeWidth = 0;
                LRenderElement _lastEleme = rendElemLineMap[posY][0];
                LRenderElement _lastDiffStartEleme = rendElemLineMap[posY][0];
                if (rendElemLineMap[posY].Count > 0)
                {
                    List<LRenderElement> lineElemArr = new List<LRenderElement>();

                    int _len2 = rendElemLineMap[posY].Count;
                    for (int _i = 0; _i < _len2; _i++)
                    {
                        LRenderElement elem = rendElemLineMap[posY][_i];
                        if (_lastEleme.type == RichType.TEXT && elem.type == RichType.TEXT)
                        {
							if (_lastEleme.isSameStyle(elem))
                            {
								// the same style can mergin one element
                                lineString += elem.strChar;
								margeWidth += elem.width;
                            }
                            else // diff style
                            {
                                if (_lastDiffStartEleme.type == RichType.TEXT)
                                {
                                    LRenderElement _newElem = _lastDiffStartEleme.Clone();
                                    _newElem.strChar = lineString;
									_newElem.width = margeWidth;
                                    lineElemArr.Add(_newElem);

                                    _lastDiffStartEleme = elem;
                                    lineString = elem.strChar;
									margeWidth = elem.width;
                                }
                            }
                        }
                        else if (elem.type == RichType.IMAGE || elem.type == RichType.ANIM || elem.type == RichType.NEWLINE)
                        {
                            //interrupt
                            if (_lastDiffStartEleme.type == RichType.TEXT)
                            {
                                LRenderElement _newEleme = _lastDiffStartEleme.Clone();
                                _newEleme.strChar = lineString;
								_newEleme.width = margeWidth;
                                lineString = "";
								margeWidth = 0;
                                lineElemArr.Add(_newEleme);
                            }
                            lineElemArr.Add(elem);

                        }
                        else if (_lastEleme.type != RichType.TEXT)
                        {
                            //interrupt
                            _lastDiffStartEleme = elem;
                            if (elem.type == RichType.TEXT)
                            {
                                lineString = elem.strChar;
								margeWidth = elem.width;
                            }
                        }
                        _lastEleme = elem;
                    }
                    // the last elementText
                    if (_lastDiffStartEleme.type == RichType.TEXT)
                    {
                        LRenderElement _newElem = _lastDiffStartEleme.Clone();
                        _newElem.strChar = lineString;
						_newElem.width = margeWidth;
                        lineElemArr.Add(_newElem);
                    }
                    rendLineArrs.Add(lineElemArr);
                }
            }

            // offset position
            int _offsetLineY = 0;
			int _offsetLineX = 0;
            realLineHeight = 0;
            len = rendLineArrs.Count;
            for (int i = 0; i < len; i++)
            {
                List<LRenderElement> _lines = rendLineArrs[i];
                int _lineHeight = 0;
                int _len3 = _lines.Count;
				int _lineWidth = 0;
                for (int _i=0;_i< _len3; _i++)
                {
                    _lineHeight = Mathf.Max(this.verticalSpace,Mathf.Max(_lineHeight, _lines[_i].height));
					_lineWidth += _lines [_i].width;
                }

                realLineHeight += _lineHeight;
                _offsetLineY += (_lineHeight - 27);
				if (alignType == RichAlignType.DESIGN_CENTER) {
					_offsetLineX = (maxLineWidth - _lineWidth) / 2;
				}

                for (int j = 0; j < _len3; j++)
                {
                    LRenderElement elem = _lines[j];
					if (alignType == RichAlignType.LEFT_TOP) {
						elem.pos = new Vector2 (elem.pos.x, elem.pos.y - _offsetLineY);
					} else if (alignType == RichAlignType.DESIGN_CENTER) {
						elem.pos = new Vector2(elem.pos.x + _offsetLineX,elem.pos.y - _offsetLineY);
					}
                    realLineHeight = Mathf.Max(realLineHeight, (int)Mathf.Abs(elem.pos.y));
                    _lines[j] = elem;
                }
                rendLineArrs[i] = _lines;
            }
            // UnityEngine.Profiling.Profiler.BeginSample("LRichElement:formatRenderersMake");
            // place all position
            realLineWidth = 0;
            GameObject obj = null;
            int _len = rendLineArrs.Count;
            for (int i = 0; i < _len; i++)
            {
                int _lineWidth = 0;
                int _leng = rendLineArrs[i].Count;
                for (int j = 0; j < _leng; j++)
                {
                    LRenderElement elem = rendLineArrs[i][j];
                    if (elem.type != RichType.NEWLINE)
                    {
                        if (elem.type == RichType.TEXT)
                        {
                            obj = getCacheLabel();
                            makeLabel(obj, elem);
                            _lineWidth += (int)obj.GetComponent<Text>().preferredWidth;
                        }
                        else if (elem.type == RichType.IMAGE)
                        {
                            obj = getCacheImage(true);
                            makeImage(obj, elem);
                            _lineWidth += (int)obj.GetComponent<Image>().preferredWidth;
                        }
                        else if (elem.type == RichType.ANIM)
                        {
                            obj = getCacheFramAnim();
                            //makeFramAnim(obj, elem);
                            _lineWidth += elem.width;
                        }
                        obj.SetActive(true);
                        obj.transform.SetParent(transform);
                        obj.transform.localPosition = new Vector2(elem.pos.x, elem.pos.y /*+ realLineHeight*/);
                        obj.transform.localScale = new Vector3(1, 1,1);
                        _objectDataMap[obj] = elem.data;
                    }
                }
                realLineWidth = Mathf.Max(_lineWidth, realLineWidth);
            }

            RectTransform rtran = this.GetComponent<RectTransform>();
            rtran.sizeDelta = new Vector2(realLineWidth, realLineHeight);
            // UnityEngine.Profiling.Profiler.EndSample();
        }

        void makeLabel(GameObject lab, LRenderElement elem)
        {
            Text comText = lab.GetComponent<Text>();
            if (comText != null)
            {
                comText.text = elem.strChar;
                comText.font = elem.font;
                comText.fontSize = elem.fontSize;
                comText.fontStyle = FontStyle.Normal;
                comText.color = elem.color;
                comText.lineSpacing = 0;
                comText.raycastTarget = this.raycastTarget;
                comText.horizontalOverflow = HorizontalWrapMode.Overflow;
                comText.verticalOverflow = VerticalWrapMode.Overflow;
            }

            GradientEffect gradient = lab.GetComponent<GradientEffect>();
            if (elem.isGradient)
            {
                if(gradient == null)
                {
                    gradient = lab.AddComponent<GradientEffect>();
                    gradient.topColor = elem.topColor;
                    gradient.bottomColor = elem.bottomColor;
                }
            }else{
                if (gradient)
                {
                    Destroy(gradient);
                }
            }

            Outline outline = lab.GetComponent<Outline>();
            if (elem.isOutLine)
            {
                if (outline == null)
                {
                    outline = lab.AddComponent<Outline>();
                    outline.effectColor = elem.outLineColor;
                }
            }
            else {
                if (outline)
                {
                    Destroy(outline);
                }
            }

            if (elem.isUnderLine)
            {
                GameObject underLine = getCacheImage(false);
                Image underImg = underLine.GetComponent<Image>();
                underImg.color = elem.color;
                underImg.GetComponent<RectTransform>().sizeDelta = new Vector2(comText.preferredWidth, 1);
                underLine.SetActive(true);
                underLine.transform.SetParent(transform);
                underLine.transform.localScale = new Vector3(1, 1,1);
                underLine.transform.localPosition = new Vector2(elem.pos.x, elem.pos.y-1);
                underLine.transform.SetAsFirstSibling();
                comText.raycastTarget = true;
            }
        }

        void makeImage(GameObject img, LRenderElement elem)
        {
            Image comImage = img.GetComponent<Image>();
            comImage.enabled = false;
            if (comImage != null)
            {
				//string atlas = System.IO.Path.GetDirectoryName(elem.path);
				//string spname = System.IO.Path.GetFileName(elem.path);
                //FXGame.Managers.ResourceManager.Instance.GetSpriteByName(atlas, spname,(Sprite sp)=>{
                //    comImage.sprite = sp;
                //    comImage.enabled = true;
                //    comImage.raycastTarget = this.raycastTarget;
                //});
            }
        }

        void makeFramAnim(GameObject anim, LRenderElement elem)
        {
            //LMovieClip comFram = anim.GetComponent<LMovieClip>();
            //if (comFram != null)
            //{
            //    comFram.path = elem.path;
            //    comFram.fps = elem.fs;
            //    comFram.loadTexture();
            //    comFram.play();
            //    comFram.GetComponent<Image>().raycastTarget = this.raycastTarget;
            //}
        }

        protected GameObject getCacheLabel()
        {
            GameObject ret = null;
            int len = _cacheLabElements.Count;
            for (int i = 0; i < len; i++)
            {
                LRichCacheElement cacheElem = _cacheLabElements[i];
                if (cacheElem.isUse == false)
                {
                    cacheElem.isUse = true;
                    ret = cacheElem.node;
                    break;
                }
            }
            if (ret == null)
            {
                ret = new GameObject();
                ret.AddComponent<Text>().supportRichText = false;
                ContentSizeFitter fit = ret.AddComponent<ContentSizeFitter>();
                fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                fit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

                RectTransform rtran = ret.GetComponent<RectTransform>();
                rtran.pivot = Vector2.zero;
                rtran.anchorMax = new Vector2(0, 1);
                rtran.anchorMin = new Vector2(0, 1);

                LRichCacheElement cacheElem = new LRichCacheElement(ret);
                cacheElem.isUse = true;
                _cacheLabElements.Add(cacheElem);
            }
            return ret;
        }

        protected GameObject getCacheImage(bool isFitSize)
        {
            GameObject ret = null;
            int len = _cacheImgElements.Count;
            for (int i = 0; i < len; i++)
            {
                LRichCacheElement cacheElem = _cacheImgElements[i];
                if (cacheElem.isUse == false)
                {
                    cacheElem.isUse = true;
                    ret = cacheElem.node;
                    break;
                }
            }
            if (ret == null)
            {
                ret = new GameObject();
                ret.AddComponent<Image>();
                ContentSizeFitter fit = ret.AddComponent<ContentSizeFitter>();
                fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                fit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

                RectTransform rtran = ret.GetComponent<RectTransform>();
                rtran.pivot = Vector2.zero;
                rtran.anchorMax = new Vector2(0, 1);
                rtran.anchorMin = new Vector2(0, 1);

                LRichCacheElement cacheElem = new LRichCacheElement(ret);
                cacheElem.isUse = true;
                _cacheImgElements.Add(cacheElem);
            }
            ContentSizeFitter fitCom = ret.GetComponent<ContentSizeFitter>();
            fitCom.enabled = isFitSize;
            return ret;
        }

        protected GameObject getCacheFramAnim()
        {
            GameObject ret = null;
            int len = _cacheFramAnimElements.Count;
            for (int i = 0; i < len; i++)
            {
                LRichCacheElement cacheElem = _cacheFramAnimElements[i];
                if (cacheElem.isUse == false)
                {
                    cacheElem.isUse = true;
                    ret = cacheElem.node;
                    break;
                }
            }
            if (ret == null)
            {
                ret = new GameObject();
                ret.AddComponent<Image>();
                ContentSizeFitter fit = ret.AddComponent<ContentSizeFitter>();
                fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                fit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

                RectTransform rtran = ret.GetComponent<RectTransform>();
                rtran.pivot = Vector2.zero;
                rtran.anchorMax = new Vector2(0, 1);
                rtran.anchorMin = new Vector2(0, 1);

                //ret.AddComponent<LMovieClip>();

                LRichCacheElement cacheElem = new LRichCacheElement(ret);
                cacheElem.isUse = true;
                _cacheFramAnimElements.Add(cacheElem);
            }
            return ret;
        }

        protected bool isChinese(string text)
        {
            bool hasChinese = false;
            // char[] c = text.ToCharArray();
            // int len = c.Length;
            // for (int i = 0; i < len; i++)
            // {
            //     if (c[i] >= 0x4e00 && c[i] <= 0x9fbb)
            //     {
            //         hasChinese = true;
            //         break;
            //     }
            // }
            for(int i=0;i<text.Length;i++)
            {
                if((int)text[i] > 127)
                    hasChinese = true;
            }
            // 换行判断 有疑问 先紧急处理
            if(System.Text.RegularExpressions.Regex.IsMatch(text, @"^[a-zA-Z0-9]*$")){
                hasChinese = true;
            }
            return hasChinese;
        }

        public void OnPointerClick(PointerEventData data)
        {
            if (_objectDataMap.ContainsKey(data.pointerEnter))
            {
                if ((onClickHandler != null) && (_objectDataMap[data.pointerEnter] != ""))
                {
                    onClickHandler.Invoke(_objectDataMap[data.pointerEnter]);
                }
            }
        }
//---------------------parse rich element content from string----------------------------------------
        private string[] SaftSplite(string content, char separater)
        {
            List<string> arr = new List<string>();
            char[] charArr = content.ToCharArray();
            bool strFlag = false;
            List<char> line = new List<char>();
            for (int i =0;i< charArr.Length; i++)
            {
                if((charArr[i] == '"') && (charArr[i-1] != '\\')) //string start
                {
                    strFlag = !strFlag;
                }
                if(charArr[i] == separater)
                {
                    if (!strFlag)
                    {
                        arr.Add(new string(line.ToArray()));
                        line.Clear();
                    }
                    else
                    {
                        line.Add(charArr[i]);
                    }
                }
                else
                {
                    line.Add(charArr[i]);
                }
            }
            if(line.Count > 0)
            {
                arr.Add(new string(line.ToArray()));
            }
            return arr.ToArray();
        }

        public void parseRichElemString(string content, UnityAction<string, Dictionary<string, string>> handleFunc)
        {
            List<string> elemStrs = new List<string>();

            int startIndex = 0;
            elemStrs = executeParseRichElem(elemStrs, content, startIndex);

            int len = elemStrs.Count;
            for (int i = 0; i < len; i++)
            {
                string flag = elemStrs[i].Substring(0, elemStrs[i].IndexOf(" "));
                string paramStr = elemStrs[i].Substring(elemStrs[i].IndexOf(" ") + 1);
                string[] paramArr = SaftSplite(paramStr,' ');

                Dictionary<string, string> param = new Dictionary<string, string>();
                int paramArrLen = paramArr.Length;
                for (int j = 0; j < paramArrLen; j++)
                {
                    string[] paramObj = SaftSplite(paramArr[j], '=');
                    string left = paramObj[0].Trim();
                    string right = paramObj[1].Trim();

                    if (right.EndsWith("\"") && right.StartsWith("\""))
                    {
                        param.Add(left, right.Trim('"'));
                    }
                    else
                    {
                        param.Add(left, right);
                    }
                }
                handleFunc.Invoke(flag, param);
            }
        }

        private List<string> executeParseRichElem(List<string> result, string content, int startIndex)
        {
            bool hasMatch = false;
            int matchIndex = content.IndexOf("<", startIndex);
            if (matchIndex != -1)
            {
                result.Add(string.Format("lab txt=\"{0}\"", content.Substring(startIndex, matchIndex - startIndex))); //match head
                startIndex = matchIndex;

                matchIndex = content.IndexOf("/>", startIndex);
                if (matchIndex != -1)
                {
                    hasMatch = true;
                    result.Add(content.Substring(startIndex + 1, matchIndex - (startIndex + 1))); //match tail
                    startIndex = matchIndex + 2;
                }
            }

            if (hasMatch)
            {
                return executeParseRichElem(result, content, startIndex);
            }
            else
            {
                result.Add(string.Format("lab txt=\"{0}\"", content.Substring(startIndex, content.Length - startIndex)));
                return result;
            }
        }

        public void parseRichDefaultString(string content, UnityAction<string, Dictionary<string, string>> specHandleFunc=null)
        {
            //try
            //{ 
                parseRichElemString(content, (flag, param) =>
                {
                    if (flag == "lab")
                    {
                        this.insertElement(
                            param.ContainsKey("txt") ? param["txt"] : "",
                            StringToColor(param.ContainsKey("color") ? param["color"] : defaultLabColor),
                            param.ContainsKey("size") ? System.Convert.ToInt32(param["size"]) : defaultLabSize,
                            param.ContainsKey("isUnderLine") ? System.Convert.ToBoolean(param["isUnderLine"]) : false,
                            param.ContainsKey("isOutLine") ? System.Convert.ToBoolean(param["isOutLine"]) : false,
                            StringToColor(param.ContainsKey("outLineColor") ? param["outLineColor"] : "#000000"),
                            param.ContainsKey("isGradient") ? System.Convert.ToBoolean(param["isGradient"]) : false,
                            StringToColor(param.ContainsKey("topColor") ? param["topColor"] : "#000000"),
                            StringToColor(param.ContainsKey("bottomColor") ? param["bottomColor"] : "#000000"),
                            param.ContainsKey("data") ? param["data"] : ""
                            );
                    }else if(flag == "img")
                    {
                        this.insertElement(
                             param.ContainsKey("path") ? param["path"] : "",
                             param.ContainsKey("data") ? param["data"] : "",
                             param.ContainsKey("customWidth") ? System.Convert.ToSingle(param["customWidth"]) : 30.0f,
                             param.ContainsKey("customHeight") ? System.Convert.ToSingle(param["customHeight"]) : 30.0f);
                    }else if(flag == "anim")
                    {
                        this.insertElement(param["path"],
                             param.ContainsKey("fps") ? System.Convert.ToSingle(param["fps"]) : 15f,
                             param.ContainsKey("data") ? param["data"] : "",
                             param.ContainsKey("customWidth") ? System.Convert.ToSingle(param["customWidth"]) : 30.0f,
                             param.ContainsKey("customHeight") ? System.Convert.ToSingle(param["customHeight"]) : 30.0f);
                    }else if(flag == "newline")
                    {
                        this.insertElement(1);
                    }
                    else
                    {
                        if(specHandleFunc != null) specHandleFunc.Invoke(flag, param);
                    }
                });

                this.reloadData();
            //}
            //catch(System.Exception e)
            //{
            //    Debug.Log("error:"+e);
            //    Debug.Log("content:"+content);
            //}
        }

        public Color StringToColor(string color)
        {
            int red, green, blue = 0;
            char[] rgb;
            color = color.TrimStart('#');
            color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
            switch (color.Length)
            {
                case 3:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[0].ToString(), 16);
                    green = Convert.ToInt32(rgb[1].ToString() + rgb[1].ToString(), 16);
                    blue = Convert.ToInt32(rgb[2].ToString() + rgb[2].ToString(), 16);
                    return new Color(red / 255.0f, green / 255.0f, blue / 255.0f, 1);
                case 6:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[1].ToString(), 16);
                    green = Convert.ToInt32(rgb[2].ToString() + rgb[3].ToString(), 16);
                    blue = Convert.ToInt32(rgb[4].ToString() + rgb[5].ToString(), 16);
                    return new Color(red / 255.0f, green / 255.0f, blue / 255.0f, 1);
                default:
                    return Color.white;
            }
        }
    }
    
}