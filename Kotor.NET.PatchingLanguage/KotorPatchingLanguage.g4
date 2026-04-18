grammar KotorPatchingLanguage;

/*
 * Parser Rules
 */
 
root                        : edit_appearance_row EOF ;
edit_appearance_row         : 'edit' 'appearance' 'row' edit_appearance_row_mod* 'end' 'edit' ;
edit_appearance_row_mod     : 'assign' 'column' STRING_LITERAL 'value' STRING_LITERAL  ;

/*
 * Lexer Rules
 */
IDENTIFIER                  : ([a-z] | [A-Z]) ([a-z] | [A-Z] | [0-9])* ;
STRING_LITERAL              : '"' ( ~["\\] | '\\' . )* '"' ;
WHITESPACE                  : (' ' | '\t' | '\r\n' | '\n' | '\r')+ -> skip ;

// WHITESPACE                  : (' ' | '\t' | NEWLINE)+ -> skip ;
// NEWLINE                     : ('\r'? '\n' | '\r')+ -> skip ;

/*
fragment A          : ('A'|'a') ;
fragment S          : ('S'|'s') ;
fragment Y          : ('Y'|'y') ;
fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;
SAYS                : S A Y S ;
WORD                : (LOWERCASE | UPPERCASE)+ ;
TEXT                : '"' .*? '"' ;
WHITESPACE          : (' '|'\t')+ -> skip ;
NEWLINE             : ('\r'? '\n' | '\r')+ ;
*/