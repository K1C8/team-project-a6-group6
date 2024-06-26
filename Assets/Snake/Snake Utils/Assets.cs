﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils {

    /*
     * Global Asset references
     * */
    public class Assets : MonoBehaviour {

        // Internal instance reference
        private static Assets _i; 

        // Instance reference
        public static Assets i {
            get { 
                if (_i == null) _i = (Instantiate(Resources.Load("Assets")) as GameObject).GetComponent<Assets>(); 
                return _i; 
            }
        }


        // All references

        public Sprite s_White;

    }

}
