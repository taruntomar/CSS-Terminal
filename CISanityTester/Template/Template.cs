using CISanityTester.Template.Entities;
using CISanityTester.Template.Entities.InfineraProductLine;
using System.Collections.Generic;

namespace CISanityTester.Template
{
    //[System.Serializable]
    //[System.Xml.Serialization.XmlInclude(typeof(Build))]
    public class Template : BaseEntity
    {
        public List<Build> Builds { get; set; }
        public List<ServerSession> ServerSessions { get; set; }
        public List<NetworkElement> NEs { get; set; }

    }
}
