// LuaTemplateGen.Program
using System;
using System.IO;
using System.Text;

internal class Program
{
    private static string s_WorkPath;

    private static string s_WorkGamePath;

    private static string s_WorkTemplatePath;

    private static string s_WorkDefinePath;

    private static void Main(string[] args)
    {
        try
        {
            s_WorkPath = args[0];
            s_WorkDefinePath = Path.Combine(s_WorkPath, "game/modules/module_register.lua");
            s_WorkGamePath = Path.Combine(s_WorkPath, "game");
            s_WorkTemplatePath = Path.Combine(s_WorkPath, "template");
            if (args[1] == "createModule")
            {
                CreateModule(args[2]);
                UpdateDefine();
            }
            else if (args[1] == "createView")
            {
                CreateView(args[2], (args.Length >= 3) ? args[3] : null);
            }
            else if (args[1] == "createItemRenderer")
            {
                CreateItemRenderer(args[2], args[3]);
            }
            else if (args[1] == "createContentRenderer")
            {
                CreateContentRenderer(args[2], args[3]);
            }
            else if (args[1] == "createPanel")
            {
                CreatePanel(args[2], (args.Length >= 3) ? args[3] : null);
            }
            else if (args[1] == "createWindow")
            {
                CreateWindow(args[2], (args.Length >= 3) ? args[3] : null);
            }
            else if (args[1] == "updateDefine")
            {
                UpdateDefine();
            }
            Console.WriteLine("Generate finish !");
        }
        catch (Exception ex)
        {
            Console.Write(ex.ToString());
        }
    }

    private static void CreateModule(string moduleName)
    {
        if (string.IsNullOrEmpty(moduleName))
        {
            Console.WriteLine($"moduleName IsNullOrEmpty moduleName={moduleName}");
            return;
        }
        string value = new string(new char[1]
        {
            moduleName[0]
        }).ToUpper();
        moduleName = moduleName.Remove(0, 1).Insert(0, value);
        string str = moduleName.ToLower();
        string text = Path.Combine(s_WorkGamePath, "modules/" + str);
        if (!Directory.Exists(text))
        {
            Directory.CreateDirectory(text);
        }
        CreateView(moduleName);
        CreateTemplate(moduleName, Path.Combine(text, moduleName + "Module.lua"), Path.Combine(s_WorkTemplatePath, "module_template.lua"));
        CreateTemplate(moduleName, Path.Combine(text, moduleName + "Proxy.lua"), Path.Combine(s_WorkTemplatePath, "proxy_template.lua"));
        CreateTemplate(moduleName, Path.Combine(text, moduleName + "Config.lua"), Path.Combine(s_WorkTemplatePath, "config_template.lua"));
        CreateTemplate(moduleName, Path.Combine(text, moduleName + "Data.lua"), Path.Combine(s_WorkTemplatePath, "data_template.lua"));
    }

    private static void CreateView(string moduleName, string viewName = null)
    {
        string str = moduleName.ToLower();
        string text = Path.Combine(s_WorkGamePath, "modules/" + str);
        if (!Directory.Exists(text))
        {
            Console.WriteLine("{0} 模块不存在", text);
            return;
        }
        string text2 = Path.Combine(text, "views");
        if (!Directory.Exists(text2))
        {
            Directory.CreateDirectory(text2);
        }
        viewName = (string.IsNullOrEmpty(viewName) ? moduleName : viewName);
        CreateTemplate(viewName, Path.Combine(text2, viewName + "View.lua"), Path.Combine(s_WorkTemplatePath, "view_template.lua"));
    }

    private static void CreatePanel(string moduleName, string panelName = null)
    {
        string str = moduleName.ToLower();
        string text = Path.Combine(s_WorkGamePath, "modules/" + str);
        if (!Directory.Exists(text))
        {
            Console.WriteLine("{0} 模块不存在", text);
            return;
        }
        string text2 = Path.Combine(text, "views/panels");
        if (!Directory.Exists(text2))
        {
            Directory.CreateDirectory(text2);
        }
        panelName = (string.IsNullOrEmpty(panelName) ? moduleName : panelName);
        CreateTemplate(panelName, Path.Combine(text2, panelName + "Panel.lua"), Path.Combine(s_WorkTemplatePath, "panel_template.lua"));
    }

