using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.CustomProperties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Jpp.Common.Backend.Projects
{
    class DomesticFeeTaskSchedule : JppProjectDocument
    {
        public DomesticFeeTaskSchedule()
        {
            AssetTemplate = "ms-appx:///Assets/Templates/F05-03 Fee and Task Schedule (Domestic).docm";
            TemplatePath = "\\Financial\\Quotes";
            TemplateTitle = "Fee and Task Schedule";
            TemplateExtension = "docm";
        }

        public override Stream ProcessTemplate(Stream fileStream, ProjectModel pm)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(fileStream, true))
            {
                DocumentSettingsPart settingsPart = doc.MainDocumentPart.GetPartsOfType<DocumentSettingsPart>().First();
                
                // Create object to update fields on open
                UpdateFieldsOnOpen updateFields = new UpdateFieldsOnOpen();
                updateFields.Val = new DocumentFormat.OpenXml.OnOffValue(true);

                // Insert object into settings part.
                settingsPart.Settings.PrependChild<UpdateFieldsOnOpen>(updateFields);
                settingsPart.Settings.Save();

                UpdateProperty(doc, "Title", pm.Name);
                UpdateProperty(doc, "Revision", CurrentRevision);
                UpdateProperty(doc, "ProjectReference", pm.Code);
                UpdateProperty(doc, "Service", pm.Discipline);

                doc.Close();

                return fileStream;
            }
        }
    }
}
