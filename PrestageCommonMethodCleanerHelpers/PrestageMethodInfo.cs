using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PrestageCommonMethodCleanerHelpers
{
    public class PrestageMethodInfo
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private List<ParamerterInfo> _parameters;

        public List<ParamerterInfo> Parameters
        {
            get 
            {
                if (null == _parameters) {
                    _parameters = new List<ParamerterInfo>();
                }
                return _parameters; 
            }
            set { _parameters = value; }
        }


        private string _accessModifier;

        public string AccessModifier
        {
            get { return _accessModifier; }
            set { _accessModifier = value; }
        }

        
        private string _returnType;

	    public string ReturnType
	    {
		    get { return _returnType;}
		    set { _returnType = value;}
	    }

        private string _eventType;

	    public string EventType
	    {
		    get { return _eventType;}
		    set { _eventType = value;}
	    }

        private string _assocaitedPreSection;

        public string AssociatedPreSection
        {
            get { return _assocaitedPreSection; }
            set { _assocaitedPreSection = value; }
        }



        private string _body;

        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        private Region _region = Region.None;

        public Region RegionToPlaceIn
        {
            get { return _region; }
            set { _region = value; }
        }


        private ReplacementAction _replacementAction = ReplacementAction.None;

        public ReplacementAction MethodReplacementAction
        {
            get { return _replacementAction; }
            set { _replacementAction = value; }
        }


        public void ExtractDeffinitionInfo(string fullDeffinition)
        {
            Match match = ExtractAccessModifier(fullDeffinition,0);
            this.AccessModifier = match.Value;
            match = ExtractName(fullDeffinition, (match.Index + match.Length));
            this.Name = match.Value;
            int startIndex = ExtractParameters(fullDeffinition, (match.Index + match.Length));
            match = ExtractReturnType(fullDeffinition, startIndex);
            this.ReturnType = match.Value;
            match = ExtractEventType(fullDeffinition, (match.Index + match.Length));
            this.EventType = match.Value;

        }

        public static Match ExtractAccessModifier(string fullDef,int startIndex)
        {
            Regex regex = new Regex("public|private|protected", RegexOptions.IgnoreCase);

            Match match = regex.Match(fullDef,startIndex);

            return match;

        }

        public static Match ExtractName(string fullDef, int startIndex)
        {
            Regex regexFunctionOrSub = new Regex(" sub|function ",RegexOptions.IgnoreCase);
            Match matchFunctionOrSub = regexFunctionOrSub.Match(fullDef, startIndex);

            startIndex = matchFunctionOrSub.Index + matchFunctionOrSub.Length;

            Regex regex = new Regex(@" [a-zA-Z0-9_]*(?=\()");
            Match match = regex.Match(fullDef, startIndex);

            return match;

        }

        private int ExtractParameters(string fullDef, int startIndex)
        {
            int closingParenthesesSpot = fullDef.IndexOf(')', startIndex);

            while (startIndex < closingParenthesesSpot) {
                startIndex = ExtractNextParameter(fullDef, startIndex);
            }

            return startIndex;
        }

        private int ExtractNextParameter(string fullDef, int startIndex)
        {

            Match match = ExtractReferenceType(fullDef, startIndex);
            if (!match.Success) {
                return startIndex + 1;
            }
            string type = match.Value;
            match = ExtractParameterName(fullDef, (match.Index + match.Length));
            string name = match.Value;
            match = ExtractParameterType(fullDef, (match.Index + match.Length));
            ParamerterInfo parameterInfo = new ParamerterInfo(type, name, match.Value);

            Parameters.Add(parameterInfo);

            return match.Index + match.Length;
        }

        public static Match ExtractReferenceType(string fullDef, int startIndex)
        {
            Regex regex = new Regex("byref|byval", RegexOptions.IgnoreCase);

            return regex.Match(fullDef, startIndex);
        }

        public static Match ExtractParameterName(string fullDef, int startIndex)
        {
            Regex regex = new Regex(" [A-Za-z0-9_]* ");
            return regex.Match(fullDef,startIndex);
        }

        public static Match ExtractParameterType(string fullDef, int startIndex)
        {
            Regex regex = new Regex(@"(?<=([Aa][Ss] ))[A-Za-z0-9_\.]*");

            return regex.Match(fullDef, startIndex);
        }

        public static Match ExtractReturnType(string fullDef, int startIndex)
        {
            Regex regexAs = new Regex(" as ",RegexOptions.IgnoreCase);
            Match match = regexAs.Match(fullDef, startIndex);
            if (!match.Success) {
                return match;
            }
            Regex regexType = new Regex(@"[A-Za-z0-9_\.]* ");
            return regexType.Match(fullDef, (match.Index + match.Length));
        }

        public static Match ExtractEventType(string fullDef, int startIndex)
        {
            Regex regexHandles = new Regex(" handles ",RegexOptions.IgnoreCase);
            Match match = regexHandles.Match(fullDef, startIndex);
            if (!match.Success)
            {
                return match;
            }
            Regex regexType = new Regex(@"[A-Za-z0-9_\.]*");
            return regexType.Match(fullDef, (match.Index + match.Length));
        }

    }
}
