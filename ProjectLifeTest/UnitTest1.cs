using System;
using ProjectLife;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProjectLifeTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetRectangle_isAlive_4resolution()
        {
            bool isAlive = true;
            int resolution = 4;
            Rectangle expected = new Rectangle();
            expected.Height = resolution;
            expected.Width = resolution;
            expected.Fill = Brushes.Green;
            expected.Stroke = Brushes.Green;

            Rectangle actual = CalcForProjectLife.GetRectangle(isAlive, resolution);
            Assert.AreEqual(expected.ActualHeight, actual.ActualHeight);
            Assert.AreEqual(expected.ActualWidth, actual.ActualWidth);
            Assert.AreEqual(expected.Fill, actual.Fill);
            Assert.AreEqual(expected.Stroke, actual.Stroke);
        }

        [TestMethod]
        public void GetRectangle_isDead_6resolution()
        {
            bool isAlive = false;
            int resolution = 6;
            Rectangle expected = new Rectangle();
            expected.Height = resolution;
            expected.Width = resolution;
            expected.Fill = Brushes.Black;
            expected.Stroke = Brushes.Black;

            Rectangle actual = CalcForProjectLife.GetRectangle(isAlive, resolution);
            Assert.AreEqual(expected.ActualHeight, actual.ActualHeight);
            Assert.AreEqual(expected.ActualWidth, actual.ActualWidth);
            Assert.AreEqual(expected.Fill, actual.Fill);
            Assert.AreEqual(expected.Stroke, actual.Stroke);
        }

        [TestMethod]
        public void CheckIsAlive_1neighbour_isAlive()
        {
            bool isAlive = true;
            int neighbours = 1;

            bool expected = false;
            bool actual = CalcForProjectLife.CheckIsAlive(neighbours, isAlive);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIsAlive_2neighbour_isAlive()
        {
            bool isAlive = true;
            int neighbours = 2;

            bool expected = true;
            bool actual = CalcForProjectLife.CheckIsAlive(neighbours, isAlive);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIsAlive_3neighbour_isAlive()
        {
            bool isAlive = true;
            int neighbours = 3;

            bool expected = true;
            bool actual = CalcForProjectLife.CheckIsAlive(neighbours, isAlive);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIsAlive_5neighbour_isAlive()
        {
            bool isAlive = true;
            int neighbours = 5;

            bool expected = false;
            bool actual = CalcForProjectLife.CheckIsAlive(neighbours, isAlive);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIsAlive_1neighbour_isDead()
        {
            bool isAlive = false;
            int neighbours = 1;

            bool expected = false;
            bool actual = CalcForProjectLife.CheckIsAlive(neighbours, isAlive);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIsAlive_2neighbour_isDead()
        {
            bool isAlive = false;
            int neighbours = 2;

            bool expected = false;
            bool actual = CalcForProjectLife.CheckIsAlive(neighbours, isAlive);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIsAlive_3neighbour_isDead()
        {
            bool isAlive = false;
            int neighbours = 3;

            bool expected = true;
            bool actual = CalcForProjectLife.CheckIsAlive(neighbours, isAlive);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIsAlive_5neighbour_isDead()
        {
            bool isAlive = false;
            int neighbours = 5;

            bool expected = false;
            bool actual = CalcForProjectLife.CheckIsAlive(neighbours, isAlive);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckNeighbours_0x0_withBorders_0isAlive_0expected()
        {
            int columns = 4,
                rows = 4,
                x = 0,
                y = 0;
            bool[,] arr = new bool[4, 4]
                { 
                    { false, false, false, false },
                    { false, false, false, false },
                    { false, false, false, false },
                    { false, false, false, false }
                };
            bool universeIsLocked = false;

            int expected = 0;
            int actual = CalcForProjectLife.CheckNeighbours(arr, universeIsLocked, x, y, columns, rows);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckNeighbours_0x0_withBorders_5isAlive_1expected()
        {
            int columns = 4,
                rows = 4,
                x = 0,
                y = 0;
            bool[,] arr = new bool[4, 4]
                {
                    { false, true, true, false },
                    { false, false, false, false },
                    { false, false, true, false },
                    { true, false, false, true }
                };
            bool universeIsLocked = false;

            int expected = 1;
            int actual = CalcForProjectLife.CheckNeighbours(arr, universeIsLocked, x, y, columns, rows);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckNeighbours_0x0_withBorders_5isAlive_3expected()
        {
            int columns = 4,
                rows = 4,
                x = 0,
                y = 0;
            bool[,] arr = new bool[4, 4]
                {
                    { false, true, true, false },
                    { true, true, false, false },
                    { false, false, false, false },
                    { true, false, false, false }
                };
            bool universeIsLocked = false;

            int expected = 3;
            int actual = CalcForProjectLife.CheckNeighbours(arr, universeIsLocked, x, y, columns, rows);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckNeighbours_aliveItself_0x0_withBorders_6isAlive_3expected()
        {
            int columns = 4,
                rows = 4,
                x = 0,
                y = 0;
            bool[,] arr = new bool[4, 4]
                {
                    { true, true, true, false },
                    { true, true, false, false },
                    { false, false, false, false },
                    { true, false, false, false }
                };
            bool universeIsLocked = false;

            int expected = 3;
            int actual = CalcForProjectLife.CheckNeighbours(arr, universeIsLocked, x, y, columns, rows);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckNeighbours_0x0_withoutBorders_5isAlive_3expected()
        {
            int columns = 4,
                rows = 4,
                x = 0,
                y = 0;
            bool[,] arr = new bool[4, 4]
                {
                    { false, true, false, true },
                    { false, true, false, false },
                    { false, false, false, true },
                    { false, false, true, false }
                };
            bool universeIsLocked = true;

            int expected = 3;
            int actual = CalcForProjectLife.CheckNeighbours(arr, universeIsLocked, x, y, columns, rows);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckNeighbours_0x0_withoutBorders_5isAlive_5expected()
        {
            int columns = 4,
                rows = 4,
                x = 0,
                y = 0;
            bool[,] arr = new bool[4, 4]
                {
                    { false, true, false, true },
                    { false, true, false, true },
                    { false, false, false, false },
                    { true, false, false, false }
                };
            bool universeIsLocked = true;

            int expected = 5;
            int actual = CalcForProjectLife.CheckNeighbours(arr, universeIsLocked, x, y, columns, rows);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckNeighbours_aliveItself_0x0_withoutBorders_5isAlive_4expected()
        {
            int columns = 4,
                rows = 4,
                x = 0,
                y = 0;
            bool[,] arr = new bool[4, 4]
                {
                    { true, true, false, false },
                    { false, true, false, true },
                    { false, false, false, false },
                    { true, false, false, false }
                };
            bool universeIsLocked = true;

            int expected = 4;
            int actual = CalcForProjectLife.CheckNeighbours(arr, universeIsLocked, x, y, columns, rows);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AnalizeGame_sameArrays()
        {
            int columns = 4,
                rows = 4;
            bool[,] arr1 = new bool[4, 4]
                {
                    { true, true, false, false },
                    { false, true, false, true },
                    { false, false, false, false },
                    { true, false, false, false }
                };
            bool[,] arr2 = new bool[4, 4]
                {
                    { true, true, false, false },
                    { false, true, false, true },
                    { false, false, false, false },
                    { true, false, false, false }
                };

            bool expected = true;
            bool actual = CalcForProjectLife.AnalizeGame(arr1, arr2, columns, rows);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AnalizeGame_differentArrays()
        {
            int columns = 4,
                rows = 4;
            bool[,] arr1 = new bool[4, 4]
                {
                    { true, false, false, false },
                    { false, true, false, true },
                    { false, false, false, false },
                    { true, false, false, false }
                };
            bool[,] arr2 = new bool[4, 4]
                {
                    { true, true, false, false },
                    { true, true, false, true },
                    { false, false, false, false },
                    { true, false, false, false }
                };

            bool expected = false;
            bool actual = CalcForProjectLife.AnalizeGame(arr1, arr2, columns, rows);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ArrayToString_test1()
        {
            int columns = 4,
                rows = 4;
            bool[,] arr = new bool[4, 4]
                {
                    { true, false, false, false },
                    { false, true, false, true },
                    { false, false, false, false },
                    { true, false, false, false }
                };

            string expected = "1000010100001000";
            string actual = CalcForProjectLife.ArrayToString(arr, columns, rows);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ArrayToString_test2()
        {
            int columns = 4,
                rows = 4;
            bool[,] arr = new bool[4, 4]
                {
                    { true, true, true, true },
                    { false, false, false, false },
                    { true, true, true, true },
                    { false, false, false, false }
                };

            string expected = "1111000011110000";
            string actual = CalcForProjectLife.ArrayToString(arr, columns, rows);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StringToArray_test1()
        {
            int columns = 4,
                rows = 4;
            string str = "1000010100001000";

            bool[,] expected = new bool[4, 4]
                {
                    { true, false, false, false },
                    { false, true, false, true },
                    { false, false, false, false },
                    { true, false, false, false }
                };
            bool[,] actual = CalcForProjectLife.StringToArray(str, columns, rows);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StringToArray_test2()
        {
            int columns = 4,
                rows = 4;
            string str = "1111000011110000";

            bool[,] expected = new bool[4, 4]
                {
                    { true, true, true, true },
                    { false, false, false, false },
                    { true, true, true, true },
                    { false, false, false, false }
                };
            bool[,] actual = CalcForProjectLife.StringToArray(str, columns, rows);

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
