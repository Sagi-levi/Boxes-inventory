using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using BLService.InnerData;
using System.Threading;

namespace BLService
{

    public class Manager
    {
        const int SPLIT_MAX = 4;
        private BST<DataX> _mainTree = new BST<DataX>();
        private DoubleLinkedList<BoxByTime> _linkedList = new DoubleLinkedList<BoxByTime>();
        private int _maxItemsPerBox;
        private int _requirementsReorderMinAmount; 
        private TimeSpan _checkPeriod; 
        private TimeSpan _expirationDate; 
        private ICommunicator _communicator;
        private Timer _timer;

        public Manager(int maxItemsPerBox, int requirementsReOrderMinAmount, TimeSpan checkPeriod,
            TimeSpan expirationDate, ICommunicator communicator)
        {
            _mainTree = new BST<DataX>();
            _linkedList = new DoubleLinkedList<BoxByTime>();
            this._maxItemsPerBox = maxItemsPerBox;
            this._requirementsReorderMinAmount = requirementsReOrderMinAmount;
            this._checkPeriod = checkPeriod;
            this._expirationDate = expirationDate;
            _communicator = communicator;
            _timer = new Timer( RemoveUnPopularItems, null, _expirationDate, _checkPeriod);
        }
        /// <summary>
        /// this method using a status method and use the result to insert the new box into the memory correctly
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="amount"></param>
        public void Add(double x, double y, int amount)
        {
            DataX dataX;
            DataY dataY;
            ExistensStutus stutus = IsBoxExist(x, y, out dataX, out dataY);

            BoxByTime box = new BoxByTime(x, y);

            switch (stutus)
            {
                case ExistensStutus.BothExist:
                    if (_maxItemsPerBox < amount + dataY.Amount)
                    {
                        dataY.Amount = _maxItemsPerBox;
                        _communicator.OnMessage($"adding amount was bigger them max capacity so we added" +
                            $" only :{_maxItemsPerBox} boxes");
                    }
                    else
                    {
                        dataY.Amount += amount;
                    }
                    break;

                case ExistensStutus.XExist:
                    dataY = ManageDataY(y, amount, box);
                    dataX.YTree.Add(dataY);
                    break;

                case ExistensStutus.NotExist:
                    dataY = ManageDataY(y, amount, box);

                    dataX = new DataX(x);
                    _mainTree.Add(dataX);
                    dataX.YTree.Add(dataY);
                    break;
            }
            _communicator.OnMessage("adding went seccsesfully");

        }
        /// <summary>
        /// manageing all the DataY info by the amount he gets from the user
        /// </summary>
        /// <param name="y"></param>
        /// <param name="amount"></param>
        /// <param name="box"></param>
        /// <returns>DataY with full properties</returns>
        private DataY ManageDataY(double y, int amount, BoxByTime box)
        {
            DataY dataY;
            if (_maxItemsPerBox < amount)   //manage trees
            {
                dataY = new DataY(y, _maxItemsPerBox);

                _linkedList.AddLast(box);     //manage LinkedList
                dataY.Node = _linkedList.End;
                _communicator.OnMessage($"adding amount was bigger them max capacity so we added" +
                    $" only :{_maxItemsPerBox} boxes");
            }
            else
            {
                dataY = new DataY(y, amount);
                _linkedList.AddLast(box);     //manage LinkedList
                dataY.Node = _linkedList.End;
            }

            return dataY;
        }

