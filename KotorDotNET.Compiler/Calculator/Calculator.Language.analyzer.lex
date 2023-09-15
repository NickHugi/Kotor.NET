%namespace KotorDotNET_Compiler.Calculator
%scannertype CalculatorScanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers 

NOP                 nop
Comment             \/\/[^\n]*\n
MultilineComment    \/\*(\*(?!\/)|[^*])*\*\/
Include             \#include
ObjectSelf          OBJECT_SELF
ObjectInvalid       OBJECT_INVALID
True                TRUE
False               FALSE
Break               break
Continue            continue
Case                case
Default             default
Do                  do
Else                else
Switch              switch
While               while
For                 wall
If                  if
Return              return
Struct              struct
Int                 int
Float               float
Object              object
Void                void
Event               event
Effect              effect
ItemProperty        itemproperty
Location            location
String              string
Talent              talent
Action              action
Vector              vector
Identifier          [a-zA-Z_]+[a-zA-Z0-9_]*
StringLiteral       \"[^\"]*\"
FloatLiteral        [0-9]+\.[0-9]+f?|[0-9]f
HexLiteral          0x[0-9a-fA-F]+
IntLiteral          [0-9]+
IncrementOp         \+\+
DecrementOp         \-\-
AddAssignment       \+\=
SubtractAssignemnt  \-\=
MultiplyAssignment  \*\=
DivisionAssignment  /\=
BitwiseLeftOp       \<\<
BitwiseRightOp      >>
AddOp               \+
MinusOp             \-
MultiplyOp          \*
DivideOp            /
ModOp               \%
EqualityOp          \=\=
InequalityOp        \!=
GreaterOrEqualOp    >\=
GreaterOp           >
LesserOrEqualOp     \<=
LesserOp            \<
LogicalAndOp        &&
LogicalOrOp         \|\|
NotOp               \!
BitwiseAndOp        &
BitwiseOrOp         \|
BitwiseXorOp        \^
BitwiseNotOp        \~
Space               [ \t]
EOL                 \n

%{

%}

%%

{EOL}+		    /* skip */
{Space}+		/* skip */

{NOP}		{ return (int)Token.NOP; }
{Comment}		{ return (int)Token.COMMENT_INLINE; }
{MultilineComment}		{ return (int)Token.COMMENT_MULTILINE; }
{Include}		{ return (int)Token.INCLUDE; }
{ObjectSelf}		{ return (int)Token.VALUE_OBJECT_SELF; }
{ObjectInvalid}		{ return (int)Token.VALUE_OBJECT_INVALID; }
{True}		{ return (int)Token.VALUE_TRUE; }
{False}		{ return (int)Token.VALUE_FALSE; }
{Break}		{ return (int)Token.FLOW_BREAK; }
{Continue}		{ return (int)Token.FLOW_CONTINUE; }
{Case}		{ return (int)Token.FLOW_CASE; }
{Default}		{ return (int)Token.FLOW_DEFAULT; }
{Do}		{ return (int)Token.FLOW_DO; }
{Else}		{ return (int)Token.FLOW_ELSE; }
{Switch}		{ return (int)Token.FLOW_SWITCH; }
{While}		{ return (int)Token.FLOW_WHILE; }
{For}		{ return (int)Token.FLOW_FOR; }
{If}		{ return (int)Token.FLOW_IF; }
{Return}		{ return (int)Token.FLOW_RETURN; }
{Struct}		{ return (int)Token.TYPE_STRUCT; }
{Int}		{ return (int)Token.TYPE_INT; }
{Float}		{ return (int)Token.TYPE_FLOAT; }
{Object}		{ return (int)Token.TYPE_OBJECT; }
{Void}		{ return (int)Token.TYPE_VOID; }
{Event}		{ return (int)Token.TYPE_EVENT; }
{Effect}		{ return (int)Token.TYPE_EFFECT; }
{ItemProperty}		{ return (int)Token.TYPE_ITEM_PROPERTY; }
{Location}		{ return (int)Token.TYPE_LOCATION; }
{String}		{ return (int)Token.TYPE_STRING; }
{Talent}		{ return (int)Token.TYPE_TALENT; }
{Action}		{ return (int)Token.TYPE_ACTION; }
{Vector}		{ return (int)Token.TYPE_VECTOR; }
{Identifier}		{ return (int)Token.IDENTIFIER; }
{StringLiteral}		{ return (int)Token.LITERAL_STRING; }
{FloatLiteral}		{ return (int)Token.LITERAL_FLOAT; }
{HexLiteral}		{ return (int)Token.LITERAL_INT_HEX; }
{IntLiteral}		{ return (int)Token.LITERAL_INT; }
{IncrementOp}		{ return (int)Token.OP_INCREMENT; }
{DecrementOp}		{ return (int)Token.OP_DECREMENT; }
{AddAssignment}		{ return (int)Token.ASSIGNMENT_ADD; }
{SubtractAssignemnt}		{ return (int)Token.ASSIGNMENT_SUBTRACT; }
{MultiplyAssignment}		{ return (int)Token.ASSIGNMENT_MULTIPLY; }
{DivisionAssignment}		{ return (int)Token.ASSIGNMENT_DIVISION; }
{BitwiseLeftOp}		{ return (int)Token.OP_BITWISE_LEFT; }
{BitwiseRightOp}		{ return (int)Token.OP_BITWISE_RIGHT; }
{AddOp}		{ return (int)Token.OP_ADD; }
{MinusOp}		{ return (int)Token.OP_SUBTRACT; }
{MultiplyOp}		{ return (int)Token.OP_MULTIPLY; }
{DivideOp}		{ return (int)Token.OP_DIVIDE; }
{ModOp}		{ return (int)Token.OP_MODULUS; }
{EqualityOp}		{ return (int)Token.OP_EQUALITY; }
{InequalityOp}		{ return (int)Token.OP_INEQUALITY; }
{GreaterOrEqualOp}		{ return (int)Token.OP_GREATER_THAN_OR_EQUAL; }
{GreaterOp}		{ return (int)Token.OP_GREATER_THAN; }
{LesserOrEqualOp}		{ return (int)Token.OP_LESS_THAN_OR_EQUAL; }
{LesserOp}		{ return (int)Token.OP_LESS_THAN; }
{LogicalAndOp}		{ return (int)Token.OP_LOGICAL_AND; }
{LogicalOrOp}		{ return (int)Token.OP_LOGICAL_OR; }
{NotOp}		{ return (int)Token.OP_NOT; }
{BitwiseAndOp}		{ return (int)Token.OP_BITWISE_AND; }
{BitwiseOrOp}		{ return (int)Token.OP_BITWISE_OR; }
{BitwiseXorOp}		{ return (int)Token.OP_BITWISE_XOR; }
{BitwiseNotOp}		{ return (int)Token.OP_BITWISE_NOT; }

%%