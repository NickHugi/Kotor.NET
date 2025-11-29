%namespace Kotor_NET_Script_Compiler.Calculator
%scannertype CalculatorScanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers

Identifier [A-Za-z_][A-Za-z0-9_]*
Digit [0-9]
Whitespace [ \t\r\n]+

%{

%}


%%


{Whitespace}+		/* skip */

"{"		{ return (int)Token.LBRACE; } /* yytext */
"}"		{ return (int)Token.RBRACE; }
"("		{ return (int)Token.LPAREN; }
")"		{ return (int)Token.RPAREN; }
";"		{ return (int)Token.SEMI; }
","		{ return (int)Token.COMMA; }
"void"		            { return (int)Token.VOID_TYPE; }
"int"		            { return (int)Token.INT_TYPE; }
"float"		            { return (int)Token.FLOAT_TYPE; }
"string"		        { return (int)Token.STRING_TYPE; }
"vector"		        { return (int)Token.VECTOR_TYPE; }
"location"		        { return (int)Token.LOCATION_TYPE; }
"effect"		        { return (int)Token.EFFECT_TYPE; }
{Identifier}		    { return (int)Token.IDENTIFIER; }
{Digit}+		        { return (int)Token.INT_LITERAL; }
"true"                  { return (int)Token.BOOL_LITERAL; }
"false"                 { return (int)Token.BOOL_LITERAL; }
"objectself"            { return (int)Token.OBJECT_LITERAL; }
"objectinvalid"         { return (int)Token.OBJECT_LITERAL; }
"return"                { return (int)Token.RETURN; }
%%
