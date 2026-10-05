using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiPlugin.Ribbon
{
    internal class CreateButton
    {
        public static void CreateTabPanelButton(UIControlledApplication app) 
        {
            createTab(app);
            RibbonPanel panel = createPanelButton(app);
            createBtn(panel);

        }
        private static void createTab(UIControlledApplication app)
        {
            app.CreateRibbonTab(_tabName);
        }
        private static RibbonPanel createPanelButton(UIControlledApplication app)
        {
            return app.CreateRibbonPanel(_tabName, _panelName);
        }
        private static void createBtn(RibbonPanel panel)
        {
            string route = Assembly.GetExecutingAssembly().Location;
            PushButtonData btn = new PushButtonData("id_button", _btnName, route, typeof(Command.ButtonCommand).FullName);
            panel.AddItem(btn);
        }
        //Variables
        private static string _tabName = "Hola";
        private static string _panelName = "Que";
        private static string _btnName = "Tal";


    }
}
