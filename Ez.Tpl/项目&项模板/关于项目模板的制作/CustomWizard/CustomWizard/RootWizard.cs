using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE;
using System.Windows.Forms;
namespace UBIQProjTemplateWizard
{
    public class RootWizard : IWizard
    {
        public static Dictionary<string, string> GlobalDictionary = new Dictionary<string, string>();
        private UBIQProjectDlg inputForm;
        private string customMessage;
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
            try
            {
                inputForm = new UBIQProjectDlg(replacementsDictionary["$safeprojectname$"]);
                DialogResult result = inputForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string proj_code = inputForm.Proj_code;
                    string proj_name = inputForm.Proj_name;
                    string company = inputForm.Proj_company;
                    string uistyle = inputForm.Proj_uistyle;
                    replacementsDictionary.Add("$projcode$", proj_code);
                    GlobalDictionary["$saferootprojectname$"] = "UBIQ." + replacementsDictionary["$safeprojectname$"];
                    replacementsDictionary.Add("$saferootprojectname$", GlobalDictionary["$saferootprojectname$"]);
                    GlobalDictionary["$projcode$"] = replacementsDictionary["$projcode$"];
                    replacementsDictionary.Add("$projname$", proj_name);
                    GlobalDictionary["$projname$"] = replacementsDictionary["$projname$"];
                    replacementsDictionary.Add("$uistyle$", uistyle);
                    GlobalDictionary["$uistyle$"] = replacementsDictionary["$uistyle$"];
                    replacementsDictionary.Add("$company$", company);
                    GlobalDictionary["$company$"] = replacementsDictionary["$company$"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
