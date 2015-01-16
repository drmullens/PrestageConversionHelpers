using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;

namespace PrestageCommonMethodCleanerHelpers
{
    class PrestageXMLInfo
    {
        public void WriteToXML( PrestageClassInfo Item)
        {
            //this method takes the current match and writes it to the XML file

            //string testString = "This is a test </Name> </Class>"; 
            //This did not affect how the XML file is constructed, possible to cause problems on read (not yet tested)

            XElement xmlTree = new XElement("Class",
                    new XElement("Name", Item.ClassName.Trim()),
                    new XElement("AccessModifier", Item.AccessModifier.Trim()),
                    new XElement("TopSection", Item.TopSection),
                    new XElement("BottomSection", Item.BottomSection)               
             );

            foreach (var ListItem in Item.Methods)
            {
                XElement xmlParameters = new XElement("Parameters");

                foreach (var Parameter in ListItem.Parameters)
                {
                    xmlParameters.Add(new XElement("Parameter",
                        new XElement("Name", Parameter.Name.Trim()),
                        new XElement("ReferenceType", Parameter.ReferenceType.Trim()),
                        new XElement("Type", Parameter.Type.Trim())
                        )
                      );
                }

                xmlTree.Add(
                    new XElement("Method",
                        new XElement("Name", ListItem.Name.Trim()),
                        new XElement("AccessModifier", ListItem.AccessModifier.Trim()),
                        new XElement(xmlParameters),
                        new XElement("EventType", ListItem.EventType.Trim()),
                        new XElement("ReturnType", ListItem.ReturnType.Trim()),
                        new XElement("Body", ListItem.Body),
                        new XElement("ReplacementAction", ListItem.MethodReplacementAction.ToString()),
                        new XElement("Region", ListItem.RegionToPlaceIn.ToString())
                        )
                    );
        
            }
            if (xmlTree.HasElements)
                WriteToFile(Item.ClassName, xmlTree);
    
                
        }

        public string getClassName(string section)
        {
            //this method will grab the name of the class from TopSection

            string ClassName = "";

            Regex TypeGroup = new Regex(@"private|friend|public", RegexOptions.IgnoreCase);
            Match t = TypeGroup.Match(section, 0);

            Regex ClassGroup = new Regex(@" Class ", RegexOptions.IgnoreCase);
            Match c = ClassGroup.Match(section, t.Index + t.Length);

            Regex NameGroup = new Regex(@"[\w]*", RegexOptions.IgnoreCase);
            Match m = NameGroup.Match(section, c.Index + c.Length);

            if (m.Success)
            {
                ClassName = m.Value;
            }
            else
            {
                ClassName = "none";
            }

            return ClassName;

        }

        public string getClassAccessModifier(string section)
        {
            string modifier = "";

            Regex TypeModifier = new Regex(@"private|friend|public|protected", RegexOptions.IgnoreCase);
            Match t = TypeModifier.Match(section, 0);

            if (t.Success)
            {
                modifier = t.Value;
            }
            else
            {
                modifier = "none";
            }

            return modifier;
        }

        private void WriteToFile(string ClassName, XElement xmlTree)
        {
            string path = @"C:\temp\Prestage_" + ClassName + "_Class.xml";
            
            File.WriteAllText(path, xmlTree.ToString());
        }
    }
}
