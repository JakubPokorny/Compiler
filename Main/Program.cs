using Library.LexerLib;
using Library.ParserLib;
using Library.ParserLib.blocks;
using Library.InterpretLib;

string code1 = "BEGIN double a, b; int c; a = 5; b = 4; c = 2; if (a > (b+c)) { if (b > (c+a)) { if (c > (a+b)) { call vypis(\"trojuhelnik lze sestrojit\"); } else { call vypis(\"trojuhelnik nelze sestrojit\"); } } else { call vypis(\"trojuhelnik lze sestrojit\"); } } else { call vypis(\"trojuhelnik lze sestrojit\"); } END;";
string code2 = "BEGIN int i; string Adam, Eva; for ( i = 0; i < (10/2); i = i+1) { call vypis(i); } Adam = \"Adam\"; Eva = \"Eva\"; if( Adam == Eva){ if (Adam < \"Josefina\"){ call vypis(Adam); } else { call vypis(\"Josefina\"); } }else{ call vypis(Eva); } END;";
string code3 = "BEGIN string stanice; double nastupiste, a, Harry; stanice = \"King's Cross\"; nastupiste = 9.75; Harry = 2; a = 0.5; function secti(double a, double b){ return a + b; } function goHarryGo(double aktualni_nastupiste, double postup,  string cil){ aktualni_nastupiste = call secti(aktualni_nastupiste, a); return aktualni_nastupiste; } while (Harry < nastupiste) { Harry = call goHarryGo(Harry, 0.25, \"Bradavice\"); call vypis(Harry); } END;";
string code4 = "BEGIN string stanice; double nastupiste, Harry; nastupiste = 9.75; Harry = 2; function secti(double a, double b){ return a + b; } function goHarryGo(double aktualni_nastupiste, double postup){ aktualni_nastupiste = call secti(aktualni_nastupiste, postup); return aktualni_nastupiste; } while (Harry != nastupiste) { Harry = call goHarryGo(Harry, 0.25); call vypis(Harry); } END;";
string i = "if (a+45 == z) {int j; j  = a+0;} else {b = 105; }";
string function = "function funkce (int param1, double param2, string param3){return param1+param2;}";
string call = "call funkce (100,100.0,s1);";
string math = "string s1, s2, s3; int a,b,c; double z,x,y; a = -5 ; b = a; z = -5.0; a = 5+6; z = 5 + 51; s1 = \"hello\"; s2 =\" world\"; s3= s1 + s2 + (\"!!\");";
string expr = $"BEGIN {math} z = {call} {i} {function}END;";
string closure = "BEGIN string stanice; double nastupiste, Harry; nastupiste = 9.75; Harry = 2; function goHarryGo(double aktualni_nastupiste, double postup){ function secti(double a, double b){ return a + b; } aktualni_nastupiste = call secti(aktualni_nastupiste, postup); return aktualni_nastupiste; } while (Harry != nastupiste) { Harry = call goHarryGo(Harry, 0.25); call vypis(Harry); } END;";
string recursion = "BEGIN string stanice; double nastupiste, Harry; nastupiste = 9.75; Harry = 0.0; function secti(double a, double b){ if (a < 100) { a = a + b; call secti(a, b); } return a + b; } function goHarryGo(double aktualni_nastupiste, double postup){ aktualni_nastupiste = call secti(aktualni_nastupiste, postup); return aktualni_nastupiste; } while (Harry < nastupiste) { Harry = call goHarryGo(Harry, 20.0); call vypis(Harry); } END;";
string recursion2 = "BEGIN string stanice; double nastupiste, Harry; nastupiste = 9.75; Harry = 0.0; function secti(double a, double b){ if (a < 100) { a = a + b; a = call secti(a, b); } return a + b; } function goHarryGo(double aktualni_nastupiste, double postup){ aktualni_nastupiste = call secti(aktualni_nastupiste, postup); return aktualni_nastupiste; } while (Harry < nastupiste) { Harry = call goHarryGo(Harry, 20.0); call vypis(Harry); } END;";
Lexer lexer;
Parser parser;

try
{
    Interpret interpret = new Interpret(recursion2);
    interpret.Run();
}
catch (Exception ex)
{

    Console.WriteLine($"Chyba: {ex.Message}");
    return;
}
