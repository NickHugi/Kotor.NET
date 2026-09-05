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

edit_creature
    : 'edit' 'creature' STRING_LITERAL edit_creature_mod* 'end' 'edit'      # EditCreature
    ;
edit_creature_mod
    : gff_copy_template
    | gff_assign_uint8
    | gff_assign_uint16
    | edit_creature_field_appearance
    | edit_creature_field_portrait
    | edit_creature_field_gender
    ;
edit_creature_field_appearance
    : 'set' 'appearance' gff_value_uint16                                   # EditCreatureAppearance
    | 'set' 'appearance' 'from' 'label' STRING_LITERAL                      # EditCreatureAppearanceFromLabel
    ;
edit_creature_field_portrait
    : 'set' 'appearance' gff_value_uint16                                   # EditCreaturePortrait
    | 'set' 'appearance' 'from' 'label' STRING_LITERAL                      # EditCreaturePortraitFromLabel
    ;
edit_creature_field_gender
    : 'set' 'appearance' gff_value_uint8                                    # EditCreatureGender
    | 'set' 'appearance' (TLK_GENDER | 'both' | 'other' | 'none')           # EditCreatureGenderFromKeyword
    ;
edit_creature_field_race
    : 'set' 'race' gff_value_uint8                                          # EditCreatureRace
    | 'set' 'race' ('human' | 'droid')                                      # EditCreatureRaceFromKeyword
    ;
edit_creature_field_subrace
    : 'set' 'race' gff_value_uint8                                          # EditCreatureSubrace
    | 'set' 'race' ('none' | 'wookie' | 'beast')                            # EditCreatureSubraceFromKeyword
    ;


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
