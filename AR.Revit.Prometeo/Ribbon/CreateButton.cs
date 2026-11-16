using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AR.Revit.Prometeo.Ribbon
{
    internal class CreateButton
    {
        public static void CreateTabPanelButton(UIControlledApplication app, string tabName, string panelName, string btnName, string command) 
        {
           CreateTab(app, tabName);
            RibbonPanel panel = CreatePanelButton(app, tabName, panelName);
           CreateBtn(panel, btnName, command);
        }
        public static void CreateTab(UIControlledApplication app, string tabName)
        {
            app.CreateRibbonTab(tabName);
        }
        public static RibbonPanel CreatePanelButton(UIControlledApplication app, string tabName, string panelName)
        {
            return app.CreateRibbonPanel(tabName, panelName);
        }
        public static void CreateBtn(RibbonPanel panel, string btnName, string command)
        {
            string route = Assembly.GetExecutingAssembly().Location;
            PushButtonData btn = new("id_button",btnName, route, command);
            panel.AddItem(btn);
        }
    }
}
