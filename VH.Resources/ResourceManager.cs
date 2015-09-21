using System.Globalization;

namespace VH.Resources
{
   
        #region Culture class
        /// <summary>
        /// Culture contains properties to get for supported Cultures.
        /// When new culture support is added, by adding a new culture
        /// specific resx file, a new coresponding property will need 
        /// to be added
        /// </summary>
        public class ResourceManager
        {
            public static CultureInfo Default { get { return CultureInfo.CurrentCulture; } }

            #region All cultures are defined Here
            public static CultureInfo Afrikaans_SouthAfrica { get { return new CultureInfo("af-ZA"); } }
            public static CultureInfo Arabic_UAE { get { return new CultureInfo("ar-AE"); } }
            public static CultureInfo Arabic_Bahrain { get { return new CultureInfo("ar-BH"); } }
            public static CultureInfo Arabic_Algeria { get { return new CultureInfo("ar-DZ"); } }
            public static CultureInfo Arabic_Egypt { get { return new CultureInfo("ar-EG"); } }
            public static CultureInfo Arabic_Iraq { get { return new CultureInfo("ar-IQ"); } }
            public static CultureInfo Arabic_Jordan { get { return new CultureInfo("ar-JO"); } }
            public static CultureInfo Arabic_Kuwait { get { return new CultureInfo("ar-KW"); } }
            public static CultureInfo Arabic_Lebanon { get { return new CultureInfo("ar-LB"); } }
            public static CultureInfo Arabic_Libya { get { return new CultureInfo("ar-LY"); } }
            public static CultureInfo Arabic_Morocco { get { return new CultureInfo("ar-MA"); } }
            public static CultureInfo Arabic_Oman { get { return new CultureInfo("ar-OM"); } }
            public static CultureInfo Arabic_Qatar { get { return new CultureInfo("ar-QA"); } }
            public static CultureInfo Arabic_SaudiArabia { get { return new CultureInfo("ar-SA"); } }
            public static CultureInfo Arabic_Syria { get { return new CultureInfo("ar-SY"); } }
            public static CultureInfo Arabic_Tunisia { get { return new CultureInfo("ar-TN"); } }
            public static CultureInfo Arabic_Yemen { get { return new CultureInfo("ar-YE"); } }
            public static CultureInfo Belarusian_Belarus { get { return new CultureInfo("be-BY"); } }
            public static CultureInfo Bulgarian_Bulgaria { get { return new CultureInfo("bg-BG"); } }
            public static CultureInfo Catalan_Catalan { get { return new CultureInfo("ca-ES"); } }
            public static CultureInfo Czech_CzechRepublic { get { return new CultureInfo("cs-CZ"); } }
            public static CultureInfo Welsh_UnitedKingdom { get { return new CultureInfo("cy-GB"); } }
            public static CultureInfo Danish_Denmark { get { return new CultureInfo("da-DK"); } }
            public static CultureInfo German_Austria { get { return new CultureInfo("de-AT"); } }
            public static CultureInfo German_Switzerland { get { return new CultureInfo("de-CH"); } }
            public static CultureInfo German_Germany { get { return new CultureInfo("de-DE"); } }
            public static CultureInfo German_Liechtenstein { get { return new CultureInfo("de-LI"); } }
            public static CultureInfo German_Luxembourg { get { return new CultureInfo("de-LU"); } }
            public static CultureInfo Greek_Greece { get { return new CultureInfo("el-GR"); } }
            public static CultureInfo English_Australia { get { return new CultureInfo("en-AU"); } }
            public static CultureInfo English_Belize { get { return new CultureInfo("en-BZ"); } }
            public static CultureInfo English_Canada { get { return new CultureInfo("en-CA"); } }
            public static CultureInfo English_Caribbean { get { return new CultureInfo("en-CB"); } }
            public static CultureInfo English_UnitedKingdom { get { return new CultureInfo("en-GB"); } }
            public static CultureInfo English_Ireland { get { return new CultureInfo("en-IE"); } }
            public static CultureInfo English_Jamaica { get { return new CultureInfo("en-JM"); } }
            public static CultureInfo English_NewZealand { get { return new CultureInfo("en-NZ"); } }
            public static CultureInfo English_RepublicofthePhilippines { get { return new CultureInfo("en-PH"); } }
            public static CultureInfo English_TrinidadandTobago { get { return new CultureInfo("en-TT"); } }
            public static CultureInfo English_UnitedStates { get { return new CultureInfo("en-US"); } }
            public static CultureInfo English_SouthAfrica { get { return new CultureInfo("en-ZA"); } }
            public static CultureInfo English_Zimbabwe { get { return new CultureInfo("en-ZW"); } }
            public static CultureInfo Spanish_Argentina { get { return new CultureInfo("es-AR"); } }
            public static CultureInfo Spanish_Bolivia { get { return new CultureInfo("es-BO"); } }
            public static CultureInfo Spanish_Chile { get { return new CultureInfo("es-CL"); } }
            public static CultureInfo Spanish_Colombia { get { return new CultureInfo("es-CO"); } }
            public static CultureInfo Spanish_CostaRica { get { return new CultureInfo("es-CR"); } }
            public static CultureInfo Spanish_DominicanRepublic { get { return new CultureInfo("es-DO"); } }
            public static CultureInfo Spanish_Ecuador { get { return new CultureInfo("es-EC"); } }
            public static CultureInfo Spanish_Spain { get { return new CultureInfo("es-ES"); } }
            public static CultureInfo Spanish_Guatemala { get { return new CultureInfo("es-GT"); } }
            public static CultureInfo Spanish_Honduras { get { return new CultureInfo("es-HN"); } }
            public static CultureInfo Spanish_Mexico { get { return new CultureInfo("es-MX"); } }
            public static CultureInfo Spanish_Nicaragua { get { return new CultureInfo("es-NI"); } }
            public static CultureInfo Spanish_Panama { get { return new CultureInfo("es-PA"); } }
            public static CultureInfo Spanish_Peru { get { return new CultureInfo("es-PE"); } }
            public static CultureInfo Spanish_PuertoRico { get { return new CultureInfo("es-PR"); } }
            public static CultureInfo Spanish_Paraguay { get { return new CultureInfo("es-PY"); } }
            public static CultureInfo Spanish_ElSalvador { get { return new CultureInfo("es-SV"); } }
            public static CultureInfo Spanish_Uruguay { get { return new CultureInfo("es-UY"); } }
            public static CultureInfo Spanish_Venezuela { get { return new CultureInfo("es-VE"); } }
            public static CultureInfo Estonian_Estonia { get { return new CultureInfo("et-EE"); } }
            public static CultureInfo Basque_Basque { get { return new CultureInfo("eu-ES"); } }
            public static CultureInfo Persian_Iran { get { return new CultureInfo("fa-IR"); } }
            public static CultureInfo Finnish_Finland { get { return new CultureInfo("fi-FI"); } }
            public static CultureInfo Faroese_FaroeIslands { get { return new CultureInfo("fo-FO"); } }
            public static CultureInfo French_Belgium { get { return new CultureInfo("fr-BE"); } }
            public static CultureInfo French_Canada { get { return new CultureInfo("fr-CA"); } }
            public static CultureInfo French_Switzerland { get { return new CultureInfo("fr-CH"); } }
            public static CultureInfo French_France { get { return new CultureInfo("fr-FR"); } }
            public static CultureInfo French_Luxembourg { get { return new CultureInfo("fr-LU"); } }
            public static CultureInfo French_PrincipalityofMonaco { get { return new CultureInfo("fr-MC"); } }
            public static CultureInfo Galician_Galician { get { return new CultureInfo("gl-ES"); } }
            public static CultureInfo Gujarati_India { get { return new CultureInfo("gu-IN"); } }
            public static CultureInfo Hebrew_Israel { get { return new CultureInfo("he-IL"); } }
            public static CultureInfo Hindi_India { get { return new CultureInfo("hi-IN"); } }
            public static CultureInfo Croatian_BosniaandHerzegovina { get { return new CultureInfo("hr-BA"); } }
            public static CultureInfo Croatian_Croatia { get { return new CultureInfo("hr-HR"); } }
            public static CultureInfo Hungarian_Hungary { get { return new CultureInfo("hu-HU"); } }
            public static CultureInfo Armenian_Armenia { get { return new CultureInfo("hy-AM"); } }
            public static CultureInfo Indonesian_Indonesia { get { return new CultureInfo("id-ID"); } }
            public static CultureInfo Icelandic_Iceland { get { return new CultureInfo("is-IS"); } }
            public static CultureInfo Italian_Switzerland { get { return new CultureInfo("it-CH"); } }
            public static CultureInfo Italian_Italy { get { return new CultureInfo("it-IT"); } }
            public static CultureInfo Japanese_Japan { get { return new CultureInfo("ja-JP"); } }
            public static CultureInfo Georgian_Georgia { get { return new CultureInfo("ka-GE"); } }
            public static CultureInfo Kazakh_Kazakhstan { get { return new CultureInfo("kk-KZ"); } }
            public static CultureInfo Kannada_India { get { return new CultureInfo("kn-IN"); } }
            public static CultureInfo Korean_Korea { get { return new CultureInfo("ko-KR"); } }
            public static CultureInfo Kyrgyz_Kyrgyzstan { get { return new CultureInfo("ky-KG"); } }
            public static CultureInfo Lithuanian_Lithuania { get { return new CultureInfo("lt-LT"); } }
            public static CultureInfo Latvian_Latvia { get { return new CultureInfo("lv-LV"); } }
            public static CultureInfo Maori_NewZealand { get { return new CultureInfo("mi-NZ"); } }
            public static CultureInfo Macedonian_FormerYugoslavRepublicofMacedonia { get { return new CultureInfo("mk-MK"); } }
            public static CultureInfo Mongolian_CyrillicMongolia { get { return new CultureInfo("mn-MN"); } }
            public static CultureInfo Marathi_India { get { return new CultureInfo("mr-IN"); } }
            public static CultureInfo Malay_BruneiDarussalam { get { return new CultureInfo("ms-BN"); } }
            public static CultureInfo Malay_Malaysia { get { return new CultureInfo("ms-MY"); } }
            public static CultureInfo Maltese_Malta { get { return new CultureInfo("mt-MT"); } }
            public static CultureInfo NorwegianBokmål_Norway { get { return new CultureInfo("nb-NO"); } }
            public static CultureInfo Dutch_Belgium { get { return new CultureInfo("nl-BE"); } }
            public static CultureInfo Dutch_Netherlands { get { return new CultureInfo("nl-NL"); } }
            public static CultureInfo NorwegianNynorsk_Norway { get { return new CultureInfo("nn-NO"); } }
            public static CultureInfo NorthernSotho_SouthAfrica { get { return new CultureInfo("ns-ZA"); } }
            public static CultureInfo Punjabi_India { get { return new CultureInfo("pa-IN"); } }
            public static CultureInfo Polish_Poland { get { return new CultureInfo("pl-PL"); } }
            public static CultureInfo Portuguese_Brazil { get { return new CultureInfo("pt-BR"); } }
            public static CultureInfo Portuguese_Portugal { get { return new CultureInfo("pt-PT"); } }
            public static CultureInfo Romanian_Romania { get { return new CultureInfo("ro-RO"); } }
            public static CultureInfo Russian_Russia { get { return new CultureInfo("ru-RU"); } }
            public static CultureInfo Sanskrit_India { get { return new CultureInfo("sa-IN"); } }
            public static CultureInfo Sami_Northern_Finland { get { return new CultureInfo("se-FI"); } }
            public static CultureInfo Sami_Northern_Norway { get { return new CultureInfo("se-NO"); } }
            public static CultureInfo Sami_Northern_Sweden { get { return new CultureInfo("se-SE"); } }
            public static CultureInfo Slovak_Slovakia { get { return new CultureInfo("sk-SK"); } }
            public static CultureInfo Slovenian_Slovenia { get { return new CultureInfo("sl-SI"); } }
            public static CultureInfo Albanian_Albania { get { return new CultureInfo("sq-AL"); } }
            public static CultureInfo Swedish_Finland { get { return new CultureInfo("sv-FI"); } }
            public static CultureInfo Swedish_Sweden { get { return new CultureInfo("sv-SE"); } }
            public static CultureInfo Kiswahili_Kenya { get { return new CultureInfo("sw-KE"); } }
            public static CultureInfo Tamil_India { get { return new CultureInfo("ta-IN"); } }
            public static CultureInfo Telugu_India { get { return new CultureInfo("te-IN"); } }
            public static CultureInfo Thai_Thailand { get { return new CultureInfo("th-TH"); } }
            public static CultureInfo Tswana_SouthAfrica { get { return new CultureInfo("tn-ZA"); } }
            public static CultureInfo Turkish_Turkey { get { return new CultureInfo("tr-TR"); } }
            public static CultureInfo Tatar_Russia { get { return new CultureInfo("tt-RU"); } }
            public static CultureInfo Ukrainian_Ukraine { get { return new CultureInfo("uk-UA"); } }
            public static CultureInfo Urdu_IslamicRepublicofPakistan { get { return new CultureInfo("ur-PK"); } }
            public static CultureInfo Vietnamese_Vietnam { get { return new CultureInfo("vi-VN"); } }
            public static CultureInfo Xhosa_SouthAfrica { get { return new CultureInfo("xh-ZA"); } }
            public static CultureInfo Chinese_PeoplesRepublicofChina { get { return new CultureInfo("zh-CN"); } }
            public static CultureInfo Chinese_HongKongSAR { get { return new CultureInfo("zh-HK"); } }
            public static CultureInfo Chinese_MacaoSAR { get { return new CultureInfo("zh-MO"); } }
            public static CultureInfo Chinese_Singapore { get { return new CultureInfo("zh-SG"); } }
            public static CultureInfo Chinese_Taiwan { get { return new CultureInfo("zh-TW"); } }
            public static CultureInfo Zulu_SouthAfrica { get { return new CultureInfo("zu-ZA"); } }
            #endregion

            public static System.Globalization.CultureInfo AllResourceCultures
            {
                set
                {
                  //  MenuResources.Culture = value;
                    LabelResources.Culture = value;
                    ErrorResources.Culture = value;
                    ButtonResources.Culture = value;
                    TooltipResources.Culture = value;
                    TitleResources.Culture = value;
                    EnumResources.Culture = value;
                    MessageResources.Culture = value;
                }
            }

        }
        #endregion
    }

