using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Ara3D.BIMOpenSchema.Revit2025
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class OpenSchemaExternalCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                if (OpenSchemaApp.Instance == null)
                    throw new Exception("Application was never instantiated");
                    
                OpenSchemaApp.Instance.Run(commandData.Application);
                return Result.Succeeded;
			}
			catch ( Exception ex)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Error",ex.ToString());
                return Result.Failed;
            }
        }
    }
}
