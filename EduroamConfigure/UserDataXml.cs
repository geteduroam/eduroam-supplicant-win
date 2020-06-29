﻿using System.Xml.Linq;

namespace EduroamConfigure
{
    /// <summary>
    /// User data XML generator.
    /// Generates user data for the following EAP types:
    /// - PEAP-MSCHAPv2 (25/26)
    /// - TTLS (21) [NOT YET FUNCTIONAL]
    /// 
    /// Documentation:
    /// https://docs.microsoft.com/en-us/windows/win32/eaphost/eaphostusercredentialsschema-schema
    /// https://docs.microsoft.com/en-us/windows/win32/eaphost/user-profiles
    /// https://github.com/rozmansi/WLANSetEAPUserData/tree/master/Examples
    /// C:\Windows\schemas\EAPMethods
    /// C:\Windows\schemas\EAPHost
    /// </summary>
    class UserDataXml
    {
        // Namespaces:

        static readonly XNamespace nsEHUC = "http://www.microsoft.com/provisioning/EapHostUserCredentials";
        static readonly XNamespace nsEC = "http://www.microsoft.com/provisioning/EapCommon";
        static readonly XNamespace nsBEMUC = "http://www.microsoft.com/provisioning/BaseEapMethodUserCredentials";
        static readonly XNamespace nsEUP = "http://www.microsoft.com/provisioning/EapUserPropertiesV1";
        static readonly XNamespace nsXSI = "http://www.w3.org/2001/XMLSchema-instance";
        static readonly XNamespace nsBEUP = "http://www.microsoft.com/provisioning/BaseEapUserPropertiesV1";
        
        // PEAP / MSCHAPv2 specific
        static readonly XNamespace nsMPUP = "http://www.microsoft.com/provisioning/MsPeapUserPropertiesV1";
        static readonly XNamespace nsMCUP = "http://www.microsoft.com/provisioning/MsChapV2UserPropertiesV1";
        
        // TTLS specific
        static readonly XNamespace nsTTLS = "http://www.microsoft.com/provisioning/EapTtlsUserPropertiesV1";
       
        // TLS specific
        static readonly XNamespace nsTLS = "http://www.microsoft.com/provisioning/EapTtlsUserPropertiesV1";

