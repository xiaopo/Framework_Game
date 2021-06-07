using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAdpat {

    private static float m_adpTargetScale = 0f;
    public static float AdpTargetScale
    {
        get
        {
            if (m_adpTargetScale == 0)
            {
                float ap = Screen.width / (float)Screen.height;
                if (ap < 1.76)
                {
                    m_adpTargetScale = ap / (1280 / 720f);
                }
                else
                    m_adpTargetScale = 1;
            }
            return m_adpTargetScale;
        }
    }

    private static bool first = true;
    private static bool m_isCanAdpScale = false;
    public static bool IsCanAdpScale
    {
        get
        {
            if(first)
            {
                m_isCanAdpScale = (Screen.width / (float)Screen.height) < 1.76;
                first = false;
            }
            return m_isCanAdpScale;
        }
    }
}
