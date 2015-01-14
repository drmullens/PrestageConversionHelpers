using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestageCommonMethodCleanerHelpers
{
    public class ParamerterInfo
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _referenceType;

        public string ReferenceType
        {
            get { return _referenceType; }
            set { _referenceType = value; }
        }


        public ParamerterInfo(string type, string name, string referenceType)
        {
            this.Name = name;
            this.Type = type;
            this.ReferenceType = referenceType;
        }

    }
}
