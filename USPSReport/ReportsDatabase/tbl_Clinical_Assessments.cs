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
    
    public partial class tbl_Clinical_Assessments
    {
        public int ID { get; set; }
        public int Account { get; set; }
        public short Member { get; set; }
        public Nullable<System.DateTime> AssessmentDate { get; set; }
        public string MedicalHistory { get; set; }
        public Nullable<short> Bowel_Incontinent { get; set; }
        public Nullable<short> Bladder_Incontinent { get; set; }
        public Nullable<short> BowelBladder_Program { get; set; }
        public Nullable<short> Ambulatory { get; set; }
        public string AssistDevice1 { get; set; }
        public string AssistDevice2 { get; set; }
        public string AssistDevice3 { get; set; }
        public Nullable<short> SkinIntegrityIntact { get; set; }
        public Nullable<short> SkinIntegrityRed { get; set; }
        public Nullable<short> SkinIntegrityOpen { get; set; }
        public string SkinIntegrityWhere { get; set; }
        public string Allergies { get; set; }
        public Nullable<short> LatexAllergy { get; set; }
        public Nullable<short> RubberAllergy { get; set; }
        public Nullable<short> VinylAllergy { get; set; }
        public Nullable<short> PlasticAllergy { get; set; }
        public Nullable<short> DrugAllergy { get; set; }
        public Nullable<short> Weight { get; set; }
        public string HipWaistSize { get; set; }
        public string QtyUsedPerDay_old { get; set; }
        public string HipSize { get; set; }
        public string WaistSize { get; set; }
        public string Diet { get; set; }
        public Nullable<short> TubeFeeding { get; set; }
        public string TotalDailyVolume { get; set; }
        public string CommunicationSkills { get; set; }
        public string CognitiveLevel { get; set; }
        public string MentalStatus { get; set; }
        public string ProductsCurrentlyUsed { get; set; }
        public Nullable<short> LevelOfIncontinence { get; set; }
        public string NumberOfDailyChanges { get; set; }
        public Nullable<short> CurrentMedsForDiuretic { get; set; }
        public string CurrentMedsDiureticDosageFreq { get; set; }
        public Nullable<short> CurrentMedsForStool { get; set; }
        public string CurrentMedsStoolDosageFreq { get; set; }
        public Nullable<short> CurrentMedsForLaxative { get; set; }
        public string CurrentMedsLaxativeDosageFreq { get; set; }
        public string BT_SchoolTrainingProgram { get; set; }
        public Nullable<short> BT_TrainingTime { get; set; }
        public string BT_TrainingSteps { get; set; }
        public Nullable<short> BT_TimedTraining { get; set; }
        public string BT_TimedTrainingHow { get; set; }
        public Nullable<short> BT_ToiletOnOwn { get; set; }
        public Nullable<short> BT_NeedAssistance { get; set; }
        public string BT_NeedAssistanceHow { get; set; }
        public string BT_ToiletCommunicate { get; set; }
        public string BT_OftenBM { get; set; }
        public string BT_BMinToilet { get; set; }
        public Nullable<short> BT_ScaleTimesInDay { get; set; }
        public Nullable<short> BT_OvernightWetDry { get; set; }
        public Nullable<short> BT_WakeAtNight { get; set; }
        public Nullable<short> BT_WakeAtNightTimes { get; set; }
        public Nullable<short> BT_MorningsPerWeekDry { get; set; }
        public Nullable<short> BT_BathroomSelf { get; set; }
        public string PrimaryCaregiver { get; set; }
        public string AssessmentCompletedWith { get; set; }
        public string RelationshipToPatient { get; set; }
        public string AbilityToPerformADLs { get; set; }
        public string ProductsUsed { get; set; }
        public Nullable<int> QuantityPerDay { get; set; }
        public Nullable<int> QuantityPerMonth { get; set; }
        public Nullable<short> MedNecessityReq { get; set; }
        public Nullable<short> HasScript { get; set; }
        public Nullable<short> ScriptAttached { get; set; }
        public Nullable<short> EligibilityVerified { get; set; }
        public Nullable<System.DateTime> EligibilityVerifiedDate { get; set; }
        public string Comments { get; set; }
        public Nullable<int> ID_CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> ID_Changed { get; set; }
        public Nullable<System.DateTime> LastChange { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public Nullable<int> ID_DeletedBy { get; set; }
        public Nullable<short> Duration { get; set; }
        public Nullable<short> PON_LiveAt { get; set; }
        public Nullable<short> PON_SupportServicesAtHome { get; set; }
        public string PON_SupportServicesStaffProvided { get; set; }
        public string PON_OtherSitesServicesProvided { get; set; }
        public Nullable<short> PON_AbleToWalk { get; set; }
        public string PON_UseofWalkerWhatAndWhy { get; set; }
        public Nullable<short> PON_HelpWithToilet { get; set; }
        public string PON_HelpWithToiletType { get; set; }
        public Nullable<short> PON_NeedToUrinate { get; set; }
        public Nullable<short> PON_NeedToHaveBM { get; set; }
        public Nullable<short> PON_DressIndependently { get; set; }
        public string PON_DressIndependentlyExplain { get; set; }
        public string PON_OftenChangedIn24Hrs { get; set; }
        public Nullable<short> PON_AbleToChangeSoiled { get; set; }
        public string PON_AbleToChangeSoiledExplain { get; set; }
        public Nullable<short> PON_AttendDayProgramDays { get; set; }
        public Nullable<short> PON_AttendDayProgramHours { get; set; }
        public Nullable<short> PON_TimesChangedWhileThere { get; set; }
        public string PON_ChangeYourselfOrWithHelp { get; set; }
        public Nullable<short> PON_TimesAwakeAtNight { get; set; }
        public Nullable<short> PON_NumberOfDryMornings { get; set; }
        public Nullable<short> PON_TimesToBathWithoutWet { get; set; }
        public Nullable<short> PON_SupportServicesAtHomeAide { get; set; }
        public string PON_SupportServicesAtHomeTimes { get; set; }
    }
}
