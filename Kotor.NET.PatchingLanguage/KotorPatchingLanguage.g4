grammar KotorPatchingLanguage;

/*
 * Parser Rules
 */
 
script                        
    : instruction* EOF
    ;

instruction                 
    : edit_appearance
    | edit_creature
    | edit_item
    | copy_files
    ;

/* */
file_operation
    : 'create'                          # File_Operation_Create
    | 'modify'                          # File_Operation_Modify
    | 'create' 'or' 'replace'           # File_Operation_CreateOrReplace
    | 'create' 'or' 'modify'            # File_Operation_CreateOrModify
    ;
file_source
    : 'from' 'key'                             # File_Source_Key
    | 'from' 'module' STRING_LITERAL           # File_Source_Module
    | 'from' 'override'                        # File_Source_Override
    ;
file_target
    : 'to' 'module' STRING_LITERAL           # File_Target_Module
    | 'to' 'override'                        # File_Target_Override
    ;
/* */
/* Copy Files */
copy_files
    : 'copy' 'files' 'to' copy_files_target copy_files_command* 'end' 'copy'    # CopyFiles
    ;
copy_files_target
    : 'module' STRING_LITERAL                   # Copy_Files_Target_Module
    | 'override'                                # Copy_Files_Target_Override
    ;
copy_files_command
    : STRING_LITERAL                            # Copy_Files_Command_KeepName
    | STRING_LITERAL 'as' STRING_LITERAL        # Copy_Files_Command_ChangeName
    ;

/* */
/* 2DA */
twoda_assign_cell
    : 'assign' 'cell' 'set' STRING_LITERAL 'to' STRING_LITERAL              # TwoDAAssignCell
    ;
twoda_target_row
    : 'target' 'row' 'where' STRING_LITERAL 'is' STRING_LITERAL             # TwoDATargetRow
    ;
twoda_copy_row
    : 'copy' 'row' 'where' STRING_LITERAL 'is' STRING_LITERAL               # TwoDACopyRow
    ;

/* */
/* GFF */
gff_copy_template
    : 'copy' 'from' 'template' STRING_LITERAL                                  
    ;
gff_locate_field
    : STRING_LITERAL                                                        # GFFLocateField
    ;

gff_assign_uint8
    : 'assign' 'uint8' 'set' gff_locate_field 'to' gff_value_uint8          # GFFAssignUInt8
    ;
gff_value_uint8
    : INT_LITERAL                                                           # GFFValueUInt8Literal
    | MEMORY_TOKEN                                                          # GFFValueUInt8Token
    | '2da' STRING_LITERAL 'where' STRING_LITERAL 'is' STRING_LITERAL       # GFFValueUInt8From2DA
    | BOOL_LITERAL                                                          # GFFValueUInt8BoolLiteral
    ;

gff_assign_uint16
    : 'assign' 'uint16' 'set' gff_locate_field 'to' gff_value_uint16        # GFFAssignUInt16
    ;
gff_value_uint16
    : INT_LITERAL                                                           # GFFValueUInt16Literal
    | MEMORY_TOKEN                                                          # GFFValueUInt16Token
    | '2da' STRING_LITERAL 'where' STRING_LITERAL 'is' STRING_LITERAL       # GFFValueUInt16From2DA
    ;

gff_assign_uint32
    : 'assign' 'uint32' 'set' gff_locate_field 'to' gff_value_uint32        # GFFAssignUInt32
    ;
gff_value_uint32
    : INT_LITERAL                                                           # GFFValueUInt32Literal
    | MEMORY_TOKEN                                                          # GFFValueUInt32Token
    | '2da' STRING_LITERAL 'where' STRING_LITERAL 'is' STRING_LITERAL       # GFFValueUInt32From2DA
    ;

gff_assign_uint64
    : 'assign' 'uint64' 'set' gff_locate_field 'to' gff_value_uint64        # GFFAssignUInt64
    ;
gff_value_uint64
    : INT_LITERAL                                                           # GFFValueUInt64Literal
    | MEMORY_TOKEN                                                          # GFFValueUInt64Token
    | '2da' STRING_LITERAL 'where' STRING_LITERAL 'is' STRING_LITERAL       # GFFValueUInt64From2DA
    ;
    
