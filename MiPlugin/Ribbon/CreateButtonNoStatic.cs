using Autodesk.Revit.UI;
using MiPlugin.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiPlugin.Ribbon
{
    internal class CreateButtonNoStatic
    {
        private readonly UIControlledApplication _app;

        private readonly string _tabName = "Hola";
        private readonly string _panelName = "Que";
        private readonly string _btnName = "Tal";

        public CreateButtonNoStatic(UIControlledApplication app)
        {
            _app = app;
        }

        public void CreateTabPanelButton()
        {
            CreateTab();
            RibbonPanel panel = createPanelButton();
            CreateBtn(panel);
        }

        private void CreateTab()
        {
            _app.CreateRibbonTab(_tabName);
        }

        private RibbonPanel createPanelButton()
        {
            return _app.CreateRibbonPanel(_tabName, _panelName);
        }

        private void CreateBtn(RibbonPanel panel)
        {
            string route = Assembly.GetExecutingAssembly().Location;
            PushButtonData btn = new PushButtonData("id_button", _btnName, route, typeof(Command.ButtonCommand).FullName);
            panel.AddItem(btn);
        }
    }
}