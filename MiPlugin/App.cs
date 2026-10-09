using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MiPlugin.Ribbon;
using System.Security.Cryptography.X509Certificates;

namespace MiPlugin
{
    public class App : IExternalApplication
    {

        public Result OnShutdown(UIControlledApplication application)
        {
            throw new NotImplementedException();
        }

        public Result OnStartup(UIControlledApplication application)
        {
            CreateButton.CreateTabPanelButton(application);
            return Result.Succeeded;
        }
    }
}
