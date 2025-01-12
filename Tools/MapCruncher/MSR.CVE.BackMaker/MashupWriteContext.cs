using System.Collections.Generic;
using System.Xml;

namespace MSR.CVE.BackMaker
{
    public class MashupWriteContext
    {
        private XmlTextWriter _writer;
        private Dictionary<object, string> identityMap = new Dictionary<object, string>();
        private int nextId;

        public XmlTextWriter writer
        {
            get
            {
                return _writer;
            }
        }

        public MashupWriteContext(XmlTextWriter writer)
        {
            _writer = writer;
        }

        public void WriteIdentityAttr(object target)
        {
            writer.WriteAttributeString("id", GetIdentity(target));
        }

        public string GetIdentity(object target)
        {
            if (identityMap.ContainsKey(target))
            {
                return identityMap[target];
            }

            string text = nextId.ToString();
            nextId++;
            identityMap[target] = text;
            return text;
        }
    }
}