    private static void CreateWindow(string moduleName, string winName = null)
    {
        string str = moduleName.ToLower();
        string text = Path.Combine(s_WorkGamePath, "modules/" + str);
        if (!Directory.Exists(text))
        {
            Console.WriteLine("{0} 模块不存在", text);
            return;
        }
        string text2 = Path.Combine(text, "views");
        if (!Directory.Exists(text2))
        {
            Directory.CreateDirectory(text2);
        }
        winName = (string.IsNullOrEmpty(winName) ? moduleName : winName);
        CreateTemplate(winName, Path.Combine(text2, winName + "Window.lua"), Path.Combine(s_WorkTemplatePath, "window_template.lua"));
    }

    private static void CreateItemRenderer(string moduleName, string viewName)
    {
        string str = moduleName.ToLower();
        string text = Path.Combine(s_WorkGamePath, "modules/" + str);
        if (!Directory.Exists(text))
        {
            Console.WriteLine("{0} 模块不存在", text);
            return;
        }
        string text2 = Path.Combine(text, "views");
        if (!Directory.Exists(text2))
        {
            Directory.CreateDirectory(text2);
        }
        viewName = (string.IsNullOrEmpty(viewName) ? moduleName : viewName);
        CreateTemplate(viewName, Path.Combine(text2, viewName + "ItemRenderer.lua"), Path.Combine(s_WorkTemplatePath, "list_renderer_template.lua"));
    }

    private static void CreateContentRenderer(string moduleName, string viewName)
    {
        string str = moduleName.ToLower();
        string text = Path.Combine(s_WorkGamePath, "modules/" + str);
        if (!Directory.Exists(text))
        {
            Console.WriteLine("{0} 模块不存在", text);
            return;
        }
        string text2 = Path.Combine(text, "views");
        if (!Directory.Exists(text2))
        {
            Directory.CreateDirectory(text2);
        }
        viewName = (string.IsNullOrEmpty(viewName) ? moduleName : viewName);
        CreateTemplate(viewName, Path.Combine(text2, viewName + "ContentRenderer.lua"), Path.Combine(s_WorkTemplatePath, "content_renderer_template.lua"));
    }

    private static void CreateTemplate(string name, string targetPath, string templatePath)
    {
        if (File.Exists(targetPath))
        {
            Console.WriteLine($"{targetPath} is Exists");
            return;
        }
        if (!File.Exists(templatePath))
        {
            Console.WriteLine($"{templatePath} is not Exists");
            return;
        }
        string newValue = name.ToLower();
        string text = File.ReadAllText(templatePath);
        text = text.Replace("${NAME}", name).Replace("${NAME_L}", newValue);
        File.WriteAllText(targetPath, text);
        Console.WriteLine($"{targetPath} succeed.");
    }

    private static void UpdateDefine()
    {
        string text = Path.Combine(s_WorkTemplatePath, "define_template.lua");
        if (!File.Exists(text))
        {
            Console.WriteLine($"{text} 不存在!");
            return;
        }
        string text2 = Path.Combine(s_WorkGamePath, "modules");
        if (!Directory.Exists(text2))
        {
            Console.WriteLine($"{text2} 不存在!");
            return;
        }
        string[] files = Directory.GetFiles(text2, "*Module.lua", SearchOption.AllDirectories);
        StringBuilder stringBuilder = new StringBuilder();
        int num = 0;
        string[] array = files;
        foreach (string text3 in array)
        {
            num++;
            string text4 = text3.Substring(s_WorkPath.Length).Replace("\\", ".").Replace(".lua", "");
            stringBuilder.AppendLine($"context.registerModule('{text4}')");
            Console.WriteLine(" {0}  registerModule【 {1} 】", num, text4);
        }
        Console.WriteLine($" Define Count: {num}");
        string text5 = File.ReadAllText(text);
        text5 = text5.Replace("${ALL_MODULE}", stringBuilder.ToString());
        File.WriteAllText(s_WorkDefinePath, text5);
    }
}
