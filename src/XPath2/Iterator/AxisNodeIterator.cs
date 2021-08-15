// Microsoft Public License (Ms-PL)
// See the file License.rtf or License.txt for the license details.

// Copyright (c) 2011, Semyon A. Chertkov (semyonc@gmail.com)
// All rights reserved.

using System.Xml.XPath;
using Wmhelp.XPath2.MS;
using Wmhelp.XPath2.Properties;

namespace Wmhelp.XPath2.Iterator
{
    internal abstract class AxisNodeIterator : XPath2NodeIterator
    {
        protected XPath2Context context;
        protected XmlQualifiedNameTest nameTest;
        protected SequenceType typeTest;
        protected bool matchSelf;
        protected XPath2NodeIterator iter;
        protected XPathNavigator curr;

        protected int sequentialPosition;
        protected bool accept;

        protected AxisNodeIterator()
        {
        }

        public AxisNodeIterator(XPath2Context context, object nodeTest, bool matchSelf, XPath2NodeIterator iter)
        {
            this.context = context;
            if (nodeTest is XmlQualifiedNameTest)
                nameTest = (XmlQualifiedNameTest) nodeTest;
            else if (nodeTest is SequenceType && nodeTest != SequenceType.Node)
                typeTest = (SequenceType) nodeTest;
            this.matchSelf = matchSelf;
            this.iter = iter;
        }

        public override XPath2NodeIterator CreateBufferedIterator()
        {
            return new BufferedNodeIterator(this);
        }

        protected void AssignFrom(AxisNodeIterator src)
        {
            context = src.context;
            typeTest = src.typeTest;
            nameTest = src.nameTest;
            matchSelf = src.matchSelf;
            iter = src.iter.Clone();
        }

        protected bool TestItem()
        {
            if (nameTest != null)
            {
                switch (curr.NodeType)
                {
                    case XPathNodeType.Element:
                        if (nameTest.Namespace == string.Empty)
                        {
                            return curr.NamespaceURI == context.NamespaceManager.DefaultNamespace && curr.LocalName == nameTest.Name;
                        }
                        else
                        {
                            return (nameTest.IsNamespaceWildcard || nameTest.Namespace == curr.NamespaceURI) &&
                                   (nameTest.IsNameWildcard || nameTest.Name == curr.LocalName);
                        }
                    case XPathNodeType.Attribute:
                        return (nameTest.IsNamespaceWildcard || nameTest.Namespace == curr.NamespaceURI) &&
                               (nameTest.IsNameWildcard || nameTest.Name == curr.LocalName);
                    default:
                        return false;
                }
            }
            else if (typeTest != null)
                return typeTest.Match(curr, context);
            return true;
        }

        protected bool MoveNextIter()
        {
            if (!iter.MoveNext())
                return false;
            XPathNavigator nav = iter.Current as XPathNavigator;
            if (nav == null)
                throw new XPath2Exception("XPTY0019", Resources.XPTY0019, iter.Current.Value);
            if (curr == null || !curr.MoveTo(nav))
                curr = nav.Clone();
            sequentialPosition = 0;
            accept = true;
            return true;
        }

        public override int SequentialPosition => sequentialPosition;

        public override void ResetSequentialPosition()
        {
            accept = false;
        }
    }
}