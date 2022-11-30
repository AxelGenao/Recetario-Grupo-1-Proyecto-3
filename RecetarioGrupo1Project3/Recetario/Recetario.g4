grammar Recetario;

program : (entry)+ EOF ;

entry : recetas porciones (preparacion)* (coccion)* calorias ingredientes elaboracion;

recetas : GUION SP REC_LBL SEP TEXT SP?;
porciones : GUION SP POR_LBL SEP NUMBER SP TEXT SP?;
preparacion : GUION SP TPRE_LBL SEP NUMBER SP TEXT SP?;
coccion : GUION SP TCOCC_LBL SEP NUMBER SP TEXT SP?;
calorias : GUION SP CAL_LBL SEP NUMBER SP TEXT SP? ;
ingredientes : GUION SP ING_LBL SEP NUMBER SP TEXT (COM NUMBER SP TEXT)* SP? ;
elaboracion : GUION SP ELA_LBL SEP (LISTA SP TEXT)* SP? ;

GUION   : '-' ;
REC_LBL : 'RECETA' ;
POR_LBL : 'PORCIONES' ;
TPRE_LBL : 'PREPARACION' ;
TCOCC_LBL : 'COCCION' ;
CAL_LBL : 'CALORIAS' ;
ING_LBL : 'INGREDIENTES' ;
ELA_LBL : 'ELABORACION' ;

FECHA_LBL : 'FECHA_ELABORACION' ;
HORA_LBL : 'HORA_ESTIMADA' ;
DURACION_LBL : 'TIEMPO_DURACION' ;
TEMA_LBL : 'TEMA' ;

COM : SP? ',' SP? ;
SEP : SP? ':' SP? ;
SP : ' '+ ;

NUMBER  : INT | FLOAT;
INT     : [0-9]+;
FLOAT   : [0-9]+'.'[0-9]+;

LISTA:   [0-9]+ ')' ;
TEXT :   PALABRA(SP PALABRA)* ;
PALABRA:   [A-Za-z_]+ ;

WS: [\n\r\t] -> skip ;