using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TemplateWizard;

namespace UBIQProjTemplateWizard
{
    public class ChildWizard:IWizard
    {

        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {
            
        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {
           
        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
            
        }

        public void RunFinished()
        {
            
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            replacementsDictionary.Add("$saferootprojectname$",RootWizard.GlobalDictionary["$saferootprojectname$"]);
            replacementsDictionary.Add("$projcode$", RootWizard.GlobalDictionary["$projcode$"]);
            replacementsDictionary.Add("$projname$", RootWizard.GlobalDictionary["$projname$"]);
            replacementsDictionary.Add("$uistyle$", RootWizard.GlobalDictionary["$uistyle$"]);
            replacementsDictionary.Add("$company$", RootWizard.GlobalDictionary["$company$"]);
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
