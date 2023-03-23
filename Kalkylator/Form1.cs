using System;
using System.Collections.Generic;

namespace Kalkylator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Variabler 
        bool operatorInputBlocked = true;
        bool numInputBlocked = false;
        bool programIsShowingResult = false;
        string currentInput = "0";
        double calcResult = 0;
        List<string> serialInput = new List<string>();
        string result = string.Empty;
        int maximumInputCount = 16;

        //Funktioner 
        private void OnInputDisplayUpdate()
        {

            if (currentInput.Length > 0) currentInput = currentInput.TrimStart('0');
            if (currentInput != "0" && currentInput != "") operatorInputBlocked = false;
            
            Display_Input.Text = currentInput;
            char puMa = ' ';
            string historyString = String.Join(puMa, serialInput);
           if (historyString.Length > 26) historyString = "..." + historyString.Substring(historyString.Length - 27);
            Display.Text = historyString;

        }

        // Visar resultat från Calculate() : Parameter(O) string character - Char som kommer före (= result), T.EX. ([√] = [result])  
        private void DisplayResult(string character = "")
        { 
            programIsShowingResult = true;
            Display_Input.Text = character + " = " + calcResult.ToString();
        }


        // Funktion som lägger till tal i serialInput : Parameter string input - Nummer
        private void AddToInput(string input)
        {
            
            if (input == "0")
            {
                if (currentInput != "0" && currentInput.Length <= maximumInputCount && !numInputBlocked)
                {
                    currentInput += "0";
                    OnInputDisplayUpdate();
                }
            }
            else
            {
                if (currentInput.Length <= maximumInputCount && !numInputBlocked)
                {
                    currentInput += input;
                    OnInputDisplayUpdate();
                }
            }
        }

        // Funktion som lägger till operator i serialInput : Parameter string input - Operator-typ
        private void AddOperator(string input)
        {
            programIsShowingResult = false;
            System.Diagnostics.Debug.WriteLine("Trying to add operator.");
            if (!operatorInputBlocked && currentInput != "0" && currentInput != "")
            {
                System.Diagnostics.Debug.WriteLine("if-statement passed, operator added.");
                operatorInputBlocked = true;
                if (!numInputBlocked) serialInput.Add(currentInput);
                serialInput.Add(input);
                currentInput = "0";
                OnInputDisplayUpdate();
                numInputBlocked = false;
            }
        }

        // Funktion som räknar ut det som ligger i serialInput : Tolkar elementens betydelse, T.EX "+", utför beräkning : ([prevNum] [operator] [num] = [result])   
        private void Calculate()
        {
            if (currentInput != "0" && currentInput != "" && !numInputBlocked)
            {
                string previous = "";  // Variabel för att hålla koll på föregående värde i foreach-loopen nedan : foreach(i) - 1
                numInputBlocked = true;

                if (currentInput != "0")
                {
                    serialInput.Add(currentInput);
                    OnInputDisplayUpdate();
                }
                System.Diagnostics.Debug.WriteLine(String.Join(' ', serialInput));


                string lastInput = serialInput.Last();
                if (!lastInput.Replace(",", "").All(char.IsDigit))
                {
                    serialInput.Remove(lastInput);
                }

                int runTimes = 0;
                foreach (string inputFraction in serialInput)
                {
                    runTimes++;


                    if (inputFraction.Replace(",", "").All(char.IsDigit)) // Nuvarande värde i foreach-loopen är ett tal
                    {

                        if (runTimes == 1) // Är det första gången foreach itererar
                        {


                            calcResult = double.Parse(inputFraction); // Första värdet i foreach : Tilldela utgångsvärde till calcResult

                        }
                        else // Det är inte första gången foreach itererar
                        {

                            // Räkna addition : calcResult + inputFraction 
                            if (previous == "+")
                            {

                                calcResult += double.Parse(inputFraction);
                                DisplayResult("");
                                System.Diagnostics.Debug.WriteLine("result is " + calcResult);
                            }

                            // Räkna subraktion : calcResult - inputFraction 
                            if (previous == "-")
                            {

                                calcResult -= double.Parse(inputFraction);
                                DisplayResult("");
                                System.Diagnostics.Debug.WriteLine("result is " + calcResult);
                            }

                            // Räkna multiplikation : calcResult * inputFraction 
                            if (previous == "*")
                            {

                                calcResult *= double.Parse(inputFraction);
                                DisplayResult("");
                                System.Diagnostics.Debug.WriteLine("result is " + calcResult);
                            }

                            // Räkna division : calcResult / inputFraction 
                            if (previous == "/")
                            {

                                calcResult /= double.Parse(inputFraction);
                                DisplayResult("");
                                System.Diagnostics.Debug.WriteLine("result is " + calcResult);
                            }

                        }

                    }
                    previous = inputFraction;
                }




            }
        }

        //Hantering av knapptryckningar
        private void Button_0_Click(object sender, EventArgs e)
        {
            AddToInput("0");
        }

        private void Button_1_Click(object sender, EventArgs e)
        {
            AddToInput("1");
        }

        private void Button_2_Click(object sender, EventArgs e)
        {
            AddToInput("2");
        }

        private void Button_3_Click(object sender, EventArgs e)
        {
            AddToInput("3");
        }

        private void Button_4_Click(object sender, EventArgs e)
        {
            AddToInput("4");
        }

        private void Button_5_Click(object sender, EventArgs e)
        {
            AddToInput("5");
        }

        private void Button_6_Click(object sender, EventArgs e)
        {
            AddToInput("6");
        }

        private void Button_7_Click(object sender, EventArgs e)
        {
            AddToInput("7");
        }

        private void Button_8_Click(object sender, EventArgs e)
        {
            AddToInput("8");
        }

        private void Button_9_Click(object sender, EventArgs e)
        {
            AddToInput("9");
        }

        private void Button_Comma_Click(object sender, EventArgs e)
        {
            AddToInput(",");
        }

        private void Button_Plus_Click(object sender, EventArgs e)
        {
            AddOperator("+"); 
        }

        private void Button_Minus_Click(object sender, EventArgs e)
        {
            AddOperator("-");
        }

        private void Button_Multiplied_Click(object sender, EventArgs e)
        {
            AddOperator("*");
        }

        private void Button_Divided_Click(object sender, EventArgs e)
        {
            AddOperator("/");
        }

        private void Button_Square_Click(object sender, EventArgs e) // Note to self: Bygg om denna + de tre under till en funktion om jag orkar
        {
            Calculate();

            if (calcResult != 0 && !operatorInputBlocked)
            {
                serialInput.Clear();
                
            calcResult = Math.Sqrt(calcResult);
            DisplayResult("√");
                serialInput.Add(calcResult.ToString());
                operatorInputBlocked = false;
                currentInput = calcResult.ToString();
            }
        }

        private void Button_Inverse_Click(object sender, EventArgs e)
        {
            Calculate();

            if (calcResult != 0 && !operatorInputBlocked)
            {
                serialInput.Clear();

                calcResult = 1 / calcResult;
                DisplayResult("1/x");
                serialInput.Add(calcResult.ToString());
                operatorInputBlocked = false;
                currentInput = calcResult.ToString();
            }
        }

        private void Button_Percent_Click(object sender, EventArgs e)
        {
            Calculate();

            if (calcResult != 0 && !operatorInputBlocked)
            {
                serialInput.Clear();
                calcResult = calcResult / 100;
                DisplayResult("%");
                serialInput.Add(calcResult.ToString());
                operatorInputBlocked = false;
                currentInput = calcResult.ToString();
            }
        }

        private void Button_InvertNum_Click(object sender, EventArgs e)
        {
            Calculate();

            if (calcResult != 0 && !operatorInputBlocked)
            {
                serialInput.Clear();
                calcResult *= -1;
                DisplayResult("±");
                serialInput.Add(calcResult.ToString());
                operatorInputBlocked = false;
                currentInput = calcResult.ToString();
            }
        }

        private void Button_Equals_Click(object sender, EventArgs e)
        {
            Calculate();
        }

        private void Button_Clear_Click(object sender, EventArgs e)
        {
            serialInput.Clear();
            currentInput = "0";
            OnInputDisplayUpdate();
            numInputBlocked = false;
        }


        private void Button_Erase_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentInput) && !programIsShowingResult)
            {
                currentInput = currentInput.Remove(currentInput.Length - 1);
                if (string.IsNullOrEmpty(currentInput)) currentInput = "0";
                OnInputDisplayUpdate();

                
            }
        }

        private void Button_ClearEntry_Click(object sender, EventArgs e)
        {
            if (serialInput.Count > 0 && !programIsShowingResult)
            {
                currentInput = "0";
                OnInputDisplayUpdate();
            }
        }

       
    }
}