using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace JASON_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }

        Node Program()
        {
            Node program = new Node("Main");
            program.Children.Add(Function_Statements());
            program.Children.Add(MainFunction());

            MessageBox.Show("Success");
            return program;
        }

        Node Function_Statements()
        {
            if (InputPointer < TokenStream.Count && InputPointer + 1 < TokenStream.Count &&
                (TokenStream[InputPointer].token_type == Token_Class.IntDataType ||
                TokenStream[InputPointer].token_type == Token_Class.FloatDataType ||
                TokenStream[InputPointer].token_type == Token_Class.StringDataType) && TokenStream[InputPointer + 1].token_type == Token_Class.Main)
            {
                return null;
            }
            Node functionStatements = new Node("Function_Statements");
            if (InputPointer < TokenStream.Count &&
                (TokenStream[InputPointer].token_type == Token_Class.IntDataType ||
                TokenStream[InputPointer].token_type == Token_Class.FloatDataType ||
                TokenStream[InputPointer].token_type == Token_Class.StringDataType))
            {
                functionStatements.Children.Add(Function_Statement());
                functionStatements.Children.Add(Function_Statement_Dash());
            }
            else
            {
                return null;
            }


            // write your code here to check the header sructure
            return functionStatements;
        }
        Node Function_Statement_Dash()
        {
            if (InputPointer < TokenStream.Count && InputPointer + 1 < TokenStream.Count &&
               (TokenStream[InputPointer].token_type == Token_Class.IntDataType ||
               TokenStream[InputPointer].token_type == Token_Class.FloatDataType ||
               TokenStream[InputPointer].token_type == Token_Class.StringDataType) && TokenStream[InputPointer + 1].token_type == Token_Class.Main)
            {
                return null;
            }
            Node function_Statement_Dash = new Node("Function_Statement_Dash");
            if (InputPointer < TokenStream.Count &&
                (TokenStream[InputPointer].token_type == Token_Class.IntDataType ||
                TokenStream[InputPointer].token_type == Token_Class.FloatDataType ||
                TokenStream[InputPointer].token_type == Token_Class.StringDataType))
            {
                function_Statement_Dash.Children.Add(Function_Statement());
                function_Statement_Dash.Children.Add(Function_Statement_Dash());

            }
            else
            {
                return null;
            }


            // write your code here to check the header sructure
            return function_Statement_Dash;
        }
        Node Function_Statement()
        {
            if (InputPointer < TokenStream.Count && InputPointer + 1 < TokenStream.Count &&
               (TokenStream[InputPointer].token_type == Token_Class.IntDataType ||
               TokenStream[InputPointer].token_type == Token_Class.FloatDataType ||
               TokenStream[InputPointer].token_type == Token_Class.StringDataType) && TokenStream[InputPointer + 1].token_type == Token_Class.Main)
            {
                return null;
            }
            Node function_Statement = new Node("Function_Statement");

            function_Statement.Children.Add(Function_Decalaration());
            function_Statement.Children.Add(Function_Body());

            // write your code here to check the header sructure
            return function_Statement;
        }
        Node Function_Decalaration()
        {
            Node function_Decalaration = new Node("Function_Decalaration");

            function_Decalaration.Children.Add(Datatype());
            function_Decalaration.Children.Add(Function_Name());
            function_Decalaration.Children.Add(match(Token_Class.LBracket));
            function_Decalaration.Children.Add(Parameters());
            function_Decalaration.Children.Add(match(Token_Class.RBracket));

            // write your code here to check the header sructure
            return function_Decalaration;
        }
        Node Function_Name()
        {
            Node function_Name = new Node("Function_Name");

            function_Name.Children.Add(match(Token_Class.Identifer));
            // write your code here to check the header sructure
            return function_Name;
        }
        Node Parameters()
        {
            Node parameters = new Node("Parameters");
            if (InputPointer < TokenStream.Count &&
                 (TokenStream[InputPointer].token_type == Token_Class.IntDataType ||
                 TokenStream[InputPointer].token_type == Token_Class.FloatDataType ||
                 TokenStream[InputPointer].token_type == Token_Class.StringDataType))
            {
                parameters.Children.Add(Parameter());
                parameters.Children.Add(Parameters_Dash());
            }
            else
            {
                return null;
            }


            // write your code here to check the header sructure
            return parameters;
        }
        Node Parameter()
        {
            Node parameter = new Node("Parameter");
            parameter.Children.Add(Datatype());
            parameter.Children.Add(match(Token_Class.Identifer));

            // write your code here to check the header sructure
            return parameter;
        }

        Node Parameters_Dash()
        {
            Node parameters_Dash = new Node("Parameters_Dash");
            if (InputPointer < TokenStream.Count &&
                TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                parameters_Dash.Children.Add(match(Token_Class.Comma));
                parameters_Dash.Children.Add(Parameter());
                parameters_Dash.Children.Add(Parameters_Dash());
            }
            else
            {
                return null;
            }


            // write your code here to check the header sructure
            return parameters_Dash;
        }
        Node MainFunction()
        {
            Node mainFunction = new Node("MainFunction");
            mainFunction.Children.Add(Datatype());
            mainFunction.Children.Add(match(Token_Class.Main));
            mainFunction.Children.Add(match(Token_Class.LBracket));
            mainFunction.Children.Add(match(Token_Class.RBracket));
            mainFunction.Children.Add(Function_Body());

            return mainFunction;
        }
        Node Datatype()
        {
            Node datatype = new Node("Datatype");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.IntDataType)
            {
                datatype.Children.Add(match(Token_Class.IntDataType));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.StringDataType)
            {
                datatype.Children.Add(match(Token_Class.StringDataType));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.FloatDataType)
            {
                datatype.Children.Add(match(Token_Class.FloatDataType));
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                     + " DataType \r\n");
               // InputPointer++;
                return null;
            }
            return datatype;
        }
        Node Equation()
        {
            Node equation = new Node("Equation");
            equation.Children.Add(Term());
            equation.Children.Add(Eq());
            return equation;
        }
        Node MathEquation()
        {
            Node equation_type = new Node("Equation_type");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LBracket)
            {
                equation_type.Children.Add(Bracket_Equation());
            }
            else
            {
                equation_type.Children.Add(Equation());
            }
            return equation_type;
        }
        Node Eq()
        {
            Node eq = new Node("Equation_Dash");

            eq.Children.Add(Arithmatic_Operation());
            eq.Children.Add(Equation_type());
            eq.Children.Add(Equation_Dash());
            return eq;
        }
        Node Equation_Dash()
        {
            Node equation_Dash = new Node("Equation_Dash");
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.MultiplyOp ||
                TokenStream[InputPointer].token_type == Token_Class.DivideOp ||
                TokenStream[InputPointer].token_type == Token_Class.PlusOp ||
                TokenStream[InputPointer].token_type == Token_Class.MinusOp))
            {
                equation_Dash.Children.Add(Arithmatic_Operation());
                equation_Dash.Children.Add(Equation_type());
            }
            else
            {
                return null;
            }
            return equation_Dash;
        }
        Node Equation_type()
        {
            Node equation_type = new Node("Equation_type");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LBracket)
            {
                equation_type.Children.Add(Bracket_Equation());
            }
            else
            {
                equation_type.Children.Add(Equation2());
            }
            return equation_type;
        }
        Node Equation2()
        {
            Node equation2 = new Node("Equation2");

            equation2.Children.Add(Term());
            equation2.Children.Add(Equation_Dash());

            return equation2;
        }

        Node Bracket_Equation()
        {
            Node bracket_Equation = new Node("Bracket_Equation");

            bracket_Equation.Children.Add(match(Token_Class.LBracket));
            bracket_Equation.Children.Add(Equation());
            bracket_Equation.Children.Add(match(Token_Class.RBracket));
            bracket_Equation.Children.Add(Equation_Dash());
            return bracket_Equation;
        }

        Node Arithmatic_Operation()
        {
            Node arithmatic_Operation = new Node("Arithmatic_Operation");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
            {
                arithmatic_Operation.Children.Add(match(Token_Class.MultiplyOp));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.DivideOp)
            {
                arithmatic_Operation.Children.Add(match(Token_Class.DivideOp));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.PlusOp)
            {
                arithmatic_Operation.Children.Add(match(Token_Class.PlusOp));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.MinusOp)
            {
                arithmatic_Operation.Children.Add(match(Token_Class.MinusOp));
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                     + " Arithmatic Operator \r\n");
                //InputPointer++;
                return null;
            }
            return arithmatic_Operation;
        }


        // Implement your logic here
        Node Function_Body()
        {
            Node function_Body = new Node("Function_Body");
            function_Body.Children.Add(match(Token_Class.Lcurly));
            function_Body.Children.Add(Set_Of_Statement());
            function_Body.Children.Add(Return_Statment());
            function_Body.Children.Add(match(Token_Class.Rcurly));

            return function_Body;
        }
        Node Return_Statment()
        {
            Node return_Statment = new Node("Return_Statment");
            return_Statment.Children.Add(match(Token_Class.Return));
            return_Statment.Children.Add(Expression());
            return_Statment.Children.Add(match(Token_Class.SemiColon));
            return return_Statment;
        }
        Node Read_Statment()
        {
            Node read_Statment = new Node("Read_Statment");
            read_Statment.Children.Add(match(Token_Class.Read));
            read_Statment.Children.Add(match(Token_Class.Identifer));
            read_Statment.Children.Add(match(Token_Class.SemiColon));


            return read_Statment;
        }
        Node Write_Statment()
        {
            Node write_Statment = new Node("Write_Statment");
            write_Statment.Children.Add(match(Token_Class.Write));
            write_Statment.Children.Add(Write_Content());
            write_Statment.Children.Add(match(Token_Class.SemiColon));


            return write_Statment;
        }
        Node Write_Content()
        {
            Node write_Content = new Node("Write_Content");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Endl)
            {
                write_Content.Children.Add(match(Token_Class.Endl));
            }
            else
            {
                write_Content.Children.Add(Expression());
            }
            return write_Content;
        }
        Node Statement()
        {
            Node statement = new Node("Statement");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Identifer)
            {

                if (InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer + 1].token_type == Token_Class.AssignmentOp)
                {
                    Console.WriteLine("inside Assignment");
                    statement.Children.Add(Assignment_Statement());

                    return statement;
                }
                else if (InputPointer + 1 < TokenStream.Count)
                {
                    statement.Children.Add(Condition_Statement());

                    return statement;
                }
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Read)
            {
                statement.Children.Add(Read_Statment());

                return statement;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Write)
            {

                statement.Children.Add(Write_Statment());

                return statement;
            }

            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.If)
            {

                statement.Children.Add(If_Statement());

                return statement;
            }

            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Repeat)
            {

                statement.Children.Add(Repeat_Statement());

                return statement;
            }

      

            else if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.IntDataType || TokenStream[InputPointer].token_type == Token_Class.StringDataType || TokenStream[InputPointer].token_type == Token_Class.FloatDataType))
                {
                    if (InputPointer + 2 < TokenStream.Count && TokenStream[InputPointer + 2].token_type == Token_Class.LBracket)
                    {
                        statement.Children.Add(Function_Statement());
                        return statement;
                    }
                    else
                    {
                        statement.Children.Add(Declaration_Statement());
                        return statement;
                    }
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                         + " Statement \r\n");
                    InputPointer++;
                }
            return null;
        }
        public bool check_Statement()
        {
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Identifer
                 || TokenStream[InputPointer].token_type == Token_Class.Read
                 || TokenStream[InputPointer].token_type == Token_Class.Write
                 || TokenStream[InputPointer].token_type == Token_Class.If
                 || TokenStream[InputPointer].token_type == Token_Class.Repeat
                 || TokenStream[InputPointer].token_type == Token_Class.IntDataType
                 || TokenStream[InputPointer].token_type == Token_Class.StringDataType
                 || TokenStream[InputPointer].token_type == Token_Class.FloatDataType))
            {
                return true;
            }

            return false;
        }
        public bool key_word()
        {
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.EndStatment
                 || TokenStream[InputPointer].token_type == Token_Class.Until
                 || TokenStream[InputPointer].token_type == Token_Class.Endl
                 || TokenStream[InputPointer].token_type == Token_Class.Then
                 || TokenStream[InputPointer].token_type == Token_Class.Return
                 || TokenStream[InputPointer].token_type == Token_Class.Else
                 || TokenStream[InputPointer].token_type == Token_Class.ElseIf))
            {
                return true;
            }

            return false;
        }
        Node Term()
        {
            Node term = new Node("Term");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Number)
            {
                term.Children.Add(match(Token_Class.Number));
            }
            else if (InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Identifer && TokenStream[InputPointer + 1].token_type == Token_Class.LBracket)
            {
                term.Children.Add(Function_Call());

            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Identifer)
            {
                term.Children.Add(match(Token_Class.Identifer));

            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                     + " Term \r\n");
                InputPointer++;
                return null;
            }
            return term;
        }

        Node Set_Of_Statement()
        {

            Node set_Of_Statement = new Node("Set_Of_Statement");
            bool isStatement = check_Statement();
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type != Token_Class.Return && isStatement)
            {
                set_Of_Statement.Children.Add(Statement());
                set_Of_Statement.Children.Add(Set_Of_Statement_Dash());
            }
            else
            {
                return null;
            }
            return set_Of_Statement;
        }
        Node Set_Of_Statement_Dash()
        {
            Node set_Of_Statement_Dash = new Node("Set_Of_Statement_Dash");
            bool isStatement = check_Statement();
            bool isKey = key_word();
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type != Token_Class.Return && isStatement)
            {
                set_Of_Statement_Dash.Children.Add(Statement());
                set_Of_Statement_Dash.Children.Add(Set_Of_Statement_Dash());
            }
            else if (InputPointer < TokenStream.Count &&
                 (TokenStream[InputPointer].token_type == Token_Class.SemiColon
                 || TokenStream[InputPointer].token_type == Token_Class.Comma
                 || TokenStream[InputPointer].token_type == Token_Class.Dot))
            {
                Errors.Error_List.Add("Parsing Error: undefine " + TokenStream[InputPointer].token_type.ToString() + "\r\n");
                InputPointer++;
                set_Of_Statement_Dash.Children.Add(Set_Of_Statement_Dash());
            }
            else
            {
                return null;
            }
            return set_Of_Statement_Dash;
        }
        Node Function_Call()
        {
            Node function_Call = new Node("Function_Call");
            function_Call.Children.Add(match(Token_Class.Identifer));
            function_Call.Children.Add(match(Token_Class.LBracket));
            function_Call.Children.Add(Arguments());
            function_Call.Children.Add(match(Token_Class.RBracket));
            return function_Call;
        }
        Node Arguments()
        {
            Node arguments = new Node("Arguments");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Identifer)
            {
                arguments.Children.Add(match(Token_Class.Identifer));
                arguments.Children.Add(Arguments_Dash());
            }
            else
            {
                return null;
            }
            return arguments;
        }
        Node Arguments_Dash()
        {
            Node arguments = new Node("Arguments");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                arguments.Children.Add(match(Token_Class.Comma));
                arguments.Children.Add(match(Token_Class.Identifer));
                arguments.Children.Add(Arguments_Dash());
            }
            else
            {
                return null;
            }


            return arguments;
        }

        Node Expression()
        {
            Node expression = new Node("Expression");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.StringValue)
            {
                expression.Children.Add(match(Token_Class.StringValue));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LBracket ||
                (InputPointer < TokenStream.Count && InputPointer + 1 < TokenStream.Count && (TokenStream[InputPointer + 1].token_type == Token_Class.PlusOp ||
               TokenStream[InputPointer + 1].token_type == Token_Class.MinusOp || TokenStream[InputPointer + 1].token_type == Token_Class.DivideOp ||
               TokenStream[InputPointer + 1].token_type == Token_Class.MultiplyOp)
                && (TokenStream[InputPointer].token_type == Token_Class.Identifer || TokenStream[InputPointer].token_type == Token_Class.Number)))
            {
                expression.Children.Add(MathEquation());
            }
            else if (
                InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Number ||
                TokenStream[InputPointer].token_type == Token_Class.Identifer))
            {
                expression.Children.Add(Term());
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                     + " Expression \r\n");
                //InputPointer++;
                return null;
            }
            return expression;
        }
        Node Assignment_Statement()
        {
            Node assignment_Statment = new Node("Assignment_Statement");

            assignment_Statment.Children.Add(match(Token_Class.Identifer));
            assignment_Statment.Children.Add(match(Token_Class.AssignmentOp));
            assignment_Statment.Children.Add(Expression());
            //assignment_Statment.Children.Add(match(Token_Class.SemiColon));
            return assignment_Statment;
        }
        Node Condition()
        {
            Node condition = new Node("Condition");

            condition.Children.Add(match(Token_Class.Identifer));
            condition.Children.Add(Condition_Op());
            condition.Children.Add(Term());
            return condition;
        }
        Node Condition_Op()
        {
            Node condition_Op = new Node("Condition_Op");

            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.NotEqualOp)
            {
                condition_Op.Children.Add(match(Token_Class.NotEqualOp));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.EqualOp)
            {
                condition_Op.Children.Add(match(Token_Class.EqualOp));

            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LessThanOp)
            {
                condition_Op.Children.Add(match(Token_Class.LessThanOp));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.GreaterThanOp)
            {
                condition_Op.Children.Add(match(Token_Class.GreaterThanOp));
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                     + " condition Operator \r\n");
                //InputPointer++;
                return null;
            }
            return condition_Op;
        }
        Node Boolean_Op()
        {
            Node boolean_Op = new Node("Boolean_Op");

            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.AndOp)
            {
                boolean_Op.Children.Add(match(Token_Class.AndOp));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.OrOp)
            {
                boolean_Op.Children.Add(match(Token_Class.OrOp));

            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                     + " boolean Operator \r\n");
                //InputPointer++;
                return null;
            }
            return boolean_Op;
        }
        Node Condition_Statement()
        {
            Node condition_Statement = new Node("Condition_Statement");

            condition_Statement.Children.Add(Condition());
            condition_Statement.Children.Add(Conditions());

            return condition_Statement;
        }
        Node Conditions()
        {
            Node conditions = new Node("Conditions");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.AndOp ||
                InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.OrOp)
            {
                conditions.Children.Add(Boolean_Op());
                conditions.Children.Add(Condition());
                conditions.Children.Add(Conditions_Dash());
            }
            else
            {
                return null;
            }
            return conditions;
        }
        Node Conditions_Dash()
        {
            Node conditions_Dash = new Node("Conditions_Dash");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.AndOp ||
                InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.OrOp)
            {
                conditions_Dash.Children.Add(Boolean_Op());
                conditions_Dash.Children.Add(Condition());
                conditions_Dash.Children.Add(Conditions_Dash());
            }
            else
            {
                return null;
            }
            return conditions_Dash;
        }

        Node Else_Statement()
        {
            Node else_Statement = new Node("Else_Statement");
            else_Statement.Children.Add(match(Token_Class.Else));
            else_Statement.Children.Add(Set_Of_Statement());
            else_Statement.Children.Add(match(Token_Class.EndStatment));
            return else_Statement;

        }
        Node If_Statement()
        {
            Node if_Statement = new Node("If_Statement");

            if_Statement.Children.Add(match(Token_Class.If));
            if_Statement.Children.Add(Condition_Statement());
            if_Statement.Children.Add(match(Token_Class.Then));
            if_Statement.Children.Add(Set_Of_Statement());
            if_Statement.Children.Add(Else_Cases());
            return if_Statement;
        }
        Node Else_If_Statement()
        {
            Node else_If_Statement = new Node("Else_If_Statement");

            else_If_Statement.Children.Add(match(Token_Class.ElseIf));
            else_If_Statement.Children.Add(Condition_Statement());
            else_If_Statement.Children.Add(match(Token_Class.Then));
            else_If_Statement.Children.Add(Set_Of_Statement());
            else_If_Statement.Children.Add(Else_Cases());
            return else_If_Statement;
        }
        Node Else_Cases()
        {
            Node else_Cases = new Node("Else_Cases");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.ElseIf)
            {
                else_Cases.Children.Add(Else_If_Statement());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Else)
            {
                else_Cases.Children.Add(Else_Statement());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.EndStatment)
            {
                else_Cases.Children.Add(match(Token_Class.EndStatment));
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                     + " end \r\n");
               // InputPointer++;
                return null;
            }
            return else_Cases;
        }
        Node Repeat_Statement()
        {
            Node repeat_Statement = new Node("Repeat_Statement");
            repeat_Statement.Children.Add(match(Token_Class.Repeat));
            repeat_Statement.Children.Add(Set_Of_Statement());
            repeat_Statement.Children.Add(match(Token_Class.Until));
            repeat_Statement.Children.Add(Condition_Statement());
            return repeat_Statement;
        }
        Node Declaration_Statement()
        {
            Node declaration_Statement = new Node("Declaration_Statement");
            declaration_Statement.Children.Add(Datatype());
            declaration_Statement.Children.Add(Var());
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                declaration_Statement.Children.Add(Idlist());
                declaration_Statement.Children.Add(match(Token_Class.SemiColon));
            }
            else
            {
                declaration_Statement.Children.Add(match(Token_Class.SemiColon));
            }
            return declaration_Statement;
        }

        Node Idlist()
        {
            Node idlist = new Node("Idlist");
            idlist.Children.Add(match(Token_Class.Comma));
            idlist.Children.Add(Var());
            idlist.Children.Add(Idlist_Dash());
            return idlist;
        }
        Node Var()
        {
            Node var = new Node("Var");
            if (InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Identifer
                && TokenStream[InputPointer + 1].token_type == Token_Class.AssignmentOp)
            {
                var.Children.Add(Assignment_Statement());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Identifer)
            {
                var.Children.Add(match(Token_Class.Identifer));
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                     + " Identifer \r\n");
                //InputPointer++;
                return null;
            }

            return var;
        }
        Node Idlist_Dash()
        {
            Node idlist_Dash = new Node("Idlist_Dash");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                idlist_Dash.Children.Add(match(Token_Class.Comma));
                idlist_Dash.Children.Add(Var());
                idlist_Dash.Children.Add(Idlist_Dash());
            }
            else
            {
                return null;
            }



            return idlist_Dash;
        }


        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}