gff_assign_int8
    : 'assign' 'int8' 'set' gff_locate_field 'to' gff_value_int8            # GFFAssignInt8
    ;
gff_value_int8
    : INT_LITERAL                                                           # GFFValueInt8Literal
    | MEMORY_TOKEN                                                          # GFFValueInt8Token
    | '2da' STRING_LITERAL 'where' STRING_LITERAL 'is' STRING_LITERAL       # GFFValueInt8From2DA
    ;

gff_assign_int16
    : 'assign' 'int16' 'set' gff_locate_field 'to' gff_value_int16          # GFFAssignInt16
    ;
gff_value_int16
    : INT_LITERAL                                                           # GFFValueInt16Literal
    | MEMORY_TOKEN                                                          # GFFValueInt16Token
    | '2da' STRING_LITERAL 'where' STRING_LITERAL 'is' STRING_LITERAL       # GFFValueInt16From2DA
    ;

gff_assign_int32
    : 'assign' 'int32' 'set' gff_locate_field 'to' gff_value_int32          # GFFAssignInt32
    ;
gff_value_int32
    : INT_LITERAL                                                           # GFFValueInt32Literal
    | MEMORY_TOKEN                                                          # GFFValueInt32Token
    | '2da' STRING_LITERAL 'where' STRING_LITERAL 'is' STRING_LITERAL       # GFFValueInt32From2DA
    ;

gff_assign_int64
    : 'assign' 'int64' 'set' gff_locate_field 'to' gff_value_int64          # GFFAssignInt64
    ;
gff_value_int64
    : INT_LITERAL                                                           # GFFValueInt64Literal
    | MEMORY_TOKEN                                                          # GFFValueInt64Token
    | '2da' STRING_LITERAL 'where' STRING_LITERAL 'is' STRING_LITERAL       # GFFValueInt64From2DA
    ;

gff_assign_single
    : 'assign' 'single' 'set' gff_locate_field 'to' gff_value_single        # GFFAssignSingle
    ;
gff_value_single
    : FLOAT_LITERAL                                                         # GFFValueSingleLiteral
    | MEMORY_TOKEN                                                          # GFFValueSingleToken
    | '2da' STRING_LITERAL 'where' STRING_LITERAL 'is' STRING_LITERAL       # GFFValueSingleFrom2DA
    ;

gff_assign_double
    : 'assign' 'double' 'set' gff_locate_field 'to' gff_value_double        # GFFAssignDouble
    ;
gff_value_double
    : FLOAT_LITERAL                                                         # GFFValueDoubleLiteral
    | MEMORY_TOKEN                                                          # GFFValueDoubleToken
    | '2da' STRING_LITERAL 'where' STRING_LITERAL 'is' STRING_LITERAL       # GFFValueDoubleFrom2DA
    ;

gff_assign_resref
    : 'assign' 'resref' 'set' gff_locate_field 'to' gff_value_resref        # GFFAssignResRef
    ;
gff_value_resref
    : STRING_LITERAL                                                        # GFFValueResRefLiteral
    | MEMORY_TOKEN                                                          # GFFValueResRefToken
    | '2da' STRING_LITERAL 'where' STRING_LITERAL 'is' STRING_LITERAL       # GFFValueResRefFrom2DA
    ;

gff_assign_string
    : 'assign' 'string' 'set' gff_locate_field 'to' gff_value_string        # GFFAssignString
    ;
gff_value_string
    : STRING_LITERAL                                                        # GFFValueStringLiteral
    | MEMORY_TOKEN                                                          # GFFValueStringToken
    | '2da' STRING_LITERAL 'where' STRING_LITERAL 'is' STRING_LITERAL       # GFFValueStringFrom2DA
    ;

gff_assign_binary
    : 'assign' 'binary' 'set' gff_locate_field 'to' gff_value_binary        # GFFAssignBinary
    ;
gff_value_binary
    : STRING_LITERAL                                                        # GFFValueBinaryBase64
    ;
gff_assign_locstring
    : 'assign' 'locstring' 'set' gff_locate_field 'stringref' gff_value_int32 # GFFAssignLocalizedStringStringRef
    ;
