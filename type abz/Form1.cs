using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace type_abz
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        short rtn;
        public const short axisA = 1;
        public const short axisZ = 2;
        public const short axisB = 3;
        short Card_Num0 = 1;
        int x;//Z向总长-打印机初始下降位置
        int Y;//A轴回到清洗盒的运动规划位置
        int pos1, pos2, pos3, pos4, pos5;
        int i = 0, sum, n;
        int Xi,Xb,L;//机械零点,清洗槽机械零点，L清洗预留距离
        short r;
        int sts;
        int pos = 4267, posz, posb = 3333;//posz第一次轴清洗运动距离
        int pPosz, pPosa, pPosb;
        int vel = 2;//若轴速度不同，需改动
        double pValue;
        uint pClock;
        int material_nums;
        private void button1_Click(object sender, EventArgs e)
        {
            rtn = gts.mc.GT_Open(Card_Num0, 0, 1);
            rtn = gts.mc.GT_Reset(Card_Num0);
            rtn = gts.mc.GT_LoadConfig(Card_Num0, "GTS800.cfg");
            rtn = gts.mc.GT_ClrSts(Card_Num0, axisA, 8);


            rtn = gts.mc.GT_PrfTrap(Card_Num0, axisA);//设置a轴为点位运动模式
            gts.mc.TTrapPrm trapPrm;
            rtn = gts.mc.GT_GetTrapPrm(Card_Num0, axisA, out trapPrm);//读取轴点位运动参数
            trapPrm.acc = 0.25;
            trapPrm.dec = 0.1;
            trapPrm.smoothTime = 10;//根据实际情况改动
            rtn = gts.mc.GT_SetTrapPrm(Card_Num0, axisA, ref trapPrm);//设置轴点位运动参数
            rtn = gts.mc.GT_SetVel(Card_Num0, axisA, vel);//设置轴目标速度

            rtn = gts.mc.GT_SetPos(Card_Num0, axisZ, posb);//开始刮料，设置目标位置
            rtn = gts.mc.GT_PrfTrap(Card_Num0, axisB);//设置轴为点位运动模式
            gts.mc.TTrapPrm trapPrmB;
            rtn = gts.mc.GT_GetTrapPrm(Card_Num0, axisB, out trapPrmB);//获取点位运动参数
            trapPrmB.acc = 0.25;
            trapPrmB.acc = 0.1;
            trapPrmB.smoothTime = 2;
            rtn = gts.mc.GT_SetTrapPrm(Card_Num0, axisB, ref trapPrmB);//设置点位运动参数

               
            rtn = gts.mc.GT_PrfTrap(Card_Num0, axisZ);//设置z轴为点位运动模式
            gts.mc.TTrapPrm trapPrmz;
            rtn = gts.mc.GT_GetTrapPrm(Card_Num0, axisZ, out trapPrmz);
            trapPrmz.acc = 0.25;
            trapPrmz.dec = 0.1;
            trapPrmz.smoothTime = 10;
            rtn = gts.mc.GT_SetTrapPrm(Card_Num0, axisZ, ref trapPrmz);
            rtn = gts.mc.GT_SetVel(Card_Num0, axisZ, vel);
            //z轴下降一定高度，需要高于清洗料槽


            while (n<=1900)//条件改为层切获得的最大层号curLayer
            {
                n++;
                int home ;
                int m_nMaterial=5;
                string[] m_nMaterialName = new string[] { };//材料名称组
                int[] m_MaterialEnable = new int[] { };//材料使能
               // m_MaterialEnable = 1;

                for (int i = 0; i < m_MaterialEnable.Length; i++)
                {
                    if (m_MaterialEnable[i]!=0 )//m_nMaterial使能应为判断条件,如果标志位不为0，执行，位0，则取下一位继续判断
                    {
                        //Console.WriteLine(m_nMaterialName[i]);
                        //打印
                        //取使能对应的材料名称
                        //rtn = gts.mc.GT_SetPos(Card_Num0, axisA, -pos);//设置A轴目标位置,回到零位（转到清洗）
                        //rtn = gts.mc.GT_Update(Card_Num0, 1);//启动a轴运动
                        //rtn = gts.mc.GT_SetPos(Card_Num0, axisB, posb);//设置轴位置+3333
                        //rtn = gts.mc.GT_Update(Card_Num0, 5);//启动b轴运动
                        ////rtn = gts.mc.GT_SetPos(Card_Num0, axisZ, posz);
                        //rtn = gts.mc.GT_SetPos(Card_Num0, 1, posz-(n*x));//设置目标位置
                        //rtn = gts.mc.GT_Update(Card_Num0, 3);//启动z轴运动
                        ////清洗成型材料
                        //rtn = gts.mc.GT_SetDoBit(Card_Num0, 12, 1, 1);//设置数字IO输出状态
                        //rtn = gts.mc.GT_SetDoBitReverse(Card_Num0, 12, 1, 0, 100);//使数字输出信号输出100*250us=25ms时间宽度的负脉冲
                        //System.Threading.Thread.Sleep(2000);  //延时2秒
                        ////z轴提起
                        //rtn = gts.mc.GT_SetPos(Card_Num0, axisZ, -posz+(n*x));
                        //rtn = gts.mc.GT_SetPos(Card_Num0, 1, posz);//设置目标位置
                        //rtn = gts.mc.GT_Update(Card_Num0, 3);//启动z轴运动,
                        home = i;
                        pos1 = -pos;
                        pos2 = -2 * pos;
                        pos3 = -3 * pos;
                        pos4 = -4 * pos;
                        pos5 = -5 * pos;
                        short r = (short)i;//将int类型的i强制转换为short r
                        switch (home)
                        {
                            case 0:
                                rtn = gts.mc.GT_SetPos(Card_Num0, axisA, pos);//设置轴目标位置,清洗槽左，第一个
                                rtn = gts.mc.GT_Update(Card_Num0, 1);//启动轴运动
                                r = 1;
                            Xi = 1;//根据数据类型重新定义Xi，且每个轴位置不同
                            Y = pos1;
                                break;
                            case 1:
                                rtn = gts.mc.GT_SetPos(Card_Num0, axisA, 2 * pos);//设置轴目标位置第二个
                                rtn = gts.mc.GT_Update(Card_Num0, 1);//启动轴运动
                                r = 2;
                            Y =  pos2;
                                break;
                            case 2:
                                rtn = gts.mc.GT_SetPos(Card_Num0, axisA, 3 * pos);//设置轴目标位置，第三个
                                    rtn = gts.mc.GT_Update(Card_Num0, 1);//启动轴运动
                                    r = 3;
                                Y =pos3;
                                    break;
                                case 3:
                                    rtn = gts.mc.GT_SetPos(Card_Num0, axisA, 4 * pos);//设置轴目标位置，第四个
                                    rtn = gts.mc.GT_Update(Card_Num0, 1);//启动轴运动
                                    r = 4;
                                Y = pos4;
                                    break;
                                case 4:
                                    rtn = gts.mc.GT_SetPos(Card_Num0, axisA, 5 * pos);//设置轴目标位置，第五个
                                    rtn = gts.mc.GT_Update(Card_Num0, 1);//启动轴运动
                                    r = 5;
                                Y = pos5;
                                    break;
                        }
                        rtn = gts.mc.GT_SetDoBit(Card_Num0, 12, this.r, 1);//电磁阀工作需要，设定对应的电磁阀工作 //对应电磁阀开启

                        

                        posb = 3333;
                        rtn = gts.mc.GT_SetPos(Card_Num0, axisB, posb);//设置B轴位置为3333                            
                        rtn = gts.mc.GT_Update(Card_Num0, 5);//启动b轴运动





                        //z轴下降打印位
                        posz = x + Xi - n;
                        rtn = gts.mc.GT_SetPos(Card_Num0, axisZ, posz);
                        rtn = gts.mc.GT_SetPos(Card_Num0, 1, posz);//设置目标位置
                        rtn = gts.mc.GT_Update(Card_Num0, 3);//启动z轴运动,

                        //到位检测后，打印

                        //打印


                        //打印完成后，打印台提起
                        posz = -(x + Xi - n);
                        rtn = gts.mc.GT_SetPos(Card_Num0, axisZ, posz);
                        rtn = gts.mc.GT_SetPos(Card_Num0, 1, posz);//设置目标位置
                        rtn = gts.mc.GT_Update(Card_Num0, 3);//启动z轴运动,

                        //A轴转动至清洗盒
                        rtn = gts.mc.GT_SetPos(Card_Num0,axisA,Y);
                        rtn = gts.mc.GT_Update(Card_Num0,1);
                        //B轴刮料
                        posb = -3333;
                        rtn = gts.mc.GT_SetPos(Card_Num0, axisB, posb);//设置轴位置-3333
                        rtn = gts.mc.GT_Update(Card_Num0, 5);//启动b轴运动
                        //Z轴下降
                        posz = x + Xb - L - n;
                        rtn = gts.mc.GT_SetPos(Card_Num0, 1, posz);//设置目标位置
                        rtn = gts.mc.GT_Update(Card_Num0, 3);//启动z轴运动,
                        
                        //清洗，开启对应电磁阀
                        rtn = gts.mc.GT_SetDoBit(Card_Num0, 12, 6, 1);//设置数字IO输出状态
                        rtn = gts.mc.GT_SetDoBitReverse(Card_Num0, 12, 1, 0, 100);//使数字输出信号输出100*250us=25ms时间宽度的负脉冲

                        //Z轴上升
                        posz = -(x + Xb - L - n);
                        rtn = gts.mc.GT_SetPos(Card_Num0, 1, posz);//设置目标位置
                        rtn = gts.mc.GT_Update(Card_Num0, 3);//启动z轴运动,
                        
                    }

                }
                posz = n;
                rtn = gts.mc.GT_SetPos(Card_Num0, 1, posz);//设置目标位置
                rtn = gts.mc.GT_Update(Card_Num0, 3);//启动z轴运动,
                //Z轴上升-N，N为当前材料厚度
            }      
            //完成打印工作后，电机上升到一定高度，便于取下打印台
 
        }//B轴位置未完成，光机部分未完成，数据接收未完成。（Z轴位置已完成）
    }
}
