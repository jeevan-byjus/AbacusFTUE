using System;
using System.Collections.Generic;
using UnityEngine;
using Byjus.Gamepod.Common.Vision;
using Byjus.Gamepod.AbacusFTUE.Util;

namespace Byjus.Gamepod.AbacusFTUE.Verticals {

    [Serializable]
    public class JOutput {
        public List<JItem> items;
    }

    [Serializable]
    public class JItem {        
        public int id;
        public Point pt;
        public float angle;
        public string shape;
        public string type;

        public JItem() {
        }

        public JItem(JItem other) {
            id = other.id;
            pt = other.pt;
            angle = other.angle;
            shape = other.shape;
            type = other.type;
        }

        public override string ToString() {
            return "Id: " + id + ", pos: " + pt + ", Rotation: " + angle + ", Shape: " + shape + ", Type: " + type;
        }
    }
}