gff_value_locstring
    : 'stringref' INT_LITERAL                                               # GFFValueLocalizedString
    | 'substrings' gff_value_locstring_substring* 'end' 'substrings'        # GFFValue_LocalizedString_Substrings
    | STRING_LITERAL                                                        # GFFValue_LocalizedString_MaleEnglish
    ;
gff_value_locstring_substring
    : TLK_LANGUAGE TLK_GENDER STRING_LITERAL                                # GFFValue_LocalizedString_Substring_LanguageGender
    | TLK_LANGUAGE STRING_LITERAL                                           # GFFValue_LocalizedString_Substring_Language
    ;
gff_assign_vector3
    : 'assign' 'vector3' 'set' gff_locate_field 'to' gff_value_vector3      # GFFAssignVector3
    ;
gff_value_vector3
    : VECTOR3_LITERAL                                                       # GFFValueVector3Literal
    ;

gff_assign_vector4
    : 'assign' 'vector4' 'set' gff_locate_field 'to' gff_value_vector4      # GFFAssignVector4
    ;
gff_value_vector4
    : VECTOR4_LITERAL                                                       # GFFValueVector4Literal
    ;
/* */
/* UTC */
edit_creature
    : 'edit' 'creature' STRING_LITERAL file_operation file_source file_target edit_creature_mod* 'end' 'edit'        # EditCreature
    ;
edit_creature_mod
    : utc_set_field_appearance_type
    | utc_set_field_blind_spot
    | utc_set_field_cha
    | utc_set_field_challenge_rating
    | utc_set_field_con
    | utc_set_field_conversation
    | utc_set_field_current_force
    | utc_set_field_current_hit_points
    | utc_set_field_description
    | utc_set_field_dex
    | utc_set_field_disarmable
    | utc_set_field_faction_id
    | utc_set_field_first_name
    | utc_set_field_force_points
    | utc_set_field_gender
    | utc_set_field_good_evil
    | utc_set_field_hit_points
    | utc_set_field_hologram
    | utc_set_field_ignore_cre_path
    | utc_set_field_int
    | utc_set_field_is_pc
    | utc_set_field_last_name
    | utc_set_field_max_hit_points
    | utc_set_field_min_1_hp
    | utc_set_field_multiplier_set
    | utc_set_field_natural_ac
    | utc_set_field_no_perm_death
    | utc_set_field_not_reorienting
    | utc_set_field_party_interact
    | utc_set_field_perception_range
    | utc_set_field_phenotype
    | utc_set_field_plot
    | utc_set_field_portrait_id
    | utc_set_field_race
    | utc_set_field_script_attacked
    | utc_set_field_script_damaged
    | utc_set_field_script_death
    | utc_set_field_script_dialogue
    | utc_set_field_script_disturbed
    | utc_set_field_script_end_dialogu
    | utc_set_field_script_end_round
    | utc_set_field_script_heartbeat
    | utc_set_field_script_on_blocked
    | utc_set_field_script_on_notice
    | utc_set_field_script_rested
    | utc_set_field_script_spawn
    | utc_set_field_script_spell_at
    | utc_set_field_script_user_define
    | utc_set_field_sound_set_file
    | utc_set_field_str
    | utc_set_field_subrace_index
    | utc_set_field_tag
    | utc_set_field_walk_rate 
    | utc_set_field_wis
    | utc_set_field_fortbonus
    | utc_set_field_refbonus
    | utc_set_field_willbonus
    | utc_skills_set_field_computer_use
    | utc_skills_set_field_demolitions
    | utc_skills_set_field_stealth
    | utc_skills_set_field_awareness
    | utc_skills_set_field_persuade
    | utc_skills_set_field_repair
    | utc_skills_set_field_security
    | utc_skills_set_field_treat_injury
    | utc_class
    | utc_add_feat
    | utc_add_inventory
    | utc_set_equipment
    ;
utc_set_field_appearance_type
    : 'set' 'appearance' 'to' gff_value_uint16              # UTC_AppearanceType_SetField_Int32
    | 'set' 'appearance' 'to' 'label' STRING_LITERAL        # UTC_AppearanceType_SetField_2DALabelLookup
    ;
utc_set_field_blind_spot
    : 'set' 'blind' 'spot' 'to' gff_value_single            # UTC_BlindSpot_SetField_Single
    ;
