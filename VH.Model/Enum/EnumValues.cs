using System;
using System.Runtime.Serialization;

namespace VH.Model
{
    [Serializable]
    [DataContract]
    public enum VHStatus
    {
        Ready = 0,
        Loading = 1
    }

    [Serializable]
    [DataContract]
    public enum Gender
    {
        Male = 0,
        Female = 1
    }

    public enum ConditionOperand : short
    {
        And,
        Or
    };

    public enum SortDirection : short
    {
        Ascending,
        Descending
    };

    public enum Operand : short
    {
        IsEqualTo,
        IsEqualToSoundex,
        IsBetween,
        IsGreaterThan,
        IsGreaterThanOrEqualTo,
        IsLessThan,
        IsLessThanOrEqualTo,
        IsIn,
        IsNotIn,
        Contains,
        StartsWith,
        SoundsLike,
        IsNull,
        IsNotNull,
        IsLessThanOrIsNull,
        IsGreaterThanOrIsNull,
        IsNotEqual,
        IsEqualToOrNull,
        IsNotEqualOrNull,
        IsLessThanOrEqualToOrIsNull,
        IsGreaterThanOrEqualToOrIsNull,
        DoesNotContain
    };

    public enum DirtyState : short
    {
        UnChanged = 0,
        PendingAddChange = 1,
        PendingDelete = 2,
    }


    public enum Status
    {
        InActive = 0,
        Active = 1,
    }

    public enum AuthenticationType : short
    {
        UserName = 1,
        Windows = 2,
    }

    public enum UserDefinedFieldStateType : short
    {
        NOT_USED = 0,
        OPTIONAL = 1,
        REQUIRED = 2,
    }

    public enum RequirementType : short
    {
        NotApplicable = 0,
        Optional = 1,
        Required = 2,
    }

    public enum BindingType : short
    {
        netTcp = 1,
        basicHttp = 2,
        wsHttp = 3,
        basicHttps = 4,
    }

    public enum UIStatusType : short
    {
        Ready = 1,
        Working = 2,
        Error = 3,
        PendingChanges = 4,
        OperationCancelled = 5
    }

    public enum DocumentTemplate : short
    {
        Notice = 0,
    }

    public enum ExportFormat : short
    {
        DOC = 0,
        DOCX = 1,
        PDF = 3,
        RTF = 4,
        XML = 5,
        HTML = 6,
        TXT = 7,
        CSV = 8,
        XLSX2007 = 9
    }

    public enum AccessType : short
    {
        Default = 0,
        ReadOnly = 1,
    }

    public enum EntityOperationType : short
    {
        NoChange = 0,
        Change = 1,
        UndoLastChange = 2
    }

    [DataContract]
    public enum PhoneType
    {
        [EnumMember] Mobile = 0,
        [EnumMember] Landline = 1,
    }

    [Serializable]
    public enum ActionResultType
    {
        DataFetched,
        RequestedNew,
        DataNotFound
    }

    [Serializable]
    [DataContract]
    public enum AppointmentType
    {
        [EnumMember] Audiometry = 0,
        [EnumMember] ImpedanceAudiometry = 1,
        [EnumMember] HAT = 2,
        [EnumMember] ReProgramming = 3,
        [EnumMember] Consulting = 4,
        [EnumMember] Speech = 5,
        [EnumMember] Speech_Mold = 6,
    }

    [Serializable]
    [DataContract]
    public enum Company
    {
        [EnumMember] Siemens = 0,
        [EnumMember] Widex = 1,
        [EnumMember] Phonak = 2,
        [EnumMember] Bernafon = 3,
        [EnumMember] Danavox = 4,
        [EnumMember] Pinnacle = 5,
        [EnumMember] Elkon = 6,
        [EnumMember] Alps = 7,
        [EnumMember] Rexton = 8,
        [EnumMember] AM = 9,
        [EnumMember] Viennatone = 10,
        [EnumMember] Arphi = 11,
        [EnumMember] Starkey = 12,
        [EnumMember] Belton = 13
    }

    [Serializable]
    [DataContract]
    public enum HearingAidType
    {
        [EnumMember] PKT = 0,
        [EnumMember] BTE = 1,
        [EnumMember] ITE = 2,
        [EnumMember] ITC = 3,
        [EnumMember] CIC = 4,
        [EnumMember] IIC = 5,
        [EnumMember] RIC = 6,
        [EnumMember] Micro_BTE = 7
    }

    [Serializable]
    [DataContract]
    public enum WarrantyType
    {
        [EnumMember] Warranty = 0,
        [EnumMember] NonWarranty = 1,
    }

    [Serializable]
    [DataContract]
    public enum YesNo
    {
        [EnumMember] No = 0,
        [EnumMember] Yes = 1
    }

