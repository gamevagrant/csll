using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QY.Open
{
/*
{
  "picture": {
    "data": {
      "height": 50, 
      "is_silhouette": true, 
      "url": "https://fb-s-b-a.akamaihd.net/h-ak-fbx/v/t1.0-1/c15.0.50.50/p50x50/10354686_10150004552801856_220367501106153455_n.jpg?oh=24b240ba2dc60ad31b4319fbab9bb9e2&oe=5A9CD62F&__gda__=1520094980_a601a89ba8df1e143766ac8d4b420fed", 
      "width": 50
    }
  }
}
 * */
    public class PictureData
    {

        public Picture data;

        public class Picture
        {
            public int height;
            public int width;
            public string url;
            public bool is_silhouette; 
        }
    }
}

