using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Fight
{
    public class FightFrameUtil : FightCallUtil
    {
        private static FightFrameUtil _instance;

        public static FightFrameUtil instance
        {
            get
            {
                if (_instance == null) _instance = new FightFrameUtil();

                return _instance;
            }
        }
    }
}
