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
    public interface IShallowClone<T>
    {
        T ShallowClone();
    }

    public class BarcodeQueue<TBarcodeDTO>
        where TBarcodeDTO : BarcodeDTO, IPrimitiveEntity, IShallowClone<TBarcodeDTO>, new()
    {

        /// <summary>
        /// Number of item per whole package: Pack per carton, carton per pallet
        /// This property should be set right after change commodity by setting function
        /// </summary>
        public int ItemPerSet { get; set; }         //!!! carton per palllet?? THUC TE: PACK PER CARTON: TUY THEO SAN PHAM => CAN PHAI XEM XET: EMPTY AL QUEUE: IN ORDEER TO CHANGE COMMODITY!!!
        //PHAI HET SU CHU Y: ItemPerSet




        /// <summary>
        /// This property is used to count number of times the Queueset data is sent to Zebra printer. 
        /// By now, this is used to control how the pallet label is printed by Zebra (Print the cartonsetQueue)
        /// At the initialize of cartonsetQueue, this property will be zero. The software will automatical print for the first time. Then, user may manual re-print if needed
        /// </summary>
        public int SendtoPrintCount { get; set; }




        //This is turn to TRUE for processing the last set: TO ALLOW TO MAKE A PARTILAL SET. IT MEANS: THE PENDING Item IS LESS THAN THE ItemPerSet.
        public bool LastsetProcessing { get; set; }








        private int itemPerSubQueue { get; set; }
        private bool repeatSubQueueIndex { get; set; }
        private bool invertSubQueueIndex { get; set; }

        private int noItemAdded;        //total item added, use this for Enqueue Method to add item for each sub queue

        private List<List<TBarcodeDTO>> list2DBarcode;  //Important note: List use zero based index 

        #region Contructor

        //public BarcodeQueue()
        //    : this(GlobalVariables.NoItemPerCarton() / GlobalVariables.NoSubQueue())
        //{ }

        ///// <summary>
        ///// ONLY matchingPackList USE THIS CONTRUCTOR. This contructor beside allow to set NoItemPerSubQueue, it is also allow to set RepeatedSubQueueIndex
        ///// </summary>
        ///// <param name="itemPerSubQueue"></param>
        //public BarcodeQueue(int itemPerSubQueue)
        //{
        //    this.SubQueueCount = GlobalVariables.NoSubQueue();
        //    this.ItemPerSubQueue = itemPerSubQueue;


        //    this.NoItemAdded = 0; //Inititalize

        //    this.RepeatedSubQueueIndex = GlobalVariables.RepeatedSubQueueIndex();

        //    if (this.RepeatedSubQueueIndex) this.NoItemAdded = 0 - this.ItemPerSubQueue;
        //}


        public BarcodeQueue()
            : this(1, 1, false)
        { }

        public BarcodeQueue(int noSubQueue, int itemPerSubQueue, bool repeatSubQueueIndex)
        {
            if (noSubQueue > 0)
            {
                this.NoSubQueue = noSubQueue;

                this.itemPerSubQueue = itemPerSubQueue;

                this.repeatSubQueueIndex = repeatSubQueueIndex;

                if (this.repeatSubQueueIndex) this.noItemAdded = 0 - this.itemPerSubQueue;//SHOULD CHECK AGAIN WHEN this.repeatSubQueueIndex. HAVE NOT CHECK YET FOR CHEVRON
            }
            else
                throw new Exception("Invalid queue!");

        }

        #endregion Contructor





        #region Public Properties

        public int NoSubQueue { 
            get { return this.list2DBarcode.Count; }
            set {  
                if  (this.list2DBarcode == null || this.list2DBarcode.Count != value )
                {
                    this.list2DBarcode = new List<List<TBarcodeDTO>>();
                    for (int i = 1; i <= value; i++)
                    {
                        this.list2DBarcode.Add(new List<TBarcodeDTO>());
                    }
                }
            }
        }

        /// <summary>
        /// Return the total number of items in BarcodeQueue
        /// </summary>
        public int Count { get { int count = 0; this.list2DBarcode.Each(e => count += e.Count); return count; } }
        /// <summary>
        /// Return the number of items of a specific subQueueID
        /// </summary>
        public int GetSubQueueCount(int subQueueID)
        {
            return this.list2DBarcode[subQueueID].Count;
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
                if (!this.repeatSubQueueIndex)
                    return (this.noItemAdded / this.itemPerSubQueue) % this.NoSubQueue;
                else
                {
                    int nextQueueID = this.noItemAdded < 0 ? 0 : (this.noItemAdded / this.itemPerSubQueue) % this.NoSubQueue;
                    if (this.invertSubQueueIndex) nextQueueID = (this.NoSubQueue - 1) - nextQueueID;

                    return nextQueueID;
                }
            }
        }

        public string NextQueueDescription
        {
            get { return "[" + ((this.noItemAdded % this.itemPerSubQueue) + 1).ToString() + "/" + (this.NextQueueID + 1).ToString() + "]"; }
        }

        public void ResetNextQueueID() { if (this.repeatSubQueueIndex) this.noItemAdded = 0 - this.itemPerSubQueue; else this.noItemAdded = 0; } //SHOULD CHECK AGAIN WHEN this.repeatSubQueueIndex. HAVE NOT CHECK YET FOR CHEVRON

        /// <summary>
        /// Add messageData by specific messageData.QueueID, without increase noItemAdded by 1
        /// This is used to initialize the Queue
        /// </summary>
        /// <param name="messageData"></param>
        /// <param name="packSubQueueID"></param>
        public void AddPack(TBarcodeDTO messageData) //CAN PHAI XEM XET LAI CHO NAY: GOM CHUNG VOI Enqueue. that ra: cai nay van co tac dung, nham muc dich reset autopacker, reallocate
        {
            if (messageData.QueueID < this.list2DBarcode.Count)
                this.list2DBarcode[messageData.QueueID].Add(messageData);
        }


        public void Enqueue(TBarcodeDTO messageData)
        {
            this.Enqueue(messageData, true);
        }
        /// <summary>
        /// autoEnqueue = true when Enqueue by Scanner (ThreadRoutine)
        /// </summary>
        /// <param name="barcodeDTO"></param>
        /// <param name="autoEnqueue"></param>
        public void Enqueue(TBarcodeDTO barcodeDTO, bool autoEnqueue)
        {
            if (barcodeDTO.QueueID >= 0 && barcodeDTO.QueueID < this.list2DBarcode.Count)
            {
                this.list2DBarcode[barcodeDTO.QueueID].Add(barcodeDTO);

                if (autoEnqueue)
                {
                    this.noItemAdded++; //Note: Always increase noItemAdded by 1 after autoEnqueue
                    if (this.repeatSubQueueIndex && this.noItemAdded > 0 && (this.noItemAdded % (this.NoSubQueue * this.itemPerSubQueue) == 0)) this.invertSubQueueIndex = !this.invertSubQueueIndex;
                }
            }
            else
                throw new Exception("Unknow error: Invalid sub queue ID.");
        }



        /// <summary>
        /// Default barcodeID = 0: Dequeue the first item.
        /// This will return null if no barcodeID is found.
        /// </summary>
        /// <returns></returns>
        public TBarcodeDTO Dequeue() { return this.Dequeue(0); }
        public TBarcodeDTO Dequeue(int barcodeID)
        {
            foreach (List<TBarcodeDTO> subQueue in this.list2DBarcode)
            {
                foreach (TBarcodeDTO eachBarcodeDTO in subQueue)
                {
                    if (barcodeID == 0 || eachBarcodeDTO.GetID() == barcodeID)
                    {
                        TBarcodeDTO barcodeDTO = eachBarcodeDTO.ShallowClone();
                        subQueue.Remove(eachBarcodeDTO);

                        return barcodeDTO;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Dequeue a batch of noItemPerCarton of elements from this Matching Queue, by sequence Dequeue from each sub queue, with index 0, 1, 2, 3, ... NoSubQueue-1 (Comfort with: Zero base index)
        /// </summary>
        /// <returns></returns>
        public BarcodeQueue<TBarcodeDTO> Dequeueset()
        {
            if ((((decimal)this.ItemPerSet / (decimal)this.NoSubQueue) % 1) == 0) //CHECK FOR AN Integer RESULT
            {
                BarcodeQueue<TBarcodeDTO> barcodesetQueue = new BarcodeQueue<TBarcodeDTO>(this.NoSubQueue, this.ItemPerSet / this.NoSubQueue, false) { ItemPerSet = this.ItemPerSet };

                foreach (List<TBarcodeDTO> subQueue in this.list2DBarcode)
                {
                    if (barcodesetQueue.itemPerSubQueue > subQueue.Count && !this.LastsetProcessing) return barcodesetQueue; //There is not enough element in this sub queue to dequeue. //IF LastsetProcessing: TO ALLOW TO MAKE A PARTILAL SET. IT MEANS: THE PENDING Item IS LESS THAN THE ItemPerSet, OTHERWISE: return empty
                }


                foreach (List<TBarcodeDTO> subQueue in this.list2DBarcode)
                {
                    for (int i = 0; i < barcodesetQueue.itemPerSubQueue; i++)
                    {   //THE MAXIMUM ITEM PER EACH subQueue IS itemPerSubQueue (FULL SET). 
                        //LATER: WHEN LastsetProcessing => WE CAN MODIFY HERE (ONLY HERE) TO ALLOW: THE MAXIMUM ITEM PER EACH subQueue IS GREATER THAN THE STANDARD itemPerSubQueue. TO DO THIS: FIRST WE HAVE TO ADD A NEW PROPERTY pauseStatus (SET TO TRUE) TO PAUSE THE PROCCESS OF MakePackset/ MakeCartonset BY ScannerController, THEN TURN LastsetProcessing TO true IN ORDER TO RELEASE pauseStatus (SET TO FALSE), FINALLY: MODIFY THIS CONDITION: i < barcodesetQueue.itemPerSubQueue

                        //HERE WE CHECK subQueue.Count > 0 => WHEN LastsetProcessing: SOME subQueue OF THE SET MAY NOT IS FULL
                        if (subQueue.Count > 0) { barcodesetQueue.Enqueue(subQueue.ElementAt(0)); subQueue.RemoveAt(0); }//Check subQueue.Count > 0 just for sure, however, we check it already at the begining of this method
                    }
                }

                if (barcodesetQueue.Count > 0) this.LastsetProcessing = false; //AFTER SUCCESS Dequeueset => WE CLEAR LastsetProcessing

                return barcodesetQueue;
            }
            else
                throw new Exception("Số chai/ carton không phù hợp!");
        }

        /// <summary>
        /// messageData.QueueID: Will change to new value (new position) after replace
        /// </summary>
        /// <param name="packID"></param>
        /// <param name="messageData"></param>
        /// <returns></returns>
        public bool Replace(int packID, TBarcodeDTO messageData)
        {
            foreach (List<TBarcodeDTO> subQueue in this.list2DBarcode)
            {
                for (int i = 0; i < subQueue.Count; i++)
                {
                    if (subQueue[i].GetID() == packID)
                    {
                        messageData.QueueID = subQueue[i].QueueID;
                        subQueue[i] = messageData;
                        return true;
                    }
                }
            }
            return false; //Return false if can not find any PackID
        }


        public virtual DataTable ConverttoTable()
        {
            int maxSubQueueCount = 0;
            DataTable barcodeTable = new DataTable("BarcodeTable");

            lock (this.list2DBarcode)
            {
                foreach (List<TBarcodeDTO> subQueue in this.list2DBarcode)
                {
                    maxSubQueueCount = maxSubQueueCount <= subQueue.Count ? subQueue.Count : maxSubQueueCount;
                }

                for (int i = 0; i < maxSubQueueCount; i++)//Make a table with number of column equal to maxSubQueueCount
                {
                    barcodeTable.Columns.Add((i < 9 ? " " : "") + (i + 1).ToString().Trim());
                }

                foreach (List<TBarcodeDTO> subQueue in this.list2DBarcode)
                {
                    DataRow dataRow = barcodeTable.NewRow(); //add row for each sub queue
                    for (int i = 0; i < maxSubQueueCount; i++)
                    {//Zero base queue element
                        if (subQueue.Count > i) dataRow[i] = subQueue.ElementAt<TBarcodeDTO>(i).Code + GlobalVariables.doubleTabChar + GlobalVariables.doubleTabChar + subQueue.ElementAt<TBarcodeDTO>(i).GetID(); //Fill data row
                    }
                    barcodeTable.Rows.Add(dataRow);
                }
            }
            return barcodeTable;

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
                foreach (List<TBarcodeDTO> subQueue in this.list2DBarcode)
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
            if (subQueueID >= 0 && subQueueID < this.NoSubQueue && index >= 0 && index < this.list2DBarcode[subQueueID].Count)  //Zero base index
            {
                return this.list2DBarcode[subQueueID].ElementAt(index);
            }
            return null; //Return Null if index out of range
        }

        #endregion Public Method



        public string EntityIDs
        {
            get
            {
                string entityIDs = string.Join(",", this.list2DBarcode.Select(q => string.Join(",", q.Select(l => l.GetID().ToString()).ToArray())).ToArray());

                if (this.Count < this.ItemPerSet)
                    while (entityIDs.IndexOf(",,") >= 0)
                        entityIDs = entityIDs.Replace(",,", ",");

                return entityIDs;
            }
        }

        public int TotalPacks { get { return this.list2DBarcode.Select(o => o.Select(y => y.TotalPacks).Sum()).Sum(); } }

        public decimal TotalVolume { get { return this.list2DBarcode.Select(o => o.Select(y => y.Volume).Sum()).Sum(); } }

    }
}
