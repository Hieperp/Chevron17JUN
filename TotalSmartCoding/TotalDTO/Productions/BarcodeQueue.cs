using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotalBase;
using TotalModel;

namespace TotalDTO.Productions
{
    public class BarcodeQueue<TBarcodeDTO>
        where TBarcodeDTO : BarcodeDTO, IPrimitiveEntity, new()
    {


        private string messageQueueName;

        private int noSubQueue;     //No row
        private int noItemPerSubQueue;

        private bool repeatedSubQueueIndex;
        private bool invertSubQueueIndex;

        private int noItemAdded;        //total item added, use this for Enqueue Method to add item for each sub queue

        private List<List<TBarcodeDTO>> messageSubQueue;  //Important note: List use zero based index

        #region Contructor

        public BarcodeQueue()
            : this(GlobalVariables.NoItemPerCarton() / GlobalVariables.NoSubQueue())
        { }

        /// <summary>
        /// ONLY matchingPackList USE THIS CONTRUCTOR. This contructor beside allow to set NoItemPerSubQueue, it is also allow to set RepeatedSubQueueIndex
        /// </summary>
        /// <param name="noItemPerSubQueue"></param>
        public BarcodeQueue(int noItemPerSubQueue)
        {
            this.NoSubQueue = GlobalVariables.NoSubQueue();
            this.NoItemPerSubQueue = noItemPerSubQueue;

            this.RepeatedSubQueueIndex = false;
            this.invertSubQueueIndex = false;

            this.noItemAdded = 0; //Inititalize

            this.RepeatedSubQueueIndex = GlobalVariables.RepeatedSubQueueIndex();

            if (this.RepeatedSubQueueIndex) this.noItemAdded = 0 - this.NoItemPerSubQueue;
        }


        #endregion Contructor

        #region Public Properties

        public string BarcodeQueueName
        {
            get
            {
                return this.messageQueueName;
            }

            set
            {
                if (value != this.messageQueueName)
                {
                    this.messageQueueName = value;
                }
            }
        }


        public int NoSubQueue
        {
            get
            {
                return this.noSubQueue;
            }
            protected set
            {
                if (this.noSubQueue != value)
                {
                    this.noSubQueue = value;

                    this.messageSubQueue = new List<List<TBarcodeDTO>>();
                    for (int i = 1; i <= this.NoSubQueue; i++)
                    {
                        this.messageSubQueue.Add(new List<TBarcodeDTO>());
                    }
                }
            }

        }


        public int NoItemPerCarton
        {
            get
            {
                return GlobalVariables.NoItemPerCarton(); ;
            }
        }

        private int NoItemPerSubQueue
        {
            get
            {
                return this.noItemPerSubQueue;
            }
            set
            {
                this.noItemPerSubQueue = value;
            }
        }


        private bool RepeatedSubQueueIndex
        {
            get
            {
                return this.repeatedSubQueueIndex;
            }
            set
            {
                this.repeatedSubQueueIndex = value;
            }
        }

        /// <summary>
        /// Return the total number of items in BarcodeQueue
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;

                foreach (List<TBarcodeDTO> subQueue in this.MessageSubQueue)
                {
                    count = count + subQueue.Count;
                }

                return count;
            }
        }

        /// <summary>
        /// Return the number of items of a specific subQueueID
        /// </summary>
        public int GetSubQueueCount(int subQueueID)
        {
            return this.MessageSubQueue[subQueueID].Count;
        }

        private List<List<TBarcodeDTO>> MessageSubQueue
        {
            get { return this.messageSubQueue; }
        }

        #endregion Public Properties

        #region Public Method

        /// <summary>
        /// The SubQueueID of Next Pack when Enqueue
        /// </summary>
        public int NextQueueID
        {       //Sequence Enqueue to each sub queue, this line will return: index : 0, 1, 3, ... NoSubQueue-1 (Comfort with: Zero base index)
            get
            {
                if (!this.RepeatedSubQueueIndex)
                    return (this.noItemAdded / this.NoItemPerSubQueue) % this.NoSubQueue;
                else
                {
                    int nextQueueID = this.noItemAdded < 0 ? 0 : (this.noItemAdded / this.NoItemPerSubQueue) % this.NoSubQueue;
                    if (this.invertSubQueueIndex) nextQueueID = (this.NoSubQueue - 1) - nextQueueID;

                    return nextQueueID;
                }
            }
        }

        /// <summary>
        /// Add messageData by specific messageData.QueueID, without increase noItemAdded by 1
        /// This is used to initialize the Queue
        /// </summary>
        /// <param name="messageData"></param>
        /// <param name="packSubQueueID"></param>
        public void AddPack(TBarcodeDTO messageData)
        {
            if (messageData.QueueID < this.MessageSubQueue.Count)
                this.MessageSubQueue[messageData.QueueID].Add(messageData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageData"></param>
        public void Enqueue(TBarcodeDTO messageData)
        {
            this.MessageSubQueue[this.NextQueueID].Add(messageData);
            this.noItemAdded++; //Note: Always increase noItemAdded by 1 after Enqueue

            if (this.RepeatedSubQueueIndex && this.noItemAdded > 0 && (this.noItemAdded % (this.NoSubQueue * this.NoItemPerSubQueue) == 0)) this.invertSubQueueIndex = !this.invertSubQueueIndex;

        }


        /// <summary>
        /// Dequeue a batch of noItemPerCarton of elements from this Matching Queue, by sequence Dequeue from each sub queue, with index 0, 1, 2, 3, ... NoSubQueue-1 (Comfort with: Zero base index)
        /// </summary>
        /// <returns></returns>
        public BarcodeQueue<TBarcodeDTO> DequeuePack()
        {
            BarcodeQueue<TBarcodeDTO> packInOneCarton = new BarcodeQueue<TBarcodeDTO>();


            foreach (List<TBarcodeDTO> subQueue in this.MessageSubQueue)
            {
                if (packInOneCarton.NoItemPerSubQueue > subQueue.Count) return packInOneCarton; //There is not enough element in this sub queue to dequeue. In this case, return empty
            }


            foreach (List<TBarcodeDTO> subQueue in this.MessageSubQueue)
            {
                for (int i = 0; i < packInOneCarton.NoItemPerSubQueue; i++)
                {
                    if (subQueue.Count > 0) { packInOneCarton.Enqueue(subQueue.ElementAt(0)); subQueue.RemoveAt(0); }//Check subQueue.Count > 0 just for sure, however, we check it already at the begining of this method
                }
            }

            return packInOneCarton;
        }

        public TBarcodeDTO Dequeue(int packID)
        {
            //CẦN PHẢI XEM XÉT
            //foreach (List<TBarcodeDTO> subQueue in this.MessageSubQueue)
            //{
            //    foreach (TBarcodeDTO packData in subQueue)
            //    {
            //        if (packData.PackID == packID)
            //        {
            //            TBarcodeDTO messageData = packData.ShallowClone();
            //            subQueue.Remove(packData);

            //            return messageData;
            //        }
            //    }
            //}
            return null; //Return null if can not find any PackID
        }



        /// <summary>
        /// messageData.QueueID: Will change to new value (new position) after replace
        /// </summary>
        /// <param name="packID"></param>
        /// <param name="messageData"></param>
        /// <returns></returns>
        public bool Replace(int packID, TBarcodeDTO messageData)
        {
            //CẦN PHẢI XEM XÉT
            //foreach (List<TBarcodeDTO> subQueue in this.MessageSubQueue)
            //{
            //    for (int i = 0; i < subQueue.Count; i++)
            //    {
            //        if (subQueue[i].PackID == packID)
            //        {
            //            messageData.QueueID = subQueue[i].QueueID;
            //            subQueue[i] = messageData;
            //            return true;
            //        }
            //    }
            //}
            return false; //Return false if can not find any PackID
        }


        public virtual DataTable GetAllElements()
        {
            int maxSubQueueCount = 0;
            DataTable dataTableAllElements = new DataTable("AllElements");

            foreach (List<TBarcodeDTO> subQueue in this.MessageSubQueue)
            {
                maxSubQueueCount = maxSubQueueCount <= subQueue.Count ? subQueue.Count : maxSubQueueCount;
            }

            for (int i = 0; i < maxSubQueueCount; i++)//Make a table with number of column equal to maxSubQueueCount
            {
                dataTableAllElements.Columns.Add((i < 9 ? " " : "") + (i + 1).ToString().Trim());
            }

            foreach (List<TBarcodeDTO> subQueue in this.MessageSubQueue)
            {
                DataRow dataRow = dataTableAllElements.NewRow(); //add row for each sub queue
                for (int i = 0; i < maxSubQueueCount; i++)
                {//Zero base queue element
                    //CẦN PHẢI XEM XÉT::  if (subQueue.Count > i) dataRow[i] = subQueue.ElementAt<TBarcodeDTO>(i).Code + GlobalVariables.doubleTabChar + GlobalVariables.doubleTabChar + subQueue.ElementAt<TBarcodeDTO>(i).PackID; //Fill data row
                }
                dataTableAllElements.Rows.Add(dataRow);
            }

            return dataTableAllElements;

        }

        /// <summary>
        /// Get element at index of whole queue. Zero base index. Return Null if index out of range
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TBarcodeDTO ElementAt(int index)
        {
            if (index < this.Count)  //Zero base index
            {
                int findIndex = -1;
                foreach (List<TBarcodeDTO> subQueue in this.MessageSubQueue)
                {
                    for (int i = 0; i < subQueue.Count; i++)
                    {
                        findIndex++;
                        if (findIndex == index) return subQueue.ElementAt(i);
                    }
                }
            }
            return null; //Return Null if index out of range
        }

        /// <summary>
        /// Get element at index of subQueueID. Zero base index. Return Null if index out of range
        /// </summary>
        /// <param name="subQueueID"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public TBarcodeDTO ElementAt(int subQueueID, int index)
        {
            if (subQueueID >= 0 && subQueueID < this.NoSubQueue && index >= 0 && index < this.MessageSubQueue[subQueueID].Count)  //Zero base index
            {
                return this.MessageSubQueue[subQueueID].ElementAt(index);
            }
            return null; //Return Null if index out of range
        }

        #endregion Public Method



        public string EntityIDs { get { return string.Join(",", this.MessageSubQueue.Select(q => string.Join(",", q.Select(l => l.GetID().ToString()).ToArray())).ToArray()); } }


    }
}
