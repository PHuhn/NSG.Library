BEGIN { nmSp=""; aFlg=0; sFlg=0; propFlg=0; pFlg=0; rFlg=0;
}
{
 if( aFlg == 1 ) {
  if( $1 == "</assembly>" ) {
   aFlg = 0;
  } else {
   assemblyName( $0 );
  }
 }
 if( $1 == "<assembly>" ) {
  aFlg = 1;
 }
 if( $1 == "<member" ) {
  sFlg=0; pFlg=0;
  split( $2, met, "\"" );
  if( substr(met[2],1,2) == "T:" ) {
   typeName( met[2] );
  }
  if( substr(met[2],1,2) == "M:" ) {
   methodName( met[2] );
  }
  if( substr(met[2],1,2) == "P:" ) {
   propertyName( met[2] );
  }
 } else {
  if( $1 == "<summary>" ) { sFlg=1;
  } else {
   if( $1 == "</summary>" ) { sFlg=0;
   } else {
    if( sFlg == 1 ) print $0;
    if( substr($1,1,9) == "<returns>" || rFlg == 1 ) {
     returnsName( $0 );
    }
    if( $1 == "<param" || pFlg == 1 ) {
     paramName( $0 );
    }
   }
  }
 }
}
END { 
}
#
function assemblyName( aStr ) {
 # <assembly>
 #  <name>NSG.Library.Logger</name>
 # </assembly>
 len = split( aStr, asm, ">" );
 aStr = asm[2];
 len = split( aStr, asm, "<" );
 printf( "= Assembly: %s =\n", asm[1] );
}
#
function typeName( tstr ) {
 propFlg = 0;
 pFlg = 0; # turn off param
 len = split( tstr, typ, "." );
 if ( typ[len] == "NamespaceDoc" ) {
  nmSp = substr( tstr,3 );
  gsub( /.NamespaceDoc/, "", nmSp );
  printf( "= Namespace: %s =\n", nmSp );
 } else {
  printf( "== Class: %s ==\n", typ[len] );
 }
}
#
function propertyName( pstr ) {
 # <member name="P:NSG.Library.Logger.ILogData.Id">
 #  <summary>
 #   The id/key of the log record.
 #  </summary>
 # </member>
 pFlg = 0; # turn off param
 if( propFlg == 0 ) {
  printf( "=== Properties ===\n" );
  propFlg = 1;
 }
 len = split( pstr, typ, "." );
 printf( "==== %s ====\n", typ[len] );
}
#
function methodName( mStr ) {
 # <member name="M:NSG.Foo.Bar(System.String)">
 propFlg = 0;
 pFlg = 0; # turn off param
 mStr = substr( mStr,4 + length( nmSp ) );
 # horizontal rule
 printf( "\n\n----\n\n\n=== %s ===\n", mStr );
}
#
function paramName( pStr ) {
 # <param name="fullFilePathAndName">full path and file name</param>
 propFlg = 0;
 if( pFlg == 0 ) {
  printf( "\n==== Parameters ====\n" );
  pFlg = 1;
 }
 if( $1 == "</param>" ) {
  return;
 }
 if( $1 == "<param" ) {
  split( $0, pNam, "\"" );
  printf( "===== %s =====\n", pNam[2] );
  len = split( $0, ret, "\>" );
  parStr = "";
  for( i=2; i <= len; i++ ) {
   if( ret[i] != "" ) {
    if( i != len ) {
     parStr = parStr ret[i] "\>";
    } else {
     parStr = parStr ret[i];
    }
   }
  }
 } else {
  parStr = $0;
 }
 idx = index( parStr, "</param>" );
 if( idx > 0 ) {
  parStr = substr( parStr, 1, idx-1 );
 }
 print parStr;
}
#
function returnsName( rStr ) {
 # <returns>string of prefix followed by GUID and extent</returns>
 if( rFlg == 0 ) {
  printf( "\n==== Return Value ====\n" );
  pFlg = 0; # turn off param
  rFlg = 1; # turn on return
 }
 if( $1 == "<returns>" && NF == 1 ) {
  rFlg=1;
  return;
 }
 if( $1 == "</returns>" ) {
  rFlg=0;
  return;
 }
 if( substr($1,1,9) == "<returns>" ) {
  len = split( $0, ret, "\>" );
  retStr = "";
  for( i=2; i <= len; i++ ) {
   if( ret[i] != "" ) {
    if( i != len ) {
     retStr = retStr ret[i] "\>";
    } else {
     retStr = retStr ret[i];
    }
   }
  }
 } else {
  retStr = $0;
 }
 idx = index( retStr, "</returns>" );
 if( idx > 0 ) {
  rFlg=0;
  retStr = substr( retStr, 1, idx-1 );
 }
 print retStr;
}
#