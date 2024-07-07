using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace JadedCmsCore.Services.Core;

public class HelperServices
{
    public async Task<string> ExecuteControllerAction(IServiceProvider serviceProvider, string fullyQualifiedControllerName, string actionName, params object[] parameters)
    {
        Type controllerType = Type.GetType(fullyQualifiedControllerName);
        if (controllerType == null)
        {
            throw new ArgumentException($"Controller type '{fullyQualifiedControllerName}' not found. Ensure the class library is referenced and the name is correct.", nameof(fullyQualifiedControllerName));
        }
    
        var controller = ActivatorUtilities.CreateInstance(serviceProvider, controllerType);
        var methodInfo = controllerType.GetMethod(actionName);
        if (methodInfo == null)
        {
            throw new ArgumentException($"Action '{actionName}' not found in controller '{fullyQualifiedControllerName}'.", nameof(actionName));
        }
    
        var result = await (Task<IActionResult>)methodInfo.Invoke(controller, parameters);
    
        if (result is ContentResult contentResult)
        {
            return contentResult.Content;
        }
        else
        {
            // Handle other types of IActionResult if necessary
            return null;
        }
    }
}