        public void Info(double x, double y)
        {
            DataX dataX;
            DataY dataY;
            IsBoxExist(x, y, out dataX, out dataY);
            if (dataX != null && dataY != null)
            {
                _communicator.OnMessage($"the box x={x}, y={y} have {dataY.Amount} in stock\n" +
                        $" and her last purchase/stock updating was at {dataY.Node.Data.Date}") ;
                return;
            }
            _communicator.OnMessage("the box isnt exist");
        }
        /// <summary>
        /// gets a demand for boxes and asks the user if he wants to buy it
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="amount"></param>
        public void Buy(double x, double y, int amount)
        {
            int splitMax = SPLIT_MAX;
            List<Box> boxes = new List<Box>();
            Box box;
            DataX currentDataX;
            DataY currentDataY;

            _mainTree.SearchEqualOrBigger(new DataX(x), out currentDataX);
            if (currentDataX == null)
            {
                _communicator.OnMessage("there is no match mr.white");
                return;
            }

            while (amount != 0 && splitMax != 0 && currentDataX != null)
            {
                currentDataX.YTree.SearchEqualOrBigger(new DataY(y, 1), out currentDataY);
                while (currentDataY != null)
                {
                    splitMax -= 1;
                    if (amount <= currentDataY.Amount)
                    {
                        box = new Box(currentDataX, currentDataY, amount); //create a boxForPurches and zeroing the amount
                        boxes.Add(box);
                        amount = 0;
                        break;
                    }
                    else
                    {
                        box = new Box(currentDataX, currentDataY, currentDataY.Amount);   //create a boxForPurches with all stock
                        boxes.Add(box);
                        amount -= currentDataY.Amount;
                        currentDataX.YTree.SearchNextBigger(currentDataY, out currentDataY);
                    }
                }
                if (amount > 0) _mainTree.SearchNextBigger(currentDataX, out currentDataX);
            }
            if (SPLIT_MAX == 0 || currentDataX == null)
            {
                _communicator.OnMessage("sorry but we didnt found a match for you \ntry again soon!");
                return;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var item in boxes)
            {
                sb.Append(item.ToString() + " ");
            }
            sb.Append("are you want to buy it?");
            if (_communicator.OnQuestion(sb.ToString()))
            {
                for (int i = 0; i <= boxes.Count - 2; i++)
                {
                    if (boxes[i].DataX.YTree.IsDepthIsOne())
                    {
                        _mainTree.Delete(boxes[i].DataX);
                    }
                    else boxes[i].DataX.YTree.Delete(boxes[i].DataY);
                    _linkedList.RemoveByNode(boxes[i].DataY.Node);
                }
                //delete only the leftovers of amount and not the hole object
                boxes[boxes.Count - 1].DataY.Amount -= boxes[boxes.Count - 1].AmountToPurchse;
                _linkedList.RePositeToEnd(boxes[boxes.Count - 1].DataY.Node);
                boxes[boxes.Count - 1].DataY.Node.Data.Date = DateTime.Now;
                if (boxes[boxes.Count - 1].DataY.Amount < _requirementsReorderMinAmount)
                {
                    _communicator.OnMessage($"you need to reorder this box: {boxes[boxes.Count - 1].DataY.Node.Data}");
                }
            }
        }

        /// <summary>
        /// check the enum type for understanding how to handle the adding process
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dataX"></param>
        /// <param name="dataY"></param>
        /// <returns></returns>
        private ExistensStutus IsBoxExist(double x, double y, out DataX dataX, out DataY dataY)
        {
            DataX existX;
            DataY existY;
            if (_mainTree.Search(new DataX(x), out existX) && existX.YTree.Search(new DataY(y, default), out existY))//both exist
            {
                dataX = existX;
                dataY = existY;
                return ExistensStutus.BothExist;
            }
            if (_mainTree.Search(new DataX(x), out existX)) //only xtree exist and y not
            {
                dataX = existX;
                dataY = default;
                return ExistensStutus.XExist;
            }
            else  //both not exist
            {
                dataY = default;
                dataX = default;
                return ExistensStutus.NotExist;
            }
        }
        /// <summary>
        /// delets the unpopoular boxes By limit of no demand time
        /// </summary>
        /// <param name="state"></param>
        private void RemoveUnPopularItems(object state)
        {
            if (_linkedList.IsEmpty())
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            while ((DateTime.Now - _linkedList.Start.Data.Date) > _expirationDate)
            {
                BoxByTime boxRemoved;
                _linkedList.RemoveFirst(out boxRemoved);
                sb.Append(boxRemoved.ToString());
                DataX currentDataX;
                _mainTree.Search(new DataX(boxRemoved.X), out currentDataX);
                if (currentDataX.YTree.IsDepthIsOne())
                {
                    _mainTree.Delete(currentDataX);
                }
                else
                {
                    currentDataX.YTree.Delete(new DataY(boxRemoved.Y, 1));
                }
                if (_linkedList.IsEmpty()) break;
            }
            if (sb.Length>1) _communicator.OnMessage($"the box,{sb} was deleted from memory");
        }
    }
}
