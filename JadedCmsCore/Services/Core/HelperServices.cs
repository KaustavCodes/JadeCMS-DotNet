using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace JadedCmsCore.Services.Core;

public class HelperServices
{
    private Dictionary<string, Type> controllerTypeCache = new Dictionary<string, Type>();
    public async Task<string> ExecuteControllerAction(IServiceProvider serviceProvider, string fullyQualifiedControllerName, string actionName, params object[] parameters)
    {
        if (!controllerTypeCache.TryGetValue(fullyQualifiedControllerName, out Type controllerType))
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                controllerType = assembly.GetType(fullyQualifiedControllerName);
                if (controllerType != null)
                {
                    controllerTypeCache[fullyQualifiedControllerName] = controllerType;
                    break;
                }
            }
        }
        //Type controllerType = Type.GetType(fullyQualifiedControllerName);
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
    
        dynamic result = methodInfo.Invoke(controller, parameters);

        if (result is Task)
        {
            result = await result;
        }
    
        if (result is Task<IActionResult> taskResult)
        {
            var awaitedResult = await taskResult;
            if (awaitedResult is ContentResult contentResult)
            {
                return contentResult.Content;
            }
            else
            {
                // Handle other types of IActionResult if necessary
                return null;
            }
        }
        else if (result is IActionResult actionResult)
        {
            if (actionResult is ContentResult contentResult)
            {
                return contentResult.Content;
            }
            else
            {
                // Handle other types of IActionResult if necessary
                return null;
            }
        }
        else
        {
            throw new InvalidOperationException("The action result is not an IActionResult or Task<IActionResult>.");
        }
    }
}
