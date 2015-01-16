using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PrestageCommonMethodCleanerHelpers
{
    public class PrestageClassInfo
    {
        private string _className;

        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }

        private string _accessModifier;

        public string AccessModifier
        {
            get{return _accessModifier; }
            set{ _accessModifier = value; }
        }

        private string _parentClass;

        public string ParentClass
        {
            get { return _parentClass; }
            set { _parentClass = value; }
        }


        private string _topSection;

        public string TopSection
        {
            get { return _topSection; }
            set { _topSection = value; }
        }

        private string _bottomSection;

        public string BottomSection
        {
            get { return _bottomSection; }
            set { _bottomSection = value; }
        }


        private List<PrestageMethodInfo> _methods;

        public List<PrestageMethodInfo> Methods
        {
            get 
            {
                if (null == _methods) {
                    _methods = new List<PrestageMethodInfo>();
                }
                return _methods; 
            }
            set { _methods = value; }
        }

        public PrestageClassInfo(string code)
        {
            string definitionPattern = @"(Private|Public|Protected)? (Sub|Function) [a-zA-Z0-9_]*\([a-zA-Z0-9 _\.,]*\) *((Handles[ a-zA-Z0-9_\.]*)|As[ a-zA-Z0-9_\.]*)?";
            string bodyPattern = "((.|\n)*?)(End (Sub|Function))";

            Regex methodDefinition = new Regex(definitionPattern);
            Regex methodBody = new Regex(bodyPattern);

            PrestageXMLInfo myXML = new PrestageXMLInfo();

            int startIndex = 0;
            bool continueEh = true; //canandian notation
            while (startIndex < code.Length && continueEh)
            {
                Logger log = Logger.Instance;
                log.Progress = ((double)startIndex) / ((double)code.Length);
                PrestageMethodInfo info = new PrestageMethodInfo();
                Match m = methodDefinition.Match(code, startIndex);
                continueEh = continueEh & m.Success;
                if (m.Success)
                {
                    string section = code.Substring(startIndex, m.Index - startIndex);
                    if (!string.IsNullOrWhiteSpace(section))
                    {
                        if (startIndex == 0)
                        {
                            this.TopSection = section;
                            AccessModifier = myXML.getClassAccessModifier(section);
                            ClassName = myXML.getClassName(section);
                        }
                        else
                        {
                            info.AssociatedPreSection = section;
                        }

                    }
                }

                startIndex = m.Index + m.Length;
                info.ExtractDeffinitionInfo(m.Value);
                m = methodBody.Match(code, startIndex);
                continueEh = continueEh & m.Success;
                info.Body = m.Groups[1].Value;
                Methods.Add(info);
                startIndex = m.Index + m.Length;
            }

            myXML.WriteToXML(this);
           
        }       
    }
}
