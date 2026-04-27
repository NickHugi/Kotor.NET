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
    ;

twoda_assign_cell
    : 'assign' 'cell' 'set' STRING_LITERAL 'to' STRING_LITERAL              # TwoDAAssignCell
    ;
twoda_target_row
    : 'target' 'row' 'where' STRING_LITERAL 'is' STRING_LITERAL             # TwoDATargetRow
    ;
twoda_copy_row
    : 'copy' 'row' 'where' STRING_LITERAL 'is' STRING_LITERAL               # TwoDACopyRow
    ;


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
    | 'set' 'appearance' ('male' | 'female' | 'both' | 'other' | 'none')    # EditCreatureGenderFromKeyword
    ;
edit_creature_field_race
    : 'set' 'race' gff_value_uint8                                          # EditCreatureRace
    | 'set' 'race' ('human' | 'droid')                                      # EditCreatureGenderFromKeyword
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

IDENTIFIER
    : [a-zA-Z_] [a-zA-Z0-9_]*
    ;

WHITESPACE                  
    : [ \t\r\n]+ -> skip
    ;
