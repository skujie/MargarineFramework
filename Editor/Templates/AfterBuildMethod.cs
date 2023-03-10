using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Framework.Editor.Build
{
    public static class AfterBuildMethod
    {
        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) 
        {
            
        }
    }
}
