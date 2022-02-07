//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReportsDatabase
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_MedDoc_History
    {
        public int ID { get; set; }
        public int ID_CMN { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<short> Duration { get; set; }
        public string CertType { get; set; }
        public string Dx1 { get; set; }
        public string Dx2 { get; set; }
        public string Dx3 { get; set; }
        public string Dx4 { get; set; }
        public Nullable<System.DateTime> CMNReturnDate { get; set; }
        public Nullable<System.DateTime> WrittenOrderReturnDate { get; set; }
        public Nullable<int> ID_CMNProvider { get; set; }
        public Nullable<System.DateTime> ResubmitDateforDOS { get; set; }
        public string CMNModifier1 { get; set; }
        public string CMNModifier2 { get; set; }
        public string CMNModifier3 { get; set; }
        public string HCPC1 { get; set; }
        public string HCPC2 { get; set; }
        public string HCPC3 { get; set; }
        public string HCPC4 { get; set; }
        public string HCPC5 { get; set; }
        public string HCPC6 { get; set; }
        public string HCPC7 { get; set; }
        public string HCPC8 { get; set; }
        public string HCPC9 { get; set; }
        public string HCPC10 { get; set; }
        public string HCPC11 { get; set; }
        public string HCPC12 { get; set; }
        public string BED_ReqBodyPosition { get; set; }
        public string BED_ReqBodyPositionAllevPain { get; set; }
        public string BED_ReqHeadElevate { get; set; }
        public string BED_ReqTraction { get; set; }
        public string BED_ReqBedHeight { get; set; }
        public string BED_ReqFrequentChange { get; set; }
        public string SS_DecubitusUlcers { get; set; }
        public string SS_SuperviseDevice { get; set; }
        public string SS_PulmDisease { get; set; }
        public string SS_ConservativeProgram { get; set; }
        public string SS_CompAssessment { get; set; }
        public string SS_MoistDressings { get; set; }
        public string SS_FullTimeCareGiver { get; set; }
        public string SS_Ulcer1Stage { get; set; }
        public string SS_Ulcer1Len { get; set; }
        public string SS_Ulcer1Width { get; set; }
        public string SS_Ulcer2Stage { get; set; }
        public string SS_Ulcer2Len { get; set; }
        public string SS_Ulcer2Width { get; set; }
        public string SS_Ulcer3Stage { get; set; }
        public string SS_Ulcer3Len { get; set; }
        public string SS_Ulcer3Width { get; set; }
        public string SS_PastUlcers { get; set; }
        public string WC_ReqWCInResidence { get; set; }
        public string WC_PatientQuadriplegia { get; set; }
        public string WC_Prevent90Degree { get; set; }
        public string WC_NeedHeightDifference { get; set; }
        public string WC_HoursInChair { get; set; }
        public string WC_SevereWeakness { get; set; }
        public string WC_UnableToOperateWC { get; set; }
        public string WC_AdequateSelfPropel { get; set; }
        public string WC_AdequateSelfPropelWithOrder { get; set; }
        public string CPAP_ApneaEpisodes { get; set; }
        public string CPAP_ObstructiveSleep { get; set; }
        public string LYM_MalignantTumor { get; set; }
        public string LYM_RadicalSurgery { get; set; }
        public string LYM_ChronicVenous { get; set; }
        public string LYM_Drainage { get; set; }
        public string LYM_PrescribedPressures { get; set; }
        public string OS_LongBoneFracture { get; set; }
        public string OS_MonthsPriorToFractures { get; set; }
        public string OS_FailedFusion { get; set; }
        public string OS_MonthsPriorToFusion { get; set; }
        public string OS_CongenitalPseudo { get; set; }
        public string OS_FailedSpine { get; set; }
        public string OS_MonthsPriorToFailed { get; set; }
        public string OS_Adjunct { get; set; }
        public string OS_MonthsPriorToAdjunct { get; set; }
        public string OS_MonthsPriorToAdjunctPrevious { get; set; }
        public string OS_MultiFusion { get; set; }
        public string OS_MonthsPriorToMultiFusion { get; set; }
        public string TN_AcutePostOpPain { get; set; }
        public Nullable<System.DateTime> TN_SurgeryDateForPostOPPain { get; set; }
        public string TN_ChronicIntractablePain { get; set; }
        public string TN_IntractablePainMonths { get; set; }
        public string TN_Condition { get; set; }
        public string TN_DocumentationOfMeds { get; set; }
        public string TN_Trial { get; set; }
        public Nullable<System.DateTime> TN_TrialBeginDate { get; set; }
        public Nullable<System.DateTime> TN_TrialEndDate { get; set; }
        public Nullable<System.DateTime> TN_ReEvalDate { get; set; }
        public string TN_OftenUsage { get; set; }
        public string TN_ImprovementOfPain { get; set; }
        public string TN_Leads { get; set; }
        public string SL_SevereArthritis { get; set; }
        public string SL_Neuromuscular { get; set; }
        public string SL_IncapableOfStanding { get; set; }
        public string SL_AbilityToAmbulate { get; set; }
        public string SL_ModalitiesTried { get; set; }
        public string POV_Required { get; set; }
        public string POV_WheelChairsRuledOut { get; set; }
        public string POV_OnlyOutside { get; set; }
        public string POV_PhySpecialized { get; set; }
        public string POV_OneDayFromSpecialist { get; set; }
        public string POV_ConditionPreventsVisit { get; set; }
        public string IM_HCPC1 { get; set; }
        public string IM_MG1 { get; set; }
        public string IM_TimesPerDay1 { get; set; }
        public string IM_HCPC2 { get; set; }
        public string IM_MG2 { get; set; }
        public string IM_TimesPerDay2 { get; set; }
        public string IM_HCPC3 { get; set; }
        public string IM_MG3 { get; set; }
        public string IM_TimesPerDay3 { get; set; }
        public string IM_HadTransplant { get; set; }
        public string IM_WhichOrgan1 { get; set; }
        public string IM_WhichOrgan2 { get; set; }
        public string IM_WhichOrgan3 { get; set; }
        public string IM_TransplantFacilityName { get; set; }
        public string IM_TransplantFacilityCity { get; set; }
        public string IM_TransplantFacilityState { get; set; }
        public Nullable<System.DateTime> IM_DischargeDate { get; set; }
        public string IM_TransplantSameOrgan { get; set; }
        public string IP_PumpPrescribed { get; set; }
        public string IP_HCPCForDrug { get; set; }
        public string IP_DrugName { get; set; }
        public string IP_RouteOfAdmin { get; set; }
        public string IP_MethodOfAdmin { get; set; }
        public string IP_DurInfused { get; set; }
        public string IP_IntractablePain { get; set; }
        public string PN_Gastrointestinal { get; set; }
        public string PN_DaysPerWeekInfused { get; set; }
        public string PN_AminoAcid_ML { get; set; }
        public string PN_AminoAcid_Conc { get; set; }
        public string PN_AminoAcid_Gms { get; set; }
        public string PN_Dextrose_ML { get; set; }
        public string PN_Dextrose_Conc { get; set; }
        public string PN_Lipids_ML { get; set; }
        public string PN_Lipids_Days { get; set; }
        public string PN_Lipids_Conc { get; set; }
        public string PN_RouteOfAdmin { get; set; }
        public string EN_PermNonFunction { get; set; }
        public string EN_ReqTubeFeeding { get; set; }
        public string EN_FoodName1 { get; set; }
        public string EN_FoodName2 { get; set; }
        public string EN_Food1Cal { get; set; }
        public string EN_Food2Cal { get; set; }
        public string EN_DaysPerWeek { get; set; }
        public string EN_MethodOfAdmin { get; set; }
        public string EN_DocAllergy { get; set; }
        public string EN_AdditionalInfo { get; set; }
        public string OX_ABG { get; set; }
        public string OX_Sat { get; set; }
        public Nullable<System.DateTime> OX_TestDate { get; set; }
        public string OX_TestWithinTwoDays { get; set; }
        public string OX_TestCondition { get; set; }
        public Nullable<int> OX_TestingFacility_ID { get; set; }
        public string OX_PortPatIsMobil { get; set; }
        public string OX_LPM { get; set; }
        public string OX_4LPM_ABG { get; set; }
        public string OX_4LPM_Sat { get; set; }
        public Nullable<System.DateTime> OX_4LPM_TestDate { get; set; }
        public string OX_DependentEdema { get; set; }
        public string OX_CorPulmonale { get; set; }
        public string OX_Hematocrit { get; set; }
        public Nullable<short> OX_HoursPerDay { get; set; }
        public string OX_Method { get; set; }
        public string DB_InsulinTreated { get; set; }
        public string DB_TimesTested { get; set; }
        public string NEB_DrugName1 { get; set; }
        public string NEB_TimesPerDay1 { get; set; }
        public string NEB_DrugName2 { get; set; }
        public string NEB_TimesPerDay2 { get; set; }
        public string NEB_DrugName3 { get; set; }
        public string NEB_TimesPerDay3 { get; set; }
        public Nullable<int> EqCost1IDBillingCode { get; set; }
        public Nullable<decimal> EqCost1Charge { get; set; }
        public Nullable<decimal> EqCost1Allowable { get; set; }
        public Nullable<int> EqCost2IDBillingCode { get; set; }
        public Nullable<decimal> EqCost2Charge { get; set; }
        public Nullable<decimal> EqCost2Allowable { get; set; }
        public Nullable<int> EqCost3IDBillingCode { get; set; }
        public Nullable<decimal> EqCost3Charge { get; set; }
        public Nullable<decimal> EqCost3Allowable { get; set; }
        public Nullable<int> EqCost4IDBillingCode { get; set; }
        public Nullable<decimal> EqCost4Charge { get; set; }
        public Nullable<decimal> EqCost4Allowable { get; set; }
        public Nullable<int> EqCost5IDBillingCode { get; set; }
        public Nullable<decimal> EqCost5Charge { get; set; }
        public Nullable<decimal> EqCost5Allowable { get; set; }
        public Nullable<int> EqCost6IDBillingCode { get; set; }
        public Nullable<decimal> EqCost6Charge { get; set; }
        public Nullable<decimal> EqCost6Allowable { get; set; }
        public Nullable<int> EqCost7IDBillingCode { get; set; }
        public Nullable<decimal> EqCost7Charge { get; set; }
        public Nullable<decimal> EqCost7Allowable { get; set; }
        public Nullable<int> EqCost8IDBillingCode { get; set; }
        public Nullable<decimal> EqCost8Charge { get; set; }
        public Nullable<decimal> EqCost8Allowable { get; set; }
        public Nullable<int> EqCost9IDBillingCode { get; set; }
        public Nullable<decimal> EqCost9Charge { get; set; }
        public Nullable<decimal> EqCost9Allowable { get; set; }
        public Nullable<int> EqCost10IDBillingCode { get; set; }
        public Nullable<decimal> EqCost10Charge { get; set; }
        public Nullable<decimal> EqCost10Allowable { get; set; }
        public Nullable<int> EqCost11IDBillingCode { get; set; }
        public Nullable<decimal> EqCost11Charge { get; set; }
        public Nullable<decimal> EqCost11Allowable { get; set; }
        public Nullable<int> EqCost12IDBillingCode { get; set; }
        public Nullable<decimal> EqCost12Charge { get; set; }
        public Nullable<decimal> EqCost12Allowable { get; set; }
        public Nullable<short> ConvertedCMN { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> LastChange { get; set; }
        public Nullable<int> ID_ChangedBy { get; set; }
        public Nullable<int> EqCost1Qty { get; set; }
        public Nullable<int> EqCost2Qty { get; set; }
        public Nullable<int> EqCost3Qty { get; set; }
        public Nullable<int> EqCost4Qty { get; set; }
        public Nullable<int> EqCost5Qty { get; set; }
        public Nullable<int> EqCost6Qty { get; set; }
        public Nullable<int> EqCost7Qty { get; set; }
        public Nullable<int> EqCost8Qty { get; set; }
        public Nullable<int> EqCost9Qty { get; set; }
        public Nullable<int> EqCost10Qty { get; set; }
        public Nullable<int> EqCost11Qty { get; set; }
        public Nullable<int> EqCost12Qty { get; set; }
        public Nullable<int> Tag { get; set; }
        public Nullable<int> PSR_ID { get; set; }
        public string DRUG_IN_DrDirections { get; set; }
        public Nullable<short> DRUG_IN_DaysSupply { get; set; }
        public Nullable<short> DRUG_IN_DAW { get; set; }
        public string CPAP_FreqOfSupplies { get; set; }
        public Nullable<System.DateTime> WCT_DebridementDate { get; set; }
        public string WCT_TypeOfSurgery { get; set; }
        public Nullable<System.DateTime> WCT_SurgeryDate { get; set; }
        public string WCT_NursingAgency { get; set; }
        public string WCT_1WoundLocation { get; set; }
        public string WCT_1WoundSize { get; set; }
        public Nullable<short> WCT_1Drainage { get; set; }
        public string WCT_1ChangeFreq { get; set; }
        public string WCT_1Stage { get; set; }
        public string WCT_2WoundLocation { get; set; }
        public string WCT_2WoundSize { get; set; }
        public Nullable<short> WCT_2Drainage { get; set; }
        public string WCT_2ChangeFreq { get; set; }
        public string WCT_2Stage { get; set; }
        public string WCT_3WoundLocation { get; set; }
        public string WCT_3WoundSize { get; set; }
        public Nullable<short> WCT_3Drainage { get; set; }
        public string WCT_3ChangeFreq { get; set; }
        public string WCT_3Stage { get; set; }
        public string WCT_4WoundLocation { get; set; }
        public string WCT_4WoundSize { get; set; }
        public Nullable<short> WCT_4Drainage { get; set; }
        public string WCT_4ChangeFreq { get; set; }
        public string WCT_4Stage { get; set; }
        public string WCT_5WoundLocation { get; set; }
        public string WCT_5WoundSize { get; set; }
        public Nullable<short> WCT_5Drainage { get; set; }
        public string WCT_5ChangeFreq { get; set; }
        public string WCT_5Stage { get; set; }
        public string WCT_6WoundLocation { get; set; }
        public string WCT_6WoundSize { get; set; }
        public Nullable<short> WCT_6Drainage { get; set; }
        public string WCT_6ChangeFreq { get; set; }
        public string WCT_6Stage { get; set; }
        public string CPAP_AHI15Events { get; set; }
        public string CPAP_AHI5To14Disorder { get; set; }
        public string CPAP_AHI5To14Hyper { get; set; }
        public Nullable<short> CPAP_ApneaEpisodesAvgDur { get; set; }
        public Nullable<short> CPAP_ApneaEpisodesStudyLen { get; set; }
        public string IP_HCPCForDrug2 { get; set; }
        public string IP_DrugName2 { get; set; }
        public string IP_HCPCForDrug3 { get; set; }
        public string IP_DrugName3 { get; set; }
        public string LYM_VenousUlcers { get; set; }
        public string LYM_SinceChildhood { get; set; }
        public string OS_NoRadiographicEvidence { get; set; }
        public string OS_SurgicalIntervention { get; set; }
        public string OX_TestPerformCriteria { get; set; }
        public string EN_FoodHCPC1 { get; set; }
        public string EN_FoodHCPC2 { get; set; }
        public Nullable<short> SubmitEDIAsOldDMERCFormat { get; set; }
        public string EqCost1Note { get; set; }
        public string EqCost2Note { get; set; }
        public string EqCost3Note { get; set; }
        public string EqCost4Note { get; set; }
        public string EqCost5Note { get; set; }
        public string EqCost6Note { get; set; }
        public string EqCost7Note { get; set; }
        public string EqCost8Note { get; set; }
        public string EqCost9Note { get; set; }
        public string EqCost10Note { get; set; }
        public string EqCost11Note { get; set; }
        public string EqCost12Note { get; set; }
        public Nullable<int> EqCost1IDFreq { get; set; }
        public Nullable<int> EqCost2IDFreq { get; set; }
        public Nullable<int> EqCost3IDFreq { get; set; }
        public Nullable<int> EqCost4IDFreq { get; set; }
        public Nullable<int> EqCost5IDFreq { get; set; }
        public Nullable<int> EqCost6IDFreq { get; set; }
        public Nullable<int> EqCost7IDFreq { get; set; }
        public Nullable<int> EqCost8IDFreq { get; set; }
        public Nullable<int> EqCost9IDFreq { get; set; }
        public Nullable<int> EqCost10IDFreq { get; set; }
        public Nullable<int> EqCost11IDFreq { get; set; }
        public Nullable<int> EqCost12IDFreq { get; set; }
        public Nullable<System.DateTime> LastSubmitECMN { get; set; }
        public Nullable<short> ResubmitECMN { get; set; }
        public string ExternalReference { get; set; }
        public string DX5 { get; set; }
        public string DX6 { get; set; }
        public string DX7 { get; set; }
        public string DX8 { get; set; }
        public string DX9 { get; set; }
        public string DX10 { get; set; }
        public string DX11 { get; set; }
        public string DX12 { get; set; }
        public string Comments { get; set; }
        public Nullable<long> ID_WorkFlowItem { get; set; }
    }
}