utc_set_field_cha
    : 'set' 'charisma' 'to' gff_value_uint8                 # UTC_Cha_SetField_UInt8
    ;
utc_set_field_challenge_rating
    : 'set' 'challenge' 'rating' 'to' gff_value_single      # UTC_ChallengeRating_SetField_UInt8
    ;
utc_set_field_con
    : 'set' 'constitution' 'to' gff_value_uint8             # UTC_Con_SetField_UInt8
    ;
utc_set_field_conversation
    : 'set' 'conversation' 'to' gff_value_resref             # UTC_Conversation_SetField_ResRef
    ;
utc_set_field_current_force
    : 'set' 'current' 'force' 'points' 'to' gff_value_int16 # UTC_CurrentForce_SetField_Int16
    ;
utc_set_field_current_hit_points
    : 'set' 'current' 'hit' 'points' 'to' gff_value_int16   # UTC_CurrentHitPoints_SetField_Int16
    ;
utc_set_field_description
    : 'set' 'description' 'to' gff_value_locstring          # UTC_Description_SetField_LocalizedString
    ;
utc_set_field_dex
    : 'set' 'dexterity' 'to' gff_value_uint8                # UTC_Dex_SetField_UInt8
    ;
utc_set_field_disarmable
    : 'set' 'disarmable' 'to' gff_value_uint8               # UTC_Disarmable_SetField_UInt8
    ;
utc_set_field_faction_id
    : 'set' 'faction' 'to' gff_value_uint16                 # UTC_FactionID_SetField_UInt8
    | 'set' 'faction' 'to' 'label' STRING_LITERAL           # UTC_FactionID_SetField_2DALabelLookup
    ;
utc_set_field_first_name
    : 'set' 'first' 'name' 'to' gff_value_locstring         # UTC_FirstName_SetField_LocalizedString
    ;
utc_set_field_force_points
    : 'set' 'force' 'points' 'to' gff_value_int16           # UTC_ForcePoints_SetField_Int16
    ;
utc_set_field_gender
    : 'set' 'gender' 'to' gff_value_uint8                   # UTC_Gender_SetField_UInt8
    ;
utc_set_field_good_evil
    : 'set' 'alignment' 'to' gff_value_uint8                # UTC_GoodEvil_SetField_UInt8
    ;
utc_set_field_hit_points    
    : 'set' 'hit' 'points' 'to' gff_value_int16             # UTC_HitPoints_SetField_Int16
    ;
utc_set_field_hologram
    : 'set' 'hologram' 'to' gff_value_uint8                 # UTC_Hologram_SetField_UInt8
    ;
utc_set_field_ignore_cre_path
    : 'set' 'ignore' 'creature' 'path' 'to' gff_value_uint8 # UTC_IgnoreCrePath_SetField_UInt8
    ;
utc_set_field_int
    : 'set' 'intelligence' 'to' gff_value_uint8             # UTC_Int_SetField_UInt8
    ;
utc_set_field_is_pc
    : 'set' 'is' 'pc' 'to' gff_value_uint8                  # UTC_IsPC_SetField_UInt8
    ;
utc_set_field_last_name
    : 'set' 'last' 'name' 'to' gff_value_locstring          # UTC_LastName_SetField_LocalizedString
    ;
utc_set_field_max_hit_points
    : 'set' 'max' 'hit' 'points' 'to' gff_value_int16       # UTC_MaxHitPoints_SetField_Int16
    ;
utc_set_field_min_1_hp
    : 'set' 'min' '1' 'hp' 'to' gff_value_uint8             # UTC_Min1HP_SetField_UInt8
    ;
utc_set_field_multiplier_set
    : 'set' 'multiplier' 'set' 'to' gff_value_uint8         # UTC_MultiplierSet_SetField_UInt8
    ;
utc_set_field_natural_ac
    : 'set' 'natural' 'ac' 'to' gff_value_uint8             # UTC_NaturalAC_SetField_UInt8
    ;
utc_set_field_no_perm_death
    : 'set' 'no' 'permanent' 'death' 'to' gff_value_uint8   # UTC_NoPermanentDeath_SetField_UInt8
    ;