    [Serializable]
    [DataContract]
    public enum RepairStatus
    {
        [EnumMember] NotProcessed = 0,
        [EnumMember] DispatchedToCompany = 1,
        [EnumMember] CompanyReceived = 2,
        [EnumMember] CompanyDispatched = 3,
        [EnumMember] Received = 4,
        [EnumMember] Informed = 5,
        [EnumMember] DeliveredToCustomer = 6
    }

    [Serializable]
    [DataContract]
    public enum BatterySize
    {
        [EnumMember] SixSeventyFive = 0,
        [EnumMember] Thirteen = 1,
        [EnumMember] ThreeTwelve = 2,
        [EnumMember] Ten = 3
    }

    [Serializable]
    [DataContract]
    public enum EarSide
    {
        [EnumMember] Left = 0,
        [EnumMember] Right = 1,
        [EnumMember] Both = 2
    }

    [Serializable]
    [DataContract]
    public enum WarrantyCardGiven
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Self = 1,
        [EnumMember]
        Customer = 2,
    }


    [Serializable]
    [DataContract]
    public enum EarMoldType
    {
        [EnumMember] Hard = 0,
        [EnumMember] Soft = 1,
        [EnumMember] Camisha = 2
    }

    [Serializable]
    [DataContract]
    public enum EarMoldHearingAidType
    {
        [EnumMember] PKT = 0,
        [EnumMember] BTE = 1
    }

    [Serializable]
    [DataContract]
    public enum SoftMoldTubingType
    {
        [EnumMember] Regular = 0,
        [EnumMember] LibbyHorn = 1
    }

    [Serializable]
    [DataContract]
    public enum EarMoldDesign
    {
        [EnumMember] FullShell = 0,
        [EnumMember] HalfShell = 1,
        [EnumMember] OpenEarOrCROS = 2,
        [EnumMember] Skeleton = 3,
        [EnumMember] HalfSkeleton = 4,
        [EnumMember] QuarterSkeleton = 5,
        [EnumMember] CanalOrTip = 6,
        [EnumMember] SwinPlug = 7,
        [EnumMember] ReceiverMold = 8,
        [EnumMember] LifeTubeMold = 9,
        [EnumMember] RICMold = 10
    }

    [Serializable]
    [DataContract]
    public enum VentSize
    {
        [EnumMember] Small = 0,
        [EnumMember] Medium = 1,
        [EnumMember] Large = 2,
        [EnumMember] Point5 = 3,
        [EnumMember] One = 4,
        [EnumMember] OnePoint5 = 5,
        [EnumMember] Two = 6,
        [EnumMember] TwoPoint5 = 7,
        [EnumMember] Three = 8,
    }

    [Serializable]
    [DataContract]
    public enum VentType
    {
        [EnumMember] Parallel = 0,
        [EnumMember] Diagonal = 1,
        [EnumMember] SAV = 2
    }

    [Serializable]
    [DataContract]
    public enum VentLength
    {
        [EnumMember] Long = 0,
        [EnumMember] Medium = 1,
        [EnumMember] Short = 2
    }

    [Serializable]
    [DataContract]
    public enum MoldRepair
    {
        [EnumMember] None = 0,
        [EnumMember] Retubing = 1,
        [EnumMember] Trimming = 2,
        [EnumMember] Lacquering = 3,
        [EnumMember] ElbowReplacement = 4
    }

    [Serializable]
    [DataContract]
    public enum HearingAidCondition
    {
        [EnumMember] Dead = 0,
        [EnumMember] Feedback = 1,
        [EnumMember] Noise = 2,
        [EnumMember] Intermittent = 3,
        [EnumMember] Distortion = 4,
        [EnumMember] Weak = 5,
        [EnumMember] BodyCase = 6,
        [EnumMember] BatteryDoor = 7,
        [EnumMember] VolumnControl = 8,
        [EnumMember] Trimmer = 9,
        [EnumMember] Switch = 10,
        [EnumMember] Hook = 11,
        [EnumMember] WaxGaurdSitting = 12,
        [EnumMember] PullingThread = 13,
        [EnumMember] ReShelling = 14,
        [EnumMember] HABroken = 15,
        [EnumMember] ProgramButton = 16,
        [EnumMember] FacePlateGumming = 17,
        [EnumMember] Servicing = 18,
        [EnumMember] Others = 19
    }

    [Serializable]
    [DataContract]
    public enum OrderStatus
    {
        [EnumMember] OrderTaken = 0,
        [EnumMember] InProcess = 1,
        [EnumMember] Received = 2,
        [EnumMember] Informed = 3,
        [EnumMember] Delivered = 4
    }

    [Serializable]
    [DataContract]
    public enum ReminderType
    {
        [EnumMember] Appointment = 0,
        [EnumMember] Information = 1
    }

    [Serializable]
    [DataContract]
    public enum Relationship
    {
        Self = 0,
        Father =1,
        Mother = 2,
        Sister = 3,
        Brother = 4,
        Aunty = 5,
        Uncle = 6,
        GrandFather = 7,
        GrandMother = 8,
        Other = 9,
    }
}
