using CISanityTester.Template.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CISanityTester.Template
{
    public class TemplateFileHandler
    {
        public Template CurrentTemplate { get; set; }
        public string TemplateAbsolutePath { get; set; }  // ex: C:\Users\ttomar\Documents\CISanityTester\DailyPPCSanity\
        public string TemplateFileName { get; set; } //ex: DailyPPCSanity.xml

        private static TemplateFileHandler obj = null;
        private TemplateFileHandler()
        {
            
        }

        public void SetTemplate(string filepath)
        {
            TemplateFileName = Path.GetFileName(filepath);
            TemplateAbsolutePath = filepath.Substring(0, filepath.Length - TemplateFileName.Length);

            CurrentTemplate=LoadTemplateFromFile(filepath, new Template());
        }

        public static TemplateFileHandler GetTemplateFileHandler()
        {
            if (obj == null)
            
                obj = new TemplateFileHandler();
              
            return obj;
        }
        public void CreateXML(Template template)
        {

            XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
                                                      // Initializes a new instance of the XmlDocument class.          
            XmlSerializer xmlSerializer = new XmlSerializer(template.GetType());
            // Creates a stream whose backing store is memory. 

            string XMLString = String.Empty;
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, template);
                xmlStream.Position = 0;
                //Loads the XML document from the specified string.
                xmlDoc.Load(xmlStream);
                XMLString =  xmlDoc.InnerXml;
            }

            // create an xml file
            string templatefilepath = TemplateAbsolutePath + TemplateFileName;
            Directory.CreateDirectory(TemplateAbsolutePath);
            FileStream stream = File.Open(templatefilepath, FileMode.OpenOrCreate);
            byte[] data = new UTF8Encoding(false).GetBytes(XMLString);
            stream.Write(data, 0, data.Length);

            stream.Close();
            stream.Dispose();
        }

        internal Template GenetareDefaultTemplate()
        {
            //TemplateAbsolutePath = @"C:\Users\ttomar\Documents\CISanityTester\DailyPPCSanity\";
            //TemplateFileName = "DailyPPCSanity.xml";

            Template template = new Template();
            //template.Builds = new List<Build>();
            //template.Name = "Daily Sanity PPC";
            //Build Build_153 = Create153Build();
            //Build Build_154 = Create154Build();
            //Build Build_161 = Create161Build();
            //template.Builds.Add(Build_153);
            //template.Builds.Add(Build_154);
            //template.Builds.Add(Build_161);
            template.ServerSessions = new List<ServerSession>();
            //template.ServerSessions.Add(new ServerSession() { Name = "74-TL1", ServerIP = "10.220.17.74", SessionProtocol="TL1", Port =9090,LoadOnStartup=true });
            //template.ServerSessions.Add(new ServerSession() { Name = "74-Telnet", ServerIP = "10.220.17.74", SessionProtocol = "Telnet", Port = 23, LoadOnStartup = true });
            //template.ServerSessions.Add(new ServerSession() { Name = "74-Serial", ServerIP = "10.220.17.13", SessionProtocol = "Telnet", Port =10015, LoadOnStartup = true });
            //template.ServerSessions.Add(new ServerSession() { Name = "174-TL1", ServerIP = "10.220.16.174", SessionProtocol = "TL1", Port = 9090, LoadOnStartup = true });
            //template.ServerSessions.Add(new ServerSession() { Name = "174-Telnet", ServerIP = "10.220.16.174", SessionProtocol = "Telnet", Port = 23, LoadOnStartup = true });
            //template.ServerSessions.Add(new ServerSession() { Name = "174-Serial", ServerIP = "10.220.17.13", SessionProtocol = "Telnet", Port = 10016, LoadOnStartup = true });
            //template.NEs = new List<Entities.InfineraProductLine.NetworkElement>();
            //template.NEs.Add(new Entities.InfineraProductLine.NetworkElement() { Name = "NE_74",IP="10.220.17.74" });
            //template.NEs.Add(new Entities.InfineraProductLine.NetworkElement() { Name = "NE_174",IP="10.220.16.174" });
            template.Name = "New Template*";
            CurrentTemplate = template;
            return template;
            //CreateXML(template);
        }

        internal void Save()
        {
            if (TemplateAbsolutePath == null || TemplateAbsolutePath == String.Empty || TemplateAbsolutePath.Trim().Length == 0)
                SaveAs();
            
        }

        internal void SaveAs()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                TemplateFileName =    Path.GetFileName(dlg.FileName);
                TemplateAbsolutePath = dlg.FileName.Remove(dlg.FileName.Length - TemplateFileName.Length);
                CurrentTemplate.Name = Path.GetFileNameWithoutExtension(dlg.FileName);
              Save();
                
            }
        }

        private static Build Create153Build()
        {
            /* create build 15.3 sanities*/
            Build Build_153 = new Build();
            Build_153.Sanities = new List<Sanity>();
            Build_153.Name = "R15.3";


            Sanity DTNX_15_3 = new Sanity();
            DTNX_15_3.Name = "DTNX";
            DTNX_15_3.Scripts = new List<TestScript>();

            TestScript script = new TestScript();
            script.Path = "/15.3/DTNX/InstallBuild.cit";
            script.Name = "InstallBuild.cit";
            DTNX_15_3.Scripts.Add(script);

            script = new TestScript();
            script.Path = "/15.3/DTNX/ChangePassowrd.cit";
            script.Name = "ChangePassowrd.cit";
            DTNX_15_3.Scripts.Add(script);

            script = new TestScript();
            script.Path = "/15.3/DTNX/ProvisionChassis.cit";
            script.Name = "ProvisionChassis.cit";
            DTNX_15_3.Scripts.Add(script);

            Sanity ILS3_15_3 = new Sanity();
            ILS3_15_3.Name = "ILS3";
            ILS3_15_3.Scripts = new List<TestScript>();

            Build_153.Sanities.Add(DTNX_15_3);
            Build_153.Sanities.Add(ILS3_15_3);
            /* end of build 15.3 sanities*/
            return Build_153;
        }
        private static Build Create154Build()
        {
            /* create build 15.4 sanities*/
            Build Build_154 = new Build();
            Build_154.Sanities = new List<Sanity>();
            Build_154.Name = "R15.4";


            Sanity DTNX_15_4 = new Sanity();
            DTNX_15_4.Name = "DTNX";
            DTNX_15_4.Scripts = new List<TestScript>();
            Sanity ILS4_15_4 = new Sanity();
            ILS4_15_4.Name = "ILS4";
            ILS4_15_4.Scripts = new List<TestScript>();

            Build_154.Sanities.Add(DTNX_15_4);
            Build_154.Sanities.Add(ILS4_15_4);
            /* end of build 15.4 sanities*/
            return Build_154;
        }
        private static Build Create161Build()
        {
            /* create build 15.3 sanities*/
            Build Build_161 = new Build();
            Build_161.Sanities = new List<Sanity>();
            Build_161.Name = "R15.3";


            Sanity DTNX_16_1 = new Sanity();
            DTNX_16_1.Name = "DTNX";
            DTNX_16_1.Scripts = new List<TestScript>();
            Sanity ILS3_16_1 = new Sanity();
            ILS3_16_1.Name = "ILS3";
            ILS3_16_1.Scripts = new List<TestScript>();

            Build_161.Sanities.Add(DTNX_16_1);
            Build_161.Sanities.Add(ILS3_16_1);
            /* end of build 15.3 sanities*/
            return Build_161;
        }

        public Template LoadTemplateFromFile(string XMLfile, Template template)
        {
            XmlSerializer oXmlSerializer = new XmlSerializer(template.GetType());
            //The StringReader will be the stream holder for the existing XML file 
            string XMLString  = File.ReadAllText(XMLfile);
            template = (Template) oXmlSerializer.Deserialize(new StringReader(XMLString));
            //initially deserialized, the data is represented by an object without a defined type 
            return template;
        }
    }
}

