using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Game.MScene
{
    public class MapftFrameUtil : MapftCallUtil
    {
        private static MapftFrameUtil _instance;

        public static MapftFrameUtil instance
        {
            get
            {
                if (_instance == null) _instance = new MapftFrameUtil();

                return _instance;
            }
        }
    }
}
