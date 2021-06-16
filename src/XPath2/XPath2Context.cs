// Microsoft Public License (Ms-PL)
// See the file License.rtf or License.txt for the license details.

// Copyright (c) 2011, Semyon A. Chertkov (semyonc@gmail.com)
// All rights reserved.

using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using Wmhelp.XPath2.MS;

namespace Wmhelp.XPath2
{
    public class XPath2Context
    {
        public XPath2RunningContext RunningContext { get; set; }

        public XmlNameTable NameTable { get; }

        public XmlNamespaceManager NamespaceManager { get; }

        public XmlSchemaSet SchemaSet { get; set; }

        public string DefaultFunctionNamespace { get; }

        public XPath2Context(IXmlNamespaceResolver nsManager)
        {
            NameTable = new NameTable();
            NamespaceManager = new XmlNamespaceManager(NameTable);
            SchemaSet = new XmlSchemaSet(NameTable);

            DefaultFunctionNamespace = XmlReservedNs.NsXQueryFunc;// XmlReservedNs.NsXPathFunc;

            if (nsManager != null)
            {
                foreach (KeyValuePair<string, string> ns in nsManager.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml))
                {
                    NamespaceManager.AddNamespace(ns.Key, ns.Value);
                }
            }

            if (NamespaceManager.LookupNamespace("xs") == null)
            {
                NamespaceManager.AddNamespace("xs", XmlReservedNs.NsXs);
            }
            if (NamespaceManager.LookupNamespace("xsi") == null)
            {
                NamespaceManager.AddNamespace("xsi", XmlReservedNs.NsXsi);
            }
            if (NamespaceManager.LookupNamespace("fn") == null)
            {
                NamespaceManager.AddNamespace("fn", XmlReservedNs.NsXQueryFunc);
            }
            if (NamespaceManager.LookupNamespace("local") == null)
            {
                NamespaceManager.AddNamespace("local", XmlReservedNs.NsXQueryLocalFunc);
            }
            if (NamespaceManager.LookupNamespace("wmh") == null)
            {
                NamespaceManager.AddNamespace("wmh", XmlReservedNs.NsWmhExt);
            }
        }
    }
}