utc_set_field_not_reorienting
    : 'set' 'not' 'reorienting' 'to' gff_value_uint8        # UTC_NotReorienting_SetField_UInt8
    ;
utc_set_field_party_interact
    : 'set' 'party' 'interact' 'to' gff_value_uint8         # UTC_PartInteract_SetField_UInt8
    ;
utc_set_field_perception_range
    : 'set' 'perception' 'range' 'to' gff_value_uint8       # UTC_PerceiptionRange_SetField_UInt8
    ;
utc_set_field_plot
    : 'set' 'plot' 'to' gff_value_uint8                     # UTC_Plot_SetField_UInt8
    ;
utc_set_field_phenotype
    : 'set' 'phenotype' 'to' gff_value_int32                # UTC_Phenotype_SetField_UInt8
    | 'set' 'phenotype' 'to' 'label' STRING_LITERAL           # UTC_Phenotype_SetField_2DALabelLookup
    ;
utc_set_field_portrait_id
    : 'set' 'portrait' 'to' gff_value_int32                 # UTC_PortraitID_SetField_Int32
    | 'set' 'portrait' 'to' 'label' STRING_LITERAL          # UTC_PortraitID_SetField_2DALabelLookup
    ;
utc_set_field_race
    : 'set' 'race' 'to' gff_value_uint8                     # UTC_Race_SetField_UInt8
    | 'set' 'race' 'to' 'label' STRING_LITERAL           # UTC_Race_SetField_2DALabelLookup
    ;
utc_set_field_script_attacked
    : 'set' 'script' 'attacked' 'to' gff_value_resref       # UTC_ScriptAttacked_SetField_ResRef
    ;
utc_set_field_script_damaged    
    : 'set' 'script' 'damaged' 'to' gff_value_resref        # UTC_ScriptDamaged_SetField_ResRef
    ;
utc_set_field_script_death
    : 'set' 'script' 'death' 'to' gff_value_resref          # UTC_ScriptDeath_SetField_ResRef
    ;
utc_set_field_script_dialogue
    : 'set' 'script' 'dialog' 'to' gff_value_resref         # UTC_ScriptDialogue_SetField_ResRef
    ;
utc_set_field_script_disturbed
    : 'set' 'script' 'disturbed' 'to' gff_value_resref      # UTC_ScriptDisturbed_SetField_ResRef
    ;
utc_set_field_script_end_dialogu
    : 'set' 'script' 'end' 'dialog' 'to' gff_value_resref   # UTC_ScriptEndDialog_SetField_ResRef
    ;
utc_set_field_script_end_round
    : 'set' 'script' 'end' 'round' 'to' gff_value_resref    # UTC_ScriptEndRound_SetField_ResRef
    ;
utc_set_field_script_heartbeat
    : 'set' 'script' 'heartbeat' 'to' gff_value_resref      # UTC_ScriptHeartbeat_SetField_ResRef
    ;
utc_set_field_script_on_blocked
    : 'set' 'script' 'blocked' 'to' gff_value_resref        # UTC_ScriptBlocked_SetField_ResRef
    ;
utc_set_field_script_on_notice
    : 'set' 'script' 'notice' 'to' gff_value_resref         # UTC_ScriptNotice_SetField_ResRef
    ;
utc_set_field_script_rested
    : 'set' 'script' 'rested' 'to' gff_value_resref         # UTC_ScriptRested_SetField_ResRef
    ;
utc_set_field_script_spawn
    : 'set' 'script' 'spawn' 'to' gff_value_resref          # UTC_ScriptSpawn_SetField_ResRef
    ;
utc_set_field_script_spell_at
    : 'set' 'script' 'spell' 'at' 'to' gff_value_resref     # UTC_ScriptSpellAt_SetField_ResRef
    ;
utc_set_field_script_user_define
    : 'set' 'script' 'user' 'define' 'to' gff_value_resref  # UTC_ScriptUserDefine_SetField_ResRef
    ;
utc_set_field_sound_set_file
    : 'set' 'soundset' 'to' gff_value_uint16                # UTC_SoundsetFile_SetField_UInt16
    | 'set' 'soundset' 'to' 'label' STRING_LITERAL          # UTC_SoundsetFile_SetField_2DALabelLookup
    ;
