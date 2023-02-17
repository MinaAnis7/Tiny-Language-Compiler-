using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;

public enum Token_Class
{
    Identifer, StringValue, Number, Read, Write, Repeat, Until, If, ElseIf, Else,
    Then, Return, Endl, LBracket, RBracket, Comment, PlusOp, MinusOp, MultiplyOp, DivideOp,
    AssignmentOp, IntDataType, FloatDataType, StringDataType, SemiColon, Comma, Dot,
    LessThanOp, GreaterThanOp, NotEqualOp, EqualOp, AndOp, OrOp, EndStatment, Lcurly, Rcurly, Main, Empty

}
namespace JASON_Compiler
{


    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("elseif", Token_Class.ElseIf);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("main", Token_Class.Main);

            ReservedWords.Add("end", Token_Class.EndStatment);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);

            ReservedWords.Add("int", Token_Class.IntDataType);
            ReservedWords.Add("float", Token_Class.FloatDataType);
            ReservedWords.Add("string", Token_Class.StringDataType);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("", Token_Class.Empty);


            //Operators.Add(".", Token_Class.Dot);
            Operators.Add("&&", Token_Class.AndOp);
            Operators.Add("||", Token_Class.OrOp);
            Operators.Add(";", Token_Class.SemiColon);
            Operators.Add(",", Token_Class.Comma);

            Operators.Add("(", Token_Class.LBracket);
            Operators.Add(")", Token_Class.RBracket);
            Operators.Add("{", Token_Class.Lcurly);
            Operators.Add("}", Token_Class.Rcurly);
            Operators.Add("=", Token_Class.EqualOp);

            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);

            Operators.Add(":=", Token_Class.AssignmentOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);


        }

        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;

                if (CurrentChar >= 'A' && CurrentChar <= 'Z' || CurrentChar >= 'a' && CurrentChar <= 'z') //identifier
                {
                    j++;
                    while (j < SourceCode.Length)
                    {
                        if (SourceCode[j] >= '0' && SourceCode[j] <= '9' || SourceCode[j] >= 'A' && SourceCode[j] <= 'z' || SourceCode[j] >= 'a' && SourceCode[j] <= 'z')
                        {
                            CurrentLexeme += SourceCode[j].ToString();

                        }
                        else if (SourceCode[j] == '"' && SourceCode[j] != '#')
                        {
                            CurrentLexeme += SourceCode[j].ToString();
                        }
                        else
                        {
                            break;
                        }
                        j++;
                    }

                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }

                else if (CurrentChar >= '0' && CurrentChar <= '9' || CurrentChar == '.')//number
                {
                    j++;

                    while (j < SourceCode.Length && (SourceCode[j] == '"' || SourceCode[j] >= '0' && SourceCode[j] <= '9' || SourceCode[j] == '.' || (SourceCode[j] >= 'A' && SourceCode[j] <= 'Z') || (SourceCode[j] >= 'a' && SourceCode[j] <= 'z')))
                    {
                        CurrentLexeme += SourceCode[j].ToString();
                        j++;
                    }

                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }
                /* * */

                else if (CurrentChar == '/' && SourceCode[j + 1] == '*')//comment
                {
                    bool Flag = false;
                    j++;
                    if (SourceCode[j] == '*')
                        while (j < SourceCode.Length)
                        {
                            if (SourceCode[j] == '*' && SourceCode[j + 1] == '/')
                            {
                                j += 2;
                                Flag = true;
                                break;

                            }
                            else
                            {
                                CurrentLexeme += SourceCode[j].ToString();
                            }
                            j++;
                        }
                    if (!Flag)
                    {

                        FindTokenClass(CurrentLexeme);
                    }
                    i = j;
                }
                else if (CurrentChar == '"') //string
                {
                    j++;
                    while (j < SourceCode.Length)
                    {
                        if (SourceCode[j] == '"')
                        {
                            CurrentLexeme += SourceCode[j].ToString();
                            break;
                        }
                        else { CurrentLexeme += SourceCode[j].ToString(); }
                        j++;
                    }

                    FindTokenClass(CurrentLexeme);
                    i = j;
                }

                else if (CurrentChar == '(' || CurrentChar == '{' || CurrentChar == ')' || CurrentChar == '}')
                {
                    j++;


                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }


                else if (!(CurrentChar >= 'A' && CurrentChar <= 'Z') && !(CurrentChar >= '0' && CurrentChar <= '9') && !(CurrentChar >= 'a' && CurrentChar <= 'z')) //operators
                {

                    j++;
                    while (j < SourceCode.Length && !(SourceCode[j] >= '0' && SourceCode[j] <= '9' || SourceCode[j] >= 'A' && SourceCode[j] <= 'Z' || SourceCode[j] >= 'a' && SourceCode[j] <= 'z' || SourceCode[j] == '(' || SourceCode[j] == '{' || SourceCode[j] == ')' || SourceCode[j] == '}'))
                    {

                        if (SourceCode[j] == ' ' || SourceCode[j] == '\r' || SourceCode[j] == '\n')
                            break;
                        CurrentLexeme += SourceCode[j].ToString();
                        j++;
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }
                else
                {
                    FindTokenClass(CurrentLexeme);
                }
            }

            JASON_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }
            //Is it an identifier?
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Identifer;
                Tokens.Add(Tok);
            }
            //Is it an String?
            else if (isString(Lex))
            {
                Tok.token_type = Token_Class.StringValue;
                Tokens.Add(Tok);
            }
            //Is it a Number?
            else if (isNumber(Lex))
            {
                Tok.token_type = Token_Class.Number;
                Tokens.Add(Tok);
            }
            //Is it an operator?
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }
            //Is it an undefined?
            else
            {
                Errors.Error_List.Add(Lex);
            }
        }


        bool isIdentifier(string lex)
        {
            bool isValid = true;
            // Check if the lex is an identifier or not.
            var identiferRE = new Regex("^[a-zA-Z][a-zA-Z0-9]*$", RegexOptions.Compiled);
            if (identiferRE.IsMatch(lex))
                return isValid;
            return false;
        }

        bool isString(string lex)
        {
            bool isValid = true;
            // Check if the lex is an String or not.
            var StringRegx = new Regex("^\"(.)*\"$", RegexOptions.Compiled);
            if (StringRegx.IsMatch(lex))
                return isValid;
            return false;
        }

        bool isNumber(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.
            var constRE = new Regex(@"^[0-9]+(\.[0-9]+)?$", RegexOptions.Compiled);
            if (constRE.IsMatch(lex))
                return isValid;
            return false;
        }
    }
}