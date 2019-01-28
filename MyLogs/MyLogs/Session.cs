using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;

namespace MyLogs
{
    public partial class MainForm
    {
        private void SaveCurrentSession()
        {
            XDocument SessionXML = new XDocument(new XElement("Session", new XElement("Files"), new XElement("Folders")));
            for(var i = 0; i < TabControlParent.TabPages.Count; i++)
            {
                Classes.LogTabPage logTabPage = TabControlParent.TabPages[i] as Classes.LogTabPage;
                if (!logTabPage.IsFolder)//File
                {
                    SessionXML.Root.Elements("Files").First().Add(new XElement(Path.GetFileName(logTabPage.Name),
                                                                    new XAttribute("Position", i),
                                                                    new XAttribute("Name", Path.GetFileName(logTabPage.Name)),
                                                                    new XAttribute("Path", logTabPage.Name),
                                                                    new XAttribute("IsTailFollowed", logTabPage.TailFollowed),
                                                                    new XAttribute("IsFolder", logTabPage.IsFolder)
                                                                    ));
                }
                else //Folder
                {
                    SessionXML.Root.Elements("Folders").First().Add(new XElement(logTabPage.Name,
                                                                        new XAttribute("Name", logTabPage.Name.ToString()),
                                                                        new XAttribute("Position", i),
                                                                        new XAttribute("IsFolder", logTabPage.IsFolder)
                            ));
                    TabControl subTabControl = (logTabPage.Controls.Find("SubTabControl", true).FirstOrDefault()) as TabControl;
                    for (var ik = 0; ik < subTabControl.TabPages.Count; ik++)
                    {
                        Classes.LogTabPage subLogTabPage = subTabControl.TabPages[ik] as Classes.LogTabPage;
                        SessionXML.Root.Elements("Folders").First().Elements(logTabPage.Name).First().Add(new XElement(Path.GetFileName(subLogTabPage.Name).ToString(),
                                                                    new XAttribute("Position", ik),
                                                                    new XAttribute("Name", Path.GetFileName(subLogTabPage.Name).ToString()),
                                                                    new XAttribute("Path", subLogTabPage.Name.ToString()),
                                                                    new XAttribute("IsTailFollowed", subLogTabPage.TailFollowed),
                                                                    new XAttribute("IsFolder", subLogTabPage.IsFolder)
                            ));
                    }
                }
            }
            SessionXML.Save("./Session/Session.xml");
        }

        private void LoadLastSession()
        {
            if(TabControlParent.TabPages.Count > 0)
            {
                TabControlParent.TabPages.Clear();
                FileWatchers.Clear();
            }
            try
            {
                XDocument LastSession = XDocument.Load(@"./Session/Session.xml");
                XElement files = LastSession.Root.Elements("Files").First();
                for(var i = 0; i < files.Elements().Count(); i++)
                {
                    XElement element = files.Elements().ToList()[i];
                    int index = -1;
                    Int32.TryParse(element.Attribute("Position").Value, out index);
                    CreateParentTabPageAtIndex(element.Attribute("Path").Value, index);
                }

                XElement folders = LastSession.Root.Elements("Folders").First();
                for (var f = 0; f < folders.Elements().Count(); f++)
                {
                    XElement element = folders.Elements().ToList()[f];
                    int index = -1;
                    Int32.TryParse(element.Attribute("Position").Value, out index);
                    CreateFolder(element.Attribute("Name").Value, index);

                    for (var fi = 0; fi < folders.Elements().ToList()[f].Elements().Count(); fi++)
                    {
                        int subIndex = -1;
                        Int32.TryParse(folders.Elements().ToList()[f].Elements().ToList()[fi].Attribute("Position").Value, out subIndex);
                        CreateParentTabPageAtIndex(folders.Elements().ToList()[f].Elements().ToList()[fi].Attribute("Path").Value, subIndex, parentName: element.Attribute("Name").Value);
                    }
                }
            }
            catch(IOException err)
            {
                MessageBox.Show(err.Message);
                return;
            }
        }
    }
}
