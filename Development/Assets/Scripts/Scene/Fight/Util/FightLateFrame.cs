using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Fight
{
    public class FightLateFrame : FightCallUtil
    {
        private static FightLateFrame _instance;

        public static FightLateFrame instance
        {
            get
            {
                if (_instance == null) _instance = new FightLateFrame();

                return _instance;
            }
        }
    }
}