utc_set_field_str
    : 'set' 'strength' 'to' gff_value_uint8                 # UTC_Str_SetField_UInt8
    ;
utc_set_field_subrace_index
    : 'set' 'subrace' 'to' gff_value_uint8                  # UTC_Subrace_SetField_UInt8
    ;
utc_set_field_tag
    : 'set' 'tag' 'to' gff_value_string                     # UTC_Tag_SetField_String
    ;
utc_set_field_walk_rate
    : 'set' 'walk' 'rate' 'to' gff_value_uint8              # UTC_WalkRate_SetField_UInt8
    | 'set' 'walk' 'rate' 'label' STRING_LITERAL            # UTC_WalkRate_SetField_2DALabelLookup
    ;
utc_set_field_wis
    : 'set' 'wisdom' 'to' gff_value_uint8                   # UTC_Wis_SetField_UInt8
    ;
utc_set_field_fortbonus
    : 'set' 'fortitude' 'bonus' 'to' gff_value_int16        # UTC_FortBonus_SetField_Int16
    ;
utc_set_field_refbonus
    : 'set' 'reflex' 'bonus' 'to' gff_value_int16           # UTC_RefBonus_SetField_Int16
    ;
utc_set_field_willbonus
    : 'set' 'will' 'bonus' 'to' gff_value_int16             # UTC_WillBonus_SetField_Int16
    ;
utc_skills_set_field_computer_use
    : 'set' 'computer' 'use' 'to' gff_value_uint8           # UTC_Skills_ComputerUse_SetField_UInt8
    ;
utc_skills_set_field_demolitions
    : 'set' 'demolitions' 'to' gff_value_uint8              # UTC_Skills_Demolutions_SetField_UInt8
    ;
utc_skills_set_field_stealth
    : 'set' 'stealth' 'to' gff_value_uint8                  # UTC_Skills_Stealth_SetField_UInt8
    ;
utc_skills_set_field_awareness
    : 'set' 'awareness' 'to' gff_value_uint8                # UTC_Skills_Awareness_SetField_UInt8
    ;
utc_skills_set_field_persuade
    : 'set' 'persuade' 'to' gff_value_uint8                 # UTC_Skills_Persuade_SetField_UInt8
    ;
utc_skills_set_field_repair
    : 'set' 'repair' 'to' gff_value_uint8                   # UTC_Skills_Repair_SetField_UInt8
    ;
utc_skills_set_field_security
    : 'set' 'security' 'to' gff_value_uint8                 # UTC_Skills_Security_SetField_UInt8
    ;
utc_skills_set_field_treat_injury
    : 'set' 'treat' 'injury' 'to' gff_value_uint8           # UTC_Skills_TreatInjury_SetField_UInt8
    ;
utc_add_feat
    : 'add' 'feat' gff_value_uint16                         # UTC_AddFeat_UInt16
    | 'add' 'feat' 'label' STRING_LITERAL                   # UTC_AddFeat_2DALabelLookup
    ;
utc_class
    : 'set' 'new' 'class' utc_class_mod* 'end' 'set'       # UTC_Class_SetNew
    | 'set' 'first' 'class' utc_class_mod* 'end' 'set'     # UTC_Class_SetFirst
    | 'set' 'second' 'class' utc_class_mod* 'end' 'set'    # UTC_Class_SetSecond
    ;
utc_class_mod
    : utc_class_add_power
    | utc_class_type_set_field
    | utc_class_level_set_field
    ;
utc_class_add_power
    : 'add' 'power' gff_value_uint16                # UTC_Class_AddPower_UInt16
    | 'add' 'power' 'label' STRING_LITERAL          # UTC_Class_AddPower_UInt16_2DALabelLookup
    ;
utc_class_type_set_field
    : 'set' 'class' 'to' gff_value_int32            # UTC_Class_Type_SetField_Int32
    | 'set' 'class' 'to' 'label' STRING_LITERAL     # UTC_Class_Type_SetField_2DALookup
    ;
utc_class_level_set_field
    : 'set' 'level' 'to' gff_value_int16            # UTC_Class_Level_SetField_Int16
    ;
utc_add_inventory
    : 'add' 'item' 'to' 'inventory' utc_add_inventory_mod* 'end' 'add'  # UTC_AddItem
    ;
