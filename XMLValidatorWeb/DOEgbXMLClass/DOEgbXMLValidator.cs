﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using System.Xml.Schema;

namespace DOEgbXML
{
    public class DOEgbXMLValidator
    {
       
            public int nErrors = 0;
            public int nWarnings = 0;
            private string strErrorMsg = string.Empty;
            public string Errors { get { return strErrorMsg; } }
            public string BigError;
            public string filePath { get; set; }

            public DOEgbXMLValidator()
            {
                filePath = "SupportFiles/XSD/GreenBuildingXML_Ver5.10.xsd";
            }

            public DOEgbXMLValidator(string filename)
            {
                filePath = "SupportFiles/XSD/" + filename;
            }

            public bool IsValidXmlEx(XmlReader xmlStream)
            {
                bool bStatus = false;
                try
                {
                    // Declare local objects
                    // Improve to allow any schema version to be selected.
                    string xsdSchemaLocalLocation = Path.Combine(HttpRuntime.AppDomainAppPath, this.filePath);
                    XmlReaderSettings rs = new XmlReaderSettings();
                    rs.ValidationType = ValidationType.Schema;
                    rs.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation | XmlSchemaValidationFlags.ReportValidationWarnings;
                    rs.ValidationEventHandler += new ValidationEventHandler(rs_ValidationEventHandler);

                    //add schema
                    rs.Schemas.Add(null, xsdSchemaLocalLocation);


                    using (XmlReader xmlValidatingReader = XmlReader.Create(xmlStream, rs))
                    {
                        while (xmlValidatingReader.Read())
                        {
                        }
                    }

                    ////Exception if error.s
                    if (nErrors > 0)
                    {
                        throw new Exception(strErrorMsg);
                    }
                    else { bStatus = true; }//Success
                }
                catch (Exception error)
                {
                    BigError = "BIG ERROR: " + error + "<br />";
                    bStatus = false;
                }

                return bStatus;
            }

            void rs_ValidationEventHandler(object sender, ValidationEventArgs e)
            {

                if (e.Severity == XmlSeverityType.Warning)
                {
                    strErrorMsg += "<p class='text-warning'>" + "WARNING: " + e.Exception.Message + " Line Position " + e.Exception.LinePosition + " Line Number: " + e.Exception.LineNumber + "</p>";
                    nWarnings++;
                }
                else if(!e.Exception.Message.Contains("The element cannot contain white space. Content model is empty."))
                {
                    
                    strErrorMsg += "<p class='text-error'>" + "ERROR: " + e.Exception.Message + " Line Position " + e.Exception.LinePosition + " Line Number: " + e.Exception.LineNumber + "</p>";
                    nErrors++;
                }
            }
        
    }
}