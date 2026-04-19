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

edit_appearance        
: 'edit' 'appearance' edit_appearance_mod* 'end' 'edit'                     # EditAppearance
    ;
edit_appearance_mod 
    : twoda_target_row                                                        
    | twoda_copy_row                                                                                 
    | twoda_assign_cell                                                         
    ;

edit_creature
    : 'edit' 'creature' STRING_LITERAL edit_creature_mod* 'end' 'edit'
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