        /// <summary>
        /// Generates user data xml.
        /// </summary>
        /// <param name="uname">Username.</param>
        /// <param name="pword">Password.</param>
        /// <param name="eapType">EAP type</param>
        /// <param name="innerAuthType">inner EAP type</param>
        /// <returns>Complete user data xml as string.</returns>
        public static string CreateUserDataXml(
            string uname,
            string pword,
            EapType eapType,
            InnerAuthType innerAuthType)
        {
            // TODO: install a profile for TLS with a fingerprint of the user certificate
            /*  <EapHostUserCredentials xmlns="http://www.microsoft.com/provisioning/EapHostUserCredentials"
                  xmlns:eapCommon="http://www.microsoft.com/provisioning/EapCommon"
                  xmlns:baseEap="http://www.microsoft.com/provisioning/BaseEapMethodUserCredentials">
                  <EapMethod>
                    <eapCommon:Type>13</eapCommon:Type>
                    <eapCommon:AuthorId>0</eapCommon:AuthorId>
                  </EapMethod>
                  <Credentials xmlns:eapUser="http://www.microsoft.com/provisioning/EapUserPropertiesV1"
                      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                      xmlns:baseEap="http://www.microsoft.com/provisioning/BaseEapUserPropertiesV1"
                      xmlns:eapTls="http://www.microsoft.com/provisioning/EapTlsUserPropertiesV1">
                    <baseEap:Eap>
                       <baseEap:Type>13</baseEap:Type>
                       <eapTls:EapType>
                          <eapTls:UserCert>e7 d5 3f 53 8a 30 c5 e3 8c a9 79 7e eb 40 33 a0 d9 c6 8f eb </eapTls:UserCert>
                       </eapTls:EapType>
                    </baseEap:Eap>
                  </Credentials>
                </EapHostUserCredentials> 
             */


            XElement newUserData = (eapType, innerAuthType) switch
            {
                var x when
                x == (EapType.PEAP, InnerAuthType.EAP_MSCHAPv2) =>

                    new XElement(nsEHUC + "EapHostUserCredentials",
                        new XAttribute(XNamespace.Xmlns + "eapCommon", nsEC),
                        new XAttribute(XNamespace.Xmlns + "baseEap", nsBEMUC),
                        new XElement(nsEHUC + "EapMethod",
                            new XElement(nsEC + "Type", (uint)eapType),
                            new XElement(nsEC + "AuthorId", eapType==EapType.TTLS ? 311: 0)
                            //new XElement(nsEC + "AuthorId", "67532") // geant link
                        ),  
                        new XElement(nsEHUC + "Credentials",
                            new XAttribute(XNamespace.Xmlns + "eapuser", nsEUP),
                            new XAttribute(XNamespace.Xmlns + "xsi", nsXSI),
                            new XAttribute(XNamespace.Xmlns + "baseEap", nsBEUP),
                            new XAttribute(XNamespace.Xmlns + "MsPeap", nsMPUP),
                            new XAttribute(XNamespace.Xmlns + "MsChapV2", nsMCUP),
                            new XAttribute(XNamespace.Xmlns + "eapTtls", nsTTLS), // test
                            new XElement(nsBEUP + "Eap",
                                new XElement(nsBEUP + "Type", (uint)EapType.PEAP),
                                new XElement(nsMPUP + "EapType",
                                    new XElement(nsMPUP + "RoutingIdentity"), // TODO: probably anonymous/outer identity. use it!
                                    new XElement(nsBEUP + "Eap",
                                        new XElement(nsBEUP + "Type", (uint)InnerAuthType.EAP_MSCHAPv2),
                                        new XElement(nsMCUP + "EapType",
                                            new XElement(nsMCUP + "Username", uname),
                                            new XElement(nsMCUP + "Password", pword),
                                            new XElement(nsMCUP + "LogonDomain") // what is this?
                                        )
                                    )
                                )
                            )
                        )
                    ),


                var x when
                x == (EapType.TTLS, InnerAuthType.PAP) ||
                x == (EapType.TTLS, InnerAuthType.MSCHAP) || // needs testing
                x == (EapType.TTLS, InnerAuthType.MSCHAPv2) => // needs testing

                    new XElement(nsEHUC + "EapHostUserCredentials",
                        new XAttribute(XNamespace.Xmlns + "eapCommon", nsEC),
                        new XAttribute(XNamespace.Xmlns + "baseEap", nsBEMUC),
                        new XElement(nsEHUC + "EapMethod",
                            new XElement(nsEC + "Type", (uint)eapType),
                            new XElement(nsEC + "AuthorId", eapType == EapType.TTLS ? 311 : 0)
                            //new XElement(nsEC + "AuthorId", "67532") // geant link
                        ),
                        new XElement(nsEHUC + "Credentials",
                            new XAttribute(XNamespace.Xmlns + "eapuser", nsEUP),
                            new XAttribute(XNamespace.Xmlns + "xsi", nsXSI),
                            new XAttribute(XNamespace.Xmlns + "baseEap", nsBEUP),
                            new XAttribute(XNamespace.Xmlns + "MsPeap", nsMPUP),
                            new XAttribute(XNamespace.Xmlns + "MsChapV2", nsMCUP),
                            new XAttribute(XNamespace.Xmlns + "eapTtls", nsTTLS),
                            new XElement(nsTTLS + "EapTtls", // schema says lower camelcase, but only upper camelcase works
                                new XElement(nsTTLS + "Username", uname),
                                new XElement(nsTTLS + "Password", pword)
                            )
                        )
                    ),


                // TODO: WORK IN PROGRESS - Dependent on creating a correct profile XML for TTLS
                var x when
                //x == (EapType.TTLS, InnerAuthType.EAP_PEAP_MSCHAPv2) || // TODO: <-- does not work, needs a test also
                x == (EapType.TTLS, InnerAuthType.EAP_MSCHAPv2) =>

                    new XElement(nsEHUC + "EapHostUserCredentials",
                        new XAttribute(XNamespace.Xmlns + "eapCommon", nsEC),
                        new XAttribute(XNamespace.Xmlns + "baseEap", nsBEMUC),
                        new XElement(nsEHUC + "EapMethod",
                            new XElement(nsEC + "Type", (uint)eapType), // TODO: test
                            new XElement(nsEC + "AuthorId", eapType == EapType.TTLS ? 311 : 0)
                            //new XElement(nsEC + "AuthorId", "67532") // geant link
                        ),
                        new XElement(nsEHUC + "Credentials",
                            new XAttribute(XNamespace.Xmlns + "eapuser", nsEUP),
                            new XAttribute(XNamespace.Xmlns + "xsi", nsXSI),
                            new XAttribute(XNamespace.Xmlns + "baseEap", nsBEUP),
                            new XAttribute(XNamespace.Xmlns + "MsPeap", nsMPUP),
                            new XAttribute(XNamespace.Xmlns + "MsChapV2", nsMCUP),
                            new XAttribute(XNamespace.Xmlns + "eapTtls", nsTTLS),
                            new XElement(nsTTLS + "EapTtls",
                                //new XElement(nsTTLS + "Username", uname),
                                //new XElement(nsTTLS + "Password", pword),
                                new XElement(nsBEUP + "Eap",
                                    new XElement(nsBEUP + "Type", (uint)EapType.MSCHAPv2),
                                    new XElement(nsMCUP + "EapType",
                                        new XElement(nsMCUP + "Username", uname),
                                        new XElement(nsMCUP + "Password", pword),
                                        new XElement(nsMCUP + "LogonDomain")
                                    )
                                )
                            )
                        )
                    ),


                // TODO: handle the missing EapType cases in a different way?
                _ => null,

            };

            // returns xml as string if not null
            return newUserData != null ? newUserData.ToString() : "";
        }

        public static bool IsSupported(EapConfig.AuthenticationMethod authMethod)
        {
            return IsSupported(authMethod.EapType, authMethod.InnerAuthType);
        }

        public static bool IsSupported(EapType eapType, InnerAuthType innerAuthType)
        {
            bool at_least_win10 = System.Environment.OSVersion.Version.Major >= 10;
            return (eapType, innerAuthType) switch
            {
                //(EapType.TLS, _) => true, // TODO
                (EapType.PEAP, InnerAuthType.EAP_MSCHAPv2) => true,
                (EapType.TTLS, InnerAuthType.PAP) => true,
                (EapType.TTLS, InnerAuthType.MSCHAP) => true,
                (EapType.TTLS, InnerAuthType.MSCHAPv2) => true,
                //(EapType.TTLS, InnerAuthType.EAP_MSCHAPv2) => at_least_win10, // TODO: xml matches the schema, but win32 throws an error.
                _ => false,
            };
        }
    }
}