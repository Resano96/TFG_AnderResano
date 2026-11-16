using Autodesk.Revit.UI;
using System.Reflection;


namespace AR.Revit.Prometeo.Ribbon
{
    internal static class CreateMenu
    {
        private static readonly Dictionary<string, List<(string, string, string)>> BtnDiccionary = new ()
        {
            ["AR_Solution"] =
                [
                    ("id1","AR_medir",typeof(Command.ButtonCommand).FullName!),
                    ("id2","AR_parametros",typeof(Command.ParamButton).FullName!)
                ]
        };
        private static readonly string _tabName = "AR_Prometeo";


        private static void CreateTab(UIControlledApplication app)
        {
            app.CreateRibbonTab(_tabName);
        }
        private static RibbonPanel CreatePanel(UIControlledApplication app,string name)
        {
            return app.CreateRibbonPanel(_tabName, name);
        }
        private static void CreateBtn(string id, string btnName, string btnFunction, RibbonPanel panel)
        {
            string route = Assembly.GetExecutingAssembly().Location;
            PushButtonData btn = new PushButtonData(id, btnName, route, btnFunction);
            panel.AddItem(btn);
        }
        public static void Create (UIControlledApplication app)
        { 
            CreateTab(app);
            foreach (var (key, value) in BtnDiccionary)
            {
                RibbonPanel panel = CreatePanel(app,key);
                foreach (var (id, name, command) in value)
                {
                    CreateBtn(id, name, command, panel);
                }

            }

        }
    }
}
