/****************************************************************************
 * File:            MagneticModelCollection.cs
 * Description:     Collection of magnetic model sets with serialization support
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 ****************************************************************************/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace GeoMagSharp
{
    /// <summary>
    /// Magnetic Model Collection Object - manages multiple MagneticModelSets
    /// </summary>
    public class MagneticModelCollection : IEnumerable<MagneticModelSet>
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.None)]
        public List<MagneticModelSet> TList { get; private set; }

        #region Constructors

        public MagneticModelCollection()
        {
            TList = new List<MagneticModelSet>();
        }

        #endregion

        #region Base Class Methods

        public void Add(MagneticModelSet item)
        {
            TList.Add(item);
        }

        public void AddRange(IEnumerable<MagneticModelSet> collection)
        {
            TList.AddRange(collection);
        }

        public MagneticModelSet Find(Predicate<MagneticModelSet> match)
        {
            return TList.Find(match);
        }

        public List<MagneticModelSet> FindAll(Predicate<MagneticModelSet> match)
        {
            return TList.FindAll(match);
        }

        /// <summary>
        /// Finds an existing model that contains the specified filename
        /// </summary>
        /// <param name="fileName">The filename to search for (case-insensitive)</param>
        /// <returns>The matching MagneticModelSet, or null if not found</returns>
        public MagneticModelSet FindByFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;

            return TList.Find(m =>
                m.FileNames != null &&
                m.FileNames.Any(f =>
                    string.Equals(f, fileName, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Adds a model to the collection, or replaces an existing model with the same filename
        /// </summary>
        /// <param name="item">The model to add or replace</param>
        /// <returns>True if an existing model was replaced, false if a new model was added</returns>
        public bool AddOrReplace(MagneticModelSet item)
        {
            if (item == null || item.FileNames == null || !item.FileNames.Any())
            {
                Add(item);
                return false;
            }

            // Check if any of the item's filenames already exist in the collection
            var existingModel = FindByFileName(item.FileNames.First());

            if (existingModel != null)
            {
                // Replace the existing model
                int index = TList.IndexOf(existingModel);
                TList[index] = item;
                return true;
            }

            // Add as new model
            Add(item);
            return false;
        }

        public IEnumerator<MagneticModelSet> GetEnumerator()
        {
            return TList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Object Serializers

        public bool Save(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return false;

            bool wasSucessful;

            var inData = this;

            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Newtonsoft.Json.Formatting.Indented
                };

                using (StreamWriter sw = new StreamWriter(filename))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, inData);
                }

                wasSucessful = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("MagneticModelCollection Error: {0}", ex.ToString());
                wasSucessful = false;
            }

            return wasSucessful;
        }

        public static MagneticModelCollection Load(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return new MagneticModelCollection();

            if (!System.IO.File.Exists(filename)) return new MagneticModelCollection();

            MagneticModelCollection outData = null;

            try
            {
                JsonSerializer serializer = new JsonSerializer();

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Formatting = Newtonsoft.Json.Formatting.Indented;

                using (var sr = new System.IO.StreamReader(filename))
                using (var reader = new JsonTextReader(sr))
                {
                    var deserializeList = JsonConvert.DeserializeObject<IEnumerable<MagneticModelSet>>(serializer.Deserialize(reader).ToString());

                    outData = new MagneticModelCollection();

                    outData.AddRange(deserializeList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MagneticModelCollection Error: {0}", ex.ToString());
                outData = null;
            }

            return outData;
        }

        #endregion

        #region getters & setters

        public DataTable GetDataTable
        {
            get
            {
                var DtModels = new DataTable();

                DtModels.Columns.Add(new DataColumn("ID", typeof(Guid)));
                DtModels.Columns.Add(new DataColumn("ModelName", typeof(string)));
                DtModels.Columns.Add(new DataColumn("FileNames", typeof(string)));
                DtModels.Columns.Add(new DataColumn("DateMin", typeof(DateTime)));
                DtModels.Columns.Add(new DataColumn("DateMax", typeof(DateTime)));
                DtModels.Columns.Add(new DataColumn("NumberOfModels", typeof(Int32)));
                DtModels.Columns.Add(new DataColumn("Type", typeof(string)));

                foreach (var model in TList)
                {
                    var fRow = DtModels.NewRow();

                    fRow["ID"] = model.ID;
                    fRow["ModelName"] = model.Name;
                    fRow["FileNames"] = string.Join(",", model.FileNames);
                    fRow["DateMin"] = model.MinDate.ToDateTime();
                    fRow["DateMax"] = model.MaxDate.ToDateTime();
                    fRow["NumberOfModels"] = model.NumberOfModels;
                    fRow["Type"] = model.Type;

                    DtModels.Rows.Add(fRow);
                }

                return DtModels.Copy();
            }
        }

        #endregion
    }
}
