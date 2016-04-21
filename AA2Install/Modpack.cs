﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AA2Install
{
    public class ModDL
    {
        public string Name;
        public Uri URL;
        public ModDL(string name, Uri uri)
        {
            Name = name;
            URL = uri;
        }
    }
    public class Modpack
    {
        protected int CurrentRev => 1;

        public string Name;
        public string Description;
        public string Authors;
        public int Version;
        public List<ModDL> Mods = new List<ModDL>();

        private int Revision;

        public Modpack(Stream modpackStream)
        {
            XmlReader xml = XmlReader.Create(modpackStream);

            xml.ReadStartElement();

            if (xml.GetAttribute("revision") != CurrentRev.ToString())
                throw new ArgumentException("Modpack supplied is invalid or not supported.");

            Name = xml.ReadElementContentAsString();
            Description = xml.ReadElementContentAsString();
            Authors = xml.ReadElementContentAsString();
            Version = xml.ReadElementContentAsInt();

            while (xml.IsStartElement("mod"))
            {
                xml.ReadStartElement();

                string name = xml.ReadElementContentAsString();
                Uri uri = new Uri(xml.ReadElementContentAsString());
                Mods.Add(new ModDL(name, uri));

                xml.ReadEndElement();
            }

            xml.ReadEndElement();
        }

        public Modpack(string filename) : this(new FileStream(filename, FileMode.Open))
        {

        }
    }
}
