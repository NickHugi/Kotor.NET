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
gff_assign_uint8
    : 'assign' 'uint8' 'set' STRING_LITERAL 'to' INT_LITERAL               # GFFAssignUInt8
    ;
gff_assign_uint16
    : 'assign' 'uint16' 'set' STRING_LITERAL 'to' INT_LITERAL               # GFFAssignUInt16
    ;
gff_assign_uint32
    : 'assign' 'uint32' 'set' STRING_LITERAL 'to' INT_LITERAL               # GFFAssignUInt32
    ;
gff_assign_uint64
    : 'assign' 'uint64' 'set' STRING_LITERAL 'to' INT_LITERAL               # GFFAssignUInt64
    ;
gff_assign_int8
    : 'assign' 'int8' 'set' STRING_LITERAL 'to' INT_LITERAL                 # GFFAssignInt8
    ;
gff_assign_int16
    : 'assign' 'int16' 'set' STRING_LITERAL 'to' INT_LITERAL                # GFFAssignInt16
    ;
gff_assign_int32
    : 'assign' 'int32' 'set' STRING_LITERAL 'to' INT_LITERAL                # GFFAssignInt32
    ;
gff_assign_int64
    : 'assign' 'int64' 'set' STRING_LITERAL 'to' INT_LITERAL                # GFFAssignInt64
    ;
gff_assign_float
    : 'assign' 'float' 'set' STRING_LITERAL 'to' FLOAT_LITERAL              # GFFAssignFloat
    ;
gff_assign_double
    : 'assign' 'double' 'set' STRING_LITERAL 'to' FLOAT_LITERAL             # GFFAssignDouble
    ;
gff_assign_resref
    : 'assign' 'resref' 'set' STRING_LITERAL 'to' STRING_LITERAL            # GFFAssignResRef
    ;
gff_assign_string
    : 'assign' 'string' 'set' STRING_LITERAL 'to' STRING_LITERAL            # GFFAssignString
    ;
gff_assign_binary
    : 'assign' 'binary' 'set' STRING_LITERAL 'to' STRING_LITERAL            # GFFAssignBinary
    ;
gff_assign_locstring
    : 'assign' 'locstring' 'set' STRING_LITERAL 'to' INT_LITERAL            # GFFAssignLocalizedString
    ;
gff_assign_vector3
    : 'assign' 'vector3' 'set' STRING_LITERAL 'to' VECTOR3_LITERAL          # GFFAssignVector3
    ;
gff_assign_vector4
    : 'assign' 'vector4' 'set' STRING_LITERAL 'to' VECTOR4_LITERAL          # GFFAssignVector4
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
    | edit_creature_field_appearance
    ;
edit_creature_field_appearance
    : 'set' 'appearance' INT_LITERAL
    ;

/*
 * Lexer Rules
 */

 
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