utc_add_inventory_mod
    : 'set' 'resref' 'to' gff_value_resref                                   # UTC_AddItem_SetField_ResRef
    | 'set' 'dropable' 'to' gff_value_uint8                                  # UTC_AddItem_SetField_Dropable
    ;
utc_set_equipment                                                                          
    : 'add' 'equipment' 'to' equipment_slot utc_set_equipment_mod* 'end' 'add'       # UTC_SetEquipment
    ;
utc_set_equipment_mod
    : 'set' 'resref' 'to' gff_value_resref                                      # UTC_SetEquipment_ResRef_ResRef
    | 'set' 'dropable' 'to' gff_value_uint8                                     # UTC_SetEquipment_Dropable_UInt8
    ;
equipment_slot
    : 'implant'                     # EquipmentSlot_Implant
    | 'left' 'utility'              # EquipmentSlot_LeftUtility
    | 'head'                        # EquipmentSlot_Head
    | 'sensor'                      # EquipmentSlot_Sensor
    | 'hands'                       # EquipmentSlot_Hands
    | 'right' 'utility'             # EquipmentSlot_RightUtility
    | 'left' 'arm'                  # EquipmentSlot_LeftArm
    | 'left' 'special' 'weapon'     # EquipmentSlot_LeftSpecialWeapon
    | 'body'                        # EquipmentSlot_Body
    | 'plating'                     # EquipmentSlot_Plating
    | 'right' 'arm'                 # EquipmentSlot_RightArm
    | 'right' 'special' 'weapon'    # EquipmentSlot_RightSpecialWeapon
    | 'left' 'weapon'               # EquipmentSlot_LeftWeapon
    | 'belt'                        # EquipmentSlot_Belt
    | 'shield'                      # EquipmentSlot_Shield
    | 'right' 'weapon'              # EquipmentSlot_RightWeapon
    | 'first' 'claw'                # EquipmentSlot_FirstClaw
    | 'second' 'claw'               # EquipmentSlot_SecondClaw
    | 'third' 'claw'                # EquipmentSlot_ThirdClaw
    | 'hide'                        # EquipmentSlot_Hide
    | 'alt' 'left' 'weapon'         # EquipmentSlot_AltLeftWeapon
    | 'alt' 'right' 'weapon'        # EquipmentSlot_AltRight_Weapon
    ;

/* */
/* UTI */
edit_item
    : 'edit' 'item' STRING_LITERAL file_operation file_source file_target edit_item_mod* 'end' 'edit'              # EditItem
    ;
edit_item_mod
    : uti_set_field_base_item
    | uti_set_field_localized_name
    | uti_set_field_description
    | uti_set_field_tag
    | uti_set_field_charges
    | uti_set_field_max_charges
    | uti_set_field_cost
    | uti_set_field_stack_size
    | uti_set_field_plot
    | uti_set_field_model_variation
    | uti_set_field_texture_variation
    | uti_add_property
    ;
uti_set_field_base_item
    : 'set' 'base' 'item' 'to' gff_value_int32                      # UTI_BaseItem_SetField_GFFValue
    | 'set' 'base' 'item' 'to' 'label' STRING_LITERAL               # UTI_BaseItem_SetField_2DALabelLookup
    ;
uti_set_field_localized_name
    : 'set' 'name' 'to' gff_value_locstring                         # UTI_LocalizedName_SetField_GFFValue
    ;
uti_set_field_description
    : 'set' 'description' 'to' gff_value_locstring                  # UTI_Description_SetField_GFFValue
    ;
uti_set_field_tag
    : 'set' 'tag' 'to' gff_value_string                             # UTI_Tag_SetField_GFFValue
    ;
uti_set_field_charges
    : 'set' 'charges' 'to' gff_value_uint8                          # UTI_Charges_SetField_GFFValue
    ;
uti_set_field_max_charges
    : 'set' 'max' 'charges' 'to' gff_value_uint8                    # UTI_MaxCharges_SetField_GFFValue
    ;
uti_set_field_cost
    : 'set' 'cost' 'to' gff_value_uint32                            # UTI_Cost_SetField_GFFValue
    ;
