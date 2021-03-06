using System.IO;
using System.Collections.Generic;
using System.Xml;

using UnityEngine;

namespace PDollarGestureRecognizer
{
    public class GestureIO
    {
        /// <summary>
        /// Reads a multistroke gesture from an XML file
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
		public static Gesture ReadGestureFromXML(string xml) 
		{
			XmlTextReader xmlReader = null;
			Gesture gesture = null;

			try 
			{
				xmlReader = new XmlTextReader(new StringReader(xml));
				gesture = ReadGesture(xmlReader);
			} 
			finally 
			{
				if (xmlReader != null)
					xmlReader.Close();
			}

			return gesture;
		}
		//parsing XML
		private static Gesture ReadGesture(XmlTextReader xmlReader)
        {
            List<Point> points = new List<Point>();
            string gestureName = "";

            try
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType != XmlNodeType.Element) continue;
                    switch (xmlReader.Name)
                    {
                        case "Gesture":
                            gestureName = xmlReader["Name"];
                            if (gestureName.Contains("~")) // '~' character is specific to the naming convention of the MMG set
                                gestureName = gestureName.Substring(0, gestureName.LastIndexOf('~'));
                            if (gestureName.Contains("_")) // '_' character is specific to the naming convention of the MMG set
                                gestureName = gestureName.Replace('_', ' ');
                            break;

                        case "Point":
                            points.Add(new Point(float.Parse(xmlReader["X"]), float.Parse(xmlReader["Y"])));
                            break;
                    }
                }
            }
            finally
            {
                if (xmlReader != null)
                    xmlReader.Close();
            }
            return new Gesture(points.ToArray(), gestureName);
        }
    }
}