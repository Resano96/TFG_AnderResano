using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiPlugin.Ribbon
{
    internal class CreateButton1
    {
        private  string _tabName { get; init; }
        private  string _panelName { get; init; }
        private  string _btnName { get; init; }
        private  string _btnid { get; init; }
        private UIControlledApplication _app;

        public CreateButton1(
            string tab, 
            string panel, 
            string btn,
            string id,
            UIControlledApplication app)
        {
            _app = app;
            _tabName = tab;
            _panelName = panel;
            _btnName = btn;
            _btnid = id;


        }
        public CreateButton1(UIControlledApplication app)
        {
            _app = app;
        }
        public void Create(string tab, string panel, string btn, string id)
        {
            CreateTab(tab);
            CreateBtn(CreatePanel(tab, panel), id, btn);

        }

        public void CreateTab(string nombreTab)
        {
            _app.CreateRibbonTab(nombreTab);
        }
        public RibbonPanel CreatePanel(string tab, string nombrePanel)
        {
            return _app.CreateRibbonPanel(tab, nombrePanel);
        }
        public void CreateBtn(RibbonPanel panel,string id, string nombreBoton)
        {
            string route = Assembly.GetExecutingAssembly().Location;
            PushButtonData btn = new PushButtonData(
                id, 
                nombreBoton, 
                route, 
                typeof(Command.ButtonCommand).FullName
                );
            panel.AddItem(btn);            
        }


    }
}