uti_set_field_stack_size
    : 'set' 'stack' 'size' 'to' gff_value_uint16                    # UTI_StackSize_SetField_GFFValue
    ;
uti_set_field_plot
    : 'set' 'plot' 'to' gff_value_int8                              # UTI_Plot_SetField_GFFValue
    | 'set' 'plot' 'to' BOOL_LITERAL                                # UTI_Plot_SetField_Bool
    ;
uti_set_field_model_variation
    : 'set' 'model' 'variation' 'to' gff_value_uint8                # UTI_ModelVariation_SetField_GFFValue
    ;
uti_set_field_texture_variation
    : 'set' 'texture' 'variation' 'to' gff_value_uint8              # UTI_TextureVariation_SetField_GFFValue
    ;
uti_add_property
    : 'add property' uti_property_mod* 'end'                        # UTI_AddProperties
    ;
uti_property_mod
    : uti_property_set_field_property_name
    | uti_property_set_field_subtype
    | uti_property_set_field_chance_appear
    | uti_property_set_field_cost_table
    | uti_property_set_field_cost_value
    | uti_property_set_field_param1
    | uti_property_set_field_param1_value
    | uti_property_set_field_upgrade_type
    ;
uti_property_set_field_property_name
    : 'set' 'property' 'name' 'to' gff_value_uint16                 # UTI_Property_PropertyName_SetField_GFFValue
    ;
uti_property_set_field_subtype
    : 'set' 'subtype' 'to' gff_value_uint16                         # UTI_Property_SubType_SetField_GFFValue
    ;
uti_property_set_field_chance_appear
    : 'set' 'chance' 'appear' 'to' gff_value_uint8                  # UTI_Property_ChanceAppear_SetField_GFFValue
    ;
uti_property_set_field_cost_table
    : 'set' 'cost' 'table' 'to' gff_value_uint8                     # UTI_Property_CostTable_SetField_GFFValue
    ;
uti_property_set_field_cost_value
    : 'set' 'cost' 'value' 'to' gff_value_uint16                    # UTI_Property_CostValue_SetField_GFFValue
    ;
uti_property_set_field_param1
    : 'set' 'param' 'to' gff_value_uint8                            # UTI_Property_Param1_SetField_GFFValue
    ;
uti_property_set_field_param1_value
    : 'set' 'param' 'value' 'to' gff_value_uint8                    # UTI_Property_Param1Value_SetField_GFFValue
    ;
uti_property_set_field_upgrade_type
    : 'set' 'upgrade' 'type' 'to' gff_value_uint8                   # UTI_Property_UpgradeType_SetField_GFFValue
    ;

/* */
/* Appearance */
edit_appearance        
    : 'edit' 'appearance' edit_appearance_mod* 'end' 'edit'                 # EditAppearance
    ;
edit_appearance_mod 
    : twoda_target_row                                                        
    | twoda_copy_row                                                                                 
    | twoda_assign_cell                                                         
    ;
/* */
/*
 * Lexer Rules
 */
MEMORY_TOKEN
    : '@' IDENTIFIER
    ;
 
VECTOR3_LITERAL
    : '(' FLOAT_LITERAL ',' FLOAT_LITERAL ',' FLOAT_LITERAL ')'
    ;

VECTOR4_LITERAL
    : '(' FLOAT_LITERAL ',' FLOAT_LITERAL ',' FLOAT_LITERAL ',' FLOAT_LITERAL ')'
    ;

STRING_LITERAL              
    : '"' ( ~["\\] | '\\' . )* '"'
    ;

INT_LITERAL                 
    : '-'? [0-9]+
    ;

FLOAT_LITERAL
    : [0-9]+ '.' [0-9]* EXPONENT?
    | '.' [0-9]+ EXPONENT?
    | [0-9]+ EXPONENT
    ;
fragment EXPONENT
    : [eE] [+-]? [0-9]+
    ;

BOOL_LITERAL
    : 'true'
    | 'false'
    ;
TLK_GENDER
    : 'male'
    | 'female'
    ;
TLK_LANGUAGE
    : 'english'
    ;
IDENTIFIER
    : [a-zA-Z_] [a-zA-Z0-9_]*
    ;

WHITESPACE                  
    : [ \t\r\n]+ -> skip
    ;
