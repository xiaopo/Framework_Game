using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace XGUI
{
	public class XButtonGroup : MonoBehaviour
	{
        List<XButton> m_Buttons = new List<XButton>();
        private void ValidateToggleIsInGroup(XButton button)
        {
            if (button == null || !m_Buttons.Contains(button))
                throw new System.ArgumentException(string.Format("XButton {0} is not part of XButtonGroup {1}", new object[] { button, this }));
        }

        public void NotifyToggleOn(XButton button)
        {
            ValidateToggleIsInGroup(button);

            for (var i = 0; i < m_Buttons.Count; i++)
            {
                if (m_Buttons[i] == button)
                    continue;

                m_Buttons[i].isSelected = false;
            }
        }

        public void UnregisterToggle(XButton button)
        {
            if (m_Buttons.Contains(button))
                m_Buttons.Remove(button);
        }

        public void RegisterToggle(XButton button)
        {
            if (!m_Buttons.Contains(button))
                m_Buttons.Add(button);
        }

        public bool AnyTogglesOn()
        {
            return m_Buttons.Find(x => x.isSelected) != null;
        }

        public IEnumerable<XButton> ActiveToggles()
        {
            return m_Buttons.Where(x => x.isSelected);
        }

        public void SetAllTogglesOff()
        {
            for (var i = 0; i < m_Buttons.Count; i++)
                m_Buttons[i].isSelected = false;
        }
	}
}
