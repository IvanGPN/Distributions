using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursachBumarik
{
    public partial class FormMain : System.Windows.Forms.Form
    {


        public FormMain()
        {
            InitializeComponent();
              
            
            textBoxLeftBorderOne.Enabled = false;
            textBoxRightBorderOne.Enabled = false;
            textBoxLeftBorderTwo.Enabled = false;
            textBoxRightBorderTwo.Enabled = false;
            textBoxLambdaValue.Enabled = false;
            textBoxLambda2.Enabled = false;
            textBoxLambda3.Enabled = false;//для эрлангова распределения
            textBoxSmechenieX.Enabled = false;//смещение для гиперэкспоненциального распределения
            textBoxKoefA.Enabled = false;
            textBoxKoefC.Enabled = false;
            textBoxKoefM.Enabled = false;
        }


         



        //Записывает в массив количество чисел, попавшие в определенные интервалы
        private double[] SortFromIntervals(double[] arrayRandomSymbol, ref double Step, ref double Min, ref double Max)//изменения элементов  в вызывающем методе отражаются в вызываемом методе - ref
        {
            double[] Intervals = new double[Convert.ToInt32(textBoxCountIntervals.Text) + 1];

            double[] IntervalsValue = new double[Convert.ToInt32(textBoxCountIntervals.Text)];

            Min = double.MaxValue; Max = double.MinValue; Step = 0;//максимальные и минимальные числа для double

            int CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);//количество интервалов(столбцов в гистограмме)

            for (int i = 0; i < arrayRandomSymbol.Length; i++)
            {
                if (arrayRandomSymbol[i] > Max) { Max = arrayRandomSymbol[i]; }//поиск максимума в массиве случайных чисел
                else { if (arrayRandomSymbol[i] < Min) { Min = arrayRandomSymbol[i]; } }//поиск минимума
            }
            Step = Math.Abs(Max - Min) / CountInterval;//шаг между соседними интервалами

            Array.Sort(arrayRandomSymbol);

            Intervals[0] = Min;
            for (int i = 1; i < Intervals.Length; i++)
            {
                Intervals[i] = Intervals[i - 1] + Step;//прибавляем шаг к каждому следующему интервалу
            }

            int checkPoint = 0;
            for (int j = 0; j < IntervalsValue.Length; j++)
            {
                for (int i = checkPoint; i < arrayRandomSymbol.Length; i++)
                {
                    if ((arrayRandomSymbol[i] >= Intervals[j]) && (arrayRandomSymbol[i] < Intervals[j + 1]))//если случайное число попадает в данный интервал
                    {
                        IntervalsValue[j] += 1;//значение интервала возрастает на один
                    }
                    else
                    {
                        checkPoint = i;//иначе проверяем следующий интервал на попадание
                        break;
                    }
                }
            }

            return IntervalsValue;

        }

        //Равномерное распределение
        private double[] DistributionUniform(double[] arrayRandomSymbol)
        {
            int LeftBorder = Convert.ToInt32(textBoxLeftBorderOne.Text), RightBorder = Convert.ToInt32(textBoxRightBorderOne.Text);
            int CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);
            for (int i = 0; i < arrayRandomSymbol.Length; i++)
            {
                arrayRandomSymbol[i] = LeftBorder + arrayRandomSymbol[i] * (RightBorder - LeftBorder);
            }
            return arrayRandomSymbol;
        }

        // распределение Лехмера
        private double[] DistributionLehmer(double[] arrayRandomSymbol)
        {
            int LeftBorder = Convert.ToInt32(textBoxLeftBorderOne.Text), RightBorder = Convert.ToInt32(textBoxRightBorderOne.Text);
            int CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);
            double Xi = DateTime.Now.Second;
            double A = 42135815;
            double c = 13542135;
            double m = 4257287;

            for (int i = 0; i < arrayRandomSymbol.Length; i++)
            {
                arrayRandomSymbol[i] = (A * Xi + c) % m;
                Xi = arrayRandomSymbol[i];
                arrayRandomSymbol[i] = LeftBorder + arrayRandomSymbol[i] * (RightBorder - LeftBorder);
            }
            return arrayRandomSymbol;
        }

        //распределение методом простых конгурэнций
        private double[] DistributionCong(double[] arrayRandomSymbol)
        {
            int LeftBorder = Convert.ToInt32(textBoxLeftBorderOne.Text), RightBorder = Convert.ToInt32(textBoxRightBorderOne.Text);
            int CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);
            double Xi = DateTime.Now.Second;
            double A = DateTime.Now.Hour * DateTime.Now.Minute;
            for (int i = 0; i < arrayRandomSymbol.Length; i++)
            {
                arrayRandomSymbol[i] = (A * Xi) % 9973;
                Xi = arrayRandomSymbol[i];
                arrayRandomSymbol[i] = LeftBorder + arrayRandomSymbol[i] * (RightBorder - LeftBorder);
            }
            return arrayRandomSymbol;
        }

        //Экспoненциальное распределение
        private double[] DistributionExp(double[] arrayRandomSymbol)
        {
            double y = Convert.ToInt32(textBoxLambdaValue.Text);//лямбда для экспоненциального распределения
            int LeftBorder = Convert.ToInt32(textBoxLeftBorderOne.Text), RightBorder = Convert.ToInt32(textBoxRightBorderOne.Text);
            int CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);

            for (int i = 0; i < arrayRandomSymbol.Length - 1; i++)
            {
                arrayRandomSymbol[i] = (-1 / y * Math.Log(arrayRandomSymbol[i]));//формула из методички
                arrayRandomSymbol[i] = LeftBorder + arrayRandomSymbol[i] * (RightBorder - LeftBorder);
            }
            return arrayRandomSymbol;
        }

        //гиперэкспoненциальное распределение
        private double[] DistributionGyperExp(double[] arrayRandomSymbol,double[] arrayRandomSymbol1, double[] arrayRandomSymbol2)
        {
            double y = Convert.ToInt32(textBoxLambdaValue.Text);//лямбда1 для гиперэкспоненциального распределения
            double y2 = Convert.ToInt32(textBoxLambda2.Text);//лямбда2 для гиперэкспоненциального распределения
            int LeftBorder = Convert.ToInt32(textBoxLeftBorderOne.Text), RightBorder = Convert.ToInt32(textBoxRightBorderOne.Text);
            int CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);

            int q = Convert.ToInt32(textBoxSmechenieX.Text);
            for (int i = 0; i < arrayRandomSymbol1.Length - 1; i++)
            {
                double x1 = (-1 / y * Math.Log(arrayRandomSymbol1[i]));
                double x2 = (-1 / y * Math.Log(arrayRandomSymbol2[i]));
                arrayRandomSymbol[i] = q * y * Math.Exp(-y * (-1 / y * x1) + (1 - q) * y2 * Math.Exp(-y * (-1 / y * x2)));
               
                arrayRandomSymbol[i] = LeftBorder + arrayRandomSymbol[i] * (RightBorder - LeftBorder);
                
            }

            return arrayRandomSymbol;
        }


        

            //Эрлангово распределение
            private double[] DistributionErlang(double[] arrayRandomSymbol)
            {
            double y = Convert.ToInt32(textBoxLambdaValue.Text);//лямбда для экспоненциального распределения
            int LeftBorder = Convert.ToInt32(textBoxLeftBorderOne.Text), RightBorder = Convert.ToInt32(textBoxRightBorderOne.Text);
            int CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);
            int k = Convert.ToInt32(textBoxSmechenieX.Text);//коэффициент смещения

            for (int i = 0; i < arrayRandomSymbol.Length - 1; i++)
            {
                arrayRandomSymbol[i] = 0;
            }

            for (int i = 0; i < arrayRandomSymbol.Length - 1; i++)
            {
                for (int j = 0; j < k; j++)
                {
                   
                    arrayRandomSymbol[i] += DistributionExp(RandomNumbers())[i];

                }
               
                arrayRandomSymbol[i] = LeftBorder + arrayRandomSymbol[i] * (RightBorder - LeftBorder);

            }
          
            return arrayRandomSymbol;
            }




        //Нормальное распределение
        private double[] DistributionNormal(double[] arrayRandomSymbol, double[] arrayRandomSymbol1, double[] arrayRandomSymbol2, double[] arrayRandomSymbol3, double[] arrayRandomSymbol4, double[] arrayRandomSymbol5)
        {
            int LeftBorder = Convert.ToInt32(textBoxLeftBorderOne.Text), RightBorder = Convert.ToInt32(textBoxRightBorderOne.Text);

            int mato = Convert.ToInt32(textBoxMat.Text);
            int disp = Convert.ToInt32(textBoxDisp.Text);

            for (int i = 0; i < arrayRandomSymbol.Length; i++)
            {
                arrayRandomSymbol[i] = (LeftBorder + arrayRandomSymbol[i] * (RightBorder - LeftBorder)
                    + (LeftBorder + arrayRandomSymbol2[i] * (RightBorder - LeftBorder))
                    + (LeftBorder + arrayRandomSymbol3[i] * (RightBorder - LeftBorder)
                    + (LeftBorder + arrayRandomSymbol4[i] * (RightBorder - LeftBorder)
                    + (LeftBorder + arrayRandomSymbol5[i] * (RightBorder - LeftBorder)
                    + (LeftBorder + arrayRandomSymbol1[i] * (RightBorder - LeftBorder)))))) / Math.Sqrt(0.5) * disp - 4.25 + mato;
            }

            return arrayRandomSymbol;

        }



        // распределение Симпсона
        private double[] DistributionTrapeze(double[] arrayRandomSymbol1, double[] arrayRandomSymbol2)
        {
            int LeftBorder = Convert.ToInt32(textBoxLeftBorderOne.Text), RightBorder = Convert.ToInt32(textBoxRightBorderOne.Text);
            int LeftBorderTwo = Convert.ToInt32(textBoxLeftBorderTwo.Text), RightBorderTwo = Convert.ToInt32(textBoxRightBorderTwo.Text);
            int CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);
            for (int i = 0; i < arrayRandomSymbol1.Length; i++)
            {
                arrayRandomSymbol1[i] = (LeftBorder + arrayRandomSymbol1[i] * (RightBorder - LeftBorder)) + (LeftBorderTwo + arrayRandomSymbol2[i] * (RightBorderTwo - LeftBorderTwo));//сумма двух равномерных распределений
            }

            return arrayRandomSymbol1;
        }


        //равномерное распределение
        private void buttonUniformDistribution_Click(object sender, EventArgs e)
        {
            //блокировка текстбоксов, которые не потребуются для решения этой задачи
            textBoxLeftBorderOne.Enabled = true;
            textBoxRightBorderOne.Enabled = true;
            textBoxLeftBorderTwo.Enabled = false;
            textBoxRightBorderTwo.Enabled = false;
            textBoxLambdaValue.Enabled = false;
            textBoxLambda2.Enabled = false;
            textBoxLambda3.Enabled = false;
            textBoxSmechenieX.Enabled = false;
            textBoxKoefA.Enabled = false;
            textBoxKoefC.Enabled = false;
            textBoxKoefM.Enabled = false;

            ChangeForm(" равномерного распределения", buttonUniformDistribution);
            int CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);
            double Max = 0, Min = 0, Step = 0;
            double[] ArrayIntervalValue = new double[CountInterval];
            ArrayIntervalValue = SortFromIntervals(DistributionUniform(RandomNumbers()), ref Step, ref Min, ref Max);
            BuildChart(ArrayIntervalValue, Step, Min);//построение плотности распределения
            PaintArray(ArrayIntervalValue);//вывод ряда чисел в текстбокс
            BuildChart2(DistributionUniform(RandomNumbers()), Step, Min);

        }

        //метод простых конгурэнций
        private void buttonCongurDustribution_Click(object sender, EventArgs e)
        {
            //блокировка текстбоксов, которые не потребуются для решения этой задачи
            textBoxLeftBorderOne.Enabled = true;
            textBoxRightBorderOne.Enabled = true;
            textBoxLeftBorderTwo.Enabled = false;
            textBoxRightBorderTwo.Enabled = false;
            textBoxLambdaValue.Enabled = false;
            textBoxLambda2.Enabled = false;
            textBoxLambda3.Enabled = false;
            textBoxSmechenieX.Enabled = false;
            textBoxKoefA.Enabled = false;
            textBoxKoefC.Enabled = false;
            textBoxKoefM.Enabled = false;
            ChangeForm(" метода простых конгурэнций", buttonCongurDustribution);
            int CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);
            double Max = 0, Min = 0, Step = 0;
            double[] ArrayIntervalValue = new double[CountInterval];
            ArrayIntervalValue = SortFromIntervals(DistributionCong(RandomNumbers()), ref Step, ref Min, ref Max);
            BuildChart(ArrayIntervalValue, Step, Min);//построение плотности распределения
            PaintArray(ArrayIntervalValue);//вывод ряда чисел в текстбокс
            BuildChart2(DistributionCong(RandomNumbers()), Step, Min);

        }

        //распределение лехмера
        private void buttonLechmerDistribution_Click(object sender, EventArgs e)
        {
            //блокировка текстбоксов, которые не потребуются для решения этой задачи
            textBoxLeftBorderOne.Enabled = true;
            textBoxRightBorderOne.Enabled = true;
            textBoxLeftBorderTwo.Enabled = false;
            textBoxRightBorderTwo.Enabled = false;
            textBoxLambdaValue.Enabled = false;
            textBoxLambda2.Enabled = false;
            textBoxLambda3.Enabled = false;
            textBoxSmechenieX.Enabled = false;
            textBoxKoefA.Enabled = false;
            textBoxKoefC.Enabled = false;
            textBoxKoefM.Enabled = false;
            ChangeForm(" метода линейной конгруэнтной последовательности Д. Г. Лемера", buttonLechmerDistribution);
            int CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);
            double Max = 0, Min = 0, Step = 0;
            double[] ArrayIntervalValue = new double[CountInterval];
            ArrayIntervalValue = SortFromIntervals(DistributionLehmer(RandomNumbers()), ref Step, ref Min, ref Max);
            BuildChart(ArrayIntervalValue, Step, Min);//построение плотности распределения
            PaintArray(ArrayIntervalValue);//вывод ряда чисел в текстбокс
            BuildChart2(DistributionLehmer(RandomNumbers()), Step, Min);
        }

        // распределение  Симпсона
        private void buttonTriangularDistribution_Click(object sender, EventArgs e)
        {
            //блокировка текстбоксов, которые не потребуются для решения этой задачи
            textBoxLeftBorderOne.Enabled = true;
            textBoxRightBorderOne.Enabled = true;
            textBoxLeftBorderTwo.Enabled = true;
            textBoxRightBorderTwo.Enabled = true;
            textBoxLambdaValue.Enabled = false;
            textBoxLambda2.Enabled = false;
            textBoxLambda3.Enabled = false;
            textBoxSmechenieX.Enabled = false;
            textBoxKoefA.Enabled = false;
            textBoxKoefC.Enabled = false;
            textBoxKoefM.Enabled = false;

            ChangeForm("  распределения Симпсона", buttonTriangularDistribution);
            int CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);
            double Max = 0, Min = 0, Step = 0;
            double[] ArrayIntervalValue = new double[CountInterval];
            ArrayIntervalValue = SortFromIntervals(DistributionTrapeze(RandomNumbers(), RandomNumbers()), ref Step, ref Min, ref Max);
            BuildChart(ArrayIntervalValue, Step, Min);
            PaintArray(ArrayIntervalValue);
            BuildChart2(DistributionTrapeze(RandomNumbers(), RandomNumbers()), Step, Min);

        }

        //нормальное распределение по методичке
        private void buttonNormalDistribution_Click(object sender, EventArgs e)
        {
            //блокировка текстбоксов, которые не потребуются для решения этой задачи
            textBoxLeftBorderOne.Enabled = false;
            textBoxRightBorderOne.Enabled = false;
            textBoxLeftBorderTwo.Enabled = false;
            textBoxRightBorderTwo.Enabled = false;
            textBoxLambdaValue.Enabled = true;
            textBoxLambda2.Enabled = false;
            textBoxLambda3.Enabled = false;
            textBoxSmechenieX.Enabled = false;
            textBoxKoefA.Enabled = false;
            textBoxKoefC.Enabled = false;
            textBoxKoefM.Enabled = false;

            ChangeForm(" нормального распределения", buttonNormalDistribution);
            int CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);
            double Max = 0, Min = 0, Step = 0;
            double[] ArrayIntervalValue = new double[CountInterval];
            ArrayIntervalValue = SortFromIntervals(DistributionNormal(RandomNumbers(), RandomNumbers(), RandomNumbers(), RandomNumbers(), RandomNumbers(), RandomNumbers()), ref Step, ref Min, ref Max);
            BuildChart(ArrayIntervalValue, Step, Min);
            PaintArray(ArrayIntervalValue);
            BuildChart2(DistributionNormal(RandomNumbers(), RandomNumbers(), RandomNumbers(), RandomNumbers(), RandomNumbers(), RandomNumbers()), Step, Min);
        }




        //экспоненциальное распределение
        private void buttonExponentialDistribution_Click(object sender, EventArgs e)
        {
            //блокировка текстбоксов, которые не потребуются для решения этой задачи
            textBoxLeftBorderOne.Enabled = false;
            textBoxRightBorderOne.Enabled = false;
            textBoxLeftBorderTwo.Enabled = false;
            textBoxRightBorderTwo.Enabled = false;
            textBoxLambdaValue.Enabled = true;
            textBoxLambda2.Enabled = false;
            textBoxLambda3.Enabled = false;
            textBoxSmechenieX.Enabled = false;
            textBoxKoefA.Enabled = false;
            textBoxKoefC.Enabled = false;
            textBoxKoefM.Enabled = false;

            ChangeForm(" экспоненциального распределения", buttonExponentialDistribution);
            int CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);
            double Max = 0, Min = 0, Step = 0;
            double[] ArrayIntervalValue = new double[CountInterval];
            ArrayIntervalValue = SortFromIntervals(DistributionExp(RandomNumbers()), ref Step, ref Min, ref Max);
            BuildChart(ArrayIntervalValue, Step, Min);
            PaintArray(ArrayIntervalValue);
            BuildChart2(DistributionExp(RandomNumbers()), Step, Min);

        }

        //гиперэкспоненциальное распределение
        private void buttonHyperexponentiaDlistribution_Click(object sender, EventArgs e)
        {
            //блокировка текстбоксов, которые не потребуются для решения этой задачи
            textBoxLeftBorderOne.Enabled = false;
            textBoxRightBorderOne.Enabled = false;
            textBoxLeftBorderTwo.Enabled = false;
            textBoxRightBorderTwo.Enabled = false;
            textBoxLambdaValue.Enabled = true;
            textBoxLambda2.Enabled = true;
            textBoxLambda3.Enabled = false;
            textBoxSmechenieX.Enabled = true;
            textBoxKoefA.Enabled = false;
            textBoxKoefC.Enabled = false;
            textBoxKoefM.Enabled = false;
            ChangeForm(" гиперэкспоненциального распределения", buttonHyperexponentiaDlistribution);

            int CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);
            double Max = 0, Min = 0, Step = 0;
            double[] ArrayIntervalValue = new double[CountInterval];
            ArrayIntervalValue = SortFromIntervals(DistributionGyperExp(RandomNumbers(), RandomNumbers(), RandomNumbers()), ref Step, ref Min, ref Max);
            BuildChart(ArrayIntervalValue, Step, Min);
            PaintArray(ArrayIntervalValue);
            BuildChart2(DistributionGyperExp(RandomNumbers(), RandomNumbers(), RandomNumbers()), Step, Min);
        }

        //эрлангово распределение
        private void buttonErlangianDistribution_Click(object sender, EventArgs e)
        {

            //блокировка текстбоксов, которые не потребуются для решения этой задачи
            textBoxLeftBorderOne.Enabled = false;
            textBoxRightBorderOne.Enabled = false;
            textBoxLeftBorderTwo.Enabled = false;
            textBoxRightBorderTwo.Enabled = false;
            textBoxLambdaValue.Enabled = true;
            textBoxLambda2.Enabled = false;
            textBoxLambda3.Enabled = true;
            textBoxSmechenieX.Enabled = true;
            textBoxKoefA.Enabled = false;
            textBoxKoefC.Enabled = false;
            textBoxKoefM.Enabled = false;
            ChangeForm(" эрлангова распределения", buttonErlangianDistribution);
            int CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);
            double Max = 0, Min = 0, Step = 0;
            double[] ArrayIntervalValue = new double[CountInterval];
            ArrayIntervalValue = SortFromIntervals(DistributionErlang(RandomNumbers()), ref Step, ref Min, ref Max);
            BuildChart(ArrayIntervalValue, Step, Min);
            PaintArray(ArrayIntervalValue);
            BuildChart2(DistributionErlang(RandomNumbers()), Step, Min);
        }

        //построение гистогораммы
        public void BuildChart(double[] ArrayIntervalValue, double Step, double Min)
        {
            double StepSum = Min;
            chart1.Series[0].Points.Clear();
            for (int i = 0; i < ArrayIntervalValue.Length; i++)
            {
                chart1.Series[0].Points.AddXY(Math.Round(StepSum, 2), ArrayIntervalValue[i]);
                StepSum += Step;
            }
        }

        //построение гистогораммы
        public void BuildChart2(double[] ArrayIntervalValue, double Step, double Min)
        {
            double StepSum = Min;
            chart2.Series[0].Points.Clear();
            for (int i = 0; i < ArrayIntervalValue.Length; i++)
            {
                chart2.Series[0].Points.AddXY(Math.Round(StepSum, 2), ArrayIntervalValue[i]);
                StepSum += Step;
            }
        }

        //изменение информации при нажатии на кнопку
        private void ChangeForm(string text, Button button)
        {
            ChangeColor();
            labelGis.Text = "Гистограмма "+ text;
            button.BackColor = Color.Red;
        }

        //вывести массив функции распределения в текстбокс
        private void PaintArray(double[] arrayInterval)
        {
          
            richTextBoxArray.Clear();
            for (int i =0; i<arrayInterval.Length; i++)
            {
                richTextBoxArray.Text += arrayInterval[i]+" " ;
            }
        }

        //костыль, перекрашивает все кнопки в их изначальные цвета
        private void ChangeColor()
        {
            buttonCongurDustribution.BackColor = Color.SpringGreen;
            buttonLechmerDistribution.BackColor = Color.SpringGreen;
            buttonUniformDistribution.BackColor = Color.SpringGreen;
            buttonTriangularDistribution.BackColor = Color.CornflowerBlue;
            buttonNormalDistribution.BackColor = Color.Khaki;
            buttonExponentialDistribution.BackColor = Color.Lavender;
            buttonHyperexponentiaDlistribution.BackColor = Color.Lavender;
            buttonErlangianDistribution.BackColor = Color.LimeGreen;

        }



        //генерирует псевдослучайные числв, возвращает отсортированный массив и максимальное/минимальное значение в этом массиве
        private double[] RandomNumbers()
        {


            double[] A1 = new double[] { 106, 211, 421, 430, 936, 1366, 171, 859, 419, 967, 141 };
            double[] C1 = new double[] { 1283, 1663, 1663, 2531, 1399, 1283, 11213, 2531, 6173, 3041, 28411 };
            double[] M1 = new double[] { 6075, 7875, 7875, 11979, 6655, 6075, 53125, 11979, 29282, 13306, 134456 };
            double A = Convert.ToInt32(textBoxKoefA.Text), C = Convert.ToInt32(textBoxKoefC.Text), M = Convert.ToInt32(textBoxKoefM.Text);
            int CountElement = Convert.ToInt32(textBoxCountElements.Text), CountInterval = Convert.ToInt32(textBoxCountIntervals.Text);
            double[] arrayRandomSymbol = new double[CountElement];

            double Max = double.MinValue;

            arrayRandomSymbol[0] = ((A + C) % M);//остаток от деления
            for (int i = 1; i < CountElement; i++)
            {
                arrayRandomSymbol[i] = ((A * arrayRandomSymbol[i - 1] + C) % M);
                if (Max < arrayRandomSymbol[i])
                    Max = arrayRandomSymbol[i];
            }


            for (int i = 0; i < CountElement; i++)
            {
                arrayRandomSymbol[i] /= Max;
            }

            ChangeData();
            return arrayRandomSymbol;
        }


        //вывод коеффициентов в текстбоксы
        private void ChangeData()
        {
            double[] A1 = new double[] { 106, 211, 421, 430, 936, 1366, 171, 859, 419, 967, 141 };
            double[] C1 = new double[] { 1283, 1663, 1663, 2531, 1399, 1283, 11213, 2531, 6173, 3041, 28411 };
            double[] M1 = new double[] { 6075, 7875, 7875, 11979, 6655, 6075, 53125, 11979, 29282, 13306, 134456 };

            int index = 0;
            for (int i = 0; i < A1.Length; i++)
            {
                if (A1[i] == (Convert.ToInt32(textBoxKoefA.Text)))
                {
                    index = i;
                    break;
                }
            }

            if (index == A1.Length - 1)
            {
                textBoxKoefC.Text = C1[0].ToString();
                textBoxKoefM.Text = M1[0].ToString();
                textBoxKoefA.Text = A1[0].ToString();

            }
            else
            {
                textBoxKoefA.Text = A1[index + 1].ToString();
                textBoxKoefC.Text = C1[index + 1].ToString();
                textBoxKoefM.Text = M1[index + 1].ToString();
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }


            //пустые нажатия. пока работает, не трогать
        private void label5_Click(object sender, EventArgs e)
        {

        }



        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click_1(object sender, EventArgs e)
        {

        }

        private void label18_Click_2(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click_3(object sender, EventArgs e)
        {

        }

        private void textBoxDisp_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxLambdaValue_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        
        private void textBoxCountElements_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }

        }
    }
}
