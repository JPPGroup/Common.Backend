using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.CustomProperties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.VariantTypes;

namespace Jpp.Common.Backend.Projects
{
    public abstract class JppProjectDocument : ProjectDocument
    {
        public string TemplatePath { get; set; }
        public string AssetTemplate { get; set; }

        public string TemplateTitle { get; set; }
        public string TemplateExtension { get; set; }

        public abstract Stream ProcessTemplate(Stream fileStream, ProjectModel pm);

        internal void UpdateProperty(WordprocessingDocument doc, string PropertName, string PropertyValue)
        {
            var props = doc.CustomFilePropertiesPart.Properties;
            var prop = props.Where(p => ((CustomDocumentProperty)p).Name.Value == PropertName).FirstOrDefault();

            if (prop != null)
            {
                prop.Remove();
            }

            var newProp = new CustomDocumentProperty();
            newProp.VTLPWSTR = new VTLPWSTR(PropertyValue);
            newProp.FormatId = "{D5CDD505-2E9C-101B-9397-08002B2CF9AE}";
            newProp.Name = PropertName;

            props.AppendChild(newProp);
            int pid = 2;
            foreach (CustomDocumentProperty item in props)
            {
                item.PropertyId = pid++;
            }

            props.Save();
        }

        public async void Create(Stream templateContents, IStorageProvider storage)
        {
            using (Stream outStream = await storage.OpenFileForWrite($"{this.Parent.Path}\\{PhysicalName}", true))
            {
                if (outStream != null)
                {
                    MemoryStream modified = new MemoryStream();
                    templateContents.CopyTo(modified);

                    Stream modifiedData = ProcessTemplate(modified, GetProjectModel());
                    modifiedData.Position = 0;

                    await modifiedData.CopyToAsync(outStream);
                    await outStream.FlushAsync();
                }
            }
        }
    }
}
