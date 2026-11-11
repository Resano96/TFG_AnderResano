using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AR.Revit.Prometeo.Ribbon;
using System.Security.Cryptography.X509Certificates;

namespace AR.Revit.Prometeo
{
    public class App : IExternalApplication
    {

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            CreateButton.CreateTabPanelButton(application);
            return Result.Succeeded;
        }
    }
}
